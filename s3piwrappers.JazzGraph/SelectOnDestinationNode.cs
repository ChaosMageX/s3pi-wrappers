using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;

namespace s3piwrappers.JazzGraph
{
    public class SelectOnDestinationNode : SelectNode<State>
    {
        public const uint ResourceType = 0x02EEDBA5;
        public const string ResourceTag = "DG00";

        public SelectOnDestinationNode()
            : base(ResourceType, ResourceTag)
        {
        }

        public override GenericRCOLResource.ChunkEntry Export(
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            System.IO.Stream s = null;
            TGIBlock tgi
                = new TGIBlock(0, null, "ITG", ResourceType, 0, 0);
            JazzSelectOnDestinationNode jsodn
                = new JazzSelectOnDestinationNode(0, null, s);
            if (this.CaseCount > 0)
            {
                int j;
                Case c;
                Case[] cases = this.Cases;
                JazzSelectOnDestinationNode.Match match;
                JazzSelectOnDestinationNode.MatchList mList = jsodn.Matches;
                JazzChunk.ChunkReferenceList dgi;
                DecisionGraphNode[] targets;
                for (int i = 0; i < cases.Length; i++)
                {
                    c = cases[i];
                    match = new JazzSelectOnDestinationNode.Match(0, null);
                    match.StateIndex = c.Value == null
                        ? NullCRef : c.Value.ChunkReference;
                    dgi = match.DecisionGraphIndexes;
                    targets = c.Targets;
                    for (j = 0; j < targets.Length; j++)
                    {
                        dgi.Add(targets[i] == null
                            ? NullCRef : targets[i].ChunkReference);
                    }
                }
            }
            return new GenericRCOLResource.ChunkEntry(0, null, tgi, jsodn);
        }
    }
}
