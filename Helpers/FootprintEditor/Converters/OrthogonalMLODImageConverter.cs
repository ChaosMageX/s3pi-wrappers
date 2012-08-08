using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Data;
using meshExpImp.ModelBlocks;
using s3pi.GenericRCOLResource;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace s3piwrappers.Converters
{
    class OrthogonalMLODImageConverter : IValueConverter
    {
        public System.Windows.Media.Color Fill { get; set; }
        public System.Windows.Media.Color Stroke { get; set; }
        public double Scale { get; set; }
        public double Offset { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (String.IsNullOrEmpty(value as string))
            {
                return null;
            }

            var path = value as string;
            if (path != null && File.Exists(path))
            {
                using (var s = File.OpenRead(path))
                {
                    var rcol = new GenericRCOLResource(0, s);
                    var mlod = rcol.ChunkEntries
                        .Where(x => x.RCOLBlock is MLOD)
                        .Select(x => x.RCOLBlock)
                        .Cast<MLOD>()
                        .FirstOrDefault();


                    double minX = double.MaxValue, minZ = double.MaxValue, maxX = double.MinValue, maxZ = double.MinValue;





                    var pnty = new List<Point[]>();
                    foreach (var mesh in mlod.Meshes)
                    {
                        var vbuf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexBufferIndex) as VBUF;
                        var ibuf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.IndexBufferIndex) as IBUF;
                        var vrtf = GenericRCOLResource.ChunkReference.GetBlock(rcol, mesh.VertexFormatIndex) as VRTF;
                        if(vrtf == null)
                            continue;
                        var verts = vbuf.GetVertices(mesh, vrtf, null);
                        var indices = ibuf.GetIndices(mesh);
                        for (int i = 0; i < indices.Length / 3; i++)
                        {
                            var tst = new int[3];
                            var poly = new Point[3];
                            for (int j = 0; j < 3; j++)
                            {
                                
                                var vert = verts[indices[i*3 + j]];
                                var x = (Offset + (vert.Position[0] * Scale));
                                var z = -(Offset + (vert.Position[2] * Scale));
                                poly[j] = new Point((int)x, (int)z);
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
                    int width = (int)(maxX - minX);
                    int height = (int)(maxZ - minZ);
                    var bmp = new Bitmap(width, height);
                    Graphics gBmp = Graphics.FromImage(bmp);
                    gBmp.CompositingMode = CompositingMode.SourceCopy;
                    float bright = 1.1f;
                    var c = System.Drawing.Color.FromArgb(255, this.Fill.R, this.Fill.G, this.Fill.B);
                    var c2 = System.Drawing.Color.FromArgb(255, this.Stroke.R, this.Stroke.G, this.Stroke.B);
                    
                    var b = new SolidBrush(c);
                    
                    
                    var b2 = new SolidBrush(c2);
                    var pen = new Pen(b2);
                    foreach (var pointse in pnty)
                    {
                        var scaled = pointse.Select(old => new Point((int) (old.X - minX), (int) (old.Y - minZ))).ToArray();
                        gBmp.FillPolygon(b, scaled);
                        

                    }
                    foreach (var pointse in pnty)
                    {
                        var scaled = pointse.Select(old => new Point((int)(old.X - minX), (int)(old.Y - minZ))).ToArray();
                        gBmp.DrawPolygon(pen, scaled);

                    }
                    var bmpImg = new System.Windows.Media.Imaging.BitmapImage();
                    
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

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}