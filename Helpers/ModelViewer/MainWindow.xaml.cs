using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using s3pi.GenericRCOLResource;

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
                return State!=null?"0x" + State.Name.ToString("X8"):"None";
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
            public override string ToString()
            {
                return "0x"+ Mesh.Name.ToString("X8");
            }
        }

        private List<SceneMesh> mSceneMeshes;
        private GenericRCOLResource rcol;
        private Material mHiddenMaterial = new DiffuseMaterial();
        private MaterialGroup mNonSelectedMaterial = new MaterialGroup();
        private MaterialGroup mSelectedMaterial = new MaterialGroup();
        private Material mXrayMaterial;
        public MainWindow()
        {
            InitializeComponent();
            mSceneMeshes= new List<SceneMesh>();
            mHiddenMaterial = new DiffuseMaterial();
            mNonSelectedMaterial.Children.Add(new DiffuseMaterial(Brushes.LightGray));
            mNonSelectedMaterial.Children.Add(new SpecularMaterial(Brushes.GhostWhite,20d));
            mSelectedMaterial.Children.Add(new DiffuseMaterial(Brushes.Red));
            mSelectedMaterial.Children.Add(new SpecularMaterial(Brushes.GhostWhite, 40d));
            mXrayMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromScRgb(0.4f,1f,0f,0f)));
        }
        public MainWindow(Stream s)
            : this()
        {
            rcol = new GenericRCOLResource(0, s);
            InitScene();
        }
        void InitScene()
        {
            MLOD mlod = (MLOD)rcol.ChunkEntries.FirstOrDefault(x => x.RCOLBlock is MLOD).RCOLBlock;
            if (mlod != null)
            {
                foreach (var m in mlod.Meshes)
                {
                    
                    var model = DrawModel(MLODUtil.GetVertices(m, rcol), MLODUtil.GetIndices(m, rcol),mNonSelectedMaterial);

                    var sceneMesh = new SceneMesh(m, model);
                    SceneGeostate[] sceneGeostates = new SceneGeostate[m.GeometryStates.Count];
                    for (int i = 0; i < sceneGeostates.Length; i++)
                    {
                        var state = DrawModel(MLODUtil.GetVertices(m, m.GeometryStates[i], rcol),
                                                   MLODUtil.GetIndices(m, m.GeometryStates[i], rcol), mHiddenMaterial);
                        mGroupMeshes.Children.Add(state);
                        sceneGeostates[i] = new SceneGeostate(sceneMesh, m.GeometryStates[i], state);
                    }
                    sceneMesh.States = sceneGeostates;
                    mGroupMeshes.Children.Add(model);
                    mSceneMeshes.Add(sceneMesh);
                }
            }
            foreach(var s in mSceneMeshes)
            {
                mMeshListView.Items.Add(s);
            }
        }
        static GeometryModel3D DrawModel(Vertex[] verts, Int32[] indices, Material material)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            for (int k = 0; k < verts.Length; k++)
            {
                Vertex v = verts[k];
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

        static Random sRng = new Random();

        private void mMeshListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mStateListView.Items.Clear();
            foreach(var item in e.RemovedItems)
            {
                var m = (SceneMesh)item;
                m.Model.Material = mNonSelectedMaterial;
            }
            if(e.AddedItems.Count >0)
            {
                var m = (SceneMesh)e.AddedItems[0];
                m.Model.Material = mSelectedMaterial;
                mStateListView.Items.Add(new SceneGeostate(m, null, null));
                foreach(var s in m.States)
                {
                    mStateListView.Items.Add(s);
                }
                mStateListView.SelectedIndex = 0;

            }
        }
        private void mStateListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.RemovedItems.Count >0)
            {
                var s = (SceneGeostate)e.RemovedItems[0];
                if(s.State == null)
                {
                    s.Owner.Model.Material = mHiddenMaterial;
                } else
                {
                    s.Model.Material = mHiddenMaterial;
                }
                
            }
            if(e.AddedItems.Count >0)
            {
                var s = (SceneGeostate)e.AddedItems[0];
                if(s.State == null)
                {
                    s.Owner.Model.Material = mSelectedMaterial;
                } else
                {
                    s.Model.Material = mSelectedMaterial;
                }
                
            }
        }

    }
}
