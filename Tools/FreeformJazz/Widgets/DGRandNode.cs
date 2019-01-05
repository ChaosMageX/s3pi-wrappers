using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using s3pi.GenericRCOLResource;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;
using GraphForms.Algorithms;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class DGRandNode : DGNode
    {
        private RandomNode mRandomNode;
        private SliceList mSlices;

        private GraphicsPath mBorderPath;

        private AnchorPoint[] mSliceAnchors;
        private string[] mSliceStrings;
        private int mSliceCount;
        private float mMGH;

        public DGRandNode(RandomNode rand, StateNode state)
            : base(rand, state)
        {
            if (rand == null)
            {
                throw new ArgumentNullException("rand");
            }
            this.mRandomNode = rand;
            this.mSlices = new SliceList(this);

            this.mSliceAnchors = new AnchorPoint[0];
            this.mSliceStrings = new string[0];
            this.mSliceCount = 0;
            this.mMGH = 7;

            this.UpdateVisualization();
        }

        public class SliceList : IList<float>, 
            s3piwrappers.Helpers.Collections.IHasGenericAdd,
            s3piwrappers.Helpers.Collections.IHasGenericInsert
        {
            private DGRandNode mNode;
            private List<RandomNode.Slice> mSlices;

            public SliceList(DGRandNode node)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("node");
                }
                this.mNode = node;
                this.mSlices = node.mRandomNode.Slices;
            }

            public bool Add()
            {
                this.mSlices.Add(new RandomNode.Slice());
                this.mNode.UpdateVisualization();
                return true;
            }

            public bool Insert(int index)
            {
                if (index < 0 || index > this.mSlices.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.InsertInternal(index, 0);
                return true;
            }

            public int IndexOf(float item)
            {
                for (int i = 0; i < this.mSlices.Count; i++)
                {
                    if (this.mSlices[i].Weight == item)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Insert(int index, float item)
            {
                if (index < 0 || index > this.mSlices.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.InsertInternal(index, item);
            }

            private void InsertInternal(int index, float item)
            {
                this.mSlices.Insert(index, new RandomNode.Slice(item));
                AnchorPoint[] aps = this.mNode.mSliceAnchors;
                if (aps.Length <= this.mNode.mSliceCount)
                {
                    if (this.mNode.mSliceCount == 0)
                    {
                        aps = new AnchorPoint[4];
                    }
                    else
                    {
                        aps = new AnchorPoint[2 * this.mNode.mSliceCount];
                        if (index > 0)
                        {
                            Array.Copy(this.mNode.mSliceAnchors, 
                                0, aps, 0, index);
                        }
                        Array.Copy(this.mNode.mSliceAnchors, index, aps, 
                            index + 1, this.mNode.mSliceCount - index);
                    }
                    this.mNode.mSliceAnchors = aps;
                }
                else if (this.mNode.mSliceCount > 0)
                {
                    Array.Copy(aps, index, aps, index + 1, 
                        this.mNode.mSliceCount - index);
                }
                this.mNode.UpdateVisualization();
            }

            public void RemoveAt(int index)
            {
                if (index < 0 || index >= this.mSlices.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                this.RemoveAtInternal(index);
                this.mNode.mScene.Container.UndoRedo.Submit(
                    new RemoveSliceCommand(this, index));
            }

            private class RemoveSliceCommand : Command
            {
                private DGRandNode mNode;
                private List<RandomNode.Slice> mSliceList;
                private int mIndex;
                private AnchorPoint mAP;
                private RandomNode.Slice mSlice;
                private List<DGEdge> mSelectedEdges;
                private IEdgeAction[] mActions;
                private int mCount;

                public RemoveSliceCommand(SliceList list, int index)
                {
                    this.mNode = list.mNode;
                    this.mSliceList = list.mSlices;
                    this.mIndex = index;
                    this.mAP = list.mNode.mSliceAnchors[index];
                    this.mSlice = list.mSlices[index];
                    this.mSelectedEdges = new List<DGEdge>();
                    this.mActions = new IEdgeAction[this.mAP.Edges.Count];
                    this.mCount = 0;
                    DGNode node;
                    IEdgeAction action;
                    foreach (DGEdge edge in this.mAP.Edges)
                    {
                        if (edge.Selected)
                        {
                            this.mSelectedEdges.Add(edge);
                        }
                        node = edge.DstNode;
                        if (node == this.mNode)
                        {
                            node = edge.SrcNode;
                        }
                        action = node.RemoveEdge(edge);
                        if (action != null)
                        {
                            this.mActions[this.mCount++] = action;
                        }
                    }
                    this.mLabel = "Remove Slice from Random Node";
                }

                public override bool Execute()
                {
                    int i;
                    DGEdge edge;
                    StateNode fState = this.mNode.mScene.FocusedState;
                    if (fState != null && fState.Equals(this.mNode.mState))
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mSelectedEdges[i];
                            this.mNode.mScene.SelectDGEdge(edge, false);
                        }
                    }
                    Digraph<DGNode, DGEdge> dg 
                        = this.mNode.mState.DecisionGraph;
                    for (i = this.mAP.Edges.Count - 1; i >= 0; i--)
                    {
                        edge = this.mAP.Edges[i];
                        dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                        edge.SetParent(null);
                    }
                    for (i = 0; i < this.mCount; i++)
                    {
                        this.mActions[i].Redo();
                    }
                    this.mNode.mState.AddRootDGEdges();

                    this.mSliceList.RemoveAt(this.mIndex);
                    this.mNode.mSliceCount--;
                    Array.Copy(
                        this.mNode.mSliceAnchors, this.mIndex + 1,
                        this.mNode.mSliceAnchors, this.mIndex,
                        this.mNode.mSliceCount - this.mIndex);
                    this.mAP.SetParent(null);
                    this.mNode.UpdateVisualization();
                    return true;
                }

                public override void Undo()
                {
                    int i;
                    DGEdge edge;
                    Digraph<DGNode, DGEdge> dg
                        = this.mNode.mState.DecisionGraph;
                    this.mSliceList.Insert(this.mIndex, this.mSlice);
                    Array.Copy(
                        this.mNode.mSliceAnchors, this.mIndex,
                        this.mNode.mSliceAnchors, this.mIndex + 1,
                        this.mNode.mSliceCount - this.mIndex);
                    this.mNode.mSliceAnchors[this.mIndex] = this.mAP;
                    this.mNode.mSliceCount++;
                    this.mAP.SetParent(this.mNode);
                    this.mNode.UpdateVisualization();

                    for (i = 0; i < this.mCount; i++)
                    {
                        this.mActions[i].Undo();
                    }
                    for (i = this.mAP.Edges.Count - 1; i >= 0; i--)
                    {
                        edge = this.mAP.Edges[i];
                        edge.SetParent(this.mNode.mState);
                        dg.AddEdge(edge);
                    }
                    this.mNode.mState.RemoveRootDGEdges();

                    StateNode fState = this.mNode.mScene.FocusedState;
                    if (fState != null && fState.Equals(this.mNode.mState))
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mSelectedEdges[i];
                            this.mNode.mScene.SelectDGEdge(edge, false);
                        }
                    }
                }
            }

            private class ClearSlicesCommand : Command
            {
                private DGRandNode mNode;
                private List<RandomNode.Slice> mSliceList;
                private AnchorPoint[] mAPs;
                private RandomNode.Slice[] mSlices;
                private List<DGEdge> mSelectedEdges;
                private List<IEdgeAction> mActions;

                public ClearSlicesCommand(SliceList list)
                {
                    this.mNode = list.mNode;
                    this.mSliceList = list.mSlices;
                    this.mAPs = new AnchorPoint[list.mNode.mSliceCount];
                    Array.Copy(list.mNode.mSliceAnchors, 0, this.mAPs, 0, 
                               list.mNode.mSliceCount);
                    this.mSlices = list.mSlices.ToArray();
                    this.mSelectedEdges = new List<DGEdge>();
                    this.mActions = new List<IEdgeAction>();
                    DGNode node;
                    IEdgeAction action;
                    for (int i = this.mAPs.Length - 1; i >= 0; i--)
                    {
                        foreach (DGEdge edge in this.mAPs[i].Edges)
                        {
                            if (edge.Selected)
                            {
                                this.mSelectedEdges.Add(edge);
                            }
                            node = edge.DstNode;
                            if (node == this.mNode)
                            {
                                node = edge.SrcNode;
                            }
                            action = node.RemoveEdge(edge);
                            if (action != null)
                            {
                                this.mActions.Add(action);
                            }
                        }
                    }
                    this.mLabel = "Clear Slices from Random Node";
                }

                public override bool Execute()
                {
                    int i, j;
                    DGEdge edge;
                    List<DGEdge> edges;
                    Digraph<DGNode, DGEdge> dg
                        = this.mNode.mState.DecisionGraph;
                    StateNode fState = this.mNode.mScene.FocusedState;
                    if (fState != null && fState.Equals(this.mNode.mState))
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mSelectedEdges[i];
                            this.mNode.mScene.SelectDGEdge(edge, false);
                        }
                    }

                    for (i = this.mAPs.Length - 1; i >= 0; i--)
                    {
                        edges = this.mAPs[i].Edges;
                        for (j = edges.Count - 1; j >= 0; j--)
                        {
                            edge = edges[j];
                            dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                            edge.SetParent(null);
                        }
                    }
                    foreach (IEdgeAction action in this.mActions)
                    {
                        action.Redo();
                    }
                    this.mNode.mState.AddRootDGEdges();

                    this.mSliceList.Clear();
                    Array.Clear(this.mNode.mSliceAnchors, 0, 
                                this.mNode.mSliceCount);
                    this.mNode.mSliceCount = 0;
                    for (i = this.mAPs.Length - 1; i >= 0; i--)
                    {
                        this.mAPs[i].SetParent(null);
                    }
                    this.mNode.UpdateVisualization();
                    return true;
                }

                public override void Undo()
                {
                    int i, j;
                    DGEdge edge;
                    List<DGEdge> edges;
                    Digraph<DGNode, DGEdge> dg
                        = this.mNode.mState.DecisionGraph;
                    this.mSliceList.AddRange(this.mSlices);
                    Array.Copy(this.mAPs, 0, this.mNode.mSliceAnchors, 0, 
                               this.mAPs.Length);
                    this.mNode.mSliceCount = this.mAPs.Length;
                    for (i = this.mAPs.Length - 1; i >= 0; i--)
                    {
                        this.mAPs[i].SetParent(this.mNode);
                    }
                    this.mNode.UpdateVisualization();
                    
                    foreach (IEdgeAction action in this.mActions)
                    {
                        action.Undo();
                    }
                    for (i = this.mAPs.Length - 1; i >= 0; i--)
                    {
                        edges = this.mAPs[i].Edges;
                        for (j = edges.Count - 1; j >= 0; j--)
                        {
                            edge = edges[j];
                            edge.SetParent(this.mNode.mState);
                            dg.AddEdge(edge);
                        }
                    }
                    this.mNode.mState.RemoveRootDGEdges();

                    StateNode fState = this.mNode.mScene.FocusedState;
                    if (fState != null && fState.Equals(this.mNode.mState))
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mSelectedEdges[i];
                            this.mNode.mScene.SelectDGEdge(edge, false);
                        }
                    }
                }
            }

            private void RemoveAtInternal(int index)
            {
                this.mSlices.RemoveAt(index);
                DGNode node;
                AnchorPoint ap;
                AnchorPoint[] aps = this.mNode.mSliceAnchors;
                List<DGEdge> edges = aps[index].Edges;
                Digraph<DGNode, DGEdge> dg = this.mNode.mState.DecisionGraph;
                foreach (DGEdge edge in edges)
                {
                    node = edge.DstNode;
                    if (node == this.mNode)
                    {
                        node = edge.SrcNode;
                    }
                    node.RemoveEdge(edge);
                    if (edge.Selected)
                    {
                        this.mNode.mScene.SelectDGEdge(edge, false);
                    }
                    edge.SetParent(null);
                    dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                }
                edges.Clear();
                ap = aps[index];
                this.mNode.mSliceCount--;
                Array.Copy(aps, index + 1, aps, index, 
                    this.mNode.mSliceCount - index);
                ap.SetParent(null);
                this.mNode.UpdateVisualization();
                this.mNode.mState.ConnectOrphanDGNodes();
            }

            public float this[int index]
            {
                get { return this.mSlices[index].Weight; }
                set { this.mSlices[index].Weight = value; }
            }

            public void Add(float item)
            {
                this.mSlices.Add(new RandomNode.Slice(item));
                this.mNode.UpdateVisualization();
            }

            public void Clear()
            {
                this.ClearInternal();
            }

            private void ClearInternal()
            {
                if (this.mSlices.Count > 0)
                {
                    int i, j;
                    DGEdge edge;
                    DGNode node;
                    AnchorPoint ap;
                    AnchorPoint[] aps = this.mNode.mSliceAnchors;
                    List<DGEdge> edges;
                    Digraph<DGNode, DGEdge> dg
                        = this.mNode.mState.DecisionGraph;
                    for (i = this.mNode.mSliceCount - 1; i >= 0; i--)
                    {
                        edges = aps[i].Edges;
                        for (j = edges.Count - 1; j >= 0; j--)
                        {
                            edge = edges[j];
                            node = edge.DstNode;
                            if (node == this.mNode)
                            {
                                node = edge.SrcNode;
                            }
                            node.RemoveEdge(edge);
                            if (edge.Selected)
                            {
                                this.mNode.mScene.SelectDGEdge(edge, false);
                            }
                            edge.SetParent(null);
                            dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                        }
                        edges.Clear();
                        ap = aps[i];
                        ap.SetParent(null);
                    }
                    Array.Clear(aps, 0, this.mNode.mSliceCount);
                    this.mNode.mSliceCount = 0;
                    this.mNode.UpdateVisualization();
                    this.mNode.mState.ConnectOrphanDGNodes();
                }
            }

            public bool Contains(float item)
            {
                for (int i = this.mSlices.Count - 1; i >= 0; i--)
                {
                    if (this.mSlices[i].Weight == item)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void CopyTo(float[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (arrayIndex < 0 || arrayIndex >= array.Length)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }
                if (array.Length - arrayIndex > this.mSlices.Count)
                {
                    throw new ArgumentException("Invalid Offset Length");
                }
                foreach (RandomNode.Slice slice in this.mSlices)
                {
                    array[arrayIndex++] = slice.Weight;
                }
            }

            public int Count
            {
                get { return this.mSlices.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(float item)
            {
                int index = this.IndexOf(item);
                if (index < 0)
                {
                    return false;
                }
                this.RemoveAtInternal(index);
                return true;
            }

            private class Enumerator : IEnumerator<float>
            {
                private SliceList mList;
                private int mIndex;
                private float mCurrent;

                public Enumerator(SliceList list)
                {
                    this.mList = list;
                    this.mIndex = 0;
                    this.mCurrent = 0;
                }

                public float Current
                {
                    get { return this.mCurrent; }
                }

                public void Dispose()
                {
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return this.mCurrent; }
                }

                public bool MoveNext()
                {
                    if (this.mIndex < this.mList.mSlices.Count)
                    {
                        this.mCurrent 
                            = this.mList.mSlices[this.mIndex].Weight;
                        this.mIndex++;
                        return true;
                    }
                    this.mCurrent = 0;
                    return false;
                }

                public void Reset()
                {
                    this.mIndex = 0;
                    this.mCurrent = 0;
                }
            }

            public IEnumerator<float> GetEnumerator()
            {
                return new Enumerator(this);
            }

            System.Collections.IEnumerator 
                System.Collections.IEnumerable.GetEnumerator()
            {
                return new Enumerator(this);
            }
        }

        public SliceList Slices
        {
            get { return this.mSlices; }
            set
            {
                if (this.mSlices != value)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private class FlagsCommand : PropertyCommand<DGRandNode, JazzRandomNode.Flags>
        {
            public FlagsCommand(DGRandNode dgrn, JazzRandomNode.Flags newValue)
                : base(dgrn, "Flags", newValue, false)
            {
                this.mLabel = "Set Random Node Flags";
            }
        }

        private void CreateFlagsCommand(object value)
        {
            JazzRandomNode.Flags flags = (JazzRandomNode.Flags)value;
            this.mScene.Container.UndoRedo.Submit(new FlagsCommand(this, flags));
        }

        [Undoable("CreateFlagsCommand")]
        public JazzRandomNode.Flags Flags
        {
            get { return this.mRandomNode.Flags; }
            set
            {
                if (this.mRandomNode.Flags != value)
                {
                    this.mRandomNode.Flags = value;
                    this.UpdateVisualization();
                }
            }
        }

        #region Visualization

        private static Font sTextFont 
            = new Font(FontFamily.GenericSansSerif, 5);

        public static Font TextFont
        {
            get { return sTextFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Font");
                }
                if (!sTextFont.Equals(value))
                {
                    sTextFont = value;
                }
            }
        }

        public override void UpdateVisualization()
        {
            if (this.mScene.StateView == null)
            {
                return;
            }
            SizeF size;
            float h, w = 10;
            Graphics g = this.mScene.StateView.CreateGraphics();
            this.mSliceCount = this.mRandomNode.Slices.Count;
            this.mMGH = 7;
            if (this.mSliceCount == 0)
            {
                h = 15;
            }
            else
            {
                int i;
                AnchorPoint ap;
                string sliceString;
                List<RandomNode.Slice> slices = this.mRandomNode.Slices;
                if (this.mSliceAnchors.Length < this.mSliceCount)
                {
                    AnchorPoint[] sAnchors 
                        = new AnchorPoint[this.mSliceCount];
                    Array.Copy(this.mSliceAnchors, 0, sAnchors, 0,
                        this.mSliceAnchors.Length);
                    this.mSliceAnchors = sAnchors;
                    this.mSliceStrings = new string[this.mSliceCount];
                }
                for (i = 0; i < this.mSliceCount; i++)
                {
                    sliceString = slices[i].Weight.ToString();
                    size = g.MeasureString(sliceString, sTextFont);
                    w = Math.Max(w, size.Width + 5);
                    this.mMGH = Math.Max(this.mMGH, size.Height);
                    this.mSliceStrings[i] = sliceString;
                }
                if (this.mSliceCount == 1)
                {
                    h = this.mMGH + 5;
                    this.mSliceStrings[0] 
                        = slices[0].Weight.ToString();
                    if (this.mSliceAnchors[0] == null)
                    {
                        ap = new AnchorPoint(this, 1, 0);
                        ap.SetPosition(w, h / 2);
                        this.mSliceAnchors[0] = ap;
                    }
                }
                else
                {
                    this.mSliceStrings[0] 
                        = slices[0].Weight.ToString();
                    ap = this.mSliceAnchors[0];
                    if (ap == null)
                    {
                        ap = new AnchorPoint(this, 0, -1);
                        this.mSliceAnchors[0] = ap;
                    }
                    else
                    {
                        ap.SetDirection(0, -1);
                    }
                    ap.SetPosition(w / 2, 0);

                    h = this.mMGH * this.mSliceCount + 5;
                    this.mSliceCount--;
                    this.mSliceStrings[this.mSliceCount] 
                        = slices[this.mSliceCount].Weight.ToString();
                    ap = this.mSliceAnchors[this.mSliceCount];
                    if (ap == null)
                    {
                        ap = new AnchorPoint(this, 0, 1);
                        this.mSliceAnchors[this.mSliceCount] = ap;
                    }
                    else
                    {
                        ap.SetDirection(0, 1);
                    }
                    ap.SetPosition(w / 2, h);

                    h = this.mMGH + 2.5f;
                    for (i = 1; i < this.mSliceCount; i++)
                    {
                        this.mSliceStrings[i] 
                            = slices[i].Weight.ToString();
                        ap = this.mSliceAnchors[i];
                        if (ap == null)
                        {
                            ap = new AnchorPoint(this, 1, 0);
                            this.mSliceAnchors[i] = ap;
                        }
                        else
                        {
                            ap.SetDirection(1, 0);
                        }
                        h += this.mMGH / 2;
                        ap.SetPosition(w, h);
                        h += this.mMGH / 2;
                    }
                    this.mSliceCount++;
                    h = this.mMGH * this.mSliceCount + 5;
                }
            }

            this.mEntryAnchor.SetPosition(0, h / 2);

            if (this.mBorderPath == null)
            {
                this.mBorderPath = new GraphicsPath();
            }
            this.mBorderPath.Reset();
            AddRoundedRectangle(this.mBorderPath, 0, 0, w, h, 5);

            float bbp = 2 * AnchorPoint.Radius;
            this.BoundingBox = new RectangleF(
                -bbp, -bbp, w + 2 * bbp, h + 2 * bbp);
        }

        public AnchorPoint GetSliceAnchor(int index)
        {
            return this.mSliceAnchors[index];
        }

        public override AnchorPoint GetAnchorFor(DGEdge edge)
        {
            if (this.mEntryAnchor.Edges.Contains(edge))
            {
                return this.mEntryAnchor;
            }
            AnchorPoint ap;
            for (int i = this.mSliceCount - 1; i >= 0; i--)
            {
                ap = this.mSliceAnchors[i];
                if (ap.Edges.Contains(edge))
                {
                    return ap;
                }
            }
            return null;
        }

        private class SliceTargetAction : IEdgeAction
        {
            private bool bAdd;
            private DGEdge mEdge;
            private int mSliceIndex;
            private int mTargetIndex;
            private DGRandNode mNode;

            public SliceTargetAction(bool add, DGEdge edge, 
                int sliceIndex, int targetIndex, DGRandNode node)
            {
                this.bAdd = add;
                this.mEdge = edge;
                this.mSliceIndex = sliceIndex;
                this.mTargetIndex = targetIndex;
                this.mNode = node;
            }

            public void Redo()
            {
                AnchorPoint ap = this.mNode.mSliceAnchors[this.mSliceIndex];
                RandomNode.Slice slice 
                    = this.mNode.mRandomNode.Slices[this.mSliceIndex];
                if (this.bAdd)
                {
                    ap.Edges.Insert(this.mTargetIndex, this.mEdge);
                    DGNode node = this.mEdge.DstNode;
                    if (node == this.mNode)
                    {
                        node = this.mEdge.SrcNode;
                    }
                    slice.Targets.Insert(this.mTargetIndex, node.DGN);
                }
                else
                {
                    ap.Edges.RemoveAt(this.mTargetIndex);
                    slice.Targets.RemoveAt(this.mTargetIndex);
                }
            }

            public void Undo()
            {
                AnchorPoint ap = this.mNode.mSliceAnchors[this.mSliceIndex];
                RandomNode.Slice slice
                    = this.mNode.mRandomNode.Slices[this.mSliceIndex];
                if (this.bAdd)
                {
                    ap.Edges.RemoveAt(this.mTargetIndex);
                    slice.Targets.RemoveAt(this.mTargetIndex);
                }
                else
                {
                    ap.Edges.Insert(this.mTargetIndex, this.mEdge);
                    DGNode node = this.mEdge.DstNode;
                    if (node == this.mNode)
                    {
                        node = this.mEdge.SrcNode;
                    }
                    slice.Targets.Insert(this.mTargetIndex, node.DGN);
                }
            }
        }

        public override IEdgeAction AddEdge(DGEdge edge, AnchorPoint ap)
        {
            if (edge == null || ap == null)
            {
                return null;
            }
            if (ap == this.mEntryAnchor)
            {
                return new EdgeAction(true, ap.Edges.Count, edge, ap);
            }
            for (int k = this.mSliceCount - 1; k >= 0; k--)
            {
                if (this.mSliceAnchors[k] == ap)
                {
                    return new SliceTargetAction(
                        true, edge, k, ap.Edges.Count, this);
                }
            }
            return null;
        }

        public override IEdgeAction RemoveEdge(DGEdge edge)
        {
            if (edge == null)
            {
                return null;
            }
            int i = this.mEntryAnchor.Edges.IndexOf(edge);
            if (i >= 0)
            {
                return new EdgeAction(false, i, edge, this.mEntryAnchor);
            }
            AnchorPoint ap;
            for (int k = this.mSliceCount - 1; k >= 0; k--)
            {
                ap = this.mSliceAnchors[k];
                i = ap.Edges.IndexOf(edge);
                if (i >= 0)
                {
                    return new SliceTargetAction(false, edge, k, i, this);
                }
            }
            return null;
        }

        public override Region Shape()
        {
            return new Region(this.mBorderPath);
        }

        private static readonly Pen sBorderPen = new Pen(Color.Black, 0.5f);

        private static SolidBrush sBGBrush
            = new SolidBrush(Color.Wheat);

        public static Color BackgroundColor
        {
            get { return sBGBrush.Color; }
            set
            {
                Color bgColor = sBGBrush.Color;
                if (!bgColor.Equals(value))
                {
                    sBGBrush.Color = value;
                }
            }
        }

        private static SolidBrush sTextBrush = new SolidBrush(Color.Black);

        public static Color TextColor
        {
            get { return sTextBrush.Color; }
            set
            {
                Color color = sTextBrush.Color;
                if (!color.Equals(value))
                {
                    sTextBrush.Color = value;
                }
            }
        }

        protected override void OnDrawBackground(
            System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillPath(sBGBrush, this.mBorderPath);
            sBorderPen.Color = this.Selected
                ? sBorderSelectColor : (this.InEditMode
                ? sBorderInEditColor : sBorderNormalColor);
            g.DrawPath(sBorderPen, this.mBorderPath);
            if (this.mSliceCount > 0)
            {
                float bbp = 4 * AnchorPoint.Radius + 5;
                string sliceString;
                RectangleF rect = this.BoundingBox;
                rect.X = 2.5f;
                rect.Y = 2.5f;
                rect.Width = rect.Width - bbp;
                rect.Height = this.mMGH;
                for (int i = 0; i < this.mSliceCount; i++)
                {
                    sliceString = this.mSliceStrings[i];
                    if (!string.IsNullOrEmpty(sliceString))
                    {
                        g.DrawString(sliceString, sTextFont, 
                            sTextBrush, rect, TextFormat);
                    }
                    rect.Y = rect.Y + this.mMGH;
                }
            }
        }

        #endregion
    }
}
