using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3pi.GenericRCOLResource;

namespace s3piwrappers.SceneGraph
{
    public class DefaultNode : AResourceNode
    {
        protected List<TGIBlock> kindredRCOLChunkKeys = new List<TGIBlock>();
        protected List<RKContainer> rkContainers = new List<RKContainer>();

        public override bool SetRK(IResourceKey newKey, IResourceKey originalKey)
        {
            bool success = true;
            int i, count = this.kindredRCOLChunkKeys.Count;
            for (i = 0; i < count; i++)
            {
                this.kindredRCOLChunkKeys[i].Instance = newKey.Instance;
            }
            return success && base.SetRK(newKey, originalKey);
        }

        public override bool CommitChanges()
        {
            bool success = true;
            int i, count = this.rkContainers.Count;
            for (i = 0; i < count; i++)
            {
                if (!this.rkContainers[i].CommitChanges())
                {
                    success = false;
                    Diagnostics.Log("CommitChanges Failed for " +
                        this.rkContainers[i].AbsolutePath);
                }
            }
            return success && base.CommitChanges();
        }

        /// <summary>
        /// How a ChunkEntry's TGIBlock should be treated by the ResourceGraph
        /// when resource key re-numbering occurs.
        /// </summary>
        public enum ChunkEntryType
        {
            /// <summary>
            /// The ChunkEntry's TGIBlock should remain unchanged.
            /// Example: JAZZ graph chunks.
            /// </summary>
            Internal,
            /// <summary>
            /// The ChunkEntry's Instance ID should be changed
            /// to match the Instance ID of its containing resource.
            /// Example: MLODs in MODLs.
            /// </summary>
            Kindred, // Would this ever have to be expanded to make more complex changes?
            /// <summary>
            /// The ChunkEntry's TGIBlock is given a new unique Instance ID,
            /// as if it were a completely separate resource
            /// referenced by its containing resource.
            /// Example: MATDs in MODLs.
            /// </summary>
            Unique
        }

        protected virtual ChunkEntryType GetChunkType(TGIBlock chunkKey)
        {
            if (chunkKey.Instance == 0)
                return ChunkEntryType.Internal;
            else if (chunkKey.Instance == this.originalKey.Instance)
                return ChunkEntryType.Kindred;
            else
                return ChunkEntryType.Unique;
        }

        public virtual string GetContentPathRootName()
        {
            try
            {
                string key = string.Concat("0x", base.originalKey.ResourceType.ToString("X8"));
                if (s3pi.Extensions.ExtList.Ext.ContainsKey(key))
                    return s3pi.Extensions.ExtList.Ext[key][0].ToLowerInvariant();
                else
                    return "root";
            }
            catch
            {
                return "root";
            }
        }

        protected bool includeDDSes = true;
        protected virtual bool ICanSlurpRK(IResourceKey key)
        {
            return this.includeDDSes || !ResourceGraph.IsDDS(key.ResourceType);
        }

        protected List<IResourceConnection> SlurpAllRKs()
        {
            List<IResourceConnection> results = new List<IResourceConnection>();
            string rootStr = this.GetContentPathRootName();
            if (!RKContainer.IsLegalFieldName(rootStr))
            {
                Diagnostics.Log("Illegal Root Field Name for "
                    + ResourceGraph.PrintRK(base.originalKey) + ":" + rootStr);
                rootStr = "root";
            }
            if (base.resource is GenericRCOLResource)
            {
                Diagnostics.Log("Slurping RCOL Resource RKs for "
                    + rootStr + ":" + ResourceGraph.PrintRK(base.originalKey));
                GenericRCOLResource rcol = base.resource as GenericRCOLResource;
                TGIBlock tgiBlock;
                int i, count = rcol.Resources.Count;
                for (i = 0; i < count; i++)
                {
                    tgiBlock = rcol.Resources[i];
                    if (includeDDSes || !ResourceGraph.IsDDS(tgiBlock.ResourceType))
                        results.Add(new DefaultConnection(tgiBlock, tgiBlock, 
                            ResourceDataActions.FindWrite, rootStr + ".Resources[" + i + "]"));
                }
                Diagnostics.Log("Slurping RCOL ChunkEntry RKs for "
                    + rootStr + ":" + ResourceGraph.PrintRK(base.originalKey));
                string absolutePath;
                GenericRCOLResource.ChunkEntry chunk;
                count = rcol.ChunkEntries.Count;
                for (i = 0; i < count; i++)
                {
                    absolutePath = rootStr + ".ChunkEntries[" + i + "].RCOLBlock";
                    chunk = rcol.ChunkEntries[i];
                    tgiBlock = chunk.TGIBlock;
                    switch (GetChunkType(tgiBlock))
                    {
                        case ChunkEntryType.Kindred:
                            this.kindredRCOLChunkKeys.Add(tgiBlock);
                            results.AddRange(RKContainer.SlurpRKsFromField(absolutePath,
                                chunk.RCOLBlock, this.rkContainers, this.ICanSlurpRK));
                            break;
                        case ChunkEntryType.Internal:
                            results.AddRange(RKContainer.SlurpRKsFromField(absolutePath,
                                chunk.RCOLBlock, this.rkContainers, this.ICanSlurpRK));
                            break;
                        case ChunkEntryType.Unique:
                            results.Add(new DefaultConnection(tgiBlock,
                                chunk.RCOLBlock, ResourceDataActions.None, absolutePath));
                            break;
                    }
                }
            }
            else if (base.resource is AApiVersionedFields)
            {
                Diagnostics.Log("Slurping RKs for "
                    + rootStr + ":" + ResourceGraph.PrintRK(base.originalKey));
                results.AddRange(RKContainer.SlurpRKsFromField(rootStr,
                    base.resource as AApiVersionedFields,
                    this.rkContainers, this.ICanSlurpRK));
            }
            return results;
        }

        public override List<IResourceConnection> SlurpConnections(object constraints)
        {
            return SlurpAllRKs();
        }

        protected virtual uint[] GetKindredResourceTypes(out string[] kinNames)
        {
            kinNames = null;
            return null;
        }

        public override List<IResourceKinHelper> CreateKinHelpers(object constraints)
        {
            string[] kinNames;
            uint[] kin = this.GetKindredResourceTypes(out kinNames);
            if (kin == null || kin.Length == 0)
                return base.CreateKinHelpers(constraints);
            int i, count = kin.Length;
            bool hasNames = (kinNames != null && kinNames.Length >= count);
            List<IResourceKinHelper> results = new List<IResourceKinHelper>(count);
            for (i = 0; i < count; i++)
            {
                results.Add(new DefaultKinHelper(kin[i], hasNames ? kinNames[i] : null));
            }
            return results;
        }

        public DefaultNode(IResource resource, IResourceKey originalKey)
            : base(resource, originalKey)
        {
        }
    }
}
