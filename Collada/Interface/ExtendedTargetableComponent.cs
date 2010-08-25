﻿using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Interface
{
    public abstract class ExtendedTargetableComponent : TargetableComponent, IExtendable
    {
        protected ExtendedTargetableComponent()
        {
            ExtendedData = new List<Extra>();
        }

        [XmlElement("extra")]
        public List<Extra> ExtendedData { get; set; }
    }
}