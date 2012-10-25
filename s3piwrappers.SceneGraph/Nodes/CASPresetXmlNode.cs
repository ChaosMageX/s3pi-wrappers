using System.Collections.Generic;
using System.Xml;
using s3pi.Filetable;
using s3pi.Interfaces;

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

            private static readonly string[] CasRgbMaskWantedKeys = new[]
                {
                    "Overlay"
                    , "Mask"
                    , "Transparency Map"
                    , "Multiplier"
                    , "Clothing Specular"
                    , "Clothing Ambient"
                    , "Stencil A"
                    , "Stencil B"
                    , "Stencil C"
                    , "Stencil D"
                    , "Stencil E"
                    , "Stencil F"
                    , "Logo Texture"
                    , "Logo 2 Texture"
                    , "Logo 3 Texture"
                    ,
                };

            private class RKElem
            {
                public string Name;
                public readonly RK OldResourceKey;
                public RK NewResourceKey;
                public readonly XmlElement Element;

                public RKElem(string name, RK key, XmlElement elem)
                {
                    Name = name;
                    OldResourceKey = NewResourceKey = key;
                    Element = elem;
                }

                public void Commit()
                {
                    Element.SetAttribute("value",
                                         "key:" + TextRKContainer.PrintRK(NewResourceKey));
                }
            }

            private readonly List<RKElem> oldToNewRKs = new List<RKElem>();
            private readonly XmlDocument xmlDocument;
            private readonly List<IResourceConnection> owners = new List<IResourceConnection>();

            public XmlDocument Document
            {
                get { return xmlDocument; }
            }

            public List<IResourceConnection> Owners
            {
                get { return owners; }
            }

            public XmlRKContainer(XmlDocument xmlDoc)
                : base("caspPresetXML", null, "caspPresetXML", null)
            {
                xmlDocument = xmlDoc;
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
                                            oldToNewRKs.Add(new RKElem(keyStr, rk, elem));
                                            owners.Add(new DefaultConnection(rk, this,
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
                for (int i = 0; i < oldToNewRKs.Count; i++)
                {
                    elem = oldToNewRKs[i];
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
                for (int i = 0; i < oldToNewRKs.Count; i++)
                {
                    oldToNewRKs[i].Commit();
                }
                return true;
            }
        }

        private XmlRKContainer mainContainer;

        public CASPresetXmlNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }

        private List<IResourceConnection> CASPKinXML_getDDSes()
        {
            Diagnostics.Log("CASP_getXMLDDSes");
            if (base.resource == null) return null;
            var caspDoc = new XmlDocument();
            caspDoc.Load(base.resource.Stream);
            mainContainer = new XmlRKContainer(caspDoc);
            return mainContainer.Owners;

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
                return CASPKinXML_getDDSes();
        }

        public override bool CommitChanges()
        {
            if (mainContainer != null)
                mainContainer.Document.Save(base.resource.Stream);
            return base.CommitChanges();
        }
    }
}
