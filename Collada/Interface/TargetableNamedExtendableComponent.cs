﻿using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Interface
{
    public abstract class TargetableNamedExtendableComponent
        : TargetableNamedComponent, IExtendable
    {
        protected TargetableNamedExtendableComponent()
        {
            Extra = new List<Extra>();
        }

        [XmlElement("extra")]
        public IList<Extra> Extra { get; set; }
    }
}