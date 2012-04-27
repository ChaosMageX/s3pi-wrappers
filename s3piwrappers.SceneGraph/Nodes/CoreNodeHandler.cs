using System;

namespace s3piwrappers.SceneGraph.Nodes
{
    public class CoreNodeHandler : AResourceNodeHandler
    {
        public CoreNodeHandler()
        {
            base.Add(typeof(VisualProxyNode), new uint[] { VisualProxyNode.VPXY_TID });
            base.Add(typeof(ModelNode), new uint[] { 
                ModelNode.MODL_TID, 
                ModelNode.MLOD_TID });
        }
    }
}