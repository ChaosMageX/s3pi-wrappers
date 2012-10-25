using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using Color = System.Windows.Media.Color;

namespace s3piwrappers.Converters
{
    internal class OrthogonalMLODImageConverter : IValueConverter
    {
        public Color Fill { get; set; }
        public Color Stroke { get; set; }
        public double Scale { get; set; }
        public double Offset { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(value as string))
            {
                return null;
            }

            var path = value as string;
            if (path != null && File.Exists(path))
            {
                using (FileStream s = File.OpenRead(path))
                {
                    var rcol = new GenericRCOLResource(0, s);
                    MLOD mlod = rcol.ChunkEntries
                        .Where(x => x.RCOLBlock is MLOD)
                        .Select(x => x.RCOLBlock)
                        .Cast<MLOD>()
                        .FirstOrDefault();


                    double minX = double.MaxValue, minZ = double.MaxValue, maxX = double.MinValue, maxZ = double.MinValue;


                    var pnty = new List<Point[]>();
                    foreach (MLOD.Mesh mesh in mlod.Meshes)
                    {
                        var vbuf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexBufferIndex) as VBUF;
                        var ibuf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.IndexBufferIndex) as IBUF;
                        var vrtf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexFormatIndex) as VRTF;
                        if (vrtf == null)
                            continue;
                        Vertex[] verts = vbuf.GetVertices(mesh, vrtf, null);
                        int[] indices = ibuf.GetIndices(mesh);
                        for (int i = 0; i < indices.Length/3; i++)
                        {
                            var tst = new int[3];
                            var poly = new Point[3];
                            for (int j = 0; j < 3; j++)
                            {
                                Vertex vert = verts[indices[i*3 + j]];
                                double x = (Offset + (vert.Position[0]*Scale));
                                double z = -(Offset + (vert.Position[2]*Scale));
                                poly[j] = new Point((int) x, (int) z);
                                if (poly[j].X > maxX)
                                    maxX = poly[j].X;

                                if (poly[j].X < minX)
                                    minX = poly[j].X;

                                if (poly[j].Y > maxZ)
                                    maxZ = poly[j].Y;

                                if (poly[j].Y < minZ)
                                    minZ = poly[j].Y;
                            }
                            pnty.Add(poly);
                        }
                    }
                    var width = (int) (maxX - minX);
                    var height = (int) (maxZ - minZ);
                    var bmp = new Bitmap(width, height);
                    Graphics gBmp = Graphics.FromImage(bmp);
                    gBmp.CompositingMode = CompositingMode.SourceCopy;
                    float bright = 1.1f;
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(255, Fill.R, Fill.G, Fill.B);
                    System.Drawing.Color c2 = System.Drawing.Color.FromArgb(255, Stroke.R, Stroke.G, Stroke.B);

                    var b = new SolidBrush(c);


                    var b2 = new SolidBrush(c2);
                    var pen = new Pen(b2);
                    foreach (Point[] pointse in pnty)
                    {
                        Point[] scaled = pointse.Select(old => new Point((int) (old.X - minX), (int) (old.Y - minZ))).ToArray();
                        gBmp.FillPolygon(b, scaled);
                    }
                    foreach (Point[] pointse in pnty)
                    {
                        Point[] scaled = pointse.Select(old => new Point((int) (old.X - minX), (int) (old.Y - minZ))).ToArray();
                        gBmp.DrawPolygon(pen, scaled);
                    }
                    var bmpImg = new BitmapImage();

                    var ms = new MemoryStream();

                    bmp.Save(ms, ImageFormat.Png);
                    ms.Position = 0L;
                    bmpImg.BeginInit();
                    bmpImg.StreamSource = ms;
                    bmpImg.EndInit();
                    return bmpImg;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
