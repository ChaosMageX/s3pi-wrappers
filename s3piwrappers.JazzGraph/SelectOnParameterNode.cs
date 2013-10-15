using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class SelectOnParameterNode : SelectNode<string>
    {
        public const uint ResourceType = 0x02EEDB92;
        public const string ResourceTag = "SoPn";

        private ParamDefinition mParameter;

        public SelectOnParameterNode()
            : base(ResourceType, ResourceTag)
        {
            this.mParameter = null;
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzSelectOnParameterNode jsopn 
                = new JazzSelectOnParameterNode(0, null, s);
            jsopn.ParameterDefinitionIndex = this.mParameter == null
                ? NullCRef : this.mParameter.ChunkReference;
            if (this.CaseCount > 0)
            {
                int j;
                Case c;
                uint hash = 0;
                Case[] cases = this.Cases;
                JazzSelectOnParameterNode.Match match;
                JazzSelectOnParameterNode.MatchList mList = jsopn.Matches;
                JazzChunk.ChunkReferenceList dgi;
                DecisionGraphNode[] targets;
                for (int i = 0; i < cases.Length; i++)
                {
                    c = cases[i];
                    match = new JazzSelectOnParameterNode.Match(0, null);
                    if (c.Value == null)
                    {
                        hash = 0;
                    }
                    else if (!c.Value.StartsWith("0x") ||
                        !uint.TryParse(c.Value.Substring(2),
                            System.Globalization.NumberStyles.HexNumber,
                            null, out hash))
                    {
                        hash = FNVHash.HashString32(c.Value);
                        if (nameMap != null && !nameMap.ContainsKey(hash) &&
                            (exportAllNames || !KeyNameReg.HasName(hash)))
                        {
                            nameMap[hash] = c.Value;
                        }
                    }
                    match.TestValue = hash;
                    dgi = match.DecisionGraphIndexes;
                    targets = c.Targets;
                    for (j = 0; j < targets.Length; j++)
                    {
                        dgi.Add(targets[i] == null 
                            ? NullCRef : targets[i].ChunkReference);
                    }
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jsopn);
        }

        public ParamDefinition Parameter
        {
            get { return this.mParameter; }
            set
            {
                if (this.mParameter != value)
                {
                    this.mParameter = value;
                }
            }
        }
    }
}
