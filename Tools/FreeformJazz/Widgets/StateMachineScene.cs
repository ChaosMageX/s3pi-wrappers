using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using GraphForms;
using GraphForms.Algorithms;
using GraphForms.Algorithms.Layout;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class StateMachineScene : AGraphNodeScene, IClusterNode
    {
        private class StateGraphHider : GraphElement
        {
            public static SolidBrush sHiderBrush
                = new SolidBrush(Color.FromArgb(127, 0, 0, 0));

            private StateMachineScene mScene;

            private bool bVisible;

            public StateGraphHider(StateMachineScene scene)
            {
                this.mScene = scene;
                this.bVisible = false;
                this.IgnoreMouseEvents = true;
                this.Zvalue = 1;
                this.SetPosition(0, 0);
                this.SetParent(scene);
            }

            public bool Visible
            {
                get { return this.bVisible; }
                set
                {
                    if (this.bVisible != value)
                    {
                        this.IgnoreMouseEvents = !value;
                        this.bVisible = value;
                        this.Invalidate();
                    }
                }
            }

            public bool MouseDownInvoked = false;

            protected override bool OnMouseDown(GraphMouseEventArgs e)
            {
                if (!e.Handled)
                {
                    this.MouseDownInvoked = true;
                }
                return base.OnMouseDown(e);
            }

            protected override bool OnMouseDoubleClick(GraphMouseEventArgs e)
            {
                if (!e.Handled)
                {
                    this.mScene.SetFocusedState(null);
                }
                return base.OnMouseDoubleClick(e);
            }

            protected override bool IsDrawn()
            {
                return this.bVisible;
            }

            protected override void OnDrawBackground(PaintEventArgs e)
            {
                e.Graphics.FillRectangle(sHiderBrush, this.BoundingBox);
            }
        }

        private static Pen sLassoPen = new Pen(Color.Black, 4);

        public static Color GraphHiderColor
        {
            get { return StateGraphHider.sHiderBrush.Color; }
            set
            {
                Color color = StateGraphHider.sHiderBrush.Color;
                if (!color.Equals(value))
                {
                    StateGraphHider.sHiderBrush.Color = value;
                }
            }
        }

        public static Color LassoColor
        {
            get { return sLassoPen.Color; }
            set
            {
                Color color = sLassoPen.Color;
                if (!color.Equals(value))
                {
                    sLassoPen.Color = value;
                }
            }
        }

        static StateMachineScene()
        {
            sLassoPen.DashStyle = DashStyle.Dash;
        }

        private Control mStateView;
        private StateMachine mStateMachine;
        private StateGraphHider mStateGraphHider;
        private Digraph<StateNode, StateEdge> mStateGraph;

        private bool bPendingLayout;
        private LayoutAlgorithm<StateNode, StateEdge> mLayout;
        private LayoutAlgorithm<StateNode, StateEdge> mPendingLayout;
        private System.Windows.Forms.Timer mLayoutTimer;

        public StateMachineScene(StateMachine stateMachine, Control stateView)
        {
            this.mStateView = stateView;
            this.mStateMachine = stateMachine;
            this.mStateGraphHider = new StateGraphHider(this);
            this.InitStateGraph();

            Size size = stateView.Size;
            RectangleF bbox = new RectangleF(0, 0, size.Width, size.Height);
            this.BoundingBox = bbox;
            this.mStateGraphHider.BoundingBox = bbox;
            this.AddView(stateView);

            this.mLayout = null;
            this.mPendingLayout = null;
            this.bPendingLayout = false;
            this.mLayoutTimer = new Timer();
            this.mLayoutTimer.Interval = 1000 / 25;
            this.mLayoutTimer.Tick += new EventHandler(OnLayoutTimerTick);
        }

        public void SetBoundingBox(RectangleF bbox)
        {
            this.BoundingBox = bbox;
            this.mStateGraphHider.BoundingBox = bbox;
        }

        private void InitStateGraph()
        {
            this.mStateGraph = new Digraph<StateNode, StateEdge>();

            int i, j;
            StateEdge sEdge;
            StateNode srcNode, dstNode;
            foreach (State state in this.mStateMachine.States)
            {
                if (state != null)
                {
                    srcNode = new StateNode(state, this);
                    this.mStateGraph.AddNode(srcNode);
                    srcNode.SetParent(this);
                }
            }

            StateNode[] nodes = this.mStateGraph.Nodes;
            for (i = 0; i < nodes.Length; i++)
            {
                srcNode = nodes[i];
                foreach (State state in srcNode.State.Transitions)
                {
                    sEdge = null;
                    for (j = 0; j < nodes.Length && sEdge == null; j++)
                    {
                        dstNode = nodes[j];
                        if (dstNode.State.Equals(state))
                        {
                            sEdge = new StateEdge(srcNode, dstNode);
                        }
                    }
                    if (sEdge != null)
                    {
                        this.mStateGraph.AddEdge(sEdge);
                        sEdge.SetParent(this);
                    }
                }
            }
        }

        public Control StateView
        {
            get { return this.mStateView; }
        }

        public StateMachine StateMachine
        {
            get { return this.mStateMachine; }
        }

        public Digraph<StateNode, StateEdge> StateGraph
        {
            get { return this.mStateGraph; }
        }

        public LayoutAlgorithm<StateNode, StateEdge> Layout
        {
            get { return this.mLayout; }
            set
            {
                if (this.mLayout != value)
                {
                    if (this.mLayout != null &&
                        this.mLayoutTimer.Enabled)
                    {
                        // Abort() stops this.mLayoutTimer on its next tick
                        this.mLayout.Abort();
                        this.bPendingLayout = true;
                        this.mPendingLayout = value;
                    }
                    else
                    {
                        this.mLayout = value;
                        this.mLayoutRunning = true;
                    }
                    if (value != null)
                    {
                        value.ResetAlgorithm();
                        value.Reset();
                    }
                }
            }
        }

        private bool mLayoutRunning = true;

        public bool LayoutPaused = false;

        public bool LayoutStateGraphOnNodeMoved = false;

        public bool LayoutDecisionGraphOnNodeMoved = false;

        public bool IsLayoutRunning
        {
            get { return this.mLayoutTimer.Enabled; }
        }

        public void StartLayout()
        {
            if (!this.mLayoutTimer.Enabled)
            {
                this.mLayoutTimer.Start();
            }
        }

        public void StopLayout()
        {
            if (this.mLayout != null && this.mLayoutTimer.Enabled)
            {
                // Abort() stops this.mLayoutTimer on its next tick
                this.mLayout.Abort();
                if (this.mStateGraph.NodeCount > 0)
                {
                    StateNode sNode;
                    for (int i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
                    {
                        sNode = this.mStateGraph.NodeAt(i);
                        sNode.DGLayout.Abort();
                    }
                }
            }
        }

        private void OnLayoutTimerTick(object sender, EventArgs e)
        {
            if (!this.LayoutPaused)
            {
                if (this.mLayoutRunning)
                {
                    this.mLayoutRunning = 
                        this.mLayout != null &&
                        this.mLayout.AsyncIterate(false);
                }
                if (this.bPendingLayout)
                {
                    this.bPendingLayout = false;
                    this.mLayout = this.mPendingLayout;
                    this.mLayoutRunning =
                        this.mLayout != null &&
                        this.mLayout.AsyncIterate(false);
                }
                bool keep = this.mLayoutRunning;
                if (this.mStateGraph.NodeCount > 0)
                {
                    StateNode sNode;
                    LayoutAlgorithm<DGNode, DGEdge> dgLayout;
                    for (int i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
                    {
                        sNode = this.mStateGraph.NodeAt(i);
                        if (sNode.DGLayoutRunning)
                        {
                            dgLayout = sNode.DGLayout;
                            sNode.DGLayoutRunning = 
                                dgLayout != null && 
                                dgLayout.AsyncIterate(false);
                        }
                        keep |= sNode.DGLayoutRunning;
                        //keep |= sNode.AsyncDriftTowardsCenter();
                    }
                }
                RectangleF bbox = this.BoundingBox;
                // Expand the view to accommodate the bounding box or
                // expand the bounding box to match the view's size
                Size size = this.mStateView.Size;
                if (size.Width < bbox.Width)
                {
                    size.Width = (int)Math.Ceiling(bbox.Width);
                }
                else
                {
                    bbox.Width = size.Width;
                }
                if (size.Height < bbox.Height)
                {
                    size.Height = (int)Math.Ceiling(bbox.Height);
                }
                else
                {
                    bbox.Height = size.Height;
                }
                this.mStateView.Size = size;
                // Linearly drift the scene into alignment with its view
                // pos = pos + 0.1 * (nPos - pos) = 0.9 * pos + 0.1 * nPos
                // where nPos = -bbox.Location
                float dx = this.X + bbox.X;
                float dy = this.Y + bbox.Y;
                if (dx * dx > 0.01f || dy * dy > 0.01f)
                {
                    dx = this.X - 0.1f * dx;
                    dy = this.Y - 0.1f * dy;
                    this.SetPosition(dx, dy);
                    keep = true;
                }
                this.BoundingBox = bbox;
                if (this.mStateGraphHider.Visible)
                {
                    this.mStateGraphHider.BoundingBox = bbox;
                }
                // Check to see if the layout timer should continue running
                if (!keep)
                {
                    this.mLayoutTimer.Stop();
                }
            }
        }

        protected override void OnNodeMovedByMouse(AGraphNode node)
        {
            bool flag;
            Size size;
            int i, index;
            float x, y, d;
            StateNode sNode;
            RectangleF bbox, nbox;
            if (node is StateNode)
            {
                index = this.mStateGraph.IndexOfNode(node as StateNode);
            }
            else
            {
                index = -1;
            }
            if (index >= 0)
            {
                // Restrict node's position within scene's bounding box
                // or expand the scene's bounding box to accommodate it
                flag = false;
                size = this.mStateView.Size;
                bbox = this.BoundingBox;
                nbox = node.ChildrenBoundingBox();
                // Calculate the new X position and expand the width of
                // the state view and the scene's bounding box if necessary
                x = node.X;
                d = x + nbox.X;
                if (d < bbox.X)
                {
                    x = bbox.X - nbox.X;
                }
                d += nbox.Width;
                if (d > bbox.Right)
                {
                    d = d - bbox.X;
                    bbox.Width = d;
                    if (size.Width < d)
                    {
                        size.Width = (int)Math.Ceiling(d);
                    }
                    flag = true;
                }
                // Calculate the new Y position and expand the height of
                // the state view and the scene's bounding box if necessary
                y = node.Y;
                d = y + nbox.Y;
                if (d < bbox.Y)
                {
                    y = bbox.Y - nbox.Y;
                }
                d += nbox.Height;
                if (d > bbox.Bottom)
                {
                    d = d - bbox.Y;
                    bbox.Height = d;
                    if (size.Height < d)
                    {
                        size.Height = (int)Math.Ceiling(d);
                    }
                    flag = true;
                }
                // Set the new position of the node
                node.SetPosition(x, y);
                // Expand state view and scene's bounding box if necessary
                if (flag)
                {
                    this.mStateView.Size = size;
                    this.BoundingBox = bbox;
                    if (this.mStateGraphHider.Visible)
                    {
                        this.mStateGraphHider.BoundingBox = bbox;
                    }
                }
                // Update edges connected to node
                Digraph<StateNode, StateEdge>.GEdge edge;
                for (i = this.mStateGraph.EdgeCount - 1; i >= 0; i--)
                {
                    edge = this.mStateGraph.InternalEdgeAt(i);
                    if (edge.SrcNode.Index == index ||
                        edge.DstNode.Index == index)
                    {
                        edge.Data.Update();
                    }
                }
                // Restart the state graph layout if necessary
                if (this.LayoutStateGraphOnNodeMoved && this.mLayout != null)
                {
                    this.mLayout.ResetAlgorithm();
                    this.mLayout.Reset();
                    this.mLayoutRunning = true;
                    this.StartLayout();
                }
            }
            else if (node is DGNode)
            {
                sNode = node.Parent as StateNode;
                // Restrict node's position within scene's bounding box
                // or expand the scene's bounding box to accommodate it
                flag = false;
                size = this.mStateView.Size;
                bbox = this.BoundingBox;
                bbox.Offset(-sNode.X, -sNode.Y);
                nbox = node.BoundingBox;
                // Calculate the new X position and expand the width of
                // the state view and the scene's bounding box if necessary
                x = node.X;
                d = x + nbox.X;
                if (d < bbox.X)
                {
                    x = bbox.X - nbox.X;
                }
                d += nbox.Width;
                if (d > bbox.Right)
                {
                    d = d - bbox.X;
                    bbox.Width = d;
                    if (size.Width < d)
                    {
                        size.Width = (int)Math.Ceiling(d);
                    }
                    flag = true;
                }
                // Calculate the new Y position and expand the height of
                // the state view and the scene's bounding box if necessary
                y = node.Y;
                d = y + nbox.Y;
                if (d < bbox.Y)
                {
                    y = bbox.Y - nbox.Y;
                }
                d += nbox.Height;
                if (d > bbox.Bottom)
                {
                    d = d - bbox.Y;
                    bbox.Height = d;
                    if (size.Height < d)
                    {
                        size.Height = (int)Math.Ceiling(d);
                    }
                    flag = true;
                }
                // Set the new position of the node
                node.SetPosition(x, y);
                // Expand state view and scene's bounding box if necessary
                if (flag)
                {
                    this.mStateView.Size = size;
                    bbox.Offset(sNode.X, sNode.Y);
                    this.BoundingBox = bbox;
                }
                // Update edges connected to node
                Digraph<DGNode, DGEdge>.GEdge edge;
                Digraph<DGNode, DGEdge> dGraph = sNode.DecisionGraph;
                index = dGraph.IndexOfNode(node as DGNode);
                for (i = dGraph.EdgeCount - 1; i >= 0; i--)
                {
                    edge = dGraph.InternalEdgeAt(i);
                    if (edge.SrcNode.Index == index ||
                        edge.DstNode.Index == index)
                    {
                        edge.Data.Update();
                    }
                }
                // Restart the decision graph layout if necessary
                LayoutAlgorithm<DGNode, DGEdge> dgLayout = sNode.DGLayout;
                if (this.LayoutDecisionGraphOnNodeMoved && dgLayout != null)
                {
                    dgLayout.ResetAlgorithm();
                    dgLayout.Reset();
                    sNode.DGLayoutRunning = true;
                    this.StartLayout();
                }
            }
            base.OnNodeMovedByMouse(node);
        }

        private float mLassoX1;
        private float mLassoY1;
        private float mLassoX2;
        private float mLassoY2;

        public event EventHandler StateNodeSelectionChanged;
        public event EventHandler StateEdgeSelectionChanged;

        private List<StateNode> mSelectedStateNodes 
            = new List<StateNode>();
        private List<StateEdge> mSelectedStateEdges
            = new List<StateEdge>();

        private bool bSelectingStateGraph = false;

        public int SelectedStateNodeCount
        {
            get { return this.mSelectedStateNodes.Count; }
        }

        public StateNode[] SelectedStateNodes
        {
            get { return this.mSelectedStateNodes.ToArray(); }
        }

        public void SelectStateNode(StateNode stateNode, bool clearSelection)
        {
            if (stateNode.Parent == this)
            {
                if (!stateNode.Selected)
                {
                    if (clearSelection)
                    {
                        foreach (StateNode sNode in this.mSelectedStateNodes)
                        {
                            sNode.Selected = false;
                        }
                        this.mSelectedStateNodes.Clear();
                    }
                    this.mSelectedStateNodes.Add(stateNode);
                    stateNode.Selected = true;
                    if (this.StateNodeSelectionChanged != null)
                    {
                        this.StateNodeSelectionChanged(
                            this, new EventArgs());
                    }
                }
                else if (!clearSelection)
                {
                    int i;
                    StateNode sNode;
                    stateNode.Selected = false;
                    for (i = this.mSelectedStateNodes.Count - 1; i >= 0; i--)
                    {
                        sNode = this.mSelectedStateNodes[i];
                        if (!sNode.Selected)
                        {
                            this.mSelectedStateNodes.RemoveAt(i);
                            break;
                        }
                    }
                    if (this.StateNodeSelectionChanged != null)
                    {
                        this.StateNodeSelectionChanged(
                            this, new EventArgs());
                    }
                }
            }
        }

        public void SelectAllStateNodes()
        {
            bool changed = false;
            StateNode sNode;
            for (int i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
            {
                sNode = this.mStateGraph.NodeAt(i);
                if (!sNode.Selected)
                {
                    this.mSelectedStateNodes.Add(sNode);
                    sNode.Selected = true;
                    changed = true;
                }
            }
            if (changed && this.StateNodeSelectionChanged != null)
            {
                this.StateNodeSelectionChanged(this, new EventArgs());
            }
        }

        public void ClearStateNodeSelection()
        {
            bool changed = this.mSelectedStateNodes.Count > 0;
            foreach (StateNode sNode in this.mSelectedStateNodes)
            {
                sNode.Selected = false;
            }
            this.mSelectedStateNodes.Clear();

            if (changed && this.StateNodeSelectionChanged != null)
            {
                this.StateNodeSelectionChanged(this, new EventArgs());
            }
        }

        public int SelectedStateEdgeCount
        {
            get { return this.mSelectedStateEdges.Count; }
        }

        public StateEdge[] SelectedStateEdges
        {
            get { return this.mSelectedStateEdges.ToArray(); }
        }

        public void SelectStateEdge(StateEdge stateEdge, bool clearSelection)
        {
            if (stateEdge.Parent == this)
            {
                if (!stateEdge.Selected)
                {
                    if (clearSelection)
                    {
                        foreach (StateEdge sEdge in this.mSelectedStateEdges)
                        {
                            sEdge.Selected = false;
                        }
                        this.mSelectedStateEdges.Clear();
                    }
                    this.mSelectedStateEdges.Add(stateEdge);
                    stateEdge.Selected = true;
                    if (this.StateEdgeSelectionChanged != null)
                    {
                        this.StateEdgeSelectionChanged(
                            this, new EventArgs());
                    }
                }
                else if (!clearSelection)
                {
                    int i;
                    StateEdge sEdge;
                    stateEdge.Selected = false;
                    for (i = this.mSelectedStateEdges.Count - 1; i >= 0; i--)
                    {
                        sEdge = this.mSelectedStateEdges[i];
                        if (!sEdge.Selected)
                        {
                            this.mSelectedStateEdges.RemoveAt(i);
                            break;
                        }
                    }
                    if (this.StateEdgeSelectionChanged != null)
                    {
                        this.StateEdgeSelectionChanged(
                            this, new EventArgs());
                    }
                }
            }
        }

        public void SelectAllStateEdges()
        {
            bool changed = false;
            StateEdge sEdge;
            for (int i = this.mStateGraph.EdgeCount - 1; i >= 0; i--)
            {
                sEdge = this.mStateGraph.EdgeAt(i);
                if (!sEdge.Selected)
                {
                    this.mSelectedStateEdges.Add(sEdge);
                    sEdge.Selected = true;
                    changed = true;
                }
            }
            if (changed && this.StateEdgeSelectionChanged != null)
            {
                this.StateEdgeSelectionChanged(this, new EventArgs());
            }
        }

        public void ClearStateEdgeSelection()
        {
            bool changed = this.mSelectedStateEdges.Count > 0;
            foreach (StateEdge sEdge in this.mSelectedStateEdges)
            {
                sEdge.Selected = false;
            }
            this.mSelectedStateEdges.Clear();

            if (changed && this.StateEdgeSelectionChanged != null)
            {
                this.StateEdgeSelectionChanged(this, new EventArgs());
            }
        }

        public event EventHandler DGNodeSelectionChanged;
        public event EventHandler DGEdgeSelectionChanged;

        private List<DGNode> mSelectedDGNodes
            = new List<DGNode>();
        private List<DGEdge> mSelectedDGEdges
            = new List<DGEdge>();

        private bool bSelectingDecisionGraph = false;

        public int SelectedDGNodeCount
        {
            get { return this.mSelectedDGNodes.Count; }
        }

        public DGNode[] SelectedDGNodes
        {
            get { return this.mSelectedDGNodes.ToArray(); }
        }

        public void SelectDGNode(DGNode dgNode, bool clearSelection)
        {
            StateNode sp = dgNode.Parent as StateNode;
            if (sp != null && sp.Equals(this.mFocusedState))
            {
                if (!dgNode.Selected)
                {
                    if (clearSelection)
                    {
                        foreach (DGNode node in this.mSelectedDGNodes)
                        {
                            node.Selected = false;
                        }
                        this.mSelectedDGNodes.Clear();
                    }
                    this.mSelectedDGNodes.Add(dgNode);
                    dgNode.Selected = true;
                    if (this.DGNodeSelectionChanged != null)
                    {
                        this.DGNodeSelectionChanged(this, new EventArgs());
                    }
                }
                else if (!clearSelection)
                {
                    int i;
                    DGNode node;
                    dgNode.Selected = false;
                    for (i = this.mSelectedDGNodes.Count - 1; i >= 0; i--)
                    {
                        node = this.mSelectedDGNodes[i];
                        if (!node.Selected)
                        {
                            this.mSelectedDGNodes.RemoveAt(i);
                            break;
                        }
                    }
                    if (this.DGNodeSelectionChanged != null)
                    {
                        this.DGNodeSelectionChanged(this, new EventArgs());
                    }
                }
            }
        }

        public void SelectAllDGNodes()
        {
            if (this.mFocusedState != null)
            {
                DGNode node;
                bool changed = false;
                Digraph<DGNode, DGEdge> dGraph 
                    = this.mFocusedState.DecisionGraph;
                for (int i = dGraph.NodeCount - 1; i >= 0; i--)
                {
                    node = dGraph.NodeAt(i);
                    if (!node.Selected)
                    {
                        this.mSelectedDGNodes.Add(node);
                        node.Selected = true;
                        changed = true;
                    }
                }
                if (changed && this.DGNodeSelectionChanged != null)
                {
                    this.DGNodeSelectionChanged(this, new EventArgs());
                }
            }
        }

        public void ClearDGNodeSelection()
        {
            bool changed = this.mSelectedDGNodes.Count > 0;
            foreach (DGNode node in this.mSelectedDGNodes)
            {
                node.Selected = false;
            }
            this.mSelectedDGNodes.Clear();

            if (changed && this.DGNodeSelectionChanged != null)
            {
                this.DGNodeSelectionChanged(this, new EventArgs());
            }
        }

        public int SelectedDGEdgeCount
        {
            get { return this.mSelectedDGEdges.Count; }
        }

        public DGEdge[] SelectedDGEdges
        {
            get { return this.mSelectedDGEdges.ToArray(); }
        }

        public void SelectDGEdge(DGEdge dgEdge, bool clearSelection)
        {
            StateNode sp = dgEdge.Parent as StateNode;
            if (sp != null && sp.Equals(this.mFocusedState))
            {
                if (!dgEdge.Selected)
                {
                    if (clearSelection)
                    {
                        foreach (DGEdge edge in this.mSelectedDGEdges)
                        {
                            edge.Selected = false;
                        }
                        this.mSelectedDGEdges.Clear();
                    }
                    this.mSelectedDGEdges.Add(dgEdge);
                    dgEdge.Selected = true;
                    if (this.DGEdgeSelectionChanged != null)
                    {
                        this.DGEdgeSelectionChanged(this, new EventArgs());
                    }
                }
                else if (!clearSelection)
                {
                    int i;
                    DGEdge edge;
                    dgEdge.Selected = false;
                    for (i = this.mSelectedDGEdges.Count - 1; i >= 0; i--)
                    {
                        edge = this.mSelectedDGEdges[i];
                        if (!edge.Selected)
                        {
                            this.mSelectedDGEdges.RemoveAt(i);
                            break;
                        }
                    }
                    if (this.DGEdgeSelectionChanged != null)
                    {
                        this.DGEdgeSelectionChanged(this, new EventArgs());
                    }
                }
            }
        }

        public void SelectAllDGEdges()
        {
            if (this.mFocusedState != null)
            {
                DGEdge edge;
                bool changed = false;
                Digraph<DGNode, DGEdge> dGraph
                    = this.mFocusedState.DecisionGraph;
                for (int i = dGraph.EdgeCount - 1; i >= 0; i--)
                {
                    edge = dGraph.EdgeAt(i);
                    if (!edge.Selected)
                    {
                        this.mSelectedDGEdges.Add(edge);
                        edge.Selected = true;
                        changed = true;
                    }
                }
                if (changed && this.DGNodeSelectionChanged != null)
                {
                    this.DGNodeSelectionChanged(this, new EventArgs());
                }
            }
        }

        public void ClearDGEdgeSelection()
        {
            bool changed = this.mSelectedDGEdges.Count > 0;
            foreach (DGEdge sEdge in this.mSelectedDGEdges)
            {
                sEdge.Selected = false;
            }
            this.mSelectedDGEdges.Clear();

            if (this.DGEdgeSelectionChanged != null)
            {
                this.DGEdgeSelectionChanged(this, new EventArgs());
            }
        }

        public event EventHandler FocusedStateChanged;

        private StateNode mFocusedState = null;

        public StateNode FocusedState
        {
            get { return this.mFocusedState; }
        }

        public void SetFocusedState(StateNode stateNode)
        {
            if (this.mFocusedState != null)
            {
                this.ClearDGNodeSelection();
                this.ClearDGEdgeSelection();
                if (stateNode == null)
                {
                    this.mStateGraphHider.Visible = false;
                }
                this.mFocusedState.ClipsChildrenToShape = true;
                this.mFocusedState.Zvalue = 0;
                this.mFocusedState.InEditMode = false;
            }
            if (stateNode != null)
            {
                this.mStateGraphHider.BoundingBox = this.BoundingBox;
                this.mStateGraphHider.Visible = true;
                this.ClearStateNodeSelection();
                this.ClearStateEdgeSelection();
            }
            this.mFocusedState = stateNode;

            if (this.FocusedStateChanged != null)
            {
                this.FocusedStateChanged(this, new EventArgs());
            }
        }

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            // Lasso selection starts when the user clicks the background
            // of the scene instead of a child of the scene. If the user
            // clicked a child, the event args will have been handled
            // already by the OnMouseDown event of the clicked child.
            if (this.mFocusedState != null)
            {
                this.bSelectingDecisionGraph
                    = this.mStateGraphHider.MouseDownInvoked;
                this.mStateGraphHider.MouseDownInvoked = false;
            }
            else
            {
                this.bSelectingStateGraph = !e.Handled;
            }
            this.mLassoX1 = e.X;
            this.mLassoY1 = e.Y;
            if (this.bSelectingStateGraph)
            {
                // Clear all the selected state nodes
                this.ClearStateNodeSelection();
                // Clear all the selected state edges
                this.ClearStateEdgeSelection();
                // Reset the second lasso point to prevent visual artifacts
                this.mLassoX2 = this.mLassoX1;
                this.mLassoY2 = this.mLassoY1;
            }
            else if (this.bSelectingDecisionGraph)
            {
                // Clear all the selected DG nodes
                this.ClearDGNodeSelection();
                // Clear all the selected DG edges
                this.ClearDGEdgeSelection();
                // Reset the second lasso point to prevent visual artifacts
                this.mLassoX2 = this.mLassoX1;
                this.mLassoY2 = this.mLassoY1;
            }
            else
            {
                foreach (StateNode sNode in this.mSelectedStateNodes)
                {
                    sNode.MouseGrabbed = true;
                }
                foreach (DGNode dgNode in this.mSelectedDGNodes)
                {
                    dgNode.MouseGrabbed = true;
                }
            }
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseMove(GraphMouseEventArgs e)
        {
            if (this.bSelectingStateGraph || this.bSelectingDecisionGraph)
            {
                int i;
                bool changed = false;
                float minX = Math.Min(this.mLassoX1, this.mLassoX2);
                float minY = Math.Min(this.mLassoY1, this.mLassoY2);
                float maxX = Math.Max(this.mLassoX1, this.mLassoX2);
                float maxY = Math.Max(this.mLassoY1, this.mLassoY2);
                this.mLassoX2 = e.X;
                this.mLassoY2 = e.Y;
                minX = Math.Min(minX, this.mLassoX2);
                minY = Math.Min(minY, this.mLassoY2);
                maxX = Math.Max(maxX, this.mLassoX2);
                maxY = Math.Max(maxY, this.mLassoY2);
                this.Invalidate(new RectangleF(minX - 5, minY - 5,
                    maxX - minX + 10, maxY - minY + 10));
                minX = Math.Min(this.mLassoX1, this.mLassoX2);
                minY = Math.Min(this.mLassoY1, this.mLassoY2);
                maxX = Math.Max(this.mLassoX1, this.mLassoX2);
                maxY = Math.Max(this.mLassoY1, this.mLassoY2);
                if (this.bSelectingStateGraph)
                {
                    StateNode sNode;
                    this.mSelectedStateNodes.Clear();
                    for (i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
                    {
                        sNode = this.mStateGraph.NodeAt(i);
                        if (minX < sNode.X + sNode.Radius &&
                            sNode.X - sNode.Radius < maxX &&
                            minY < sNode.Y + sNode.Radius &&
                            sNode.Y - sNode.Radius < maxY)
                        {
                            this.mSelectedStateNodes.Add(sNode);
                            if (!sNode.Selected)
                            {
                                changed = true;
                                sNode.Selected = true;
                            }
                        }
                        else if (sNode.Selected)
                        {
                            changed = true;
                            sNode.Selected = false;
                        }
                    }
                    if (changed && this.StateNodeSelectionChanged != null)
                    {
                        this.StateNodeSelectionChanged(this, 
                            new EventArgs());
                    }
                }
                else if (this.bSelectingDecisionGraph)
                {
                    DGNode dgNode;
                    float dgX, dgY;
                    RectangleF bbox;
                    Digraph<DGNode, DGEdge> dGraph 
                        = this.mFocusedState.DecisionGraph;
                    minX -= this.mFocusedState.X;
                    minY -= this.mFocusedState.Y;
                    maxX -= this.mFocusedState.X;
                    maxY -= this.mFocusedState.Y;
                    this.mSelectedDGNodes.Clear();
                    for (i = dGraph.NodeCount - 1; i >= 0; i--)
                    {
                        dgNode = dGraph.NodeAt(i);
                        bbox = dgNode.BoundingBox;
                        dgX = dgNode.X + bbox.X;
                        dgY = dgNode.Y + bbox.Y;
                        if (dgX < maxX && dgY < maxY &&
                            minX < dgX + bbox.Width &&
                            minY < dgY + bbox.Height)
                        {
                            this.mSelectedDGNodes.Add(dgNode);
                            if (!dgNode.Selected)
                            {
                                changed = true;
                                dgNode.Selected = true;
                            }
                        }
                        else if (dgNode.Selected)
                        {
                            changed = true;
                            dgNode.Selected = false;
                        }
                    }
                    if (changed && this.DGNodeSelectionChanged != null)
                    {
                        this.DGNodeSelectionChanged(this, 
                            new EventArgs());
                    }
                }
                return true;
            }
            else
            {
                return base.OnMouseMove(e);
            }
        }

        protected override bool OnMouseUp(GraphMouseEventArgs e)
        {
            if (this.bSelectingStateGraph || this.bSelectingDecisionGraph)
            {
                this.bSelectingStateGraph = false;
                this.bSelectingDecisionGraph = false;
                this.Invalidate(new RectangleF(
                    Math.Min(this.mLassoX1, this.mLassoX2) - 5,
                    Math.Min(this.mLassoY1, this.mLassoY2) - 5,
                    Math.Abs(this.mLassoX2 - this.mLassoX1) + 10,
                    Math.Abs(this.mLassoY2 - this.mLassoY1) + 10));
            }
            return base.OnMouseUp(e);
        }

        private bool bLearningPos = false;
        private float mMinX;
        private float mMinY;
        private float mMaxX;
        private float mMaxY;

        public void BeginLearningNodePositions()
        {
            RectangleF bbox = this.BoundingBox;
            this.bLearningPos = true;
            this.mMinX = bbox.X;
            this.mMinY = bbox.Y;
            this.mMaxX = this.mMinX + bbox.Width;
            this.mMaxY = this.mMinY + bbox.Height;
        }

        public void LearnNodePosition(float x, float y, Box2F bbox)
        {
            float dim = x + bbox.X;
            if (this.mMinX > dim)
                this.mMinX = dim;
            dim += bbox.W;
            if (this.mMaxX < dim)
                this.mMaxX = dim;
            dim = y + bbox.Y;
            if (this.mMinY > dim)
                this.mMinY = dim;
            dim += bbox.H;
            if (this.mMaxY < dim)
                this.mMaxY = dim;
        }

        public void BeginAdjustingNodePositions()
        {
            if (this.bLearningPos)
            {
                this.bLearningPos = false;
                RectangleF bbox = new RectangleF(this.mMinX, this.mMinY, 
                    this.mMaxX - this.mMinX, this.mMaxY - this.mMinY);
                this.BoundingBox = bbox;
                if (this.mStateGraphHider.Visible)
                {
                    this.mStateGraphHider.BoundingBox = bbox;
                }
            }
        }

        public Vec2F AdjustNodePosition(float x, float y, Box2F bbox)
        {
            return new Vec2F(x, y);
        }

        public Box2F LayoutBBox
        {
            get
            {
                RectangleF bbox = this.BoundingBox;
                return new Box2F(bbox.X, bbox.Y, bbox.Width, bbox.Height);
            }
        }

        public bool PositionFixed
        {
            get { return true; }
        }

        private static float sGridSize = 100;

        private static Color sBGColor = Color.White;

        private static Pen sGridPen = new Pen(Color.DarkGray, 2);

        public static float GridSize
        {
            get { return sGridSize; }
            set
            {
                if (sGridSize != value)
                {
                    if (value <= 0)
                    {
                        throw new ArgumentOutOfRangeException("GridSize");
                    }
                    sGridSize = value;
                }
            }
        }

        public static Color BackColor
        {
            get { return sBGColor; }
            set
            {
                if (!sBGColor.Equals(value))
                {
                    sBGColor = value;
                }
            }
        }

        public static Color GridColor
        {
            get { return sGridPen.Color; }
            set
            {
                Color color = sGridPen.Color;
                if (!color.Equals(value))
                {
                    sGridPen.Color = value;
                }
            }
        }

#if DEBUG
        private static readonly Font sDebugFont 
            = new Font(FontFamily.GenericSansSerif, 10);
#endif

        protected override void OnDrawBackground(PaintEventArgs e)
        {
            int i, count;
            float x1, x2, y1, y2;
            Graphics g = e.Graphics;
            RectangleF bbox = this.BoundingBox;
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            // Draw the vertical grid lines
            x1 = bbox.X;
            y1 = bbox.Y;
            y2 = y1 + bbox.Height;
            count = (int)Math.Ceiling(bbox.Width / sGridSize);
            for (i = 0; i < count; i++)
            {
                g.DrawLine(sGridPen, x1, y1, x1, y2);
                x1 += sGridSize;
            }
            // Draw the horizontal grid lines
            x1 = bbox.X;
            y1 = bbox.Y;
            x2 = x1 + bbox.Width;
            count = (int)Math.Ceiling(bbox.Height / sGridSize);
            for (i = 0; i < count; i++)
            {
                g.DrawLine(sGridPen, x1, y1, x2, y1);
                y1 += sGridSize;
            }
#if DEBUG
            Size size = this.mStateView.Size;
            g.DrawString(string.Concat(bbox, "\n", size), 
                sDebugFont, Brushes.Black, bbox);
#endif
            g.SmoothingMode = sm;
        }

        protected override void OnDrawForeground(PaintEventArgs e)
        {
            if (this.bSelectingStateGraph || this.bSelectingDecisionGraph)
            {
                e.Graphics.DrawRectangle(sLassoPen,
                    Math.Min(this.mLassoX1, this.mLassoX2),
                    Math.Min(this.mLassoY1, this.mLassoY2),
                    Math.Abs(this.mLassoX2 - this.mLassoX1),
                    Math.Abs(this.mLassoY2 - this.mLassoY1));
            }
            base.OnDrawForeground(e);
        }
    }
}
