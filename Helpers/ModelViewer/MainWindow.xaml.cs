using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers;
using _3DTools;
namespace s3piwrappers.ModelViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Stream s):this()
        {
            Render(new GenericRCOLResource(0,s));
        }

        void Render(GenericRCOLResource rcol)
        {
            group3D.Children.Clear();
            MLOD mlod = (MLOD) rcol.ChunkEntries.FirstOrDefault(x => x.RCOLBlock is MLOD).RCOLBlock;
            foreach(var m in mlod.Meshes)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();
                var verts = MLODUtil.GetVertices(m, rcol);
                for (int k = 0; k < verts.Length; k++)
                {
                    Vertex v = verts[k];
                    mesh.Positions.Add(new Point3D(v.Position[0] , v.Position[1], v.Position[2]));
                    mesh.Normals.Add(new Vector3D(v.Normal[0],v.Normal[1],v.Normal[2]));
                    mesh.TextureCoordinates.Add(new Point(v.UV[0],v.UV[1]));
                }
                var indices = MLODUtil.GetIndices(m, rcol);
                for (int i = 0; i < indices.Length; i++)
                {
                    mesh.TriangleIndices.Add(indices[i]);
                }
                MaterialGroup mat = new MaterialGroup();
                mat.Children.Add(new DiffuseMaterial(Brushes.DarkGray));
                mat.Children.Add(new SpecularMaterial(Brushes.GhostWhite,30d));
                var model = new GeometryModel3D(mesh, mat);
                
                
                group3D.Children.Add(model);

            }
        }

    }
}
