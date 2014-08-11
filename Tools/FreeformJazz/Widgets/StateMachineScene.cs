using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using GraphForms;
using GraphForms.Algorithms;
using GraphForms.Algorithms.Layout;
using s3pi.GenericRCOLResource;
using s3piwrappers.CustomForms;
using s3piwrappers.Helpers.Undo;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class StateMachineScene : AGraphNodeScene, IClusterNode, IDisposable
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

        private class TempStateEdge : GraphElement
        {
            private static Pen sArrowPen;

            static TempStateEdge()
            {
                sArrowPen = new Pen(Color.Black, 1);
                sArrowPen.StartCap = LineCap.RoundAnchor;
                //sArrowPen.EndCap = LineCap.ArrowAnchor;
                sArrowPen.CustomEndCap = new AdjustableArrowCap(
                    StateEdge.sArrowSize, StateEdge.sArrowSize, true);
                sArrowPen.LineJoin = LineJoin.Round;
                sArrowPen.DashStyle = DashStyle.Dash;
            }

            private StateMachineScene mScene;
            private GraphicsPath mPath;

            public TempStateEdge(StateMachineScene scene)
            {
                this.mScene = scene;
                this.mPath = new GraphicsPath();
                this.IgnoreMouseEvents = true;
                this.Zvalue = -1;
                this.SetParent(scene);
            }

            public void Update()
            {
                StateNode srcNode = this.mScene.mLastStateNodePressed;
                StateNode dstNode = this.mScene.mLastStateNodeHovered;
                if (srcNode != null && dstNode != null)
                {
                    this.mPath.Reset();
                    this.BoundingBox = StateEdge.AddEdgePath(
                        srcNode, dstNode, this, this.mPath);
                }
                else
                {
                    this.Invalidate();
                }
            }

            protected override bool IsDrawn()
            {
                return this.mScene.bAddingEdges &&
                    this.mScene.mLastStateNodePressed != null &&
                    this.mScene.mLastStateNodeHovered != null;
            }

            protected override void OnDrawBackground(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                SmoothingMode sm = g.SmoothingMode;

                g.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    // Draw the line itself
                    sArrowPen.Color = StateEdge.BorderNormalColor;
                    g.DrawPath(sArrowPen, this.mPath);
                }
                catch (OverflowException)
                {
                }
                g.SmoothingMode = sm;
            }
        }

        private class TempDGEdge : GraphElement
        {
            private static Pen sArrowPen;

            static TempDGEdge()
            {
                sArrowPen = new Pen(Color.Black, 1);
                sArrowPen.StartCap = LineCap.RoundAnchor;
                //sArrowPen.EndCap = LineCap.ArrowAnchor;
                sArrowPen.CustomEndCap = new AdjustableArrowCap(3, 3);
                sArrowPen.LineJoin = LineJoin.Round;
                sArrowPen.DashStyle = DashStyle.Dash;
            }

            private StateMachineScene mScene;
            private GraphicsPath mPath;

            public TempDGEdge(StateMachineScene scene)
            {
                this.mScene = scene;
                this.mPath = new GraphicsPath();
                this.IgnoreMouseEvents = true;
                this.Zvalue = -1;
            }

            public void Update()
            {
                AnchorPoint srcAP = this.mScene.mLastAnchorPressed;
                AnchorPoint dstAP = this.mScene.mLastAnchorHovered;
                if (srcAP != null && srcAP.IsEntry)
                {
                    srcAP = this.mScene.mLastAnchorHovered;
                    dstAP = this.mScene.mLastAnchorPressed;
                }
                if (srcAP != null && dstAP != null)
                {
                    this.mPath.Reset();
                    this.BoundingBox = DGEdge.AddEdgePath(
                        srcAP, dstAP, this, this.mPath);
                }
                else
                {
                    this.Invalidate();
                }
            }

            protected override bool IsDrawn()
            {
                return this.mScene.bAddingEdges &&
                    this.mScene.mLastAnchorPressed != null &&
                    this.mScene.mLastAnchorHovered != null;
            }

            protected override void OnDrawBackground(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                SmoothingMode sm = g.SmoothingMode;

                g.SmoothingMode = SmoothingMode.AntiAlias;
                try
                {
                    // Draw the line itself
                    sArrowPen.Color = DGEdge.BorderInEditColor;
                    g.DrawPath(sArrowPen, this.mPath);
                }
                catch (OverflowException)
                {
                }
                g.SmoothingMode = sm;
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
        private JazzGraphContainer mContainer;
        private StateGraphHider mStateGraphHider;
        private TempStateEdge mTempStateEdge;
        private TempDGEdge mTempDGEdge;
        private Digraph<StateNode, StateEdge> mStateGraph;

        private DefActorList mActorDefinitions;
        private DefParamList mParamDefinitions;

        private bool bPendingLayout;
        private LayoutAlgorithm<StateNode, StateEdge> mLayout;
        private LayoutAlgorithm<StateNode, StateEdge> mPendingLayout;
        private System.Windows.Forms.Timer mLayoutTimer;

        public StateMachineScene(StateMachine stateMachine, 
            Control stateView, JazzGraphContainer container)
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException("stateMachine");
            }
            if (stateView == null)
            {
                throw new ArgumentNullException("stateView");
            }
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.mStateView = stateView;
            this.mStateMachine = stateMachine;
            this.mContainer = container;
            this.mStateGraphHider = new StateGraphHider(this);
            this.mTempStateEdge = new TempStateEdge(this);
            this.mTempDGEdge = new TempDGEdge(this);
            this.InitStateGraph();

            this.mActorDefinitions = new DefActorList(this);
            this.mParamDefinitions = new DefParamList(this);

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
            this.mLayoutTimer.Tick += 
                new EventHandler(this.OnLayoutTimerTick);
        }

        public void Dispose()
        {
            if (this.mLayoutTimer != null)
            {
                if (this.mLayoutTimer.Enabled)
                {
                    if (this.mLayout != null)
                    {
                        this.StopLayout();
                    }
                    if (this.bPendingLayout)
                    {
                        this.bPendingLayout = false;
                    }
                    System.Threading.Thread.Sleep(1500 / 25);
                    Application.DoEvents();
                    this.mLayoutTimer.Stop();
                }
                this.mLayoutTimer.Tick -= 
                    new EventHandler(this.OnLayoutTimerTick);
                this.mLayoutTimer.Dispose();
                this.mLayoutTimer = null;
            }
            if (this.mStateView != null)
            {
                this.RemoveView(this.mStateView);
                this.mStateView.Dispose();
                this.mStateView = null;
            }
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

        public JazzGraphContainer Container
        {
            get { return this.mContainer; }
        }

        public void RefreshStateGraph(bool refreshDecisionGraphs)
        {
            int i, j, k;
            DGEdge dgEdge;
            DGNode dgNode;
            AnchorPoint ap;
            StateEdge stateEdge;
            StateNode stateNode;
            GraphForms.GraphElement[] children;
            GraphForms.Algorithms.Digraph<DGNode, DGEdge> decisionGraph;
            if (refreshDecisionGraphs)
            {
                this.mStateMachine.RefreshHashNames();
            }
            for (i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
            {
                stateNode = this.mStateGraph.NodeAt(i);
                stateNode.UpdateEdges();
                if (refreshDecisionGraphs)
                {
                    decisionGraph = stateNode.DecisionGraph;
                    for (j = decisionGraph.NodeCount - 1; j >= 0; j--)
                    {
                        dgNode = decisionGraph.NodeAt(j);
                        dgNode.UpdateVisualization();
                        if (dgNode.HasChildren)
                        {
                            children = dgNode.Children;
                            for (k = children.Length - 1; k >= 0; k--)
                            {
                                ap = children[k] as AnchorPoint;
                                if (ap != null)
                                {
                                    ap.UpdateBoundingBox();
                                }
                            }
                        }
                    }
                    for (j = decisionGraph.EdgeCount - 1; j >= 0; j--)
                    {
                        dgEdge = decisionGraph.EdgeAt(j);
                        dgEdge.Update();
                    }
                }
            }
            for (i = this.mStateGraph.EdgeCount - 1; i >= 0; i--)
            {
                stateEdge = this.mStateGraph.EdgeAt(i);
                stateEdge.Update();
            }
            if (this.mStateView != null)
            {
                this.mStateView.BackColor = sBGColor;
                this.mStateView.Invalidate();
            }
            this.mTempStateEdge.Update();
            this.mTempDGEdge.Update();
        }

        /*private class FlagsCommand : JazzCommand
        {
            private StateMachineScene mSMS;
            private JazzStateMachine.Flags mOldVal;
            private JazzStateMachine.Flags mNewVal;
            private bool bExtendable;

            public FlagsCommand(StateMachineScene sms,
                JazzStateMachine.Flags newValue, bool extendable)
                : base(sms.mContainer)
            {
                this.mSMS = sms;
                this.mOldVal = sms.mStateMachine.Flags;
                this.mNewVal = newValue;
                this.bExtendable = extendable;
                this.mLabel = "Set State Machine Flags";
            }

            public override bool Execute()
            {
                this.mSMS.mStateMachine.Flags = this.mNewVal;
                return true;
            }

            public override void Undo()
            {
                this.mSMS.mStateMachine.Flags = this.mOldVal;
            }

            public override bool IsExtendable(Helpers.Command possibleExt)
            {
                if (!this.bExtendable)
                {
                    return false;
                }
                FlagsCommand fc = possibleExt as FlagsCommand;
                if (fc == null || fc.mSMS != this.mSMS ||
                    fc.mNewVal == this.mOldVal)
                {
                    return false;
                }
                return true;
            }

            public override void Extend(Helpers.Command possibleExt)
            {
                FlagsCommand fc = possibleExt as FlagsCommand;
                this.mNewVal = fc.mNewVal;
            }
        }/* */

        private class FlagsCommand
            : PropertyCommand<StateMachine, JazzStateMachine.Flags>
        {
            public FlagsCommand(StateMachineScene sms,
                JazzStateMachine.Flags newValue, bool extendable)
                : base(sms.mStateMachine, "Flags", newValue, extendable)
            {
                this.mLabel = "Set State Machine Flags";
            }
        }

        /*private class PriorityCommand : JazzCommand
        {
            private StateMachineScene mSMS;
            private JazzChunk.AnimationPriority mOldVal;
            private JazzChunk.AnimationPriority mNewVal;
            private bool bExtendable;

            public PriorityCommand(StateMachineScene sms,
                JazzChunk.AnimationPriority newValue, bool extendable)
                : base(sms.mContainer)
            {
                this.mSMS = sms;
                this.mOldVal = sms.mStateMachine.DefaultPriority;
                this.mNewVal = newValue;
                this.bExtendable = extendable;
                this.mLabel = "Set State Machine Default Priority";
            }

            public override bool Execute()
            {
                this.mSMS.mStateMachine.DefaultPriority = this.mNewVal;
                return true;
            }

            public override void Undo()
            {
                this.mSMS.mStateMachine.DefaultPriority = this.mOldVal;
            }

            public override bool IsExtendable(Helpers.Command possibleExt)
            {
                if (!this.bExtendable)
                {
                    return false;
                }
                PriorityCommand pc = possibleExt as PriorityCommand;
                if (pc == null || pc.mSMS != this.mSMS ||
                    pc.mNewVal == this.mOldVal)
                {
                    return false;
                }
                return true;
            }

            public override void Extend(Helpers.Command possibleExt)
            {
                PriorityCommand pc = possibleExt as PriorityCommand;
                this.mNewVal = pc.mNewVal;
            }
        }/* */

        private class PriorityCommand
            : PropertyCommand<StateMachine, JazzChunk.AnimationPriority>
        {
            public PriorityCommand(StateMachineScene sms,
                JazzChunk.AnimationPriority newValue, bool extendable)
                : base(sms.mStateMachine, "DefaultPriority", 
                newValue, extendable)
            {
                this.mLabel = "Set State Machine Default Priority";
            }
        }

        /*private class AwarenessCommand : JazzCommand
        {
            private StateMachineScene mSMS;
            private JazzChunk.AwarenessLevel mOldVal;
            private JazzChunk.AwarenessLevel mNewVal;
            private bool bExtendable;

            public AwarenessCommand(StateMachineScene sms,
                JazzChunk.AwarenessLevel newValue, bool extendable)
                : base(sms.mContainer)
            {
                this.mSMS = sms;
                this.mOldVal = sms.mStateMachine.AwarenessOverlayLevel;
                this.mNewVal = newValue;
                this.bExtendable = extendable;
                this.mLabel = "Set State Machine Awareness Overlay Level";
            }

            public override bool Execute()
            {
                this.mSMS.mStateMachine.AwarenessOverlayLevel = this.mNewVal;
                return true;
            }

            public override void Undo()
            {
                this.mSMS.mStateMachine.AwarenessOverlayLevel = this.mOldVal;
            }

            public override bool IsExtendable(Helpers.Command possibleExt)
            {
                if (!this.bExtendable)
                {
                    return false;
                }
                AwarenessCommand ac = possibleExt as AwarenessCommand;
                if (ac == null || ac.mSMS != this.mSMS ||
                    ac.mNewVal == this.mOldVal)
                {
                    return false;
                }
                return true;
            }

            public override void Extend(Helpers.Command possibleExt)
            {
                AwarenessCommand ac = possibleExt as AwarenessCommand;
                this.mNewVal = ac.mNewVal;
            }
        }/* */

        private class AwarenessCommand
            : PropertyCommand<StateMachine, JazzChunk.AwarenessLevel>
        {
            public AwarenessCommand(StateMachineScene sms,
                JazzChunk.AwarenessLevel newValue, bool extendable)
                : base(sms.mStateMachine, "AwarenessOverlayLevel", 
                newValue, extendable)
            {
                this.mLabel = "Set State Machine Awareness Overlay Level";
            }
        }

        private List<RefToActor> SlurpActors(ActorDefinition ad)
        {
            int i, j;
            RefToActor rta;
            DGNode dgn = null;
            Digraph<DGNode, DGEdge> dg;
            List<RefToActor> actorList = new List<RefToActor>();
            for (i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
            {
                dg = this.mStateGraph.NodeAt(i).DecisionGraph;
                for (j = dg.NodeCount - 1; j >= 0; j--)
                {
                    dgn = dg.NodeAt(j);
                    if (dgn is DGMulticastNode)
                    {
                        if (dgn is DGAcOpNode)
                        {
                            rta = (dgn as DGAcOpNode).Actor;
                            if (ad == null || rta.GetValue() == ad)
                            {
                                actorList.Add(rta);
                            }
                        }
                        else if (dgn is DGPropNode)
                        {
                            rta = (dgn as DGPropNode).PropActor;
                            if (ad == null || rta.GetValue() == ad)
                            {
                                actorList.Add(rta);
                            }
                        }
                        else if (dgn is DGAnimNode)
                        {
                            rta = (dgn as DGAnimNode).Actor;
                            if (ad == null || rta.GetValue() == ad)
                            {
                                actorList.Add(rta);
                            }
                            if (dgn is DGPlayNode)
                            {
                                foreach (SlotBuilder.ActorSuffix suffix in
                                    (dgn as DGPlayNode).SlotSetup.SuffixList)
                                {
                                    rta = suffix.Actor;
                                    if (ad == null || rta.GetValue() == ad)
                                    {
                                        actorList.Add(rta);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return actorList;
        }

        public class DefActorList : List<DefActor>, 
            s3piwrappers.Helpers.Collections.IHasGenericAdd,
            s3piwrappers.Helpers.Collections.IHasGenericInsert
        {
            private StateMachineScene mScene;

            public DefActorList(StateMachineScene scene)
            {
                if (scene == null)
                {
                    throw new ArgumentNullException("scene");
                }
                this.mScene = scene;
                if (this.mScene.mStateMachine.ActorDefinitionCount > 0)
                {
                    ActorDefinition[] adList 
                        = this.mScene.mStateMachine.ActorDefinitions;
                    for (int i = 0; i < adList.Length; i++)
                    {
                        base.Add(new DefActor(adList[i], scene));
                    }
                }
            }

            private void CommitChanges()
            {
                this.mScene.mStateMachine.ClearActorDefinitions();
                ActorDefinition ad;
                Enumerator e = this.GetEnumerator();
                while (e.MoveNext())
                {
                    ad = e.Current.GetActor();
                    this.mScene.mStateMachine.AddActorDefinition(ad);
                }
            }

            private class AddNewCommand : Command
            {
                private DefActorList mList;
                private DefActor mItem;

                public AddNewCommand(DefActorList list, DefActor item)
                {
                    this.mList = list;
                    this.mItem = item;
                    this.mLabel = "Add New Actor to State Machine";
                }

                public override bool Execute()
                {
                    this.mList.Add(this.mItem);
                    this.mList.mScene.mStateMachine.AddActorDefinition(
                        this.mItem.GetActor());
                    return true;
                }

                public override void Undo()
                {
                    this.mList.RemoveAt(this.mList.Count - 1, false);
                    this.mList.mScene.mStateMachine.RemoveActorDefinition(
                        this.mItem.GetActor());
                }
            }

            private class InsertNewCommand : Command
            {
                private DefActorList mList;
                private DefActor mItem;
                private int mIndex;

                public InsertNewCommand(DefActorList list, 
                    DefActor item, int index)
                {
                    this.mList = list;
                    this.mItem = item;
                    this.mIndex = index;
                    this.mLabel = "Insert New Actor into State Machine";
                }

                public override bool Execute()
                {
                    this.mList.Insert(this.mIndex, this.mItem);
                    this.mList.CommitChanges();
                    return true;
                }

                public override void Undo()
                {
                    this.mList.RemoveAt(this.mIndex, false);
                    this.mList.CommitChanges();
                }
            }

            private class RemoveAtCommand : Command
            {
                private DefActorList mList;
                private DefActor mItem;
                private int mIndex;
                private List<RefToActor> mRefs;

                public RemoveAtCommand(DefActorList list, int index)
                {
                    this.mList = list;
                    this.mItem = list[index];
                    this.mIndex = index;
                    this.mRefs = this.mList.mScene.SlurpActors(
                        this.mItem.GetActor());
                    this.mLabel = "Remove Actor from State Machine";
                }

                public override bool Execute()
                {
                    this.mList.RemoveAt(this.mIndex, false);
                    this.mList.CommitChanges();
                    foreach (RefToActor rta in this.mRefs)
                    {
                        rta.SetValue(null);
                    }
                    return true;
                }

                public override void Undo()
                {
                    this.mList.Insert(this.mIndex, this.mItem);
                    ActorDefinition ad = this.mItem.GetActor();
                    this.mList.CommitChanges();
                    foreach (RefToActor rta in this.mRefs)
                    {
                        rta.SetValue(ad);
                    }
                }
            }

            private DefActor CreateActorPrompt()
            {
                SingleStringPrompt ssp = new SingleStringPrompt(
                    "Enter a name for the new Actor:",
                    MainForm.kName + ": Create New Actor", "x");
                if (ssp.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }
                ActorDefinition ad = new ActorDefinition(ssp.Response);
                return new DefActor(ad, this.mScene);
            }

            public bool Add()
            {
                DefActor actor = this.CreateActorPrompt();
                if (actor == null)
                {
                    return false;
                }
                this.mScene.Container.UndoRedo.Submit(
                    new AddNewCommand(this, actor));
                return true;
            }

            public bool Insert(int index)
            {
                DefActor actor = this.CreateActorPrompt();
                if (actor == null)
                {
                    return false;
                }
                this.mScene.Container.UndoRedo.Submit(
                    new InsertNewCommand(this, actor, index));
                return true;
            }

            public new void RemoveAt(int index)
            {
                this.RemoveAt(index, false);
            }

            private void RemoveAt(int index, bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new RemoveAtCommand(this, index));
                }
                else
                {
                    base.RemoveAt(index);
                }
            }
        }

        private List<RefToParam> SlurpParams(ParamDefinition pd)
        {
            int i, j;
            RefToParam rtp;
            DGNode dgn = null;
            Digraph<DGNode, DGEdge> dg;
            List<RefToParam> paramList = new List<RefToParam>();
            for (i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
            {
                dg = this.mStateGraph.NodeAt(i).DecisionGraph;
                for (j = dg.NodeCount - 1; j >= 0; j--)
                {
                    dgn = dg.NodeAt(j);
                    if (dgn is DGSoPnNode)
                    {
                        rtp = (dgn as DGSoPnNode).Parameter;
                        if (pd == null || rtp.GetValue() == pd)
                        {
                            paramList.Add(rtp);
                        }
                    }
                    else if (dgn is DGMulticastNode)
                    {
                        if (dgn is DGPropNode)
                        {
                            rtp = (dgn as DGPropNode).PropParam;
                            if (pd == null || rtp.GetValue() == pd)
                            {
                                paramList.Add(rtp);
                            }
                        }
                        else if (dgn is DGPlayNode)
                        {
                            foreach (SlotBuilder.ActorSuffix suffix in
                                (dgn as DGPlayNode).SlotSetup.SuffixList)
                            {
                                rtp = suffix.Param;
                                if (pd == null || rtp.GetValue() == pd)
                                {
                                    paramList.Add(rtp);
                                }
                            }
                        }
                    }
                }
            }
            return paramList;
        }

        public class DefParamList : List<DefParam>,
            s3piwrappers.Helpers.Collections.IHasGenericAdd,
            s3piwrappers.Helpers.Collections.IHasGenericInsert
        {
            private StateMachineScene mScene;

            public DefParamList(StateMachineScene scene)
            {
                if (scene == null)
                {
                    throw new ArgumentNullException("scene");
                }
                this.mScene = scene;
                if (this.mScene.mStateMachine.ParamDefinitionCount > 0)
                {
                    ParamDefinition[] pdList
                        = this.mScene.mStateMachine.ParamDefinitions;
                    for (int i = 0; i < pdList.Length; i++)
                    {
                        base.Add(new DefParam(pdList[i], scene));
                    }
                }
            }

            private void CommitChanges()
            {
                this.mScene.mStateMachine.ClearParamDefinitions();
                ParamDefinition pd;
                Enumerator e = this.GetEnumerator();
                while (e.MoveNext())
                {
                    pd = e.Current.GetParam();
                    this.mScene.mStateMachine.AddParamDefinition(pd);
                }
            }

            private class AddNewCommand : Command
            {
                private DefParamList mList;
                private DefParam mItem;

                public AddNewCommand(DefParamList list, DefParam item)
                {
                    this.mList = list;
                    this.mItem = item;
                    this.mLabel = "Add New Parameter to State Machine";
                }

                public override bool Execute()
                {
                    this.mList.Add(this.mItem);
                    this.mList.mScene.mStateMachine.AddParamDefinition(
                        this.mItem.GetParam());
                    return true;
                }

                public override void Undo()
                {
                    this.mList.RemoveAt(this.mList.Count - 1, false);
                    this.mList.mScene.mStateMachine.RemoveParamDefinition(
                        this.mItem.GetParam());
                }
            }

            private class InsertNewCommand : Command
            {
                private DefParamList mList;
                private DefParam mItem;
                private int mIndex;

                public InsertNewCommand(DefParamList list,
                    DefParam item, int index)
                {
                    this.mList = list;
                    this.mItem = item;
                    this.mIndex = index;
                    this.mLabel = "Insert New Actor into State Machine";
                }

                public override bool Execute()
                {
                    this.mList.Insert(this.mIndex, this.mItem);
                    this.mList.CommitChanges();
                    return true;
                }

                public override void Undo()
                {
                    this.mList.RemoveAt(this.mIndex, false);
                    this.mList.CommitChanges();
                }
            }

            private class RemoveAtCommand : Command
            {
                private DefParamList mList;
                private DefParam mItem;
                private int mIndex;
                private List<RefToParam> mRefs;

                public RemoveAtCommand(DefParamList list, int index)
                {
                    this.mList = list;
                    this.mItem = list[index];
                    this.mIndex = index;
                    this.mRefs = this.mList.mScene.SlurpParams(
                        this.mItem.GetParam());
                    this.mLabel = "Remove Actor from State Machine";
                }

                public override bool Execute()
                {
                    this.mList.RemoveAt(this.mIndex, false);
                    this.mList.CommitChanges();
                    foreach (RefToParam rtp in this.mRefs)
                    {
                        rtp.SetValue(null);
                    }
                    return true;
                }

                public override void Undo()
                {
                    this.mList.Insert(this.mIndex, this.mItem);
                    ParamDefinition pd = this.mItem.GetParam();
                    this.mList.CommitChanges();
                    foreach (RefToParam rtp in this.mRefs)
                    {
                        rtp.SetValue(pd);
                    }
                }
            }

            private DefParam CreateParamPrompt()
            {
                SingleStringPrompt ssp = new SingleStringPrompt(
                    "Enter a name for the new Parameter:",
                    MainForm.kName + ": Create New Parameter", "x:Age");
                if (ssp.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }
                ParamDefinition pd = new ParamDefinition(ssp.Response);
                return new DefParam(pd, this.mScene);
            }

            public bool Add()
            {
                DefParam param = this.CreateParamPrompt();
                if (param == null)
                {
                    return false;
                }
                this.mScene.Container.UndoRedo.Submit(
                    new AddNewCommand(this, param));
                return true;
            }

            public bool Insert(int index)
            {
                DefParam param = this.CreateParamPrompt();
                if (param == null)
                {
                    return false;
                }
                this.mScene.Container.UndoRedo.Submit(
                    new InsertNewCommand(this, param, index));
                return true;
            }

            public new void RemoveAt(int index)
            {
                this.RemoveAt(index, false);
            }

            private void RemoveAt(int index, bool viaCmd)
            {
                if (viaCmd)
                {
                    this.mScene.Container.UndoRedo.Submit(
                        new RemoveAtCommand(this, index));
                }
                else
                {
                    base.RemoveAt(index);
                }
            }
        }

        private List<RefToState> SlurpStates(StateNode stateNode)
        {
            int i, j;
            RefToState rts;
            DGNode dgn = null;
            Digraph<DGNode, DGEdge> dg;
            State state = stateNode.State;
            List<RefToState> stateList = new List<RefToState>();
            for (i = this.mStateGraph.NodeCount - 1; i >= 0; i--)
            {
                dg = this.mStateGraph.NodeAt(i).DecisionGraph;
                for (j = dg.NodeCount - 1; j >= 0; j--)
                {
                    dgn = dg.NodeAt(j);
                    if (dgn is DGSnSnNode)
                    {
                        rts = (dgn as DGSnSnNode).NextState;
                        if (state == null || rts.GetValue() == state)
                        {
                            stateList.Add(rts);
                        }
                    }
                    else if (dgn is DGSoDnNode)
                    {
                        
                    }
                }
            }
            return stateList;
        }

        public DefActorList ActorDefinitions
        {
            get { return this.mActorDefinitions; }
            set
            {
                if (this.mActorDefinitions != value)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public DefParamList ParamDefinitions
        {
            get { return this.mParamDefinitions; }
            set
            {
                if (this.mParamDefinitions != value)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public JazzStateMachine.Flags Flags
        {
            get { return this.mStateMachine.Flags; }
            set
            {
                if (this.mStateMachine.Flags != value)
                {
                    this.mContainer.UndoRedo.Submit(
                        new FlagsCommand(this, value, false));
                }
            }
        }

        public JazzChunk.AnimationPriority DefaultPriority
        {
            get { return this.mStateMachine.DefaultPriority; }
            set
            {
                if (this.mStateMachine.DefaultPriority != value)
                {
                    this.mContainer.UndoRedo.Submit(
                        new PriorityCommand(this, value, false));
                }
            }
        }

        public JazzChunk.AwarenessLevel AwarenessOverlayLevel
        {
            get { return this.mStateMachine.AwarenessOverlayLevel; }
            set
            {
                if (this.mStateMachine.AwarenessOverlayLevel != value)
                {
                    this.mContainer.UndoRedo.Submit(
                        new AwarenessCommand(this, value, false));
                }
            }
        }

        public LayoutAlgorithm<StateNode, StateEdge> Layout
        {
            get { return this.mLayout; }
            set
            {
                if (this.mLayout != value)
                {
                    if (this.mLayout != null && 
                        this.mLayoutTimer != null &&
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
            get 
            { 
                return this.mLayoutTimer != null && 
                       this.mLayoutTimer.Enabled; 
            }
        }

        public void StartLayout()
        {
            if (this.mLayoutTimer != null && !this.mLayoutTimer.Enabled)
            {
                this.mLayoutTimer.Start();
            }
        }

        public void StopLayout()
        {
            if (this.mLayout != null && 
                this.mLayoutTimer != null && this.mLayoutTimer.Enabled)
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
                // Run one iteration of the automated state graph layout
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
                // Run one iteration of each automated decision graph layout
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
                // Update the bounding box of the scene
                RectangleF bbox = this.BoundingBox;
                if (this.mStateView != null)
                {
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
                }
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
                // Update the temporary edges pending addition to the graphs
                this.UpdateLastHovered();
                this.mTempStateEdge.Update();
                this.mTempDGEdge.Update();
                // Check to see if the layout timer should continue running
                if (!keep && this.mLayoutTimer != null)
                {
                    this.mLayoutTimer.Stop();
                }
            }
        }

        private void UpdateLastHovered()
        {
            if (this.mStateView == null)
            {
                this.mLastStateNodeHovered = null;
                this.mLastAnchorHovered = null;
                return;
            }
            Point p = this.mStateView.PointToClient(Cursor.Position);
            if (p.X < 0 || p.X > this.mStateView.Width ||
                p.Y < 0 || p.Y > this.mStateView.Height)
            {
                this.mLastStateNodeHovered = null;
                this.mLastAnchorHovered = null;
                return;
            }
            float dx, dy;
            float x = p.X - this.X;
            float y = p.Y - this.Y;
            if (this.mLastStateNodePressed != null &&
                this.mLastStateNodeHovered != null)
            {
                dx = x - this.mLastStateNodeHovered.X;
                dy = y - this.mLastStateNodeHovered.Y;
                if ((dx * dx + dy * dy) >
                    this.mLastStateNodeHovered.RadiusSquared)
                {
                    this.mLastStateNodeHovered = null;
                    StateNode state;
                    GraphElement[] states = this.Children;
                    for (int j = states.Length - 1; j >= 0; j--)
                    {
                        state = states[j] as StateNode;
                        if (state == null)
                        {
                            break;
                        }
                        dx = x - state.X;
                        dy = y - state.Y;
                        if ((dx * dx + dy * dy) <= state.RadiusSquared)
                        {
                            this.bHoverInvoked = false;
                            this.OnStateNodeHovered(state);
                            break;
                        }
                    }
                }
            }
            if (this.mLastAnchorPressed != null &&
                this.mLastAnchorHovered != null)
            {
                float rad2 = AnchorPoint.Radius * AnchorPoint.Radius;
                DGNode dgn = this.mLastAnchorHovered.Owner;
                StateNode sn = dgn.State;
                x = x - sn.X;
                y = y - sn.Y;
                dx = x - dgn.X - this.mLastAnchorHovered.X;
                dy = y - dgn.Y - this.mLastAnchorHovered.Y;
                if ((dx * dx + dy * dy) > rad2)
                {
                    this.mLastAnchorHovered = null;
                    bool flag = true;
                    int i, k;
                    AnchorPoint ap;
                    GraphElement[] aps;
                    GraphElement[] dgns = sn.Children;
                    for (i = dgns.Length - 1; i >= 0 && flag; i--)
                    {
                        dgn = dgns[i] as DGNode;
                        if (dgn == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            dx = x - dgn.X;
                            dy = y - dgn.Y;
                            aps = dgn.Children;
                            for (k = aps.Length - 1; k >= 0 && flag; k--)
                            {
                                ap = aps[k] as AnchorPoint;
                                if (ap != null)
                                {
                                    dx = dx - ap.X;
                                    dy = dy - ap.Y;
                                    if ((dx * dx + dy * dy) <= rad2)
                                    {
                                        this.bHoverInvoked = false;
                                        this.OnAnchorHovered(ap);
                                        flag = false;
                                    }
                                    else
                                    {
                                        dx = dx + ap.X;
                                        dy = dy + ap.Y;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnNodeMovedByMouse(AGraphNode node)
        {
            if (this.mStateView == null)
            {
                return;
            }
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

        private class AddStateNodeCommand : Command
        {
            private StateMachineScene mSMS;
            private StateNode mState;

            public AddStateNodeCommand(
                StateMachineScene sms, StateNode state)
            {
                this.mSMS = sms;
                this.mState = state;
                this.mLabel = "Add New State Node";
            }

            public override bool Execute()
            {
                this.mSMS.mStateMachine.AddState(this.mState.State);
                this.mState.SetParent(this.mSMS);
                this.mSMS.mStateGraph.AddNode(this.mState);
                if (this.mState.Selected)
                {
                    if (this.mSMS.mFocusedState == null)
                    {
                        this.mSMS.mSelectedStateNodes.Add(this.mState);
                        if (this.mSMS.StateNodeSelectionChanged != null)
                        {
                            this.mSMS.StateNodeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    else
                    {
                        this.mState.Selected = false;
                    }
                }
                return true;
            }

            public override void Undo()
            {
                this.mSMS.mStateMachine.RemoveState(this.mState.State);
                this.mSMS.mStateGraph.RemoveNode(this.mState);
                this.mState.SetParent(null);
                if (this.mState.Selected && this.mSMS.mFocusedState == null)
                {
                    this.mSMS.mSelectedStateNodes.Remove(this.mState);
                    if (this.mSMS.StateNodeSelectionChanged != null)
                    {
                        this.mSMS.StateNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public bool AddNewStateNode()
        {
            if (this.mFocusedState != null)
            {
                return false;
            }
            SingleStringPrompt ssp = new SingleStringPrompt(
                "Enter a name for the new State:",
                MainForm.kName + ": Create New State", "");
            if (ssp.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            State state = new State(ssp.Response);
            state.DecisionGraph = new DecisionGraph();
            StateNode stateNode = new StateNode(state, this);
            this.mContainer.UndoRedo.Submit(
                new AddStateNodeCommand(this, stateNode));
            return true;
        }

        private class RemoveStateNodesCommand : Command
        {
            private StateMachineScene mSMS;
            private StateNode[] mNodes;
            private List<RefToState>[] mRefs;
            private List<StateEdge> mEdges;
            private List<int> mSelectedEdges;

            public RemoveStateNodesCommand(StateMachineScene sms)
            {
                int i;
                this.mSMS = sms;
                this.mNodes = sms.mSelectedStateNodes.ToArray();
                this.mRefs = new List<RefToState>[this.mNodes.Length];
                for (i = this.mNodes.Length - 1; i >= 0; i--)
                {
                    this.mRefs[i] = sms.SlurpStates(this.mNodes[i]);
                }
                this.mEdges = new List<StateEdge>();
                this.mSelectedEdges = new List<int>();
                int j;
                StateNode node;
                StateEdge edge;
                Digraph<StateNode, StateEdge> sg = this.mSMS.mStateGraph;
                for (i = sg.EdgeCount - 1; i >= 0; i--)
                {
                    edge = sg.EdgeAt(i);
                    for (j = this.mNodes.Length - 1; j >= 0; j--)
                    {
                        node = this.mNodes[j];
                        if (edge.SrcNode == node || edge.DstNode == node)
                        {
                            if (edge.Selected)
                            {
                                this.mSelectedEdges.Add(this.mEdges.Count);
                            }
                            this.mEdges.Add(edge);
                            break;
                        }
                    }
                }
                this.mLabel = "Remove Selected State Nodes";
            }
            public override bool Execute()
            {
                int i, j;
                StateNode node;
                StateEdge edge;
                List<RefToState> rtsList;
                Digraph<StateNode, StateEdge> sg = this.mSMS.mStateGraph;
                for (i = this.mEdges.Count - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SrcNode.State.RemoveTransition(edge.DstNode.State);
                    sg.RemoveEdge(edge.SrcNode, edge.DstNode);
                    edge.SetParent(null);
                    edge.Selected = false;
                }
                for (i = this.mNodes.Length - 1; i >= 0; i--)
                {
                    node = this.mNodes[i];
                    this.mSMS.mStateMachine.RemoveState(node.State);
                    sg.RemoveNode(node);
                    node.SetParent(null);
                    node.Selected = false;
                    rtsList = this.mRefs[i];
                    for (j = rtsList.Count - 1; j >= 0; j--)
                    {
                        rtsList[j].SetValue(null);
                    }
                }
                if (this.mSMS.mFocusedState == null)
                {
                    if (this.mSelectedEdges.Count > 0)
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            this.mSMS.mSelectedStateEdges.Remove(
                                this.mEdges[this.mSelectedEdges[i]]);
                        }
                        if (this.mSMS.StateEdgeSelectionChanged != null)
                        {
                            this.mSMS.StateEdgeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    this.mSMS.mSelectedStateNodes.Clear();
                    if (this.mSMS.StateNodeSelectionChanged != null)
                    {
                        this.mSMS.StateNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
                return true;
            }

            public override void Undo()
            {
                int i, j;
                State state;
                StateNode node;
                StateEdge edge;
                List<RefToState> rtsList;
                Digraph<StateNode, StateEdge> sg = this.mSMS.mStateGraph;
                for (i = this.mNodes.Length - 1; i >= 0; i--)
                {
                    node = this.mNodes[i];
                    this.mSMS.mStateMachine.AddState(node.State);
                    node.SetParent(this.mSMS);
                    sg.AddNode(node);
                    state = node.State;
                    rtsList = this.mRefs[i];
                    for (j = rtsList.Count - 1; j >= 0; j--)
                    {
                        rtsList[j].SetValue(state);
                    }
                }
                for (i = this.mEdges.Count - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SrcNode.State.AddTransition(edge.DstNode.State);
                    edge.SetParent(this.mSMS);
                    sg.AddEdge(edge);
                }
                if (this.mSMS.mFocusedState == null)
                {
                    if (this.mSelectedEdges.Count > 0)
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mEdges[this.mSelectedEdges[i]];
                            edge.Selected = true;
                            this.mSMS.mSelectedStateEdges.Add(edge);
                        }
                        if (this.mSMS.StateEdgeSelectionChanged != null)
                        {
                            this.mSMS.StateEdgeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    for (i = this.mNodes.Length - 1; i >= 0; i--)
                    {
                        node = this.mNodes[i];
                        node.Selected = true;
                        this.mSMS.mSelectedStateNodes.Add(node);
                    }
                    if (this.mSMS.StateNodeSelectionChanged != null)
                    {
                        this.mSMS.StateNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public void RemoveSelectedStateNodes()
        {
            if (this.mSelectedStateNodes.Count > 0)
            {
                this.mContainer.UndoRedo.Submit(
                    new RemoveStateNodesCommand(this));
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

        private class AddStateEdgeCommand : Command
        {
            private StateMachineScene mScene;
            private StateEdge mEdge;

            public AddStateEdgeCommand(StateMachineScene scene, 
                StateNode srcNode, StateNode dstNode)
            {
                this.mScene = scene;
                this.mEdge = new StateEdge(srcNode, dstNode);
                this.mLabel = "Add New State Transition";
            }

            public override bool Execute()
            {
                this.mEdge.SetParent(this.mScene);
                this.mScene.mStateGraph.AddEdge(this.mEdge);
                this.mEdge.SrcNode.State.AddTransition(
                    this.mEdge.DstNode.State);
                return true;
            }

            public override void Undo()
            {
                this.mScene.mStateGraph.RemoveEdge(
                    this.mEdge.SrcNode, this.mEdge.DstNode);
                this.mEdge.SetParent(null);
                this.mEdge.SrcNode.State.RemoveTransition(
                    this.mEdge.DstNode.State);
            }
        }

        private class RemoveStateEdgesCommand : Command
        {
            private StateMachineScene mSMS;
            private StateEdge[] mEdges;

            public RemoveStateEdgesCommand(StateMachineScene sms)
            {
                this.mSMS = sms;
                this.mEdges = sms.mSelectedStateEdges.ToArray();
                this.mLabel = "Remove Selected State Transitions";
            }

            public override bool Execute()
            {
                StateEdge edge;
                Digraph<StateNode, StateEdge> sg = this.mSMS.mStateGraph;
                for (int i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SrcNode.State.RemoveTransition(edge.DstNode.State);
                    sg.RemoveEdge(edge.SrcNode, edge.DstNode);
                    edge.SetParent(null);
                    edge.Selected = false;
                }
                if (this.mSMS.mFocusedState == null)
                {
                    this.mSMS.mSelectedStateEdges.Clear();
                    if (this.mSMS.StateEdgeSelectionChanged != null)
                    {
                        this.mSMS.StateEdgeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
                return true;
            }

            public override void Undo()
            {
                int i;
                StateEdge edge;
                Digraph<StateNode, StateEdge> sg = this.mSMS.mStateGraph;
                for (i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SrcNode.State.AddTransition(edge.DstNode.State);
                    edge.SetParent(this.mSMS);
                    sg.AddEdge(edge);
                }
                if (this.mSMS.mFocusedState == null)
                {
                    for (i = this.mEdges.Length - 1; i >= 0; i--)
                    {
                        edge = this.mEdges[i];
                        edge.Selected = true;
                        this.mSMS.mSelectedStateEdges.Add(edge);
                    }
                    if (this.mSMS.StateEdgeSelectionChanged != null)
                    {
                        this.mSMS.StateEdgeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public void RemoveSelectedStateEdges()
        {
            if (this.mSelectedStateEdges.Count > 0)
            {
                this.mContainer.UndoRedo.Submit(
                    new RemoveStateEdgesCommand(this));
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
            if (this.mSelectedDGNodes.Count > 0)
            {
                foreach (DGNode node in this.mSelectedDGNodes)
                {
                    node.Selected = false;
                }
                this.mSelectedDGNodes.Clear();

                if (this.DGNodeSelectionChanged != null)
                {
                    this.DGNodeSelectionChanged(this, new EventArgs());
                }
            }
        }

        private class AddDGNodeCommand : Command
        {
            private StateMachineScene mSMS;
            private StateNode mState;
            private DGNode mNode;
            private DGRootNode mRootNode;

            public AddDGNodeCommand(
                StateMachineScene sms, StateNode state, DGNode node)
            {
                this.mSMS = sms;
                this.mState = state;
                this.mNode = node;
                if (state.RootNode == null)
                {
                    this.mRootNode
                        = new DGRootNode(new DecisionGraph(), state);
                }
                else
                {
                    this.mRootNode = null;
                }
                this.mLabel = "Add New DG Node";
            }

            public override bool Execute()
            {
                if (this.mRootNode != null)
                {
                    this.mState.State.DecisionGraph 
                        = this.mRootNode.DecisionGraph;
                    this.mRootNode.SetParent(this.mState);
                    this.mState.DecisionGraph.InsertNode(0, this.mRootNode);
                    this.mState.UpdateRootNode(false);
                }
                if (this.mNode.IsDecisionMaker)
                {
                    this.mState.State.DecisionGraph.AddDecisionMaker(
                        this.mNode.DGN);
                }
                if (this.mNode.IsEntryPoint)
                {
                    this.mState.State.DecisionGraph.AddEntryPoint(
                        this.mNode.DGN);
                }
                this.mNode.SetParent(this.mState);
                this.mState.DecisionGraph.AddNode(this.mNode);
                this.mState.AddRootDGEdges();
                if (this.mNode.Selected)
                {
                    if (this.mSMS.mFocusedState != null &&
                        this.mSMS.mFocusedState.Equals(this.mState))
                    {
                        this.mSMS.mSelectedDGNodes.Add(this.mNode);
                        if (this.mSMS.DGNodeSelectionChanged != null)
                        {
                            this.mSMS.DGNodeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    else
                    {
                        this.mNode.Selected = false;
                    }
                }
                return true;
            }

            public override void Undo()
            {
                this.mState.State.DecisionGraph.Remove(this.mNode.DGN);
                this.mState.DecisionGraph.RemoveNode(this.mNode);
                this.mState.RemoveRootDGEdges();
                this.mNode.SetParent(null);
                if (this.mRootNode != null)
                {
                    this.mState.State.DecisionGraph = null;
                    this.mState.DecisionGraph.RemoveNodeAt(0);
                    this.mState.UpdateRootNode(false);
                    this.mRootNode.SetParent(null);
                }
                if (this.mNode.Selected &&
                    this.mSMS.mFocusedState != null && 
                    this.mSMS.mFocusedState.Equals(this.mState))
                {
                    this.mSMS.mSelectedDGNodes.Remove(this.mNode);
                    if (this.mSMS.DGNodeSelectionChanged != null)
                    {
                        this.mSMS.DGNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public bool AddNewDGNode()
        {
            if (this.mFocusedState == null)
            {
                return false;
            }
            NewDGNodePrompt ndgnp = new NewDGNodePrompt();
            if (ndgnp.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            DGNode dgNode = null;
            switch (ndgnp.NodeType)
            {
                case NextStateNode.ResourceType:
                    dgNode = new DGSnSnNode(new NextStateNode(), 
                        this.mFocusedState);
                    break;
                case RandomNode.ResourceType:
                    dgNode = new DGRandNode(new RandomNode(), 
                        this.mFocusedState);
                    break;
                case SelectOnDestinationNode.ResourceType:
                    dgNode = new DGSoDnNode(new SelectOnDestinationNode(), 
                        this.mFocusedState);
                    break;
                case SelectOnParameterNode.ResourceType:
                    dgNode = new DGSoPnNode(new SelectOnParameterNode(), 
                        this.mFocusedState);
                    break;
                case ActorOperationNode.ResourceType:
                    dgNode = new DGAcOpNode(new SetMirrorNode(), 
                        this.mFocusedState);
                    break;
                case CreatePropNode.ResourceType:
                    dgNode = new DGPropNode(new CreatePropNode(), 
                        this.mFocusedState);
                    break;
                case PlayAnimationNode.ResourceType:
                    dgNode = new DGPlayNode(new PlayAnimationNode(), 
                        this.mFocusedState);
                    break;
                case StopAnimationNode.ResourceType:
                    dgNode = new DGStopNode(new StopAnimationNode(), 
                        this.mFocusedState);
                    break;
            }
            dgNode.SetCategory(ndgnp.Category, true);
            this.mContainer.UndoRedo.Submit(
                new AddDGNodeCommand(this, this.mFocusedState, dgNode));
            return true;
        }

        private class RemoveDGNodesCommand : Command
        {
            private StateMachineScene mSMS;
            private StateNode mState;
            private DGNode[] mNodes;
            private DGRootNode mRootNode;
            private List<DGEdge> mEdges;
            private List<int> mSelectedEdges;
            private DGNode.IEdgeAction[] mActions;
            private int mCount;

            public RemoveDGNodesCommand(StateMachineScene sms)
            {
                this.mSMS = sms;
                this.mState = sms.mFocusedState;
                this.mNodes = sms.mSelectedDGNodes.ToArray();
                this.mRootNode = null;
                this.mEdges = new List<DGEdge>();
                this.mSelectedEdges = new List<int>();
                int i, j = -1;
                DGNode node;
                DGEdge edge;
                DGNode.IEdgeAction action;
                Digraph<DGNode, DGEdge> dg = this.mState.DecisionGraph;
                for (i = this.mNodes.Length - 1; i >= 0 && j < 0; i--)
                {
                    node = this.mNodes[i];
                    if (node.DGN == null)
                    {
                        this.mRootNode = node as DGRootNode;
                        if (this.mRootNode != null)
                        {
                            j = i;
                        }
                    }
                }
                if (j >= 0)
                {
                    i = this.mNodes.Length - 1;
                    DGNode[] nodes = new DGNode[i];
                    Array.Copy(this.mNodes, 0, nodes, 0, j);
                    Array.Copy(this.mNodes, j + 1, nodes, j, i - j);
                    this.mNodes = nodes;
                    // Don't remove the root node unless every node in the
                    // decision graph is being removed, since the remaining
                    // nodes need to be connected to the root node.
                    if (i <= dg.NodeCount)
                    {
                        this.mRootNode = null;
                    }
                }
                for (i = dg.EdgeCount - 1; i >= 0; i--)
                {
                    edge = dg.EdgeAt(i);
                    if (this.mRootNode != null)
                    {
                        node = this.mRootNode;
                        if (edge.SrcNode == node || edge.DstNode == node)
                        {
                            if (edge.Selected)
                            {
                                this.mSelectedEdges.Add(this.mEdges.Count);
                            }
                            this.mEdges.Add(edge);
                            continue;
                        }
                    }
                    for (j = this.mNodes.Length - 1; j >= 0; j--)
                    {
                        node = this.mNodes[j];
                        if (edge.SrcNode == node || edge.DstNode == node)
                        {
                            if (edge.Selected)
                            {
                                this.mSelectedEdges.Add(this.mEdges.Count);
                            }
                            this.mEdges.Add(edge);
                            break;
                        }
                    }
                }
                this.mCount = 2 * this.mEdges.Count;
                this.mActions = new DGNode.IEdgeAction[this.mCount];
                this.mCount = 0;
                for (i = this.mEdges.Count - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    action = edge.SrcNode.RemoveEdge(edge);
                    if (action != null)
                    {
                        this.mActions[this.mCount++] = action;
                    }
                    action = edge.DstNode.RemoveEdge(edge);
                    if (action != null)
                    {
                        this.mActions[this.mCount++] = action;
                    }
                }
                this.mLabel = "Remove Selected DG Nodes";
            }

            public override bool Execute()
            {
                int i;
                for (i = this.mCount - 1; i >= 0; i--)
                {
                    this.mActions[i].Redo();
                }
                DGNode node;
                DGEdge edge;
                DecisionGraph dGraph = this.mState.State.DecisionGraph;
                Digraph<DGNode, DGEdge> dg = this.mState.DecisionGraph;
                for (i = this.mEdges.Count - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                    edge.SetParent(null);
                    edge.Selected = false;
                }
                for (i = this.mNodes.Length - 1; i >= 0; i--)
                {
                    node = this.mNodes[i];
                    dGraph.Remove(node.DGN);
                    dg.RemoveNode(node);
                    node.SetParent(null);
                    node.Selected = false;
                }
                if (this.mRootNode == null)
                {
                    this.mState.AddRootDGEdges();
                }
                else
                {
                    dg.RemoveNodeAt(0);
                    this.mRootNode.SetParent(null);
                    this.mRootNode.Selected = false;
                    this.mState.State.DecisionGraph = null;
                    this.mState.UpdateRootNode(false);
                }
                if (this.mSMS.mFocusedState != null &&
                    this.mSMS.mFocusedState.Equals(this.mState))
                {
                    if (this.mSelectedEdges.Count > 0)
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            this.mSMS.mSelectedDGEdges.Remove(
                                this.mEdges[this.mSelectedEdges[i]]);
                        }
                        if (this.mSMS.DGEdgeSelectionChanged != null)
                        {
                            this.mSMS.DGEdgeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    this.mSMS.mSelectedDGNodes.Clear();
                    if (this.mSMS.DGNodeSelectionChanged != null)
                    {
                        this.mSMS.DGNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
                return true;
            }

            public override void Undo()
            {
                int i;
                for (i = this.mCount - 1; i >= 0; i--)
                {
                    this.mActions[i].Undo();
                }
                Digraph<DGNode, DGEdge> dg = this.mState.DecisionGraph;
                if (this.mRootNode != null)
                {
                    this.mState.State.DecisionGraph
                        = this.mRootNode.DecisionGraph;
                    this.mRootNode.SetParent(this.mState);
                    dg.InsertNode(0, this.mRootNode);
                    this.mState.UpdateRootNode(false);
                }
                DGNode node;
                DGEdge edge;
                DecisionGraph dGraph = this.mState.State.DecisionGraph;
                for (i = this.mNodes.Length - 1; i >= 0; i--)
                {
                    node = this.mNodes[i];
                    if (node.DGN != null)
                    {
                        if (node.IsDecisionMaker)
                        {
                            dGraph.AddDecisionMaker(node.DGN);
                        }
                        if (node.IsEntryPoint)
                        {
                            dGraph.AddEntryPoint(node.DGN);
                        }
                    }
                    node.SetParent(this.mState);
                    dg.AddNode(node);
                }
                for (i = this.mEdges.Count - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SetParent(this.mState);
                    dg.AddEdge(edge);
                }
                if (this.mRootNode == null)
                {
                    this.mState.RemoveRootDGEdges();
                }
                if (this.mSMS.mFocusedState != null &&
                    this.mSMS.mFocusedState.Equals(this.mState))
                {
                    if (this.mSelectedEdges.Count > 0)
                    {
                        for (i = this.mSelectedEdges.Count - 1; i >= 0; i--)
                        {
                            edge = this.mEdges[this.mSelectedEdges[i]];
                            edge.Selected = true;
                            this.mSMS.mSelectedDGEdges.Add(edge);
                        }
                        if (this.mSMS.DGEdgeSelectionChanged != null)
                        {
                            this.mSMS.DGEdgeSelectionChanged(
                                this.mSMS, new EventArgs());
                        }
                    }
                    if (this.mRootNode != null)
                    {
                        this.mRootNode.Selected = true;
                        this.mSMS.mSelectedDGNodes.Add(this.mRootNode);
                    }
                    for (i = this.mNodes.Length - 1; i >= 0; i--)
                    {
                        node = this.mNodes[i];
                        node.Selected = true;
                        this.mSMS.mSelectedDGNodes.Add(node);
                    }
                    if (this.mSMS.DGNodeSelectionChanged != null)
                    {
                        this.mSMS.DGNodeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public bool CanRemoveSelectedDGNodes()
        {
            if (this.mSelectedDGNodes.Count == 0)
            {
                return false;
            }
            if (this.mSelectedDGNodes.Count == 1)
            {
                DGNode node = this.mSelectedDGNodes[0];
                if (node.DGN == null && node is DGRootNode)
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveSelectedDGNodes()
        {
            if (this.mSelectedDGNodes.Count > 0)
            {
                this.mContainer.UndoRedo.Submit(
                    new RemoveDGNodesCommand(this));
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
                if (changed && this.DGEdgeSelectionChanged != null)
                {
                    this.DGEdgeSelectionChanged(this, new EventArgs());
                }
            }
        }

        public void ClearDGEdgeSelection()
        {
            if (this.mSelectedDGEdges.Count > 0)
            {
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
        }

        private class AddDGEdgeCommand : Command
        {
            private StateMachineScene mScene;
            private StateNode mState;
            private DGEdge mEdge;
            private DGNode.IEdgeAction mSrcAction;
            private DGNode.IEdgeAction mDstAction;

            public AddDGEdgeCommand(StateMachineScene scene,
                AnchorPoint srcAP, AnchorPoint dstAP)
            {
                this.mScene = scene;
                this.mState = scene.mFocusedState;
                if (srcAP.IsEntry)
                {
                    AnchorPoint ap = srcAP;
                    srcAP = dstAP;
                    dstAP = ap;
                }
                this.mEdge = new DGEdge(srcAP.Owner, dstAP.Owner, false);
                this.mSrcAction = srcAP.Owner.AddEdge(this.mEdge, srcAP);
                this.mDstAction = dstAP.Owner.AddEdge(this.mEdge, dstAP);
                this.mLabel = "Add New DG Link";
            }

            public override bool Execute()
            {
                if (this.mSrcAction != null)
                {
                    this.mSrcAction.Redo();
                }
                if (this.mDstAction != null)
                {
                    this.mDstAction.Redo();
                }
                this.mEdge.SetParent(this.mState);
                this.mState.DecisionGraph.AddEdge(this.mEdge);
                return true;
            }

            public override void Undo()
            {
                this.mState.DecisionGraph.RemoveEdge(
                    this.mEdge.SrcNode, this.mEdge.DstNode);
                this.mEdge.SetParent(null);
                if (this.mSrcAction != null)
                {
                    this.mSrcAction.Undo();
                }
                if (this.mDstAction != null)
                {
                    this.mDstAction.Undo();
                }
            }
        }


        private class RemoveDGEdgesCommand : Command
        {
            private StateMachineScene mSMS;
            private StateNode mState;
            private DGEdge[] mEdges;
            private DGNode.IEdgeAction[] mActions;
            private int mCount;

            public RemoveDGEdgesCommand(StateMachineScene sms)
            {
                int i;
                DGEdge edge;
                DGNode.IEdgeAction action;
                this.mSMS = sms;
                this.mState = sms.mFocusedState;
                this.mEdges = sms.mSelectedDGEdges.ToArray();
                this.mCount = this.mEdges.Length;
                for (i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    if (edge.AutoGenerated)
                    {
                        this.mCount--;
                        if (i < this.mCount)
                        {
                            Array.Copy(this.mEdges, i + 1, 
                                       this.mEdges, i, this.mCount - i);
                        }
                    }
                }
                if (this.mCount < this.mEdges.Length)
                {
                    DGEdge[] edges = new DGEdge[this.mCount];
                    Array.Copy(this.mEdges, 0, edges, 0, this.mCount);
                    this.mEdges = edges;
                }
                this.mActions = new DGNode.IEdgeAction[2 * this.mCount];
                this.mCount = 0;
                for (i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    action = edge.SrcNode.RemoveEdge(edge);
                    if (action != null)
                    {
                        this.mActions[this.mCount++] = action;
                    }
                    action = edge.DstNode.RemoveEdge(edge);
                    if (action != null)
                    {
                        this.mActions[this.mCount++] = action;
                    }
                }
                this.mLabel = "Remove Selected DG Links";
            }

            public override bool Execute()
            {
                int i;
                for (i = this.mCount - 1; i >= 0; i--)
                {
                    this.mActions[i].Redo();
                }
                DGEdge edge;
                Digraph<DGNode, DGEdge> dg = this.mState.DecisionGraph;
                for (i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    dg.RemoveEdge(edge.SrcNode, edge.DstNode);
                    edge.SetParent(null);
                    edge.Selected = false;
                }
                this.mState.AddRootDGEdges();
                if (this.mSMS.mFocusedState != null &&
                    this.mSMS.mFocusedState.Equals(this.mState))
                {
                    this.mSMS.mSelectedDGEdges.Clear();
                    if (this.mSMS.DGEdgeSelectionChanged != null)
                    {
                        this.mSMS.DGEdgeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
                return true;
            }

            public override void Undo()
            {
                int i;
                for (i = this.mCount - 1; i >= 0; i--)
                {
                    this.mActions[i].Undo();
                }
                DGEdge edge;
                Digraph<DGNode, DGEdge> dg = this.mState.DecisionGraph;
                for (i = this.mEdges.Length - 1; i >= 0; i--)
                {
                    edge = this.mEdges[i];
                    edge.SetParent(this.mState);
                    dg.AddEdge(edge);
                }
                this.mState.RemoveRootDGEdges();
                if (this.mSMS.mFocusedState != null &&
                    this.mSMS.mFocusedState.Equals(this.mState))
                {
                    for (i = this.mEdges.Length - 1; i >= 0; i--)
                    {
                        edge = this.mEdges[i];
                        edge.Selected = true;
                        this.mSMS.mSelectedDGEdges.Add(edge);
                    }
                    if (this.mSMS.DGEdgeSelectionChanged != null)
                    {
                        this.mSMS.DGEdgeSelectionChanged(
                            this.mSMS, new EventArgs());
                    }
                }
            }
        }

        public bool CanRemoveSelectedDGEdges()
        {
            if (this.mSelectedDGEdges.Count == 0)
            {
                return false;
            }
            foreach (DGEdge edge in this.mSelectedDGEdges)
            {
                if (!edge.AutoGenerated)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveSelectedDGEdges()
        {
            if (this.mSelectedDGEdges.Count > 0)
            {
                this.mContainer.UndoRedo.Submit(
                    new RemoveDGEdgesCommand(this));
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
                stateNode.Zvalue = 2;
                stateNode.ClipsChildrenToShape = false;
                stateNode.InEditMode = true;
                this.mStateGraphHider.BoundingBox = this.BoundingBox;
                this.mStateGraphHider.Visible = true;
                this.ClearStateNodeSelection();
                this.ClearStateEdgeSelection();
            }
            this.mFocusedState = stateNode;

            this.bHoverInvoked = false;
            this.bPressInvoked = false;
            this.mLastStateNodePressed = null;
            this.mLastStateNodeHovered = null;
            this.mLastAnchorPressed = null;
            this.mLastAnchorHovered = null;
            this.mTempStateEdge.Update();
            this.mTempDGEdge.SetParent(stateNode);
            this.mTempDGEdge.Update();

            if (this.FocusedStateChanged != null)
            {
                this.FocusedStateChanged(this, new EventArgs());
            }
        }

        private bool bAddingEdges = false;
        private bool bHoverInvoked = false;
        private bool bPressInvoked = false;

        public bool AddingEdges
        {
            get { return this.bAddingEdges; }
            set
            {
                if (this.bAddingEdges != value)
                {
                    this.bAddingEdges = value;
                    this.bHoverInvoked = false;
                    this.bPressInvoked = false;
                    this.mLastStateNodePressed = null;
                    this.mLastStateNodeHovered = null;
                    this.mLastAnchorPressed = null;
                    this.mLastAnchorHovered = null;
                    this.mTempStateEdge.Update();
                    this.mTempDGEdge.Update();
                }
            }
        }
        
        private StateNode mLastStateNodePressed = null;
        private StateNode mLastStateNodeHovered = null;

        public void OnStateNodeHovered(StateNode node)
        {
            if (this.bAddingEdges && !this.bHoverInvoked && 
                this.mFocusedState == null)
            {
                this.bHoverInvoked = true;
                if (this.mLastStateNodeHovered != node)
                {
                    if (this.mLastStateNodePressed != null &&
                        this.mLastStateNodePressed != node &&
                        this.mStateGraph.IndexOfEdge(
                            this.mLastStateNodePressed, node) < 0)
                    {
                        this.mLastStateNodeHovered = node;
                    }
                    else
                    {
                        this.mLastStateNodeHovered = null;
                    }
                    this.mTempStateEdge.Update();
                }
            }
        }

        public void OnStateNodePressed(StateNode node)
        {
            if (this.bAddingEdges && !this.bPressInvoked &&
                this.mFocusedState == null)
            {
                this.bPressInvoked = true;
                if (this.mLastStateNodePressed == null)
                {
                    this.mLastStateNodePressed = node;
                    this.mLastStateNodeHovered = null;
                }
                else if (this.mLastStateNodeHovered == node)
                {
                    this.mContainer.UndoRedo.Submit(
                        new AddStateEdgeCommand(this, 
                            this.mLastStateNodePressed, node));
                    this.mLastStateNodePressed = null;
                    this.mLastStateNodeHovered = null;
                }
            }
        }

        private AnchorPoint mLastAnchorPressed = null;
        private AnchorPoint mLastAnchorHovered = null;

        public void OnAnchorHovered(AnchorPoint ap)
        {
            if (this.bAddingEdges && !this.bHoverInvoked && 
                this.mFocusedState != null &&
                this.mFocusedState.Equals(ap.Owner.State))
            {
                this.bHoverInvoked = true;
                if (this.mLastAnchorHovered != ap)
                {
                    if (this.mFocusedState.RootNode != ap.Owner &&
                        this.mLastAnchorPressed != null &&
                        this.mLastAnchorPressed != ap &&
                        this.mLastAnchorPressed.Owner != ap.Owner &&
                        this.mLastAnchorPressed.IsEntry != ap.IsEntry)
                    {
                        DGNode srcNode, dstNode;
                        if (ap.IsEntry)
                        {
                            srcNode = this.mLastAnchorPressed.Owner;
                            dstNode = ap.Owner;
                        }
                        else
                        {
                            srcNode = ap.Owner;
                            dstNode = this.mLastAnchorPressed.Owner;
                        }
                        if (this.mFocusedState.DecisionGraph.IndexOfEdge(
                                srcNode, dstNode) < 0)
                        {
                            this.mLastAnchorHovered = ap;
                        }
                        else
                        {
                            this.mLastAnchorHovered = null;
                        }
                    }
                    else
                    {
                        this.mLastAnchorHovered = null;
                    }
                    this.mTempDGEdge.Update();
                }
            }
        }

        public void OnAnchorPressed(AnchorPoint ap)
        {
            if (this.bAddingEdges && !this.bPressInvoked &&
                this.mFocusedState != null && 
                this.mFocusedState.Equals(ap.Owner.State))
            {
                if (this.mLastAnchorPressed == null)
                {
                    if (this.mFocusedState.RootNode != ap.Owner)
                    {
                        this.mLastAnchorPressed = ap;
                        this.mLastAnchorHovered = null;
                    }
                }
                else if (this.mLastAnchorHovered == ap)
                {
                    this.mContainer.UndoRedo.Submit(
                        new AddDGEdgeCommand(this, 
                            this.mLastAnchorPressed, ap));
                    this.mLastAnchorPressed = null;
                    this.mLastAnchorHovered = null;
                    this.mTempDGEdge.Update();
                }
            }
        }

        protected override bool OnMouseDown(GraphMouseEventArgs e)
        {
            if (this.bAddingEdges)
            {
                this.bPressInvoked = false;
            }
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
            if (this.bAddingEdges)
            {
                if (!this.bHoverInvoked)
                {
                    this.mLastStateNodeHovered = null;
                    this.mLastAnchorHovered = null;
                    this.mTempStateEdge.Update();
                    this.mTempDGEdge.Update();
                }
                this.bHoverInvoked = false;
            }
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
            if (this.mStateView != null)
            {
                Size size = this.mStateView.Size;
                g.DrawString(string.Concat(bbox, "\n", size),
                    sDebugFont, Brushes.Black, bbox);
            }
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
