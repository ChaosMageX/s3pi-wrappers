using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.WrapperDealer;
using s3piwrappers.FreeformJazz.Widgets;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;
using s3piwrappers.JazzGraph;
using GraphForms;
using GraphForms.Algorithms.Layout.ForceDirected;

namespace s3piwrappers.FreeformJazz
{
    public class JazzGraphContainer : INamedResourceIndexEntry
    {
        public int Index;
        public string Name;
        public JazzSaveState SaveState;

        public RK Key;
        public bool Comp;
        public JazzPackage JP;
        public IResourceIndexEntry RIE;
        public StateMachineScene Scene;

        public JazzGraphContainer(int index, JazzPackage jp,
            IResourceIndexEntry rie, Control view)
        {
            this.Index = index;
            this.SaveState = JazzSaveState.Saved;
            this.Key = new RK(rie);
            this.Comp = rie.Compressed == 0xFFFF;
            this.JP = jp;
            this.RIE = rie;
            IResource res = null;
            GenericRCOLResource rcol = null;
            try
            {
                res = WrapperDealer.GetResource(0, jp.Package, rie);
            }
            catch (Exception ex)
            {
                MainForm.ShowException(ex, 
                    "Could not load JAZZ resource: " + this.Key + "\n", 
                    MainForm.kName + ": Unable to load JAZZ resource");
            }
            if (res != null)
            {
                rcol = res as GenericRCOLResource;
                if (rcol != null)
                {
                    this.Scene = new StateMachineScene(
                        new StateMachine(rcol), view);
                    KKLayoutAlgorithm<StateNode, StateEdge> layout
                        = new KKLayoutAlgorithm<StateNode, StateEdge>(
                    this.Scene.StateGraph, this.Scene);
                    layout.LengthFactor = 1.25f;
                    this.Scene.Layout = layout;
                    layout.ShuffleNodes();
                    this.Scene.LayoutPaused = true;
                    this.Scene.StartLayout();
                }
                else
                {
                    this.Scene = null;
                }
            }
            if (!KeyNameReg.TryFindName(rie.Instance, out this.Name))
            {
                this.Name = "0x" + rie.Instance.ToString("X16");
            }
        }

        public JazzGraphContainer(int index, string name, Control view)
        {
            this.Index = index;
            this.Name = name ?? "";
            this.SaveState = JazzSaveState.Dirty;
            ulong iid = 0;
            if (!this.Name.StartsWith("0x") ||
                !ulong.TryParse(this.Name.Substring(2),
                    System.Globalization.NumberStyles.HexNumber,
                    null, out iid))
            {
                iid = FNVHash.HashString64(this.Name);
            }
            this.Key = new RK(GlobalManager.kJazzTID, 0, iid);
            this.Comp = true;
            this.JP = null;
            this.RIE = null;

            StateMachine sm = new StateMachine(name);
            this.Scene = new StateMachineScene(sm, view);
            KKLayoutAlgorithm<StateNode, StateEdge> layout
                = new KKLayoutAlgorithm<StateNode, StateEdge>(
                    this.Scene.StateGraph, this.Scene);
            layout.LengthFactor = 1.25f;
            this.Scene.Layout = layout;
            layout.ShuffleNodes();
            this.Scene.LayoutPaused = true;
            this.Scene.StartLayout();
        }

        public JazzGraphContainer(int index,
            StateMachine sm, Control view)
        {
            this.Index = index;
            this.Name = sm.Name;
            this.SaveState = JazzSaveState.Dirty;
            this.Key = new RK(GlobalManager.kJazzTID, 0,
                FNVHash.HashString64(this.Name));
            this.Comp = true;
            this.JP = null;
            this.RIE = null;

            this.Scene = new StateMachineScene(sm, view);
            KKLayoutAlgorithm<StateNode, StateEdge> layout
                = new KKLayoutAlgorithm<StateNode, StateEdge>(
                    this.Scene.StateGraph, this.Scene);
            layout.LengthFactor = 1.25f;
            this.Scene.Layout = layout;
            layout.ShuffleNodes();
            this.Scene.LayoutPaused = true;
            this.Scene.StartLayout();
        }

        public bool NameIsValid()
        {
            ulong iid;
            return this.Name != null && (!this.Name.StartsWith("0x") ||
                !ulong.TryParse(this.Name.Substring(2),
                    System.Globalization.NumberStyles.HexNumber,
                    null, out iid));
        }

        public void OnTabSizeChanged(object sender, EventArgs e)
        {
            TabPage graphTab = sender as TabPage;
            if (graphTab != null)
            {
                bool needW = false;
                bool needH = false;
                RectangleF bbox;
                Size tabSize = graphTab.Size;
                Size viewSize = this.Scene.StateView.Size;
                if (viewSize.Width > tabSize.Width)
                {
                    needW = true;
                }
                else
                {
                    viewSize.Width = tabSize.Width;
                }
                if (viewSize.Height > tabSize.Height)
                {
                    needH = true;
                }
                else
                {
                    viewSize.Height = tabSize.Height;
                }
                if (needW || needH)
                {
                    int i;
                    // Can not just use this.Scene.ChildrenBoundingBox()
                    // because this.Scene.BoundingBox has to be excluded,
                    // since it is set to match the size of its view.
                    bbox = new RectangleF(0, 0, 1, 1);
                    if (this.Scene.HasChildren)
                    {
                        RectangleF cbbox;
                        GraphElement child;
                        GraphElement[] children = this.Scene.Children;
                        for (i = children.Length - 1; i >= 0; i--)
                        {
                            child = children[i];
                            cbbox = child.ChildrenBoundingBox();
                            cbbox.Offset(child.X, child.Y);
                            bbox = RectangleF.Union(bbox, cbbox);
                        }
                    }
                    i = (int)Math.Ceiling(bbox.Width);
                    if (needW && viewSize.Width > i)
                    {
                        viewSize.Width = i;
                    }
                    i = (int)Math.Ceiling(bbox.Height);
                    if (needH && viewSize.Height > i)
                    {
                        viewSize.Height = i;
                    }
                }
                this.Scene.StateView.Size = viewSize;
                bbox = this.Scene.BoundingBox;
                bbox.Width = viewSize.Width;
                bbox.Height = viewSize.Height;
                this.Scene.SetBoundingBox(bbox);
            }
        }

        string INamedResourceIndexEntry.ResourceName
        {
            get { return this.Name; }
            set
            {
                if (this.Name == null && value != null)
                {
                    this.Name = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
                else if (!this.Name.Equals(value))
                {
                    this.Name = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        #region IResourceIndexEntry Members

        uint IResourceIndexEntry.Chunkoffset
        {
            get { return this.RIE == null ? 0 : this.RIE.Chunkoffset; }
            set
            {
                if (this.RIE != null && this.RIE.Chunkoffset != value)
                {
                    this.RIE.Chunkoffset = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        ushort IResourceIndexEntry.Compressed
        {
            get { return (ushort)(this.Comp ? 0xFFFF : 0x0000); }
            set
            {
                bool comp = value == 0xFFFF;
                if (this.Comp != comp)
                {
                    this.Comp = comp;
                    if (this.RIE != null)
                    {
                        this.RIE.Compressed = value;
                    }
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        uint IResourceIndexEntry.Filesize
        {
            get { return this.RIE == null ? 0 : this.RIE.Filesize; }
            set
            {
                if (this.RIE != null && this.RIE.Filesize != value)
                {
                    this.RIE.Filesize = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        bool IResourceIndexEntry.IsDeleted
        {
            get { return this.RIE == null ? false : this.RIE.IsDeleted; }
            set
            {
                if (this.RIE != null && this.RIE.IsDeleted != value)
                {
                    this.RIE.IsDeleted = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        uint IResourceIndexEntry.Memsize
        {
            get { return this.RIE == null ? 0 : this.RIE.Memsize; }
            set
            {
                if (this.RIE != null && this.RIE.Memsize != value)
                {
                    this.RIE.Memsize = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        System.IO.Stream IResourceIndexEntry.Stream
        {
            get { return this.RIE == null ? null : this.RIE.Stream; }
        }

        ushort IResourceIndexEntry.Unknown2
        {
            get
            {
                return this.RIE == null ? (ushort)0 : this.RIE.Unknown2;
            }
            set
            {
                if (this.RIE != null && this.RIE.Unknown2 != value)
                {
                    this.RIE.Unknown2 = value;
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        bool IEquatable<IResourceIndexEntry>.Equals(
            IResourceIndexEntry other)
        {
            if (this.RIE == null)
            {
                return false;
            }
            return this.RIE.Equals(other);
        }
        #endregion

        #region AApiVersionFields

        int IApiVersion.RecommendedApiVersion
        {
            get { return 0; }
        }

        int IApiVersion.RequestedApiVersion
        {
            get { return 0; }
        }

        List<string> IContentFields.ContentFields
        {
            get
            {
                if (this.RIE == null)
                {
                    throw new NotImplementedException();
                }
                return this.RIE.ContentFields;
            }
        }

        TypedValue IContentFields.this[string index]
        {
            get
            {
                if (this.RIE == null)
                {
                    throw new NotImplementedException();
                }
                return this.RIE[index];
            }
            set
            {
                if (this.RIE == null)
                {
                    throw new NotImplementedException();
                }
                this.RIE[index] = value;
            }
        }
        #endregion

        #region IResourceKey Members

        ulong IResourceKey.Instance
        {
            get { return this.Key.IID; }
            set
            {
                if (this.Key.IID != value)
                {
                    this.Key.IID = value;
                    if (this.RIE != null)
                    {
                        this.RIE.Instance = value;
                    }
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        uint IResourceKey.ResourceGroup
        {
            get { return this.Key.GID; }
            set
            {
                if (this.Key.GID != value)
                {
                    this.Key.GID = value;
                    if (this.RIE != null)
                    {
                        this.RIE.ResourceGroup = value;
                    }
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        uint IResourceKey.ResourceType
        {
            get { return this.Key.TID; }
            set
            {
                if (this.Key.TID != value)
                {
                    this.Key.TID = value;
                    if (this.RIE != null)
                    {
                        this.RIE.ResourceType = value;
                    }
                    this.SaveState = JazzSaveState.Dirty;
                }
            }
        }

        bool IEqualityComparer<IResourceKey>.Equals(
            IResourceKey x, IResourceKey y)
        {
            return x.Equals(y);
        }

        int IEqualityComparer<IResourceKey>.GetHashCode(
            IResourceKey obj)
        {
            return obj.GetHashCode();
        }

        bool IEquatable<IResourceKey>.Equals(IResourceKey other)
        {
            return this.Key.Equals(other);
        }

        int IComparable<IResourceKey>.CompareTo(IResourceKey other)
        {
            return this.Key.CompareTo(other);
        }
        #endregion
    }
}
