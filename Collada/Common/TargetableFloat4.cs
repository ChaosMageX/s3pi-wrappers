﻿using System;
using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;
using s3piwrappers.Collada.Scene;

namespace s3piwrappers.Collada.Common
{
    [XmlInclude(typeof (Rotate))]
    public class TargetableFloat4 : TargetableSubComponent
    {
        [XmlIgnore]
        public double X { get; set; }

        [XmlIgnore]
        public double Y { get; set; }

        [XmlIgnore]
        public double Z { get; set; }

        [XmlIgnore]
        public double W { get; set; }

        [XmlText]
        public string Text
        {
            get { return String.Format("{0} {1} {2} {3}", X, Y, Z, W); }
            set
            {
                var vals = value.Split(' ');
                X = Double.Parse(vals[0]);
                Y = Double.Parse(vals[1]);
                Z = Double.Parse(vals[2]);
                W = Double.Parse(vals[3]);
            }
        }
    }
}