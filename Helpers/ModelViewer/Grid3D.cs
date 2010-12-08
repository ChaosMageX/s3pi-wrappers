using System.Windows.Media;
using System.Windows.Media.Media3D;
using _3DTools;

namespace s3piwrappers.ModelViewer
{
    public class Grid3D : ModelVisual3D
    {
        private int mGridSize = 10;
        private Color mXColor = Colors.Red;
        private Color mYColor = Colors.Yellow;
        private Color mZColor = Colors.Blue;
        private Color mGridColor = Colors.LightGray;
        private float mMajorGridLine = 1f;
        public Grid3D()
        {
            Init();
        }
        void Init()
        {
            Children.Clear();
            Children.Add(DrawLine(mGridSize, 0, 0, -mGridSize, 0, 0, mXColor));
            Children.Add(DrawLine(0, mGridSize, 0, 0, -mGridSize, 0, mYColor));
            Children.Add(DrawLine(0, 0, mGridSize, 0, 0, -mGridSize, mZColor));
            Color c = Colors.Gray;
            for (float x = 1; x < mGridSize; x += mMajorGridLine)
            {
                for (float z = 1; z < mGridSize; z += mMajorGridLine)
                {
                    Children.Add(DrawLine(x, 0, mGridSize, x, 0, -mGridSize, mGridColor));
                    Children.Add(DrawLine(-x, 0, mGridSize, -x, 0, -mGridSize, mGridColor));
                    Children.Add(DrawLine(mGridSize, 0, z, -mGridSize, 0, z, mGridColor));
                    Children.Add(DrawLine(mGridSize, 0, -z, -mGridSize, 0, -z, mGridColor));
                }
            }

            Children.Add(DrawLine(mGridSize, 0, mGridSize, -mGridSize, 0, mGridSize, mGridColor));
            Children.Add(DrawLine(-mGridSize, 0, -mGridSize, -mGridSize, 0, mGridSize, mGridColor));
            Children.Add(DrawLine(mGridSize, 0, mGridSize, mGridSize, 0, -mGridSize, mGridColor));
            Children.Add(DrawLine(-mGridSize, 0, -mGridSize, mGridSize, 0, -mGridSize, mGridColor));
            
        }
        public float MajorGridLine
        {
            get { return mMajorGridLine; }
            set { if(mMajorGridLine!=value){mMajorGridLine = value; Init();} }
        }


        public Color GridColor
        {
            get { return mGridColor; }
            set { if(mGridColor!=value){mGridColor = value; Init();} }
        }

        public Color ZColor
        {
            get { return mZColor; }
            set { if(mZColor!=value){mZColor = value; Init();} }
        }

        public Color YColor
        {
            get { return mYColor; }
            set { if(mYColor!=value){mYColor = value; Init();} }
        }

        public Color XColor
        {
            get { return mXColor; }
            set { if(mXColor!=value){mXColor = value; Init();} }
        }

        public int GridSize
        {
            get { return mGridSize; }
            set { if(mGridSize!=value){mGridSize = value; Init();} }
        }

        static ScreenSpaceLines3D DrawLine(double x1, double y1, double z1, double x2, double y2, double z2, Color color)
        {
            ScreenSpaceLines3D l = new ScreenSpaceLines3D();
            l.Points.Add(new Point3D(x1, y1, z1));
            l.Points.Add(new Point3D(x2, y2, z2));
            l.Color = color;
            return l;
        }
    }
}