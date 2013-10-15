using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.FreeformJazz.Widgets;
using GraphForms;
using GraphForms.Algorithms;
using GraphForms.Algorithms.Layout.Circular;
using GraphForms.Algorithms.Layout.ForceDirected;
using GraphForms.Algorithms.Layout.Tree;

namespace s3piwrappers.FreeformJazz
{
    public partial class SettingsDialog : Form
    {
        private static readonly KKLayoutAlgorithm<StateNode, StateEdge> sKKLayoutSettings;
        private static readonly BalloonCirclesLayoutAlgorithm2<StateNode, StateEdge> sBalloonCirclesLayoutSettings;

        private static readonly SimpleTreeLayoutAlgorithm<DGNode, DGEdge> sSimpleTreeLayoutSettings;

        static SettingsDialog()
        {
            sKKLayoutSettings = new KKLayoutAlgorithm<StateNode, StateEdge>(
                new Digraph<StateNode, StateEdge>(), new Box2F(0, 0, 1, 1));
            sKKLayoutSettings.LengthFactor = 1.25f;

            sBalloonCirclesLayoutSettings = new BalloonCirclesLayoutAlgorithm2<StateNode, StateEdge>(
                new Digraph<StateNode, StateEdge>(), new Box2F(0, 0, 1, 1));

            sSimpleTreeLayoutSettings = new SimpleTreeLayoutAlgorithm<DGNode, DGEdge>(
                new Digraph<DGNode, DGEdge>(), new Box2F(0, 0, 1, 1));
            sSimpleTreeLayoutSettings.LayerGap = 30;
        }

        private static void CopyLayoutSettings(
            KKLayoutAlgorithm<StateNode, StateEdge> source,
            KKLayoutAlgorithm<StateNode, StateEdge> target)
        {
            if (target.State != ComputeState.Running &&
                target.AsyncState != ComputeState.Running)
            {
                target.MaxIterations = source.MaxIterations;
                target.MovementTolerance = source.MovementTolerance;
            }
            target.K = source.K;
            target.AdjustForGravity = source.AdjustForGravity;
            target.ExchangeVertices = source.ExchangeVertices;
            target.LengthFactor = source.LengthFactor;
            target.DisconnectedMultiplier = source.DisconnectedMultiplier;
        }

        private static void CopyLayoutSettings(
            BalloonCirclesLayoutAlgorithm2<StateNode, StateEdge> source,
            BalloonCirclesLayoutAlgorithm2<StateNode, StateEdge> target)
        {
            if (target.State != ComputeState.Running &&
                target.AsyncState != ComputeState.Running)
            {
                target.MaxIterations = source.MaxIterations;
                target.MovementTolerance = source.MovementTolerance;
            }
            target.GroupingMethod = source.GroupingMethod;
            target.NodeSequencer = source.NodeSequencer;

            target.NodeSpacing = source.NodeSpacing;
            target.MinRadius = source.MinRadius;
            target.FreeArc = source.FreeArc;

            target.SpanningTreeGeneration = source.SpanningTreeGeneration;
            target.RootFindingMethod = source.RootFindingMethod;

            target.InSketchMode = source.InSketchMode;

            target.BranchSpacing = source.BranchSpacing;
            target.RootCentering = source.RootCentering;
            target.MinimumEdgeLength = source.MinimumEdgeLength;
            target.EqualizeBranchLengths = source.EqualizeBranchLengths;

            target.MaximumRootWedge = source.MaximumRootWedge;
            target.MaximumTreeWedge = source.MaximumTreeWedge;
            target.MaximumDeviationAngle = source.MaximumDeviationAngle;

            target.AdaptToSizeChanges = source.AdaptToSizeChanges;
            target.AdjustRootCenters = source.AdjustRootCenters;
            target.AdjustRootAngle = source.AdjustRootAngle;
            target.SpringMultiplier = source.SpringMultiplier;
            target.MagneticMultiplier = source.MagneticMultiplier;
            target.MagneticExponent = source.MagneticExponent;
            target.RootAngle = source.RootAngle;
        }

        private static void CopyLayoutSettings(
            SimpleTreeLayoutAlgorithm<DGNode, DGEdge> source,
            SimpleTreeLayoutAlgorithm<DGNode, DGEdge> target)
        {
            if (target.State != ComputeState.Running &&
                target.AsyncState != ComputeState.Running)
            {
                target.MaxIterations = source.MaxIterations;
                target.MovementTolerance = source.MovementTolerance;
            }
            target.SpanningTreeGeneration = source.SpanningTreeGeneration;
            target.RootFindingMethod = source.RootFindingMethod;

            target.VertexGap = source.VertexGap;
            target.LayerGap = source.LayerGap;
            target.Direction = source.Direction;
            target.AdaptToSizeChanges = source.AdaptToSizeChanges;
            target.AdjustRootCenters = source.AdjustRootCenters;
            target.SpringMultiplier = source.SpringMultiplier;
            target.MagneticMultiplier = source.MagneticMultiplier;
            target.MagneticExponent = source.MagneticExponent;
        }

        private MainForm mOwner;

        private bool bVisualFontSettingsDirty;
        private bool bVisualFloatSettingsDirty;
        private bool bVisualColorSettingsDirty;
        private VisualSettings mActualVisSettings;
        private VisualSettings mPreviewVisSettings;

        public SettingsDialog(MainForm owner)
        {
            if (owner == null)
            {
                throw new ArgumentNullException("owner");
            }
            InitializeComponent();

            this.mOwner = owner;

            this.mActualVisSettings = new VisualSettings();
            this.mPreviewVisSettings = new VisualSettings();
            this.mPreviewVisSettings.FloatSettingChanged += 
                new EventHandler<VisualSettings.FloatSettingEventArgs>(
                    previewFloatSettingChanged);
            this.mPreviewVisSettings.ColorSettingChanged += 
                new EventHandler<VisualSettings.ColorSettingEventArgs>(
                    previewColorSettingChanged);
            this.mPreviewVisSettings.FontSettingChanged += 
                new EventHandler<VisualSettings.FontSettingEventArgs>(
                    previewFontSettingChanged);
            this.mPreviewVisSettings.SetApplyFloatChangesImmediately(false);
            this.mPreviewVisSettings.SetApplyColorChangesImmediately(true);
            this.mPreviewVisSettings.SetApplyFontChangesImmediately(false);
            this.graphVisualPropGrid.SelectedObject 
                = this.mPreviewVisSettings;
        }

        private void OnDialogLoad(object sender, EventArgs e)
        {
            this.bVisualFontSettingsDirty = false;
            this.bVisualFloatSettingsDirty = false;
            this.bVisualColorSettingsDirty = false;
            this.mActualVisSettings.ReadSettingsFromCode();
            this.mPreviewVisSettings.ReadSettingsFromCode();
        }

        private void OnDialogShown(object sender, EventArgs e)
        {
            this.bVisualFontSettingsDirty = false;
            this.bVisualFloatSettingsDirty = false;
            this.bVisualColorSettingsDirty = false;
            this.mActualVisSettings.ReadSettingsFromCode();
            this.mPreviewVisSettings.ReadSettingsFromCode();
        }

        private void previewFloatSettingChanged(object sender, 
            VisualSettings.FloatSettingEventArgs e)
        {
            this.applyBTN.Enabled = true;
            this.bVisualFloatSettingsDirty = true;
            if (this.previewFloatChangesCHK.Checked)
            {
                this.mOwner.RefreshJazzGraphs(true);
            }
        }

        private void previewColorSettingChanged(object sender, 
            VisualSettings.ColorSettingEventArgs e)
        {
            this.applyBTN.Enabled = true;
            this.bVisualColorSettingsDirty = true;
            if (this.previewColorChangesCHK.Checked)
            {
                this.mOwner.InvalidateJazzGraphs();
            }
        }

        private void previewFontSettingChanged(object sender,
            VisualSettings.FontSettingEventArgs e)
        {
            this.applyBTN.Enabled = true;
            this.bVisualFontSettingsDirty = true;
            if (this.previewFontChangesCHK.Checked)
            {
                this.mOwner.RefreshJazzGraphs(true);
            }
        }

        private void previewFloatChangesCheckedChanged(
            object sender, EventArgs e)
        {
            bool flag = this.previewFloatChangesCHK.Checked;
            this.mPreviewVisSettings.SetApplyFloatChangesImmediately(flag);
            if (this.bVisualFloatSettingsDirty)
            {
                if (flag)
                {
                    this.mPreviewVisSettings.ApplyFloatSettingsToCode();
                    this.mOwner.RefreshJazzGraphs(true);
                }
                else
                {
                    this.mActualVisSettings.ApplyFloatSettingsToCode();
                    this.mOwner.RefreshJazzGraphs(true);
                }
            }
        }

        private void previewColorChangesCheckedChanged(
            object sender, EventArgs e)
        {
            bool flag = this.previewColorChangesCHK.Checked;
            this.mPreviewVisSettings.SetApplyColorChangesImmediately(flag);
            if (this.bVisualColorSettingsDirty)
            {
                if (flag)
                {
                    this.mPreviewVisSettings.ApplyColorSettingsToCode();
                    this.mOwner.InvalidateJazzGraphs();
                }
                else
                {
                    this.mActualVisSettings.ApplyColorSettingsToCode();
                    this.mOwner.InvalidateJazzGraphs();
                }
            }
        }

        private void previewFontChangesCheckedChanged(
            object sender, EventArgs e)
        {
            bool flag = this.previewFontChangesCHK.Checked;
            this.mPreviewVisSettings.SetApplyFontChangesImmediately(flag);
            if (this.bVisualFontSettingsDirty)
            {
                if (flag)
                {
                    this.mPreviewVisSettings.ApplyFontSettingsToCode();
                    this.mOwner.RefreshJazzGraphs(true);
                }
                else
                {
                    this.mActualVisSettings.ApplyFontSettingsToCode();
                    this.mOwner.RefreshJazzGraphs(true);
                }
            }
        }

        private void Apply()
        {
            if (this.applyBTN.Enabled)
            {
                this.applyBTN.Enabled = false;
                if (this.bVisualFontSettingsDirty)
                {
                    this.mPreviewVisSettings.ApplyAllSettingsToCode();
                    this.mActualVisSettings.ReadSettingsFromCode();
                    this.bVisualFontSettingsDirty = false;
                    this.bVisualFloatSettingsDirty = false;
                    this.bVisualColorSettingsDirty = false;
                    if (!this.previewFontChangesCHK.Checked)
                    {
                        this.mOwner.RefreshJazzGraphs(true);
                    }
                }
                else if (this.bVisualFloatSettingsDirty)
                {
                    this.mPreviewVisSettings.ApplyAllSettingsToCode();
                    this.mActualVisSettings.ReadSettingsFromCode();
                    this.bVisualFloatSettingsDirty = false;
                    this.bVisualColorSettingsDirty = false;
                    if (!this.previewFloatChangesCHK.Checked)
                    {
                        this.mOwner.RefreshJazzGraphs(true);
                    }
                }
                else if (this.bVisualColorSettingsDirty)
                {
                    this.mPreviewVisSettings.ApplyColorSettingsToCode();
                    this.mActualVisSettings.ReadSettingsFromCode();
                    this.bVisualColorSettingsDirty = false;
                    if (!this.previewColorChangesCHK.Checked)
                    {
                        this.mOwner.InvalidateJazzGraphs();
                    }
                }
            }
        }

        private void applyClick(object sender, EventArgs e)
        {
            this.Apply();
        }

        private void okClick(object sender, EventArgs e)
        {
            this.Apply();
        }

        private void cancelClick(object sender, EventArgs e)
        {
            if (this.bVisualFontSettingsDirty)
            {
                this.applyBTN.Enabled = false;
                this.mActualVisSettings.ApplyAllSettingsToCode();
                this.bVisualFontSettingsDirty = false;
                this.bVisualFloatSettingsDirty = false;
                this.bVisualColorSettingsDirty = false;
                if (this.previewFontChangesCHK.Checked)
                {
                    this.mOwner.RefreshJazzGraphs(true);
                }
            }
            else if (this.bVisualFloatSettingsDirty)
            {
                this.applyBTN.Enabled = false;
                this.mActualVisSettings.ApplyAllSettingsToCode();
                this.bVisualFloatSettingsDirty = false;
                this.bVisualColorSettingsDirty = false;
                if (this.previewFontChangesCHK.Checked)
                {
                    this.mOwner.RefreshJazzGraphs(true);
                }
            }
            else if (this.bVisualColorSettingsDirty)
            {
                this.applyBTN.Enabled = false;
                this.mActualVisSettings.ApplyColorSettingsToCode();
                this.bVisualColorSettingsDirty = false;
                if (this.previewColorChangesCHK.Checked)
                {
                    this.mOwner.InvalidateJazzGraphs();
                }
            }
        }
    }
}
