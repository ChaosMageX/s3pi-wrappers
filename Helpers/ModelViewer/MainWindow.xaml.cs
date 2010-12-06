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
        public MainWindow(Stream s)
            : this()
        {
            Render(new GenericRCOLResource(0, s));
        }
        void DrawAxes()
        {
            int size = 5;
            ScreenSpaceLines3D x = new ScreenSpaceLines3D();
            x.Points.Add(new Point3D(size, 0, 0));
            x.Points.Add(new Point3D(-size, 0, 0));
            x.Color = Colors.Red;
            mainViewport.Children.Add(x);


            ScreenSpaceLines3D y = new ScreenSpaceLines3D();
            y.Points.Add(new Point3D(0, size, 0));
            y.Points.Add(new Point3D(0, -size, 0));
            y.Color = Colors.Green;
            mainViewport.Children.Add(y);


            ScreenSpaceLines3D z = new ScreenSpaceLines3D();
            z.Points.Add(new Point3D(0, 0, size));
            z.Points.Add(new Point3D(0, 0, -size));
            z.Color = Colors.Blue;
            mainViewport.Children.Add(z);

        }
        void DrawGrid()
        {
            int max = 5;
            Color c = Colors.Gray;
            for (float x = 1; x < max; x += 1f)
            {
                for (float z = 1; z < max; z += 1f)
                {
                    ScreenSpaceLines3D l = new ScreenSpaceLines3D();
                    l.Points.Add(new Point3D(x, 0, max));
                    l.Points.Add(new Point3D(x, 0, -max));
                    l.Color = c;
                    mainViewport.Children.Add(l);


                    ScreenSpaceLines3D l2 = new ScreenSpaceLines3D();
                    l2.Points.Add(new Point3D(max, 0, z));
                    l2.Points.Add(new Point3D(-max, 0, z));
                    l2.Color = c;
                    mainViewport.Children.Add(l2);


                    ScreenSpaceLines3D l3 = new ScreenSpaceLines3D();
                    l3.Points.Add(new Point3D(-x, 0, max));
                    l3.Points.Add(new Point3D(-x, 0, -max));
                    l3.Color = c;
                    mainViewport.Children.Add(l3);


                    ScreenSpaceLines3D l4 = new ScreenSpaceLines3D();
                    l4.Points.Add(new Point3D(max, 0, -z));
                    l4.Points.Add(new Point3D(-max, 0, -z));
                    l4.Color = c;
                    mainViewport.Children.Add(l4);
                }
            }
            ScreenSpaceLines3D top = new ScreenSpaceLines3D();
            top.Color = c;
            top.Points.Add(new Point3D(max, 0, max));
            top.Points.Add(new Point3D(-max, 0, max));
            mainViewport.Children.Add(top);


            ScreenSpaceLines3D left = new ScreenSpaceLines3D();
            left.Color = c;
            left.Points.Add(new Point3D(-max, 0, -max));
            left.Points.Add(new Point3D(-max, 0, max));
            mainViewport.Children.Add(left);


            ScreenSpaceLines3D right = new ScreenSpaceLines3D();
            right.Color = c;
            right.Points.Add(new Point3D(max, 0, max));
            right.Points.Add(new Point3D(max, 0, -max));
            mainViewport.Children.Add(right);


            ScreenSpaceLines3D bottom = new ScreenSpaceLines3D();
            bottom.Color = c;
            bottom.Points.Add(new Point3D(-max, 0, -max));
            bottom.Points.Add(new Point3D(max, 0, -max));
            mainViewport.Children.Add(bottom);

        }
        void Render(GenericRCOLResource rcol)
        {
            DrawAxes();
            DrawGrid();
            MLOD mlod = (MLOD)rcol.ChunkEntries.FirstOrDefault(x => x.RCOLBlock is MLOD).RCOLBlock;
            foreach (var m in mlod.Meshes)
            {
                MeshGeometry3D mesh = new MeshGeometry3D();
                var verts = MLODUtil.GetVertices(m, rcol);
                for (int k = 0; k < verts.Length; k++)
                {
                    Vertex v = verts[k];
                    if (v.Position != null) mesh.Positions.Add(new Point3D(v.Position[0], v.Position[1], v.Position[2]));
                    if (v.Normal != null) mesh.Normals.Add(new Vector3D(v.Normal[0], v.Normal[1], v.Normal[2]));
                    if (v.UV != null) mesh.TextureCoordinates.Add(new Point(v.UV[0], v.UV[1]));
                }
                var indices = MLODUtil.GetIndices(m, rcol);
                for (int i = 0; i < indices.Length; i++)
                {
                    mesh.TriangleIndices.Add(indices[i]);
                }
                MaterialGroup mat = new MaterialGroup();
                mat.Children.Add(new DiffuseMaterial(Brushes.DarkGray));
                mat.Children.Add(new SpecularMaterial(Brushes.GhostWhite, 30d));
                var model = new GeometryModel3D(mesh, mat);

                group3D.Children.Add(model);
                //DrawWireframe(mesh);

            }
        }
        void DrawWireframe(MeshGeometry3D m)
        {
            int i = 0;
            while (i < m.TriangleIndices.Count)
            {
                ScreenSpaceLines3D l = new ScreenSpaceLines3D();
                for (int j = 0; j < 3; j++)
                {
                    l.Points.Add(m.Positions[m.TriangleIndices[i++]]);
                }
                l.Points.Add(m.Positions[m.TriangleIndices[i - 3]]);
                mainViewport.Children.Add(l);
            }

        }

    }
}
