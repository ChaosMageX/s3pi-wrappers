using System;
using System.ComponentModel;
using s3pi.GenericRCOLResource;

namespace FootprintViewer.Models
{
    public class PointViewModel : AbstractViewModel
    {
        private FTPT.PolygonPoint mPoint;
        private AreaViewModel mArea;
        private float mX;
        private float mZ;
        public PointViewModel(AreaViewModel area, FTPT.PolygonPoint point)
        {
            mArea = area;
            mPoint = point;
            mX = mPoint.X - mArea.OffsetX;
            mZ = mPoint.Z - mArea.OffsetZ;
            mArea.PropertyChanged += new PropertyChangedEventHandler(mArea_PropertyChanged);


        }

        public FTPT.PolygonPoint Point
        {
            get { return mPoint; }
        }

        public bool IsSelected
        {
            get { return mArea.SelectedPoint == this; }
        }
        void mArea_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
            case "OffsetX":
                mPoint.X = mX + mArea.OffsetX;
                break;
            case "OffsetZ":
                mPoint.Z = mZ + mArea.OffsetZ;
                break;
            case "SelectedPoint":
                this.OnPropertyChanged("IsSelected");
                break;
            }
        }
        public float X
        {
            get { return mX; }
            set
            {
                mX = value; OnPropertyChanged("X"); OnPropertyChanged("Text"); mPoint.X = mX + mArea.OffsetX;
            }
        }
        public float Z
        {
            get { return mZ; }
            set
            {
                mZ = value; OnPropertyChanged("Z"); OnPropertyChanged("Text"); mPoint.Z = mZ + mArea.OffsetZ;
            }
        }
        public String Text
        {
            get { return this.ToString(); }
        }
        public override string ToString()
        {
            return String.Format("[{0:0.00},{1:0.00}]", X, Z);
        }
    }
}