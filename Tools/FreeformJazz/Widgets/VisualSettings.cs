using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace s3piwrappers.FreeformJazz.Widgets
{
    public class VisualSettings
    {
        public class FloatSettingEventArgs : EventArgs
        {
            public readonly float OldValue;
            public readonly float NewValue;
            public string ClassName;
            public string PropertyName;

            public FloatSettingEventArgs(
                float oldValue, float newValue,
                string className, string propName)
            {
                this.OldValue = oldValue;
                this.NewValue = newValue;
                this.ClassName = className;
                this.PropertyName = propName;
            }
        }

        public class ColorSettingEventArgs : EventArgs
        {
            public readonly Color OldColor;
            public readonly Color NewColor;
            public readonly string ClassName;
            public readonly string PropertyName;

            public ColorSettingEventArgs(
                Color oldColor, Color newColor,
                string className, string propName)
            {
                this.OldColor = oldColor;
                this.NewColor = newColor;
                this.ClassName = className;
                this.PropertyName = propName;
            }
        }

        public class FontSettingEventArgs : EventArgs
        {
            public readonly Font OldFont;
            public readonly Font NewFont;
            public readonly string ClassName;
            public readonly string PropertyName;

            public FontSettingEventArgs(
                Font oldFont, Font newFont,
                string className, string propName)
            {
                this.OldFont = oldFont;
                this.NewFont = newFont;
                this.ClassName = className;
                this.PropertyName = propName;
            }
        }

        public event EventHandler<FloatSettingEventArgs> FloatSettingChanged;

        public event EventHandler<ColorSettingEventArgs> ColorSettingChanged;

        public event EventHandler<FontSettingEventArgs> FontSettingChanged;

        private static readonly Color sBlankColor = new Color();

        private bool bApplyFloatChangesImmediately;
        private bool bApplyColorChangesImmediately;
        private bool bApplyFontChangesImmediately;

        private float mStateMachineGridSize;
        private Color mStateMachineGridColor;
        private Color mStateMachineBackColor;
        private Color mStateMachineGraphHiderColor;
        private Color mStateMachineLassoColor;

        private float mStateMaxRadius;
        private float mStateMinRadius;
        private Color mStatePublicMaxColor;
        private Color mStatePrivateMaxColor;
        private Color mStatePublicMinColor;
        private Color mStatePrivateMinColor;

        private Color mStateNodeBorderNormalColor;
        private Color mStateNodeBorderSelectColor;

        private double mStateEdgeAngle;
        private Color mStateEdgeBorderNormalColor;
        private Color mStateEdgeBorderSelectColor;

        private float mAnchorPointRadius;
        private Color mAnchorPointColor;

        private float mDGEdgeCurveSize;
        private Color mDGEdgeBorderNormalColor;
        private Color mDGEdgeBorderInEditColor;
        private Color mDGEdgeBorderSelectColor;

        private Color mDGNodeBorderNormalColor;
        private Color mDGNodeBorderInEditColor;
        private Color mDGNodeBorderSelectColor;

        private float mDGRootRadius;
        private Color mDGRootBGColor;

        private Color mDGSnSnBGColor;

        private Color mDGRandBGColor;

        private Color mDGSoPnHeadColor;
        private Color mDGSoPnBodyColor;

        private Color mDGSoDnBGColor;

        private Color mDGPlayBGColor;
        private Color mDGStopBGColor;
        private Color mDGAcOpBGColor;
        private Color mDGPropBGColor;

        private Font mStateFont;
        private Color mStatePublicTxtColor;
        private Color mStatePrivateTxtColor;

        private Font mDGSnSnFont;
        private Color mDGSnSnTxtColor;

        private Font mDGRandFont;
        private Color mDGRandTxtColor;

        private Font mDGSoPnHeadFont;
        private Color mDGSoPnHeadTxtColor;
        private Font mDGSoPnBodyFont;
        private Color mDGSoPnBodyTxtColor;

        private Font mDGSoDnFont;
        private Color mDGSoDnTxtColor;

        private Font mDGPlayFont;
        private Color mDGPlayTxtColor;
        private Font mDGStopFont;
        private Color mDGStopTxtColor;
        private Font mDGAcOpFont;
        private Color mDGAcOpTxtColor;
        private Font mDGPropFont;
        private Color mDGPropTxtColor;

        public VisualSettings()
        {
            this.bApplyFloatChangesImmediately = false;
            this.bApplyColorChangesImmediately = false;
            this.bApplyFontChangesImmediately = false;

            this.mStateMachineGridSize = 0;
            this.mStateMachineGridColor = sBlankColor;
            this.mStateMachineBackColor = sBlankColor;
            this.mStateMachineGraphHiderColor = sBlankColor;
            this.mStateMachineLassoColor = sBlankColor;

            this.mStateMaxRadius = 0;
            this.mStateMinRadius = 0;
            this.mStatePublicMaxColor = sBlankColor;
            this.mStatePrivateMaxColor = sBlankColor;
            this.mStatePublicMinColor = sBlankColor;
            this.mStatePrivateMinColor = sBlankColor;

            this.mStateNodeBorderNormalColor = sBlankColor;
            this.mStateNodeBorderSelectColor = sBlankColor;

            this.mStateEdgeAngle = 0;
            this.mStateEdgeBorderNormalColor = sBlankColor;
            this.mStateEdgeBorderSelectColor = sBlankColor;

            this.mAnchorPointRadius = 0;
            this.mAnchorPointColor = sBlankColor;

            this.mDGEdgeCurveSize = 0;
            this.mDGEdgeBorderNormalColor = sBlankColor;
            this.mDGEdgeBorderInEditColor = sBlankColor;
            this.mDGEdgeBorderSelectColor = sBlankColor;

            this.mDGNodeBorderNormalColor = sBlankColor;
            this.mDGNodeBorderInEditColor = sBlankColor;
            this.mDGNodeBorderSelectColor = sBlankColor;

            this.mDGRootRadius = 0;
            this.mDGRootBGColor = sBlankColor;

            this.mDGSnSnBGColor = sBlankColor;

            this.mDGRandBGColor = sBlankColor;

            this.mDGSoDnBGColor = sBlankColor;

            this.mDGSoPnHeadColor = sBlankColor;
            this.mDGSoPnBodyColor = sBlankColor;

            this.mDGPlayBGColor = sBlankColor;
            this.mDGStopBGColor = sBlankColor;
            this.mDGAcOpBGColor = sBlankColor;
            this.mDGPropBGColor = sBlankColor;

            this.mStateFont = null;
            this.mStatePublicTxtColor = sBlankColor;
            this.mStatePrivateTxtColor = sBlankColor;

            this.mDGSnSnFont = null;
            this.mDGSnSnTxtColor = sBlankColor;

            this.mDGRandFont = null;
            this.mDGRandTxtColor = sBlankColor;

            this.mDGSoPnHeadFont = null;
            this.mDGSoPnHeadTxtColor = sBlankColor;
            this.mDGSoPnBodyFont = null;
            this.mDGSoPnBodyTxtColor = sBlankColor;

            this.mDGSoDnFont = null;
            this.mDGSoDnTxtColor = sBlankColor;

            this.mDGPlayFont = null;
            this.mDGPlayTxtColor = sBlankColor;
            this.mDGStopFont = null;
            this.mDGStopTxtColor = sBlankColor;
            this.mDGAcOpFont = null;
            this.mDGAcOpTxtColor = sBlankColor;
            this.mDGPropFont = null;
            this.mDGPropTxtColor = sBlankColor;
        }

        public bool GetApplyFloatChangesImmediately()
        {
            return this.bApplyFloatChangesImmediately;
        }

        public void SetApplyFloatChangesImmediately(bool value)
        {
            if (this.bApplyFloatChangesImmediately != value)
            {
                this.bApplyFloatChangesImmediately = value;
            }
        }

        public bool GetApplyColorChangesImmediately()
        {
            return this.bApplyColorChangesImmediately;
        }

        public void SetApplyColorChangesImmediately(bool value)
        {
            if (this.bApplyColorChangesImmediately != value)
            {
                this.bApplyColorChangesImmediately = value;
            }
        }

        public bool GetApplyFontChangesImmediately()
        {
            return this.bApplyFontChangesImmediately;
        }

        public void SetApplyFontChangesImmediately(bool value)
        {
            if (this.bApplyFontChangesImmediately != value)
            {
                this.bApplyFontChangesImmediately = value;
            }
        }

        public void ReadSettingsFromCode()
        {
            this.mStateMachineGridSize = StateMachineScene.GridSize;
            this.mStateMachineGridColor = StateMachineScene.GridColor;
            this.mStateMachineBackColor = StateMachineScene.BackColor;
            this.mStateMachineGraphHiderColor 
                = StateMachineScene.GraphHiderColor;
            this.mStateMachineLassoColor = StateMachineScene.LassoColor;

            this.mStateMaxRadius = StateNode.MaximizedRadius;
            this.mStateMinRadius = StateNode.MinimizedRadius;
            this.mStatePublicMaxColor = StateNode.PublicMaximizedColor;
            this.mStatePrivateMaxColor = StateNode.PrivateMaximizedColor;
            this.mStatePublicMinColor = StateNode.PublicMinimizedColor;
            this.mStatePrivateMinColor = StateNode.PrivateMinimizedColor;

            this.mStateNodeBorderNormalColor = StateNode.BorderNormalColor;
            this.mStateNodeBorderSelectColor = StateNode.BorderSelectedColor;

            this.mStateEdgeAngle = StateEdge.Angle;
            this.mStateEdgeBorderNormalColor = StateEdge.BorderNormalColor;
            this.mStateEdgeBorderSelectColor = StateEdge.BorderSelectedColor;

            this.mAnchorPointRadius = AnchorPoint.Radius;
            this.mAnchorPointColor = AnchorPoint.BackgroundColor;

            this.mDGEdgeCurveSize = DGEdge.CurveSize;
            this.mDGEdgeBorderNormalColor = DGEdge.BorderNormalColor;
            this.mDGEdgeBorderInEditColor = DGEdge.BorderInEditColor;
            this.mDGEdgeBorderSelectColor = DGEdge.BorderSelectedColor;

            this.mDGNodeBorderNormalColor = DGNode.BorderNormalColor;
            this.mDGNodeBorderInEditColor = DGNode.BorderInEditColor;
            this.mDGNodeBorderSelectColor = DGNode.BorderSelectedColor;

            this.mDGRootRadius = DGRootNode.Radius;
            this.mDGRootBGColor = DGRootNode.BackgroundColor;

            this.mDGSnSnBGColor = DGSnSnNode.BackgroundColor;

            this.mDGRandBGColor = DGRandNode.BackgroundColor;

            this.mDGSoPnHeadColor = DGSoPnNode.HeadColor;
            this.mDGSoPnBodyColor = DGSoPnNode.BodyColor;

            this.mDGSoDnBGColor = DGSoDnNode.BackgroundColor;

            this.mDGPlayBGColor = DGPlayNode.BackgroundColor;
            this.mDGStopBGColor = DGStopNode.BackgroundColor;
            this.mDGAcOpBGColor = DGAcOpNode.BackgroundColor;
            this.mDGPropBGColor = DGPropNode.BackgroundColor;

            this.mStateFont = StateNode.TextFont;
            this.mStatePublicTxtColor = StateNode.PublicTextColor;
            this.mStatePrivateTxtColor = StateNode.PrivateTextColor;

            this.mDGSnSnFont = DGSnSnNode.TextFont;
            this.mDGSnSnTxtColor = DGSnSnNode.TextColor;

            this.mDGRandFont = DGRandNode.TextFont;
            this.mDGRandTxtColor = DGRandNode.TextColor;

            this.mDGSoPnHeadFont = DGSoPnNode.HeadFont;
            this.mDGSoPnHeadTxtColor = DGSoPnNode.HeadTextColor;
            this.mDGSoPnBodyFont = DGSoPnNode.BodyFont;
            this.mDGSoPnBodyTxtColor = DGSoPnNode.BodyTextColor;

            this.mDGSoDnFont = DGSoDnNode.TextFont;
            this.mDGSoDnTxtColor = DGSoDnNode.TextColor;

            this.mDGPlayFont = DGPlayNode.TextFont;
            this.mDGPlayTxtColor = DGPlayNode.TextColor;
            this.mDGStopFont = DGStopNode.TextFont;
            this.mDGStopTxtColor = DGStopNode.TextColor;
            this.mDGAcOpFont = DGAcOpNode.TextFont;
            this.mDGAcOpTxtColor = DGAcOpNode.TextColor;
            this.mDGPropFont = DGPropNode.TextFont;
            this.mDGPropTxtColor = DGPropNode.TextColor;
        }

        public void ApplyAllSettingsToCode()
        {
            if (this.mStateMachineGridSize == 0)
            {
                this.ReadSettingsFromCode();
            }

            StateMachineScene.GridSize = this.mStateMachineGridSize;
            StateMachineScene.GridColor = this.mStateMachineGridColor;
            StateMachineScene.BackColor = this.mStateMachineBackColor;
            StateMachineScene.GraphHiderColor 
                = this.mStateMachineGraphHiderColor;
            StateMachineScene.LassoColor = this.mStateMachineLassoColor;

            StateNode.MaximizedRadius = this.mStateMaxRadius;
            StateNode.MinimizedRadius = this.mStateMinRadius;
            StateNode.PublicMaximizedColor = this.mStatePublicMaxColor;
            StateNode.PrivateMaximizedColor = this.mStatePrivateMaxColor;
            StateNode.PublicMinimizedColor = this.mStatePublicMinColor;
            StateNode.PrivateMinimizedColor = this.mStatePrivateMinColor;

            StateNode.BorderNormalColor = this.mStateNodeBorderNormalColor;
            StateNode.BorderSelectedColor = this.mStateNodeBorderSelectColor;

            StateEdge.Angle = this.mStateEdgeAngle;
            StateEdge.BorderNormalColor = this.mStateEdgeBorderNormalColor;
            StateEdge.BorderSelectedColor = this.mStateEdgeBorderSelectColor;

            AnchorPoint.Radius = this.mAnchorPointRadius;
            AnchorPoint.BackgroundColor = this.mAnchorPointColor;

            DGEdge.CurveSize = this.mDGEdgeCurveSize;
            DGEdge.BorderNormalColor = this.mDGEdgeBorderNormalColor;
            DGEdge.BorderInEditColor = this.mDGEdgeBorderInEditColor;
            DGEdge.BorderSelectedColor = this.mDGEdgeBorderSelectColor;

            DGNode.BorderNormalColor = this.mDGNodeBorderNormalColor;
            DGNode.BorderInEditColor = this.mDGNodeBorderInEditColor;
            DGNode.BorderSelectedColor = this.mDGNodeBorderSelectColor;

            DGRootNode.Radius = this.mDGRootRadius;
            DGRootNode.BackgroundColor = this.mDGRootBGColor;

            DGSnSnNode.BackgroundColor = this.mDGSnSnBGColor;

            DGRandNode.BackgroundColor = this.mDGRandBGColor;

            DGSoPnNode.HeadColor = this.mDGSoPnHeadColor;
            DGSoPnNode.BodyColor = this.mDGSoPnBodyColor;

            DGSoDnNode.BackgroundColor = this.mDGSoDnBGColor;

            DGPlayNode.BackgroundColor = this.mDGPlayBGColor;
            DGStopNode.BackgroundColor = this.mDGStopBGColor;
            DGAcOpNode.BackgroundColor = this.mDGAcOpBGColor;
            DGPropNode.BackgroundColor = this.mDGPropBGColor;

            StateNode.TextFont = this.mStateFont;
            StateNode.PublicTextColor = this.mStatePublicTxtColor;
            StateNode.PrivateTextColor = this.mStatePrivateTxtColor;

            DGSnSnNode.TextFont = this.mDGSnSnFont;
            DGSnSnNode.TextColor = this.mDGSnSnTxtColor;

            DGRandNode.TextFont = this.mDGRandFont;
            DGRandNode.TextColor = this.mDGRandTxtColor;

            DGSoPnNode.HeadFont = this.mDGSoPnHeadFont;
            DGSoPnNode.HeadTextColor = this.mDGSoPnHeadTxtColor;
            DGSoPnNode.BodyFont = this.mDGSoPnBodyFont;
            DGSoPnNode.BodyTextColor = this.mDGSoPnBodyTxtColor;

            DGSoDnNode.TextFont = this.mDGSoDnFont;
            DGSoDnNode.TextColor = this.mDGSoDnTxtColor;

            DGPlayNode.TextFont = this.mDGPlayFont;
            DGPlayNode.TextColor = this.mDGPlayTxtColor;
            DGStopNode.TextFont = this.mDGStopFont;
            DGStopNode.TextColor = this.mDGStopTxtColor;
            DGAcOpNode.TextFont = this.mDGAcOpFont;
            DGAcOpNode.TextColor = this.mDGAcOpTxtColor;
            DGPropNode.TextFont = this.mDGPropFont;
            DGPropNode.TextColor = this.mDGPropTxtColor;
        }

        public void ApplyFloatSettingsToCode()
        {
            if (this.mStateMachineGridSize == 0)
            {
                this.ReadSettingsFromCode();
            }

            StateMachineScene.GridSize = this.mStateMachineGridSize;

            StateNode.MaximizedRadius = this.mStateMaxRadius;
            StateNode.MinimizedRadius = this.mStateMinRadius;

            StateEdge.Angle = this.mStateEdgeAngle;

            AnchorPoint.Radius = this.mAnchorPointRadius;

            DGEdge.CurveSize = this.mDGEdgeCurveSize;

            DGRootNode.Radius = this.mDGRootRadius;
        }

        public void ApplyColorSettingsToCode()
        {
            if (this.mStateMachineGridSize == 0)
            {
                this.ReadSettingsFromCode();
            }

            StateMachineScene.GridColor = this.mStateMachineGridColor;
            StateMachineScene.BackColor = this.mStateMachineBackColor;
            StateMachineScene.GraphHiderColor
                = this.mStateMachineGraphHiderColor;
            StateMachineScene.LassoColor = this.mStateMachineLassoColor;

            StateNode.PublicMaximizedColor = this.mStatePublicMaxColor;
            StateNode.PrivateMaximizedColor = this.mStatePrivateMaxColor;
            StateNode.PublicMinimizedColor = this.mStatePublicMinColor;
            StateNode.PrivateMinimizedColor = this.mStatePrivateMinColor;

            StateNode.BorderNormalColor = this.mStateNodeBorderNormalColor;
            StateNode.BorderSelectedColor = this.mStateNodeBorderSelectColor;

            StateEdge.BorderNormalColor = this.mStateEdgeBorderNormalColor;
            StateEdge.BorderSelectedColor = this.mStateEdgeBorderSelectColor;

            AnchorPoint.BackgroundColor = this.mAnchorPointColor;

            DGEdge.BorderNormalColor = this.mDGEdgeBorderNormalColor;
            DGEdge.BorderInEditColor = this.mDGEdgeBorderInEditColor;
            DGEdge.BorderSelectedColor = this.mDGEdgeBorderSelectColor;

            DGNode.BorderNormalColor = this.mDGNodeBorderNormalColor;
            DGNode.BorderInEditColor = this.mDGNodeBorderInEditColor;
            DGNode.BorderSelectedColor = this.mDGNodeBorderSelectColor;

            DGRootNode.BackgroundColor = this.mDGRootBGColor;

            DGSnSnNode.BackgroundColor = this.mDGSnSnBGColor;

            DGRandNode.BackgroundColor = this.mDGRandBGColor;

            DGSoPnNode.HeadColor = this.mDGSoPnHeadColor;
            DGSoPnNode.BodyColor = this.mDGSoPnBodyColor;

            DGSoDnNode.BackgroundColor = this.mDGSoDnBGColor;

            DGPlayNode.BackgroundColor = this.mDGPlayBGColor;
            DGStopNode.BackgroundColor = this.mDGStopBGColor;
            DGAcOpNode.BackgroundColor = this.mDGAcOpBGColor;
            DGPropNode.BackgroundColor = this.mDGPropBGColor;

            StateNode.PublicTextColor = this.mStatePublicTxtColor;
            StateNode.PrivateTextColor = this.mStatePrivateTxtColor;

            DGSnSnNode.TextColor = this.mDGSnSnTxtColor;

            DGRandNode.TextColor = this.mDGRandTxtColor;

            DGSoPnNode.HeadTextColor = this.mDGSoPnHeadTxtColor;
            DGSoPnNode.BodyTextColor = this.mDGSoPnBodyTxtColor;

            DGSoDnNode.TextColor = this.mDGSoDnTxtColor;

            DGPlayNode.TextColor = this.mDGPlayTxtColor;
            DGStopNode.TextColor = this.mDGStopTxtColor;
            DGAcOpNode.TextColor = this.mDGAcOpTxtColor;
            DGPropNode.TextColor = this.mDGPropTxtColor;
        }

        public void ApplyFontSettingsToCode()
        {
            if (this.mDGSnSnFont == null)
            {
                this.ReadSettingsFromCode();
            }

            StateNode.TextFont = this.mStateFont;

            DGSnSnNode.TextFont = this.mDGSnSnFont;

            DGRandNode.TextFont = this.mDGRandFont;

            DGSoPnNode.HeadFont = this.mDGSoPnHeadFont;
            DGSoPnNode.BodyFont = this.mDGSoPnBodyFont;

            DGSoDnNode.TextFont = this.mDGSoDnFont;

            DGPlayNode.TextFont = this.mDGPlayFont;
            DGStopNode.TextFont = this.mDGStopFont;
            DGAcOpNode.TextFont = this.mDGAcOpFont;
            DGPropNode.TextFont = this.mDGPropFont;
        }

        public float State_Machine_Grid_Size
        {
            get { return this.mStateMachineGridSize; }
            set
            {
                if (value <= 0)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "State Machine Grid Size must be greater than zero.",
                        "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mStateMachineGridSize != value)
                {
                    float oldValue = this.mStateMachineGridSize;
                    this.mStateMachineGridSize = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        StateMachineScene.GridSize = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "StateMachineScene", "GridSize"));
                    }
                }
            }
        }

        public Color State_Machine_Grid_Color
        {
            get { return this.mStateMachineGridColor; }
            set
            {
                if (!this.mStateMachineGridColor.Equals(value))
                {
                    Color oldColor = this.mStateMachineGridColor;
                    this.mStateMachineGridColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateMachineScene.GridColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateMachineScene", "GridColor"));
                    }
                }
            }
        }

        public Color State_Machine_Background_Color
        {
            get { return this.mStateMachineBackColor; }
            set
            {
                if (!this.mStateMachineBackColor.Equals(value))
                {
                    Color oldColor = this.mStateMachineBackColor;
                    this.mStateMachineBackColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateMachineScene.BackColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateMachineScene", "BackColor"));
                    }
                }
            }
        }

        public Color State_Machine_Graph_Hider_Color
        {
            get
            {
                return Color.FromArgb(255,
                    this.mStateMachineGraphHiderColor.R,
                    this.mStateMachineGraphHiderColor.G,
                    this.mStateMachineGraphHiderColor.B);
            }
            set
            {
                Color color = Color.FromArgb(255,
                    this.mStateMachineGraphHiderColor.R,
                    this.mStateMachineGraphHiderColor.G,
                    this.mStateMachineGraphHiderColor.B);
                if (!color.Equals(value))
                {
                    Color oldColor = this.mStateMachineGraphHiderColor;
                    Color newColor = Color.FromArgb(
                        this.mStateMachineGraphHiderColor.A, value);
                    this.mStateMachineGraphHiderColor = newColor;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateMachineScene.GraphHiderColor = newColor;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, newColor,
                                "StateMachineScene", "GraphHiderColor"));
                    }
                }
            }
        }

        [System.ComponentModel.Editor(
            typeof(s3piwrappers.CustomForms.Design.ByteSliderEditor),
            typeof(System.Drawing.Design.UITypeEditor))]
        public byte State_Machine_Graph_Hider_Opacity
        {
            get { return this.mStateMachineGraphHiderColor.A; }
            set
            {
                if (this.mStateMachineGraphHiderColor.A != value)
                {
                    Color oldColor = this.mStateMachineGraphHiderColor;
                    Color newColor = Color.FromArgb(
                        value, this.mStateMachineGraphHiderColor);
                    this.mStateMachineGraphHiderColor = newColor;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateMachineScene.GraphHiderColor = newColor;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, newColor,
                                "StateMachineScene", "GraphHiderColor"));
                    }
                }
            }
        }

        public Color State_Machine_Selection_Lasso_Color
        {
            get { return this.mStateMachineLassoColor; }
            set
            {
                if (!this.mStateMachineLassoColor.Equals(value))
                {
                    Color oldColor = this.mStateMachineLassoColor;
                    this.mStateMachineLassoColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateMachineScene.LassoColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateMachineScene", "LassoColor"));
                    }
                }
            }
        }

        public float State_Node_Maximized_Radius
        {
            get { return this.mStateMaxRadius; }
            set
            {
                if (value <= this.mStateMinRadius)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "State Node Maximized Radius must be greater" + 
                        "than State Node Minimized Radius", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK, 
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mStateMaxRadius != value)
                {
                    float oldValue = this.mStateMaxRadius;
                    this.mStateMaxRadius = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        StateNode.MaximizedRadius = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "StateNode", "MaximizedRadius"));
                    }
                }
            }
        }

        public float State_Node_Minimized_Radius
        {
            get { return this.mStateMinRadius; }
            set
            {
                if (value >= this.mStateMaxRadius)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "State Node Minimized Radius must be less" +
                        "than State Node Maximized Radius", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (value <= 0)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "State Node Minimized Radius must be greater" +
                        "than zero.", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mStateMinRadius != value)
                {
                    float oldValue = this.mStateMinRadius;
                    this.mStateMinRadius = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        StateNode.MinimizedRadius = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "StateNode", "MinimizedRadius"));
                    }
                }
            }
        }

        public Color State_Node_Public_Maximized_Color
        {
            get { return this.mStatePublicMaxColor; }
            set
            {
                if (!this.mStatePublicMaxColor.Equals(value))
                {
                    Color oldColor = this.mStatePublicMaxColor;
                    this.mStatePublicMaxColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PublicMaximizedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PublicMaximizedColor"));
                    }
                }
            }
        }

        public Color State_Node_Private_Maximized_Color
        {
            get { return this.mStatePrivateMaxColor; }
            set
            {
                if (!this.mStatePrivateMaxColor.Equals(value))
                {
                    Color oldColor = this.mStatePrivateMaxColor;
                    this.mStatePrivateMaxColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PrivateMaximizedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PrivateMaximizedColor"));
                    }
                }
            }
        }

        public Color State_Node_Public_Minimized_Color
        {
            get { return this.mStatePublicMinColor; }
            set
            {
                if (!this.mStatePublicMinColor.Equals(value))
                {
                    Color oldColor = this.mStatePublicMinColor;
                    this.mStatePublicMinColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PublicMinimizedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PublicMinimizedColor"));
                    }
                }
            }
        }

        public Color State_Node_Private_Minimized_Color
        {
            get { return this.mStatePrivateMinColor; }
            set
            {
                if (!this.mStatePrivateMinColor.Equals(value))
                {
                    Color oldColor = this.mStatePrivateMinColor;
                    this.mStatePrivateMinColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PrivateMinimizedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PrivateMinimizedColor"));
                    }
                }
            }
        }

        public Color State_Node_Border_Normal_Color
        {
            get { return this.mStateNodeBorderNormalColor; }
            set
            {
                if (!this.mStateNodeBorderNormalColor.Equals(value))
                {
                    Color oldColor = this.mStateNodeBorderNormalColor;
                    this.mStateNodeBorderNormalColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.BorderNormalColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "BorderNormalColor"));
                    }
                }
            }
        }

        public Color State_Node_Border_Selected_Color
        {
            get { return this.mStateNodeBorderSelectColor; }
            set
            {
                if (!this.mStateNodeBorderSelectColor.Equals(value))
                {
                    Color oldColor = this.mStateNodeBorderSelectColor;
                    this.mStateNodeBorderSelectColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.BorderSelectedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "BorderSelectedColor"));
                    }
                }
            }
        }

        public float State_Transition_Angle
        {
            get { return (float)(180.0 * this.mStateEdgeAngle / Math.PI); }
            set
            {
                if (value < 0 || value > 90)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "State Transition Angle must be between" +
                        "0° and 90°, inclusive.", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                double newAngle = Math.PI * value / 180.0;
                // just in case of a possible round-off error
                newAngle = Math.Min(newAngle, Math.PI / 2);
                if (this.mStateEdgeAngle != newAngle)
                {
                    float oldValue = (float)this.mStateEdgeAngle;
                    this.mStateEdgeAngle = newAngle;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        StateEdge.Angle = newAngle;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, 
                                (float)newAngle, "StateEdge", "Angle"));
                    }
                }
            }
        }

        public Color State_Transition_Normal_Color
        {
            get { return this.mStateEdgeBorderNormalColor; }
            set
            {
                if (!this.mStateEdgeBorderNormalColor.Equals(value))
                {
                    Color oldColor = this.mStateEdgeBorderNormalColor;
                    this.mStateEdgeBorderNormalColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateEdge.BorderNormalColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateEdge", "BorderNormalColor"));
                    }
                }
            }
        }

        public Color State_Transition_Selected_Color
        {
            get { return this.mStateEdgeBorderSelectColor; }
            set
            {
                if (!this.mStateEdgeBorderSelectColor.Equals(value))
                {
                    Color oldColor = this.mStateEdgeBorderSelectColor;
                    this.mStateEdgeBorderSelectColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateEdge.BorderSelectedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateEdge", "BorderSelectedColor"));
                    }
                }
            }
        }

        public float Anchor_Point_Radius
        {
            get { return this.mAnchorPointRadius; }
            set
            {
                if (value <= 0)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Anchor Point Radius must be greater than zero.",
                        "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mAnchorPointRadius != value)
                {
                    float oldValue = this.mAnchorPointRadius;
                    this.mAnchorPointRadius = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        AnchorPoint.Radius = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "AnchorPoint", "Radius"));
                    }
                }
            }
        }

        public Color Anchor_Point_Color
        {
            get { return this.mAnchorPointColor; }
            set
            {
                if (!this.mAnchorPointColor.Equals(value))
                {
                    Color oldColor = this.mAnchorPointColor;
                    this.mAnchorPointColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        AnchorPoint.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "AnchorPoint", "BackgroundColor"));
                    }
                }
            }
        }

        public float Decision_Graph_Link_Curve_Size
        {
            get { return this.mDGEdgeCurveSize; }
            set
            {
                if (value <= 0)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "Decision Graph Link Curve Size must be " + 
                        "greater than zero.", "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mDGEdgeCurveSize != value)
                {
                    float oldValue = this.mDGEdgeCurveSize;
                    this.mDGEdgeCurveSize = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        DGEdge.CurveSize = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "DGEdge", "CurveSize"));
                    }
                }
            }
        }

        public Color Decision_Graph_Link_Normal_Color
        {
            get { return this.mDGEdgeBorderNormalColor; }
            set
            {
                if (!this.mDGEdgeBorderNormalColor.Equals(value))
                {
                    Color oldColor = this.mDGEdgeBorderNormalColor;
                    this.mDGEdgeBorderNormalColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGEdge.BorderNormalColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGEdge", "BorderNormalColor"));
                    }
                }
            }
        }

        public Color Decision_Graph_Link_In_Edit_Color
        {
            get { return this.mDGEdgeBorderInEditColor; }
            set
            {
                if (!this.mDGEdgeBorderInEditColor.Equals(value))
                {
                    Color oldColor = this.mDGEdgeBorderInEditColor;
                    this.mDGEdgeBorderInEditColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGEdge.BorderInEditColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGEdge", "BorderInEditColor"));
                    }
                }
            }
        }

        public Color Decision_Graph_Link_Selected_Color
        {
            get { return this.mDGEdgeBorderSelectColor; }
            set
            {
                if (!this.mDGEdgeBorderSelectColor.Equals(value))
                {
                    Color oldColor = this.mDGEdgeBorderSelectColor;
                    this.mDGEdgeBorderSelectColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGEdge.BorderSelectedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGEdge", "BorderSelectedColor"));
                    }
                }
            }
        }

        public Color Decision_Graph_Node_Border_Normal_Color
        {
            get { return this.mDGNodeBorderNormalColor; }
            set
            {
                if (!this.mDGNodeBorderNormalColor.Equals(value))
                {
                    Color oldColor = this.mDGNodeBorderNormalColor;
                    this.mDGNodeBorderNormalColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGNode.BorderNormalColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGNode", "BorderNormalColor"));
                    }
                }
            }
        }

        public Color Decision_Graph_Node_Border_In_Edit_Color
        {
            get { return this.mDGNodeBorderInEditColor; }
            set
            {
                if (!this.mDGNodeBorderInEditColor.Equals(value))
                {
                    Color oldColor = this.mDGNodeBorderInEditColor;
                    this.mDGNodeBorderInEditColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGNode.BorderInEditColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGNode", "BorderInEditColor"));
                    }
                }
            }
        }

        public Color Decision_Graph_Node_Border_Selected_Color
        {
            get { return this.mDGNodeBorderSelectColor; }
            set
            {
                if (!this.mDGNodeBorderSelectColor.Equals(value))
                {
                    Color oldColor = this.mDGNodeBorderSelectColor;
                    this.mDGNodeBorderSelectColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGNode.BorderSelectedColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGNode", "BorderSelectedColor"));
                    }
                }
            }
        }

        public float DG_Root_Radius
        {
            get { return this.mDGRootRadius; }
            set
            {
                if (value <= 0)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "DG Root Radius must be greater than zero.", 
                        "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (this.mDGRootRadius != value)
                {
                    float oldValue = this.mDGRootRadius;
                    this.mDGRootRadius = value;
                    if (this.bApplyFloatChangesImmediately)
                    {
                        DGRootNode.Radius = value;
                    }
                    if (this.FloatSettingChanged != null)
                    {
                        this.FloatSettingChanged(this,
                            new FloatSettingEventArgs(oldValue, value,
                                "DGRootNode", "Radius"));
                    }
                }
            }
        }

        public Color DG_Root_Color
        {
            get { return this.mDGRootBGColor; }
            set
            {
                if (!this.mDGRootBGColor.Equals(value))
                {
                    Color oldColor = this.mDGRootBGColor;
                    this.mDGRootBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGRootNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGRootNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Next_State_Node_Color
        {
            get { return this.mDGSnSnBGColor; }
            set
            {
                if (!this.mDGSnSnBGColor.Equals(value))
                {
                    Color oldColor = this.mDGSnSnBGColor;
                    this.mDGSnSnBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSnSnNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSnSnNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Random_Node_Color
        {
            get { return this.mDGRandBGColor; }
            set
            {
                if (!this.mDGRandBGColor.Equals(value))
                {
                    Color oldColor = this.mDGRandBGColor;
                    this.mDGRandBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGRandNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGRandNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Select_On_Parameter_Node_Head_Color
        {
            get { return this.mDGSoPnHeadColor; }
            set
            {
                if (!this.mDGSoPnHeadColor.Equals(value))
                {
                    Color oldColor = this.mDGSoPnHeadColor;
                    this.mDGSoPnHeadColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoPnNode.HeadColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoPnNode", "HeadColor"));
                    }
                }
            }
        }

        public Color DG_Select_On_Parameter_Node_Body_Color
        {
            get { return this.mDGSoPnBodyColor; }
            set
            {
                if (!this.mDGSoPnBodyColor.Equals(value))
                {
                    Color oldColor = this.mDGSoPnBodyColor;
                    this.mDGSoPnBodyColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoPnNode.BodyColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoPnNode", "BodyColor"));
                    }
                }
            }
        }

        public Color DG_Select_On_Destination_Node_Color
        {
            get { return this.mDGSoDnBGColor; }
            set
            {
                if (!this.mDGSoDnBGColor.Equals(value))
                {
                    Color oldColor = this.mDGSoDnBGColor;
                    this.mDGSoDnBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoDnNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoDnNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Play_Animation_Node_Color
        {
            get { return this.mDGPlayBGColor; }
            set
            {
                if (!this.mDGPlayBGColor.Equals(value))
                {
                    Color oldColor = this.mDGPlayBGColor;
                    this.mDGPlayBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGPlayNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGPlayNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Stop_Animation_Node_Color
        {
            get { return this.mDGStopBGColor; }
            set
            {
                if (!this.mDGStopBGColor.Equals(value))
                {
                    Color oldColor = this.mDGStopBGColor;
                    this.mDGStopBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGStopNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGStopNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Actor_Operation_Node_Color
        {
            get { return this.mDGAcOpBGColor; }
            set
            {
                if (!this.mDGAcOpBGColor.Equals(value))
                {
                    Color oldColor = this.mDGAcOpBGColor;
                    this.mDGAcOpBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGAcOpNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGAcOpNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Color DG_Create_Prop_Node_Color
        {
            get { return this.mDGPropBGColor; }
            set
            {
                if (!this.mDGPropBGColor.Equals(value))
                {
                    Color oldColor = this.mDGPropBGColor;
                    this.mDGPropBGColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGPropNode.BackgroundColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGPropNode", "BackgroundColor"));
                    }
                }
            }
        }

        public Font State_Node_Font
        {
            get { return this.mStateFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mStateFont.Equals(value))
                {
                    Font oldFont = this.mStateFont;
                    this.mStateFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        StateNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "StateNode", "TextFont"));
                    }
                }
            }
        }

        public Color State_Node_Public_Text_Color
        {
            get { return this.mStatePublicTxtColor; }
            set
            {
                if (!this.mStatePublicTxtColor.Equals(value))
                {
                    Color oldColor = this.mStatePublicTxtColor;
                    this.mStatePublicTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PublicTextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PublicTextColor"));
                    }
                }
            }
        }

        public Color State_Node_Private_Text_Color
        {
            get { return this.mStatePrivateTxtColor; }
            set
            {
                if (!this.mStatePrivateTxtColor.Equals(value))
                {
                    Color oldColor = this.mStatePrivateTxtColor;
                    this.mStatePrivateTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        StateNode.PrivateTextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "StateNode", "PrivateTextColor"));
                    }
                }
            }
        }

        public Font DG_Next_State_Node_Font
        {
            get { return this.mDGSnSnFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGSnSnFont.Equals(value))
                {
                    Font oldFont = this.mDGSnSnFont;
                    this.mDGSnSnFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGSnSnNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGSnSnNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Next_State_Node_Text_Color
        {
            get { return this.mDGSnSnTxtColor; }
            set
            {
                if (!this.mDGSnSnTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGSnSnTxtColor;
                    this.mDGSnSnTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSnSnNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSnSnNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Random_Node_Font
        {
            get { return this.mDGRandFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGRandFont.Equals(value))
                {
                    Font oldFont = this.mDGRandFont;
                    this.mDGRandFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGRandNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGRandNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Random_Node_Text_Color
        {
            get { return this.mDGRandTxtColor; }
            set
            {
                if (!this.mDGRandTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGRandTxtColor;
                    this.mDGRandTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGRandNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGRandNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Select_On_Parameter_Node_Head_Font
        {
            get { return this.mDGSoPnHeadFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGSoPnHeadFont.Equals(value))
                {
                    Font oldFont = this.mDGSoPnHeadFont;
                    this.mDGSoPnHeadFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGSoPnNode.HeadFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGSoPnNode", "HeadFont"));
                    }
                }
            }
        }

        public Color DG_Select_On_Parameter_Node_Head_Text_Color
        {
            get { return this.mDGSoPnHeadTxtColor; }
            set
            {
                if (!this.mDGSoPnHeadTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGSoPnHeadTxtColor;
                    this.mDGSoPnHeadTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoPnNode.HeadTextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoPnNode", "HeadTextColor"));
                    }
                }
            }
        }

        public Font DG_Select_On_Parameter_Node_Body_Font
        {
            get { return this.mDGSoPnBodyFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGSoPnBodyFont.Equals(value))
                {
                    Font oldFont = this.mDGSoPnBodyFont;
                    this.mDGSoPnBodyFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGSoPnNode.BodyFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGSoPnNode", "BodyFont"));
                    }
                }
            }
        }

        public Color DG_Select_On_Parameter_Node_Body_Text_Color
        {
            get { return this.mDGSoPnBodyTxtColor; }
            set
            {
                if (!this.mDGSoPnBodyTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGSoPnBodyTxtColor;
                    this.mDGSoPnBodyTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoPnNode.BodyTextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoPnNode", "BodyTextColor"));
                    }
                }
            }
        }

        public Font DG_Select_On_Destination_Node_Font
        {
            get { return this.mDGSoDnFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGSoDnFont.Equals(value))
                {
                    Font oldFont = this.mDGSoDnFont;
                    this.mDGSoDnFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGSoDnNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGSoDnNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Select_On_Destination_Node_Text_Color
        {
            get { return this.mDGSoDnTxtColor; }
            set
            {
                if (!this.mDGSoDnTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGSoDnTxtColor;
                    this.mDGSoDnTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGSoDnNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGSoDnNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Play_Animation_Node_Font
        {
            get { return this.mDGPlayFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGPlayFont.Equals(value))
                {
                    Font oldFont = this.mDGPlayFont;
                    this.mDGPlayFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGPlayNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGPlayNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Play_Animation_Node_Text_Color
        {
            get { return this.mDGPlayTxtColor; }
            set
            {
                if (!this.mDGPlayTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGPlayTxtColor;
                    this.mDGPlayTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGPlayNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGPlayNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Stop_Animation_Node_Font
        {
            get { return this.mDGStopFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGStopFont.Equals(value))
                {
                    Font oldFont = this.mDGStopFont;
                    this.mDGStopFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGStopNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGStopNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Stop_Animation_Node_Text_Color
        {
            get { return this.mDGStopTxtColor; }
            set
            {
                if (!this.mDGStopTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGStopTxtColor;
                    this.mDGStopTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGStopNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGStopNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Actor_Operation_Node_Font
        {
            get { return this.mDGAcOpFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGAcOpFont.Equals(value))
                {
                    Font oldFont = this.mDGAcOpFont;
                    this.mDGAcOpFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGAcOpNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGAcOpNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Actor_Operation_Node_Text_Color
        {
            get { return this.mDGAcOpTxtColor; }
            set
            {
                if (!this.mDGAcOpTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGAcOpTxtColor;
                    this.mDGAcOpTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGAcOpNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGAcOpNode", "TextColor"));
                    }
                }
            }
        }

        public Font DG_Create_Prop_Node_Font
        {
            get { return this.mDGPropFont; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!this.mDGPropFont.Equals(value))
                {
                    Font oldFont = this.mDGPropFont;
                    this.mDGPropFont = value;
                    if (this.bApplyFontChangesImmediately)
                    {
                        DGPropNode.TextFont = value;
                    }
                    if (this.FontSettingChanged != null)
                    {
                        this.FontSettingChanged(this,
                            new FontSettingEventArgs(oldFont, value,
                                "DGPropNode", "TextFont"));
                    }
                }
            }
        }

        public Color DG_Create_Prop_Node_Text_Color
        {
            get { return this.mDGPropTxtColor; }
            set
            {
                if (!this.mDGPropTxtColor.Equals(value))
                {
                    Color oldColor = this.mDGPropTxtColor;
                    this.mDGPropTxtColor = value;
                    if (this.bApplyColorChangesImmediately)
                    {
                        DGPropNode.TextColor = value;
                    }
                    if (this.ColorSettingChanged != null)
                    {
                        this.ColorSettingChanged(this,
                            new ColorSettingEventArgs(oldColor, value,
                                "DGPropNode", "TextColor"));
                    }
                }
            }
        }
    }
}
