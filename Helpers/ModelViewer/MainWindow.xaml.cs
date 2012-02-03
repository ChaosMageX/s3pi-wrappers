using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace s3piwrappers.ModelViewer
{

    public partial class MainWindow : Window
    {
        class SceneGeostate
        {
            public SceneGeostate(SceneMesh owner, MLOD.GeometryState state, GeometryModel3D model)
            {
                Owner = owner;
                State = state;
                Model = model;
            }

            public SceneMesh Owner { get; set; }
            public MLOD.GeometryState State { get; set; }
            public GeometryModel3D Model { get; set; }
            public override string ToString()
            {
                return State != null ? "0x" + State.Name.ToString("X8") : "None";
            }
        }
        class SceneMesh
        {
            public SceneMesh(MLOD.Mesh mesh, GeometryModel3D model)
            {
                Mesh = mesh;
                Model = model;
            }

            public MLOD.Mesh Mesh { get; set; }
            public GeometryModel3D Model { get; set; }
            public SceneGeostate[] States { get; set; }
            public SceneGeostate SelectedState { get; set; }
            public s3pi.GenericRCOLResource.MATD.ShaderType Shader { get; set; }
            public override string ToString()
            {
                return "0x" + Mesh.Name.ToString("X8");
            }
        }

        private List<SceneMesh> mSceneMeshes;
        private SceneMesh mSelectedMesh;
        private GenericRCOLResource rcol;
        private Material mHiddenMaterial = new DiffuseMaterial();
        private MaterialGroup mNonSelectedMaterial = new MaterialGroup();
        private MaterialGroup mSelectedMaterial = new MaterialGroup();
        private Material mXrayMaterial;
        private Material mCheckerMaterial;
        private Material mTexturedMaterial;
        private MaterialGroup mGlassMaterial = new MaterialGroup();
        private MaterialGroup mShadowMapMaterial = new MaterialGroup();
        public String TextureSource
        {
            set
            {
                if (File.Exists(value))
                {
                    try
                    {
                        mTexturedMaterial = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(value))));
                        rbTextured.Visibility = Visibility.Visible;
                        rbTextured.IsChecked = true;
                    }
                    catch (Exception ex)
                    {
                        mTexturedMaterial = null;
                        MessageBox.Show("Unable to load texture " + value);

                    }

                }
                else
                {
                    mTexturedMaterial = null;
                    rbTextured.Visibility = Visibility.Collapsed;
                }

            }
        }
        public MainWindow()
        {
            InitializeComponent();
            mSceneMeshes = new List<SceneMesh>();

            mNonSelectedMaterial.Children.Add(new DiffuseMaterial(Brushes.LightGray));
            mNonSelectedMaterial.Children.Add(new SpecularMaterial(Brushes.GhostWhite, 20d));
            mSelectedMaterial.Children.Add(new DiffuseMaterial(Brushes.Red));
            mSelectedMaterial.Children.Add(new SpecularMaterial(Brushes.Red, 40d));
            mXrayMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromScRgb(0.4f, 1f, 0f, 0f)));


            var checkerBrush = new ImageBrush();
            checkerBrush.ImageSource = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(typeof(MainWindow).Assembly.Location), "checkers.png")));
            checkerBrush.TileMode = TileMode.Tile;
            mCheckerMaterial = new DiffuseMaterial(checkerBrush);


            mGlassMaterial.Children.Add(new DiffuseMaterial(new SolidColorBrush(Color.FromScRgb(0.6f, .9f, .9f, 1f))));
            mGlassMaterial.Children.Add(new SpecularMaterial(Brushes.White, 100d));


            mShadowMapMaterial.Children.Add(new DiffuseMaterial(Brushes.DimGray));
            mShadowMapMaterial.Children.Add(new SpecularMaterial(Brushes.GhostWhite, 20d));

        }
        public MainWindow(Stream s)
            : this()
        {
            rcol = new GenericRCOLResource(0, s);
            rbTextured.Visibility = Visibility.Collapsed;
            InitScene();
            rbSolid.IsChecked = true;


        }
        void InitScene()
        {
            GeostatesPanel.Visibility = Visibility.Collapsed;
            MLOD mlod = (MLOD)rcol.ChunkEntries.FirstOrDefault(x => x.RCOLBlock is MLOD).RCOLBlock;

            if (mlod != null)
            {
                foreach (var m in mlod.Meshes)
                {
                    var vbuf = (VBUF)GenericRCOLResource.ChunkReference.GetBlock(rcol, m.VertexBufferIndex);
                    var ibuf = (IBUF)GenericRCOLResource.ChunkReference.GetBlock(rcol, m.IndexBufferIndex);
                    var vrtf = (VRTF)GenericRCOLResource.ChunkReference.GetBlock(rcol, m.VertexFormatIndex) ?? VRTF.CreateDefaultForMesh(m);
                    var material = GenericRCOLResource.ChunkReference.GetBlock(rcol, m.MaterialIndex);

                    var matd = FindMainMATD(rcol, material);
                    var uvscale = GetUvScales(matd);
                    if (uvscale != null)
                        Debug.WriteLine(string.Format("{0} - {1} - {2}", uvscale[0], uvscale[2], uvscale[2]));
                    else
                        Debug.WriteLine("No scales");
                    var model = DrawModel(vbuf.GetVertices(m, vrtf, uvscale), ibuf.GetIndices(m), mNonSelectedMaterial);

                    var sceneMesh = new SceneMesh(m, model);
                    if (matd != null)
                        sceneMesh.Shader = matd.Shader;
                    SceneGeostate[] sceneGeostates = new SceneGeostate[m.GeometryStates.Count];
                    for (int i = 0; i < sceneGeostates.Length; i++)
                    {
                        var state = DrawModel(vbuf.GetVertices(m, vrtf, m.GeometryStates[i], uvscale),
                                                   ibuf.GetIndices(m, vrtf, m.GeometryStates[i]), mHiddenMaterial);
                        mGroupMeshes.Children.Add(state);
                        sceneGeostates[i] = new SceneGeostate(sceneMesh, m.GeometryStates[i], state);
                    }
                    sceneMesh.States = sceneGeostates;
                    mGroupMeshes.Children.Add(model);
                    mSceneMeshes.Add(sceneMesh);
                }
            }
            foreach (var s in mSceneMeshes)
            {
                mMeshListView.Items.Add(s);
            }
        }
        static MATD FindMainMATD(GenericRCOLResource rcol, IRCOLBlock material)
        {
            float[] scales = null;
            if (material == null) return null;
            if (material is MATD)
            {
                return material as MATD;
            }
            else if (material is MTST)
            {
                MTST mtst = material as MTST;

                material = GenericRCOLResource.ChunkReference.GetBlock(rcol, mtst.Index);
                //return null;
                if (material is MATD)
                {
                    MATD matd = (MATD)material;
                    return matd;
                }
                //                foreach (var entry in mtst.Entries)
                //                {
                //                    //skip materials that don't exist in this rcol
                //                    if (entry.Index.RefType == GenericRCOLResource.ReferenceType.Private ||
                //                        entry.Index.RefType == GenericRCOLResource.ReferenceType.Public)
                //                    {
                //                        material = GenericRCOLResource.ChunkReference.GetBlock(rcol, entry.Index);
                //                        if (material is MATD)
                //                        {
                //                            MATD matd = (MATD)material;
                //                            return matd;
                //                        }
                //                        else
                //                            return FindMainMATD(rcol, material);
                //                    }
                //                }
            }
            else
            {
                throw new ArgumentException("Material must be of type MATD or MTST", "material");
            }

            return null;

        }
        static float[] GetUvScales(MATD matd)
        {
            if (matd != null)
            {
                var param =
                    (matd.Mtnf != null ? matd.Mtnf.SData : matd.Mtrl.SData).FirstOrDefault(
                                                                                           x => x.Field == MATD.FieldType.UVScales) as MATD.ElementFloat3;
                if (param != null)
                {
                    return new[] { param.Data0, param.Data1, param.Data2 };
                }
            }
            return null;
        }
        static GeometryModel3D DrawModel(meshExpImp.ModelBlocks.Vertex[] verts, Int32[] indices, Material material)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int k = 0; k < verts.Length; k++)
            {
                meshExpImp.ModelBlocks.Vertex v = verts[k];
                if (v.Position != null) mesh.Positions.Add(new Point3D(v.Position[0], v.Position[1], v.Position[2]));
                if (v.Normal != null) mesh.Normals.Add(new Vector3D(v.Normal[0], v.Normal[1], v.Normal[2]));
                if (v.UV != null && v.UV.Length > 0) mesh.TextureCoordinates.Add(new Point(v.UV[0][0], v.UV[0][1]));
            }
            for (int i = 0; i < indices.Length; i++)
            {
                mesh.TriangleIndices.Add(indices[i]);
            }
            return new GeometryModel3D(mesh, material);
        }

        private void mMeshListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mStateListView.Items.Clear();
            if (e.AddedItems.Count > 0)
            {
                var m = (SceneMesh)e.AddedItems[0];
                mSelectedMesh = m;
                GeostatesPanel.Visibility = m.States.Count() == 0 ? Visibility.Collapsed : Visibility.Visible;
                mStateListView.Items.Add(new SceneGeostate(m, null, null));
                foreach (var s in m.States)
                {
                    mStateListView.Items.Add(s);
                }
                mStateListView.SelectedIndex = 0;

            }
            UpdateMaterials();
        }
        private void UpdateMaterials()
        {
            foreach (var sceneMesh in mSceneMeshes)
            {
                Material meshMaterial = null;
                switch (mDrawMode)
                {
                    case "Solid":
                        meshMaterial = mSelectedMesh == sceneMesh ? mSelectedMaterial : mNonSelectedMaterial;
                        break;
                    case "Textured":
                        switch (sceneMesh.Shader)
                        {
                            case MATD.ShaderType.GlassForFences:
                            case MATD.ShaderType.GlassForObjects:
                            case MATD.ShaderType.GlassForObjectsTranslucent:
                            case MATD.ShaderType.GlassForPortals:
                            case MATD.ShaderType.GlassForRabbitHoles:
                                meshMaterial = mGlassMaterial;
                                break;
                            case MATD.ShaderType.ShadowMap:
                                meshMaterial = mShadowMapMaterial;
                                break;
                            default:
                                meshMaterial = mTexturedMaterial;
                                break;
                        }
                        break;
                    case "UV":
                        meshMaterial = mCheckerMaterial;
                        break;
                }
                if (sceneMesh.SelectedState != null && sceneMesh.SelectedState.Model != null)
                {
                    sceneMesh.Model.Material = mHiddenMaterial;
                    sceneMesh.SelectedState.Model.Material = meshMaterial;
                }
                else
                {
                    sceneMesh.Model.Material = meshMaterial;
                }
                foreach (var sceneState in sceneMesh.States)
                {
                    if (sceneState != sceneMesh.SelectedState)
                    {
                        sceneState.Model.Material = mHiddenMaterial;
                    }
                }
            }
        }
        private void mStateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //            if (e.RemovedItems.Count > 0)
            //            {
            //                var s = (SceneGeostate)e.RemovedItems[0];
            //                if (s.State == null)
            //                {
            //                    s.Owner.Model.Material = mHiddenMaterial;
            //                }
            //                else
            //                {
            //                    s.Model.Material = mHiddenMaterial;
            //                }
            //
            //            }
            if (e.AddedItems.Count > 0)
            {
                var s = (SceneGeostate)e.AddedItems[0];
                mSelectedMesh.SelectedState = s;
                UpdateMaterials();
                //                if (s.State == null)
                //                {
                //                    s.Owner.Model.Material = mSelectedMaterial;
                //                }
                //                else
                //                {
                //                    s.Model.Material = mSelectedMaterial;
                //                }

            }
        }

        private string mDrawMode;
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = (RadioButton)sender;
            mDrawMode = rb.Content.ToString();
            UpdateMaterials();
        }

    }
}
