using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using s3pi.Interfaces;
using s3pi.Filetable;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class CASPresetXmlNode : DefaultNode
    {
        public sealed class XmlRKContainer : RKContainer
        {
            /*
<preset>
  <complate name="CasRgbMask" reskey="blah">
    <value key="..." value="key:00B2D882:00000000:B5683B98CA67ECFC" />
  </complate>
</preset>
            /**/
            private static string[] CasRgbMaskWantedKeys = new string[] {
                 "Overlay"          
                ,"Mask"             
                ,"Transparency Map" 
                ,"Multiplier"       
                ,"Clothing Specular"
                ,"Clothing Ambient" 
                ,"Stencil A"        
                ,"Stencil B"        
                ,"Stencil C"        
                ,"Stencil D"        
                ,"Stencil E"        
                ,"Stencil F"        
                ,"Logo Texture"     
                ,"Logo 2 Texture"   
                ,"Logo 3 Texture"   
                ,
            };

            private class RKElem
            {
                public string Name;
                public RK OldResourceKey;
                public RK NewResourceKey;
                public XmlElement Element;
                public RKElem(string name, RK key, XmlElement elem)
                {
                    this.Name = name;
                    this.OldResourceKey = this.NewResourceKey = key;
                    this.Element = elem;
                }
                public void Commit()
                {
                    this.Element.SetAttribute("value", 
                        "key:" + TextRKContainer.PrintRK(this.NewResourceKey));
                }
            }

            private List<RKElem> oldToNewRKs = new List<RKElem>();
            private XmlDocument xmlDocument;
            private List<IResourceConnection> owners = new List<IResourceConnection>();
            public XmlDocument Document
            {
                get { return this.xmlDocument; }
            }
            public List<IResourceConnection> Owners
            {
                get { return this.owners; }
            }

            public XmlRKContainer(XmlDocument xmlDoc)
                : base("caspPresetXML", null, "caspPresetXML", null)
            {
                this.xmlDocument = xmlDoc;
                int i, foundIndex, length = CasRgbMaskWantedKeys.Length;
                XmlAttribute keyAttr, valueAttr;
                string keyStr, valueStr;
                foreach (XmlElement elem in xmlDoc.GetElementsByTagName("value"))
                {
                    keyAttr = elem.Attributes["key"];
                    if (keyAttr != null)
                    {
                        keyStr = keyAttr.Value;
                        if (!string.IsNullOrEmpty(keyStr))
                        {
                            foundIndex = -1;
                            for (i = 0; i < length && foundIndex < 0; i++)
                            {
                                if (CasRgbMaskWantedKeys[i].Equals(keyStr))
                                    foundIndex = i;
                            }
                            if (foundIndex >= 0)
                            {
                                valueAttr = elem.Attributes["value"];
                                if (valueAttr != null)
                                {
                                    valueStr = valueAttr.Value;
                                    if (valueStr != null)
                                    {
                                        RK rk;
                                        if (TextRKContainer.ParseRK(valueStr.Substring(4), out rk))
                                        {
                                            this.oldToNewRKs.Add(new RKElem(keyStr, rk, elem));
                                            this.owners.Add(new DefaultConnection(rk, this, 
                                                ResourceDataActions.FindWrite,
                                                "caspPresetXML{" + keyStr + "}"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public override bool SetRK(IResourceKey oldKey, IResourceKey newKey)
            {
                RKElem elem;
                bool success = false;
                for (int i = 0; i < this.oldToNewRKs.Count; i++)
                {
                    elem = this.oldToNewRKs[i];
                    if (elem.OldResourceKey.Equals(oldKey))
                    {
                        elem.NewResourceKey = new RK(newKey);
                        success = true;
                    }
                }
                return success;
            }

            public override bool CommitChanges()
            {
                for (int i = 0; i < this.oldToNewRKs.Count; i++)
                {
                    this.oldToNewRKs[i].Commit();
                }
                return true;
            }
        }

        private XmlRKContainer mainContainer = null;

        public CASPresetXmlNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        private List<IResourceConnection> CASPKinXML_getDDSes()
        {
            Diagnostics.Log("CASP_getXMLDDSes");
            if (base.resource == null) return null;
            XmlDocument caspDoc = new XmlDocument();
            caspDoc.Load(base.resource.Stream);
            this.mainContainer = new XmlRKContainer(caspDoc);
            return this.mainContainer.Owners;

            /*XElement casp = XElement.Load(base.Resource.Stream);
            foreach (Tuple<string, string> tuple in
                casp.Descendants("value")
                .Where(v => CasRgbMaskWantedKeys.Contains(v.Attribute("key").Value))
                .Select(v => Tuple.Create(v.Attribute("key").Value, v.Attribute("value").Value)))
            {
                IResourceKey rk;
                string oldKey = tuple.Item2.Substring(4).Replace('-', ':');
                string RKkey = "0x" + oldKey.Replace(":", "-0x");//translate to s3pi format
                if (RK.TryParse(RKkey, out rk))
                    Add("casp.PresetXML." + tuple.Item1, rk);
            }/**/
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            if (base.includeDDSes)
                return base.SlurpConnections(constraints);
            else
                return this.CASPKinXML_getDDSes();
        }

        public override bool CommitChanges()
        {
            if (this.mainContainer != null)
                this.mainContainer.Document.Save(base.resource.Stream);
            return base.CommitChanges();
        }
    }
}
