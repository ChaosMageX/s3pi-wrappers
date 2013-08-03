using System;
using s3piwrappers.SceneGraph.Managers;

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
            base.Add(typeof(GeometryNode), new uint[] { GeometryNode.GEOM_TID });
            base.Add(typeof(SpeedTreeNode), new uint[] { 
                SpeedTreeNode._SPT_TID, 
                SpeedTreeNode.SPT2_TID });
            //base.Add(typeof(CatalogResourceNode), Enum.GetValues(typeof(CatalogType)) as uint[]);
            base.Add(typeof(CatalogNode), CatalogNode.thumbAndModularTIDs);
            base.Add(typeof(CatalogBrushNode), CatalogBrushNode.brushTIDs);
            base.Add(typeof(CatalogObjectNode), CatalogObjectNode.objectTIDs);
            base.Add(typeof(CASPartNode), new uint[] { CASPartNode.CASP_TID });
        }
    }
}