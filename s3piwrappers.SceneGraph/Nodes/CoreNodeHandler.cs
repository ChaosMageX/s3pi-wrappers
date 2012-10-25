namespace s3piwrappers.SceneGraph.Nodes
{
    public class CoreNodeHandler : AResourceNodeHandler
    {
        public CoreNodeHandler()
        {
            base.Add(typeof (VisualProxyNode), new[] {VisualProxyNode.VPXY_TID});
            base.Add(typeof (ModelNode), new[]
                {
                    ModelNode.MODL_TID,
                    ModelNode.MLOD_TID
                });
            base.Add(typeof (GeometryNode), new[] {GeometryNode.GEOM_TID});
            base.Add(typeof (SpeedTreeNode), new[]
                {
                    SpeedTreeNode._SPT_TID,
                    SpeedTreeNode.SPT2_TID
                });
            //base.Add(typeof(CatalogResourceNode), Enum.GetValues(typeof(CatalogType)) as uint[]);
            base.Add(typeof (CatalogNode), CatalogNode.thumbAndModularTIDs);
            base.Add(typeof (CatalogBrushNode), CatalogBrushNode.brushTIDs);
            base.Add(typeof (CatalogObjectNode), CatalogObjectNode.objectTIDs);
            base.Add(typeof (CASPartNode), new[] {CASPartNode.CASP_TID});
        }
    }
}
