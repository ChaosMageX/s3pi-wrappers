using System;
using System.ComponentModel;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.Models
{
    public class PointViewModel : AbstractViewModel
    {
        private FTPT.PolygonPoint mPoint;
        private AreaViewModel mArea;
        public PointViewModel(AreaViewModel area, FTPT.PolygonPoint point)
        {
            mArea = area;
            mPoint = point;
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
                case "SelectedPoint":
                    this.OnPropertyChanged("IsSelected");
                    break;
            }
        }
        
        public float X
        {
            get { return mPoint.X; }
            set
            {
                mPoint.X = value; OnPropertyChanged("X"); OnPropertyChanged("RelativeX"); OnPropertyChanged("Text");
            }
        }
        public float Z
        {
            get { return mPoint.Z; }
            set
            {
                mPoint.Z = value; OnPropertyChanged("Z"); OnPropertyChanged("RelativeZ"); OnPropertyChanged("Text");
            }
        }

        public float RelativeX
        {
            get { return mPoint.X - mArea.OffsetX; }
            set
            {
                mPoint.X = value + mArea.OffsetX; OnPropertyChanged("X"); OnPropertyChanged("Text");
            }
        }
        public float RelativeZ
        {
            get { return mPoint.Z - mArea.OffsetZ; }
            set
            {
                mPoint.Z = value + mArea.OffsetZ; OnPropertyChanged("Z"); OnPropertyChanged("Text");
            }
        }
        public String Text
        {
            get { return this.ToString(); }
        }
        public override string ToString()
        {
            return String.Format("[{0:0.00},{1:0.00}] ({2:0.00},{3:0.00})", X, Z,RelativeX,RelativeZ);
        }
    }
}