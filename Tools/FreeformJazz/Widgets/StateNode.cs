﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using GraphForms;
using GraphForms.Algorithms;
using GraphForms.Algorithms.Layout;
using GraphForms.Algorithms.Layout.Tree;
using s3pi.GenericRCOLResource;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class StateNode : AGraphNode, ILayoutNode, IEquatable<StateNode>
    {
        private static float sMinRad = 15;

        private static float sMaxRad = 60;

        public static float MinimizedRadius
        {
            get { return sMinRad; }
            set
            {
                if (sMinRad != value)
                {
                    if (value <= 0 || value >= sMaxRad)
                    {
                        throw new ArgumentOutOfRangeException(
                            "MinimizedRadius");
                    }
                    sMinRad = value;
                }
            }
        }

        public static float MaximizedRadius
        {
            get { return sMaxRad; }
            set
            {
                if (sMaxRad != value)
                {
                    if (value <= sMinRad)
                    {
                        throw new ArgumentOutOfRangeException(
                            "MaximizedRadius");
                    }
                    sMaxRad = value;
                }
            }
        }

        private class DGNCluster : IClusterNode
        {
            private StateNode mOwner;

            public DGNCluster(StateNode owner)
            {
                this.mOwner = owner;
            }

            private float mMinX;
            private float mMinY;
            private float mMaxX;
            private float mMaxY;

            public void BeginLearningNodePositions()
            {
            }

            public void LearnNodePosition(float x, float y, Box2F bbox)
            {
            }

            public void BeginAdjustingNodePositions()
            {
                RectangleF bbox = this.mOwner.mScene.BoundingBox;
                this.mMinX = bbox.X - this.mOwner.X;
                this.mMinY = bbox.Y - this.mOwner.Y;
                this.mMaxX = this.mMinX + bbox.Width;
                this.mMaxY = this.mMinY + bbox.Height;
            }

            public Vec2F AdjustNodePosition(float x, float y, Box2F bbox)
            {
                float d;
                Vec2F v = new Vec2F(x, y);
                d = bbox.W;
                v.X = Math.Max(x + bbox.X, this.mMinX) + d;
                v.X = Math.Min(v.X, this.mMaxX) - bbox.X - d;
                d = bbox.H;
                v.Y = Math.Max(y + bbox.Y, this.mMinY) + d;
                v.Y = Math.Min(v.Y, this.mMaxY) - bbox.Y - d;
                return v;
            }

            public Box2F LayoutBBox
            {
                get 
                {
                    RectangleF bbox = this.mOwner.mScene.BoundingBox;
                    bbox.Offset(-this.mOwner.X, -this.mOwner.Y);
                    return new Box2F(
                        bbox.X, bbox.Y, bbox.Width, bbox.Height);
                }
            }

            public bool PositionFixed
            {
                get { return this.mOwner.MouseGrabbed; }
            }

            public void SetPosition(float x, float y)
            {
                this.mOwner.SetPosition(x, y);
            }

            public float X
            {
                get { return this.mOwner.X; }
            }

            public float Y
            {
                get { return this.mOwner.Y; }
            }
        }

        private State mState;
        private StateMachineScene mScene;

        private float mRad;
        private float mRadSquared;

        private bool bInEditMode;

        private bool bMinimized;

        private DGNCluster mCluster;
        private DGRootNode mRootNode;
        private Digraph<DGNode, DGEdge> mDGraph;
        private SimpleTreeLayoutAlgorithm<DGNode, DGEdge> mLayout;

        public StateNode(State state, StateMachineScene scene)
            : base(scene)
        {
            if (state == null)
                throw new ArgumentNullException("state");
            this.mState = state;
            this.mScene = scene;

            this.mRad = sMaxRad;
            this.mRadSquared = sMaxRad * sMaxRad;

            this.bInEditMode = false;

            this.bMinimized = false;

            this.ClipsChildrenToShape = true;

            this.mCluster = new DGNCluster(this);
            this.InitDecisionGraph();

            this.DGLayoutRunning = true;

            this.mLayout = new SimpleTreeLayoutAlgorithm<DGNode, DGEdge>(
                this.mDGraph, this.mCluster);
            this.mLayout.Direction = LayoutDirection.LeftToRight;
            this.mLayout.LayerGap = 30;
            this.mLayout.RootFindingMethod = TreeRootFinding.UserDefined;
            if (this.mDGraph.NodeCount > 0)
            {
                this.mLayout.AddRoot(0);
            }
            this.mLayout.ShuffleNodes();

            float rad = this.mRad + 10;
            this.BoundingBox = new RectangleF(-rad, -rad, 2 * rad, 2 * rad);
        }

        private void InitDecisionGraph()
        {
            this.mDGraph = new Digraph<DGNode, DGEdge>();

            DecisionGraph dg = this.mState.DecisionGraph;
            if (dg != null)
            {
                int i;
                DGNode dst;
                //DGEdge edge;
                //AnchorPoint ap;
                DecisionGraphNode dgn;
                DecisionGraphNode[] dgns;
                this.mRootNode = new DGRootNode(dg, this);
                this.mDGraph.AddNode(this.mRootNode);
                this.mRootNode.SetParent(this);
                if (dg.DecisionMakerCount > 0)
                {
                    dgns = dg.DecisionMakers;
                    //ap = root.DecisionMakerAnchor;
                    for (i = 0; i < dgns.Length; i++)
                    {
                        dgn = dgns[i];
                        if (dgn != null)
                        {
                            dst = this.InitDGN(this.mRootNode, dgn);
                            dst.SetCategory(DGNode.DGFlag.DecisionMaker);
                            /*edge = new DGEdge(root, dst);
                            ap.Edges.Add(edge);
                            dst.EntryAnchor.Edges.Add(edge);
                            this.mDecisionGraph.AddEdge(edge);
                            edge.SetParent(this);/* */
                        }
                    }
                }
                if (dg.EntryPointCount > 0)
                {
                    dgns = dg.EntryPoints;
                    //ap = root.EntryAnchor;
                    for (i = 0; i < dgns.Length; i++)
                    {
                        dgn = dgns[i];
                        if (dgn != null)
                        {
                            dst = this.InitDGN(this.mRootNode, dgn);
                            dst.SetCategory(DGNode.DGFlag.EntryPoint);
                            /*edge = new DGEdge(root, dst);
                            ap.Edges.Add(edge);
                            dst.EntryAnchor.Edges.Add(edge);
                            this.mDecisionGraph.AddEdge(edge);
                            edge.SetParent(this);/* */
                        }
                    }
                }
                this.AddRootDGEdges();
            }
        }

        public void AddRootDGEdges()
        {
            if (this.mRootNode == null)
            {
                DecisionGraph dg = this.mState.DecisionGraph;
                if (dg != null)
                {
                    this.mRootNode = new DGRootNode(dg, this);
                    this.mDGraph.InsertNode(0, this.mRootNode);
                    this.mRootNode.SetParent(this);
                }
            }
            if (this.mRootNode != null)
            {
                DGNode node;
                DGEdge edge;
                List<DGEdge> edges = this.mRootNode.EntryAnchor.Edges;
                for (int i = this.mDGraph.NodeCount - 1; i > 0; i--)
                {
                    node = this.mDGraph.NodeAt(i);
                    if (node.EntryAnchor.Edges.Count == 0)
                    {
                        edge = new DGEdge(this.mRootNode, node, true);
                        edges.Add(edge);
                        node.EntryAnchor.Edges.Add(edge);
                        this.mDGraph.AddEdge(edge);
                        edge.SetParent(this);
                    }
                }
            }
        }

        public void RemoveRootDGEdges()
        {
            if (this.mRootNode != null)
            {
                DGNode node;
                DGEdge edge;
                List<DGEdge> edges = this.mRootNode.EntryAnchor.Edges;
                for (int i = edges.Count - 1; i >= 0; i--)
                {
                    edge = edges[i];
                    node = edge.DstNode;
                    if (node.EntryAnchor.Edges.Count > 1)
                    {
                        edges.RemoveAt(i);
                        node.EntryAnchor.Edges.Remove(edge);
                        this.mDGraph.RemoveEdge(this.mRootNode, node);
                        edge.SetParent(null);
                    }
                }
            }
        }

        private DGNode InitDGN(DGNode src, DecisionGraphNode dgn)
        {
            int i;
            DGNode dst = null;
            for (i = this.mDGraph.NodeCount - 1; i >= 0; i--)
            {
                dst = this.mDGraph.NodeAt(i);
                if (dst.DGN == dgn)
                {
                    break;
                }
            }
            if (i < 0)
            {
                int j;
                DGEdge edge;
                AnchorPoint ap;
                DecisionGraphNode[] dgns;
                DGMulticastNode dgmcn = null;
                switch (dgn.ChunkType)
                {
                    case NextStateNode.ResourceType:
                        NextStateNode nsn = dgn as NextStateNode;
                        dst = new DGSnSnNode(nsn, this);
                        this.mDGraph.AddNode(dst);
                        dst.SetParent(this);
                        break;
                    case RandomNode.ResourceType:
                        RandomNode rand = dgn as RandomNode;
                        DGRandNode dgrn = new DGRandNode(rand, this);
                        this.mDGraph.AddNode(dgrn);
                        dgrn.SetParent(this);
                        List<RandomNode.Slice> slices = rand.Slices;
                        if (slices.Count > 0)
                        {
                            for (i = 0; i < slices.Count; i++)
                            {
                                ap = dgrn.GetSliceAnchor(i);
                                dgns = slices[i].Targets.ToArray();
                                for (j = 0; j < dgns.Length; j++)
                                {
                                    dst = this.InitDGN(dgrn, dgns[j]);
                                    edge = new DGEdge(dgrn, dst, false);
                                    ap.Edges.Add(edge);
                                    dst.EntryAnchor.Edges.Add(edge);
                                    this.mDGraph.AddEdge(edge);
                                    edge.SetParent(this);
                                }
                            }
                        }
                        dst = dgrn;
                        break;
                    case SelectOnDestinationNode.ResourceType:
                        SelectOnDestinationNode sodn
                            = dgn as SelectOnDestinationNode;
                        DGSoDnNode dgsodn
                            = new DGSoDnNode(sodn, this);
                        this.mDGraph.AddNode(dgsodn);
                        dgsodn.SetParent(this);
                        if (sodn.CaseCount > 0)
                        {
                            SelectOnDestinationNode.Case[] cases = sodn.Cases;
                            for (i = 0; i < cases.Length; i++)
                            {
                                ap = dgsodn.GetCaseAnchorAt(i);
                                dgns = cases[i].Targets;
                                for (j = 0; j < dgns.Length; j++)
                                {
                                    dst = this.InitDGN(dgsodn, dgns[j]);
                                    edge = new DGEdge(dgsodn, dst, false);
                                    ap.Edges.Add(edge);
                                    dst.EntryAnchor.Edges.Add(edge);
                                    this.mDGraph.AddEdge(edge);
                                    edge.SetParent(this);
                                }
                            }
                        }
                        dst = dgsodn;
                        break;
                    case SelectOnParameterNode.ResourceType:
                        SelectOnParameterNode sopn 
                            = dgn as SelectOnParameterNode;
                        DGSoPnNode dgsopn 
                            = new DGSoPnNode(sopn, this);
                        this.mDGraph.AddNode(dgsopn);
                        dgsopn.SetParent(this);
                        if (sopn.CaseCount > 0)
                        {
                            SelectOnParameterNode.Case[] cases = sopn.Cases;
                            for (i = 0; i < cases.Length; i++)
                            {
                                ap = dgsopn.GetCaseAnchorAt(i);
                                dgns = cases[i].Targets;
                                for (j = 0; j < dgns.Length; j++)
                                {
                                    dst = this.InitDGN(dgsopn, dgns[j]);
                                    edge = new DGEdge(dgsopn, dst, false);
                                    ap.Edges.Add(edge);
                                    dst.EntryAnchor.Edges.Add(edge);
                                    this.mDGraph.AddEdge(edge);
                                    edge.SetParent(this);
                                }
                            }
                        }
                        dst = dgsopn;
                        break;
                    case CreatePropNode.ResourceType:
                        CreatePropNode cpn = dgn as CreatePropNode;
                        DGPropNode dgcpn = new DGPropNode(cpn, this);
                        this.mDGraph.AddNode(dgcpn);
                        dgcpn.SetParent(this);
                        dgmcn = dgcpn;
                        break;
                    case ActorOperationNode.ResourceType:
                        ActorOperationNode aon = dgn as ActorOperationNode;
                        DGAcOpNode dgaon = new DGAcOpNode(aon, this);
                        this.mDGraph.AddNode(dgaon);
                        dgaon.SetParent(this);
                        dgmcn = dgaon;
                        break;
                    case StopAnimationNode.ResourceType:
                        StopAnimationNode san = dgn as StopAnimationNode;
                        DGStopNode dgsan = new DGStopNode(san, this);
                        this.mDGraph.AddNode(dgsan);
                        dgsan.SetParent(this);
                        dgmcn = dgsan;
                        break;
                    case PlayAnimationNode.ResourceType:
                        PlayAnimationNode pan = dgn as PlayAnimationNode;
                        DGPlayNode dgpan = new DGPlayNode(pan, this);
                        this.mDGraph.AddNode(dgpan);
                        dgpan.SetParent(this);
                        dgmcn = dgpan;
                        break;
                }
                if (dgmcn != null)
                {
                    MulticastDecisionGraphNode mcn 
                        = dgn as MulticastDecisionGraphNode;
                    if (mcn.TargetCount > 0)
                    {
                        ap = dgmcn.TargetAnchor;
                        dgns = mcn.Targets;
                        for (i = 0; i < dgns.Length; i++)
                        {
                            dst = this.InitDGN(dgmcn, dgns[i]);
                            edge = new DGEdge(dgmcn, dst, false);
                            ap.Edges.Add(edge);
                            dst.EntryAnchor.Edges.Add(edge);
                            this.mDGraph.AddEdge(edge);
                            edge.SetParent(this);
                        }
                    }
                    dst = dgmcn;
                }
            }
            return dst;
        }

        public State State
        {
            get { return this.mState; }
        }

        public StateMachineScene Scene
        {
            get { return this.mScene; }
        }

        public DGRootNode RootNode
        {
            get { return this.mRootNode; }
        }

        public void UpdateRootNode(bool add)
        {
            this.mRootNode = null;
            if (this.mDGraph.NodeCount > 0)
            {
                int i, count = this.mDGraph.NodeCount;
                for (i = 0; i < count; i++)
                {
                    this.mRootNode = this.mDGraph.NodeAt(i) as DGRootNode;
                    if (this.mRootNode != null)
                    {
                        break;
                    }
                }
                if (this.mRootNode != null && i > 0)
                {
                    // TODO: Swap root node and this.mDGraph.NodeAt(0)
                }
            }
            if (this.mRootNode == null && add)
            {
                DecisionGraph dg = this.mState.DecisionGraph;
                if (dg != null)
                {
                    this.mRootNode = new DGRootNode(dg, this);
                    this.mDGraph.InsertNode(0, this.mRootNode);
                    this.mRootNode.SetParent(this);
                }
            }
        }

        public Digraph<DGNode, DGEdge> DecisionGraph
        {
            get { return this.mDGraph; }
        }

        private abstract class StatePropertyCommand<P>
            : PropertyCommand<State, P>
        {
            private StateNode mSN;

            public StatePropertyCommand(StateNode sn, 
                string property, P newValue, bool extendable)
                : base(sn.mState, property, newValue, extendable)
            {
                this.mSN = sn;
                this.mLabel = "Set State Node ";
            }

            public override bool Execute()
            {
                bool flag = base.Execute();
                if (flag)
                {
                    this.mSN.Invalidate();
                }
                return flag;
            }

            public override void Undo()
            {
                base.Undo();
                this.mSN.Invalidate();
            }
        }

        private class NameCommand : StatePropertyCommand<string>
        {
            public NameCommand(StateNode sn, 
                string newValue, bool extendable)
                : base(sn, "Name", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Name";
            }
        }

        private class FlagsCommand : StatePropertyCommand<JazzState.Flags>
        {
            public FlagsCommand(StateNode sn,
                JazzState.Flags newValue, bool extendable)
                : base(sn, "Flags", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Flags";
            }
        }

        private class AwarenessCommand
            : StatePropertyCommand<JazzChunk.AwarenessLevel>
        {
            public AwarenessCommand(StateNode sn,
                JazzChunk.AwarenessLevel newValue, bool extendable)
                : base(sn, "AwarenessOverlayLevel", newValue, extendable)
            {
                this.mLabel = this.mLabel + "Awareness Overlay Level";
            }
        }

        public string Name
        {
            get { return this.mState.Name; }
            set
            {
                if (this.mState.Name != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new NameCommand(this, value, false));
                }
            }
        }

        public JazzState.Flags Flags
        {
            get { return this.mState.Flags; }
            set
            {
                if (this.mState.Flags != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new FlagsCommand(this, value, false));
                }
            }
        }

        public JazzChunk.AwarenessLevel AwarenessOverlayLevel
        {
            get { return this.mState.AwarenessOverlayLevel; }
            set
            {
                if (this.mState.AwarenessOverlayLevel != value)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new AwarenessCommand(this, value, false));
                }
            }
        }

        public bool DGLayoutRunning;

        public SimpleTreeLayoutAlgorithm<DGNode, DGEdge> DGLayout
        {
            get { return this.mLayout; }
        }

        public bool AsyncDriftTowardsCenter()
        {
            if (this.mDGraph.NodeCount == 0)
            {
                return false;
            }
            int i;
            // Calculate the net bounding box of all dg nodes
            DGNode node = this.mDGraph.NodeAt(0);
            RectangleF nbox = node.ChildrenBoundingBox();
            nbox.Offset(node.X, node.Y);
            RectangleF bbox = nbox;
            for (i = this.mDGraph.NodeCount - 1; i >= 1; i--)
            {
                node = this.mDGraph.NodeAt(i);
                if (!node.MouseGrabbed)
                {
                    nbox = node.ChildrenBoundingBox();
                    nbox.Offset(node.X, node.Y);
                    bbox = RectangleF.Union(bbox, nbox);
                }
            }
            // Calculate the center of the net bounding box
            float x = bbox.X + bbox.Width / 2;
            float y = bbox.Y + bbox.Height / 2;
            // Calculate the ideal position of this center
            nbox = this.mScene.BoundingBox;
            nbox.Offset(-this.X, -this.Y);
            float dx, dy;
            if (bbox.Width / 2 > nbox.Right)
            {
                dx = nbox.Right - bbox.Width / 2 - x;
            }
            else if (bbox.Width / -2 < nbox.X)
            {
                dx = nbox.X + bbox.Width / 2 - x;
            }
            else
            {
                dx = -x;
            }
            if (bbox.Height / 2 > nbox.Bottom)
            {
                dy = nbox.Bottom - bbox.Height / 2 - y;
            }
            else if (bbox.Height / -2 < nbox.Y)
            {
                dy = nbox.Y + bbox.Height / 2 - y;
            }
            else
            {
                dy = -y;
            }
            if (dx * dx < 0.01f && dy * dy < 0.01f)
            {
                return false;
            }
            // Drift the net bounding box's center towards origin
            for (i = this.mDGraph.NodeCount - 1; i >= 0; i--)
            {
                node = this.mDGraph.NodeAt(i);
                if (!node.MouseGrabbed)
                {
                    node.SetPosition(node.X + 0.1f * dx, node.Y + 0.1f * dy);
                }
            }
            return true;
        }

        public float Radius
        {
            get { return this.mRad; }
        }

        public float RadiusSquared
        {
            get { return this.mRadSquared; }
        }

        public void UpdateRadius()
        {
            float r = this.bMinimized ? sMinRad : sMaxRad;
            if (this.mRad != r)
            {
                this.mRad = r;
                this.mRadSquared = r * r;
                r += 10;
                this.BoundingBox = new RectangleF(-r, -r, 2 * r, 2 * r);
            }
        }

        public void UpdateEdges()
        {
            Digraph<StateNode, StateEdge>.GEdge edge;
            Digraph<StateNode, StateEdge> graph = this.mScene.StateGraph;
            int index = graph.IndexOfNode(this);
            for (int i = graph.EdgeCount - 1; i >= 0; i--)
            {
                edge = graph.InternalEdgeAt(i);
                if (edge.SrcNode.Index == index ||
                    edge.DstNode.Index == index)
                {
                    edge.Data.Update();
                }
            }
        }

        public bool InEditMode
        {
            get { return this.bInEditMode; }
            set
            {
                if (this.bInEditMode != value)
                {
                    this.bInEditMode = value;
                    int i;
                    DGNode node;
                    for (i = this.mDGraph.NodeCount - 1; i >= 0; i--)
                    {
                        node = this.mDGraph.NodeAt(i);
                        node.InEditMode = value;
                    }
                    DGEdge edge;
                    for (i = this.mDGraph.EdgeCount - 1; i >= 0; i--)
                    {
                        edge = this.mDGraph.EdgeAt(i);
                        edge.InEditMode = value;
                    }
                    this.Invalidate();
                }
            }
        }

        public bool Minimized
        {
            get { return this.bMinimized; }
        }

        public void SetMinimized(bool minimized)
        {
            if (this.bMinimized != minimized)
            {
                this.bMinimized = minimized;
                this.UpdateRadius();
                this.UpdateEdges();
                if (minimized)
                {
                    this.ClipsChildrenToShape = true;
                }
                int i;
                DGNode node;
                DGEdge edge;
                bool visible = !minimized;
                for (i = this.mDGraph.NodeCount - 1; i >= 0; i--)
                {
                    node = this.mDGraph.NodeAt(i);
                    node.Visible = visible;
                }
                for (i = this.mDGraph.EdgeCount - 1; i >= 0; i--)
                {
                    edge = this.mDGraph.EdgeAt(i);
                    edge.Visible = visible;
                }
            }
        }

        public Box2F LayoutBBox
        {
            get
            {
                float rad = this.mRad + 10;
                return new Box2F(-rad, -rad, 2 * rad, 2 * rad);
            }
        }

        public bool PositionFixed
        {
            get { return this.MouseGrabbed; }
        }

        public override Region Shape()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(-mRad, -mRad, 2 * mRad, 2 * mRad);
            return new Region(path);
        }

        public override bool Contains(PointF pt)
        {
            return (pt.X * pt.X + pt.Y * pt.Y) <= this.mRadSquared;
        }

        // State Properties:
        // Public: green background instead of blue background
        // Entry: monospaced "ENTER" at top
        // Exit: monospaced "EXIT" at bottom
        // Loop: three curved arrows circling around outer border
        // OneShot: 
        // OneShotHold:
        // Synchronized: 
        // Join: 
        // Explicit: bold outer border

        private static Color sBorderNormalColor = Color.Black;
        private static Color sBorderSelectColor = Color.Yellow;

        public static Color BorderNormalColor
        {
            get { return sBorderNormalColor; }
            set
            {
                if (!sBorderNormalColor.Equals(value))
                {
                    sBorderNormalColor = value;
                }
            }
        }

        public static Color BorderSelectedColor
        {
            get { return sBorderSelectColor; }
            set
            {
                if (!sBorderSelectColor.Equals(value))
                {
                    sBorderSelectColor = value;
                }
            }
        }

        private static SolidBrush sPublicMaxBrush 
            = new SolidBrush(Color.LightGreen);
        private static SolidBrush sPrivateMaxBrush 
            = new SolidBrush(Color.LightBlue);
        private static SolidBrush sPublicMinBrush 
            = new SolidBrush(Color.Orange);
        private static SolidBrush sPrivateMinBrush 
            = new SolidBrush(Color.Orange);

        public static Color PublicMaximizedColor
        {
            get { return sPublicMaxBrush.Color; }
            set
            {
                Color color = sPublicMaxBrush.Color;
                if (!color.Equals(value))
                {
                    sPublicMaxBrush.Color = value;
                }
            }
        }

        public static Color PrivateMaximizedColor
        {
            get { return sPrivateMaxBrush.Color; }
            set
            {
                Color color = sPrivateMaxBrush.Color;
                if (!color.Equals(value))
                {
                    sPrivateMaxBrush.Color = value;
                }
            }
        }

        public static Color PublicMinimizedColor
        {
            get { return sPublicMinBrush.Color; }
            set
            {
                Color color = sPublicMinBrush.Color;
                if (!color.Equals(value))
                {
                    sPublicMinBrush.Color = value;
                }
            }
        }

        public static Color PrivateMinimizedColor
        {
            get { return sPrivateMinBrush.Color; }
            set
            {
                Color color = sPrivateMinBrush.Color;
                if (!color.Equals(value))
                {
                    sPrivateMinBrush.Color = value;
                }
            }
        }

        private static SolidBrush sPublicTxtBrush
            = new SolidBrush(Color.Black);
        private static SolidBrush sPrivateTxtBrush
            = new SolidBrush(Color.Black);

        public static Color PublicTextColor
        {
            get { return sPublicTxtBrush.Color; }
            set
            {
                Color color = sPublicTxtBrush.Color;
                if (!color.Equals(value))
                {
                    sPublicTxtBrush.Color = value;
                }
            }
        }

        public static Color PrivateTextColor
        {
            get { return sPrivateTxtBrush.Color; }
            set
            {
                Color color = sPrivateTxtBrush.Color;
                if (!color.Equals(value))
                {
                    sPrivateTxtBrush.Color = value;
                }
            }
        }

        private static Font sTextFont;

        public static Font TextFont
        {
            get { return sTextFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("TextFont");
                }
                if (!sTextFont.Equals(value))
                {
                    sTextFont = value;
                }
            }
        }

        private readonly Pen mBorderPen = new Pen(Color.Black, 2);

        private static readonly float sArrowSize = 4;
        private static readonly Pen sArrowPen;

        private static readonly StringFormat sTextFormat;

        static StateNode()
        {
            /*GraphicsPath ahPath = new GraphicsPath();
            float sin = (float)(sArrowSize * Math.Sin(Math.PI / 3.0));
            float cos = sArrowSize * 0.5f;
            ahPath.AddPolygon(new PointF[] 
            { 
                new PointF(0, 0), 
                new PointF(cos, -sin), 
                new PointF(-cos, -sin) 
            });
            CustomLineCap arrowHead = new CustomLineCap(ahPath, null);/* */

            sArrowPen = new Pen(Color.Black, 1);
            sArrowPen.StartCap = LineCap.Round;
            //sArrowPen.CustomEndCap = arrowHead;
            sArrowPen.EndCap = LineCap.Round;
            sArrowPen.LineJoin = LineJoin.Round;

            sTextFormat = new StringFormat();
            sTextFormat.Alignment = StringAlignment.Center;
            sTextFormat.LineAlignment = StringAlignment.Center;
            sTextFont = new Font(
                FontFamily.GenericSansSerif, 10, FontStyle.Bold);
        }

        private const Keys kSelectKey = Keys.Control;

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            bool flag = base.OnMouseDown(e);
            if (!e.Handled)
            {
                if (this.ClipsChildrenToShape)
                {
                    this.mScene.SelectStateNode(this,
                        (Control.ModifierKeys & kSelectKey) != kSelectKey);
                }
                else
                {
                    return false;
                }
            }
            this.mScene.OnStateNodePressed(this);
            return flag;
        }

        protected override bool OnMouseMove(GraphMouseEventArgs e)
        {
            this.mScene.OnStateNodeHovered(this);
            return base.OnMouseMove(e);
        }

        protected override bool OnMouseDoubleClick(GraphMouseEventArgs e)
        {
            if (!e.Handled)
            {
                if (this.bMinimized)
                {
                    this.SetMinimized(false);
                }
                if (this.ClipsChildrenToShape)
                {
                    this.mScene.SetFocusedState(this);
                }
                else
                {
                    return false;
                }
            }
            return base.OnMouseDoubleClick(e);
        }

        private bool bSelected = false;

        public bool Selected
        {
            get { return this.bSelected; }
            set
            {
                if (this.bSelected != value)
                {
                    this.bSelected = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnDrawBackground(PaintEventArgs e)
        {
            Brush bgBrush;
            if (this.bMinimized)
            {
                bgBrush = this.mState.Public
                    ? sPublicMinBrush : sPrivateMinBrush;
            }
            else
            {
                bgBrush = this.mState.Public
                    ? sPublicMaxBrush : sPrivateMaxBrush;
            }
            Graphics g = e.Graphics;
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillEllipse(bgBrush, -mRad, -mRad, 2 * mRad, 2 * mRad);
            if (!this.ClipsChildrenToShape)
            {
                this.DrawStateDetails(g);
            }
            g.SmoothingMode = sm;
        }

        protected override void OnDrawForeground(PaintEventArgs e)
        {
            if (this.ClipsChildrenToShape)
            {
                Graphics g = e.Graphics;
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                this.DrawStateDetails(g);
                g.SmoothingMode = sm;
            }
        }

        private const double s13pi = Math.PI / 3;
        private const double s23pi = 2 * Math.PI / 3;

        private void DrawStateDetails(Graphics g)
        {
            float r = this.mRad;
            this.mBorderPen.Width = this.mState.Explicit ? 3 : 1;
            this.mBorderPen.Color = this.bSelected 
                ? sBorderSelectColor : sBorderNormalColor;

            g.DrawEllipse(this.mBorderPen, -r, -r, 2 * r, 2 * r);
            if (!this.bMinimized)
            {
                float h;
                SizeF size;
                string name = this.mState.Name;
                if (string.IsNullOrEmpty(name))
                {
                    name = "{NULL}";
                }
                Brush txtBrush = this.mState.Public 
                    ? sPublicTxtBrush : sPrivateTxtBrush;
                //h = (float)Math.Sqrt(r * r - (r - 10) * (r - 10));
                h = (float)Math.Sqrt(20 * r - 100);
                RectangleF box = new RectangleF(2 - r, -h, 2 * r - 4, 2 * h);
                g.DrawString(name, sTextFont, txtBrush, box, sTextFormat);
                if (this.mState.Entry)
                {
                    size = g.MeasureString("ENTER", sTextFont);
                    h = size.Width + 5;
                    h = (float)Math.Sqrt(2 * h * r - h * h);
                    box = new RectangleF(
                        -r, size.Height / 4 - h, 2 * r, size.Height);
                    g.DrawString("ENTER", sTextFont, 
                        txtBrush, box, sTextFormat);
                }
                if (this.mState.Exit)
                {
                    size = g.MeasureString("EXIT", sTextFont);
                    h = size.Width + 5;
                    h = (float)Math.Sqrt(2 * h * r - h * h);
                    box = new RectangleF(
                        -r, h - size.Height, 2 * r, size.Height);
                    g.DrawString("EXIT", sTextFont,
                        txtBrush, box, sTextFormat);
                }
            }
            if (this.mState.Loop)
            {
                sArrowPen.Color = this.bSelected
                    ? sBorderSelectColor : sBorderNormalColor;
                SolidBrush brush = new SolidBrush(this.bSelected
                    ? sBorderSelectColor : sBorderNormalColor);
                r += 5;
                double a, x, y, sz = 2 * sArrowSize;
                PointF[] pts = new PointF[3];
                g.DrawArc(sArrowPen, -r, -r, 2 * r, 2 * r, 0, 90);
                x = 0;
                y = r;
                a = Math.PI;
                pts[0] = new PointF((float)x, (float)y);
                pts[1] = new PointF(
                    (float)(x + Math.Sin(a - s13pi) * sz),
                    (float)(y + Math.Cos(a - s13pi) * sz));
                pts[2] = new PointF(
                    (float)(x + Math.Sin(a - s23pi) * sz),
                    (float)(y + Math.Cos(a - s23pi) * sz));
                g.FillPolygon(brush, pts);

                g.DrawArc(sArrowPen, -r, -r, 2 * r, 2 * r, 120, 90);
                a = -5 * Math.PI / 6;
                x = r * Math.Cos(a);
                y = r * Math.Sin(a);
                a = Math.PI / 3;
                pts[0] = new PointF((float)x, (float)y);
                pts[1] = new PointF(
                    (float)(x + Math.Sin(a - s13pi) * sz),
                    (float)(y + Math.Cos(a - s13pi) * sz));
                pts[2] = new PointF(
                    (float)(x + Math.Sin(a - s23pi) * sz),
                    (float)(y + Math.Cos(a - s23pi) * sz));
                g.FillPolygon(brush, pts);

                g.DrawArc(sArrowPen, -r, -r, 2 * r, 2 * r, 240, 90);
                a = -Math.PI / 6;
                x = r * Math.Cos(a);
                y = r * Math.Sin(a);
                a = -Math.PI / 3;
                pts[0] = new PointF((float)x, (float)y);
                pts[1] = new PointF(
                    (float)(x + Math.Sin(a - s13pi) * sz),
                    (float)(y + Math.Cos(a - s13pi) * sz));
                pts[2] = new PointF(
                    (float)(x + Math.Sin(a - s23pi) * sz),
                    (float)(y + Math.Cos(a - s23pi) * sz));
                g.FillPolygon(brush, pts);
            }
        }

        public bool Equals(StateNode other)
        {
            return other != null && other.mState.Equals(this.mState);
        }

        internal void ConnectOrphanDGNodes()
        {
            throw new NotImplementedException();
        }
    }
}
