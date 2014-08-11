using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3piwrappers.FreeformJazz.Widgets;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz
{
    public partial class NewDGNodePrompt : Form
    {
        private static string[] sCategoryStrs = new string[]
        {
            "Entry Point",
            "Decision Maker",
            "Both"
        };

        private static DGNode.DGFlag[] sCategoryVals = new DGNode.DGFlag[]
        {
            DGNode.DGFlag.EntryPoint,
            DGNode.DGFlag.DecisionMaker,
            DGNode.DGFlag.Both
        };

        private static string[] sNodeTypeStrs = new string[]
        {
            "Play Animation",
            "Stop Animation",
            "Create Prop",
            "Actor Operation",
            "Goto Next State",
            "Random Selection",
            "Select On Parameter",
            "Select On Destination"
        };

        private static readonly uint[] sNodeTypeVals = new uint[]
        {
            PlayAnimationNode.ResourceType,
            StopAnimationNode.ResourceType,
            CreatePropNode.ResourceType,
            ActorOperationNode.ResourceType,
            NextStateNode.ResourceType,
            RandomNode.ResourceType,
            SelectOnParameterNode.ResourceType,
            SelectOnDestinationNode.ResourceType
        };

        private DGNode.DGFlag mCategory;
        private uint mNodeType;

        public NewDGNodePrompt()
        {
            InitializeComponent();

            this.Text = MainForm.kName + ": Create New DG Node";
            this.categoryCMB.Items.AddRange(sCategoryStrs);
            this.nodeTypeCMB.Items.AddRange(sNodeTypeStrs);

            this.mCategory = (DGNode.DGFlag)0;
            this.mNodeType = 0;
        }

        public DGNode.DGFlag Category
        {
            get { return this.mCategory; }
        }

        public uint NodeType
        {
            get { return this.mNodeType; }
        }

        private void categorySelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.categoryCMB.SelectedIndex;
            if (index >= 0)
            {
                this.mCategory = sCategoryVals[index];
                this.okBTN.Enabled = this.nodeTypeCMB.SelectedIndex >= 0;
            }
            else
            {
                this.mCategory = (DGNode.DGFlag)0;
                this.okBTN.Enabled = false;
            }
        }

        private void nodeTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.nodeTypeCMB.SelectedIndex;
            if (index >= 0)
            {
                this.mNodeType = sNodeTypeVals[index];
                this.okBTN.Enabled = this.categoryCMB.SelectedIndex >= 0;
            }
            else
            {
                this.mNodeType = 0;
                this.okBTN.Enabled = false;
            }
        }
    }
}
