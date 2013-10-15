namespace s3piwrappers.FreeformJazz
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageGraphToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportScriptStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packageAllIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shuffleStatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeStatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewTransitionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTransitionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decisionGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shuffleDGNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewDGNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDGNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewDGLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDGLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutStateNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutDGNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jazzGraphTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportAllScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllLinksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.jazzGraphTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.stateGraphToolStripMenuItem,
            this.decisionGraphToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(584, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.packageGraphToolStripSeparator,
            this.closeToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripSeparator2,
            this.packageIntoToolStripMenuItem,
            this.packageAllIntoToolStripMenuItem,
            this.toolStripSeparator3,
            this.exportScriptStripMenuItem,
            this.exportAllScriptsToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.new_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.open_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.load_Click);
            // 
            // packageGraphToolStripSeparator
            // 
            this.packageGraphToolStripSeparator.Name = "packageGraphToolStripSeparator";
            this.packageGraphToolStripSeparator.Size = new System.Drawing.Size(194, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.close_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Enabled = false;
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.closeAllToolStripMenuItem.Text = "Close All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.save_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Enabled = false;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAll_Click);
            // 
            // exportScriptStripMenuItem
            // 
            this.exportScriptStripMenuItem.Enabled = false;
            this.exportScriptStripMenuItem.Name = "exportScriptStripMenuItem";
            this.exportScriptStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exportScriptStripMenuItem.Text = "Export script...";
            this.exportScriptStripMenuItem.Click += new System.EventHandler(this.exportSource_Click);
            // 
            // packageIntoToolStripMenuItem
            // 
            this.packageIntoToolStripMenuItem.Enabled = false;
            this.packageIntoToolStripMenuItem.Name = "packageIntoToolStripMenuItem";
            this.packageIntoToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.packageIntoToolStripMenuItem.Text = "Package into...";
            this.packageIntoToolStripMenuItem.Click += new System.EventHandler(this.packageInto_Click);
            // 
            // packageAllIntoToolStripMenuItem
            // 
            this.packageAllIntoToolStripMenuItem.Enabled = false;
            this.packageAllIntoToolStripMenuItem.Name = "packageAllIntoToolStripMenuItem";
            this.packageAllIntoToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.packageAllIntoToolStripMenuItem.Text = "Package all into...";
            this.packageAllIntoToolStripMenuItem.Click += new System.EventHandler(this.packageAllInto_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exit_Click);
            // 
            // stateGraphToolStripMenuItem
            // 
            this.stateGraphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shuffleStatesToolStripMenuItem,
            this.addNewStateToolStripMenuItem,
            this.minimizeStatesToolStripMenuItem,
            this.removeStatesToolStripMenuItem,
            this.addNewTransitionsToolStripMenuItem,
            this.removeTransitionsToolStripMenuItem});
            this.stateGraphToolStripMenuItem.Enabled = false;
            this.stateGraphToolStripMenuItem.Name = "stateGraphToolStripMenuItem";
            this.stateGraphToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.stateGraphToolStripMenuItem.Text = "State Graph";
            // 
            // shuffleStatesToolStripMenuItem
            // 
            this.shuffleStatesToolStripMenuItem.Name = "shuffleStatesToolStripMenuItem";
            this.shuffleStatesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.shuffleStatesToolStripMenuItem.Text = "Shuffle State Nodes";
            this.shuffleStatesToolStripMenuItem.Click += new System.EventHandler(this.shuffleStateNodes_Click);
            // 
            // addNewStateToolStripMenuItem
            // 
            this.addNewStateToolStripMenuItem.Enabled = false;
            this.addNewStateToolStripMenuItem.Name = "addNewStateToolStripMenuItem";
            this.addNewStateToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.addNewStateToolStripMenuItem.Text = "Add New State Node...";
            this.addNewStateToolStripMenuItem.Click += new System.EventHandler(this.addNewState_Click);
            // 
            // minimizeStatesToolStripMenuItem
            // 
            this.minimizeStatesToolStripMenuItem.Enabled = false;
            this.minimizeStatesToolStripMenuItem.Name = "minimizeStatesToolStripMenuItem";
            this.minimizeStatesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.minimizeStatesToolStripMenuItem.Text = "Minimize State Node(s)";
            this.minimizeStatesToolStripMenuItem.Click += new System.EventHandler(this.minimizeStates_Click);
            // 
            // removeStatesToolStripMenuItem
            // 
            this.removeStatesToolStripMenuItem.Enabled = false;
            this.removeStatesToolStripMenuItem.Name = "removeStatesToolStripMenuItem";
            this.removeStatesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.removeStatesToolStripMenuItem.Text = "Remove State Node(s)";
            this.removeStatesToolStripMenuItem.Click += new System.EventHandler(this.removeStates_Click);
            // 
            // addNewTransitionsToolStripMenuItem
            // 
            this.addNewTransitionsToolStripMenuItem.CheckOnClick = true;
            this.addNewTransitionsToolStripMenuItem.Enabled = false;
            this.addNewTransitionsToolStripMenuItem.Name = "addNewTransitionsToolStripMenuItem";
            this.addNewTransitionsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.addNewTransitionsToolStripMenuItem.Text = "Add New Transition(s)";
            this.addNewTransitionsToolStripMenuItem.Click += new System.EventHandler(this.addNewTransitions_Click);
            // 
            // removeTransitionsToolStripMenuItem
            // 
            this.removeTransitionsToolStripMenuItem.Enabled = false;
            this.removeTransitionsToolStripMenuItem.Name = "removeTransitionsToolStripMenuItem";
            this.removeTransitionsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.removeTransitionsToolStripMenuItem.Text = "Remove Transition(s)";
            this.removeTransitionsToolStripMenuItem.Click += new System.EventHandler(this.removeTransitions_Click);
            // 
            // decisionGraphToolStripMenuItem
            // 
            this.decisionGraphToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shuffleDGNodesToolStripMenuItem,
            this.addNewDGNodeToolStripMenuItem,
            this.removeDGNodesToolStripMenuItem,
            this.addNewDGLinksToolStripMenuItem,
            this.removeDGLinksToolStripMenuItem});
            this.decisionGraphToolStripMenuItem.Enabled = false;
            this.decisionGraphToolStripMenuItem.Name = "decisionGraphToolStripMenuItem";
            this.decisionGraphToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.decisionGraphToolStripMenuItem.Text = "Decision Graph";
            // 
            // shuffleDGNodesToolStripMenuItem
            // 
            this.shuffleDGNodesToolStripMenuItem.Name = "shuffleDGNodesToolStripMenuItem";
            this.shuffleDGNodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.shuffleDGNodesToolStripMenuItem.Text = "Shuffle DG Nodes";
            this.shuffleDGNodesToolStripMenuItem.Click += new System.EventHandler(this.shuffleDGNodes_Click);
            // 
            // addNewDGNodeToolStripMenuItem
            // 
            this.addNewDGNodeToolStripMenuItem.Enabled = false;
            this.addNewDGNodeToolStripMenuItem.Name = "addNewDGNodeToolStripMenuItem";
            this.addNewDGNodeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addNewDGNodeToolStripMenuItem.Text = "Add New DG Node...";
            this.addNewDGNodeToolStripMenuItem.Click += new System.EventHandler(this.addNewDGNode_Click);
            // 
            // removeDGNodesToolStripMenuItem
            // 
            this.removeDGNodesToolStripMenuItem.Enabled = false;
            this.removeDGNodesToolStripMenuItem.Name = "removeDGNodesToolStripMenuItem";
            this.removeDGNodesToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.removeDGNodesToolStripMenuItem.Text = "Remove DG Node(s)";
            this.removeDGNodesToolStripMenuItem.Click += new System.EventHandler(this.removeDGNodes_Click);
            // 
            // addNewDGLinksToolStripMenuItem
            // 
            this.addNewDGLinksToolStripMenuItem.CheckOnClick = true;
            this.addNewDGLinksToolStripMenuItem.Enabled = false;
            this.addNewDGLinksToolStripMenuItem.Name = "addNewDGLinksToolStripMenuItem";
            this.addNewDGLinksToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addNewDGLinksToolStripMenuItem.Text = "Add New DG Link(s)";
            this.addNewDGLinksToolStripMenuItem.Click += new System.EventHandler(this.addNewDGLinks_Click);
            // 
            // removeDGLinksToolStripMenuItem
            // 
            this.removeDGLinksToolStripMenuItem.Enabled = false;
            this.removeDGLinksToolStripMenuItem.Name = "removeDGLinksToolStripMenuItem";
            this.removeDGLinksToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.removeDGLinksToolStripMenuItem.Text = "Remove DG Link(s)";
            this.removeDGLinksToolStripMenuItem.Click += new System.EventHandler(this.removeDGLinks_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.layoutStateNodesToolStripMenuItem,
            this.layoutDGNodesToolStripMenuItem,
            this.preferencesToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // layoutStateNodesToolStripMenuItem
            // 
            this.layoutStateNodesToolStripMenuItem.CheckOnClick = true;
            this.layoutStateNodesToolStripMenuItem.Name = "layoutStateNodesToolStripMenuItem";
            this.layoutStateNodesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.layoutStateNodesToolStripMenuItem.Text = "Layout State Nodes";
            this.layoutStateNodesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.layoutStateNodesCheckedChanged);
            // 
            // layoutDGNodesToolStripMenuItem
            // 
            this.layoutDGNodesToolStripMenuItem.CheckOnClick = true;
            this.layoutDGNodesToolStripMenuItem.Name = "layoutDGNodesToolStripMenuItem";
            this.layoutDGNodesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.layoutDGNodesToolStripMenuItem.Text = "Layout DG Nodes";
            this.layoutDGNodesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.layoutDGNodesCheckedChanged);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences...";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferences_Click);
            // 
            // jazzGraphTabControl
            // 
            this.jazzGraphTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jazzGraphTabControl.Controls.Add(this.tabPage1);
            this.jazzGraphTabControl.Location = new System.Drawing.Point(0, 28);
            this.jazzGraphTabControl.Name = "jazzGraphTabControl";
            this.jazzGraphTabControl.SelectedIndex = 0;
            this.jazzGraphTabControl.Size = new System.Drawing.Size(584, 535);
            this.jazzGraphTabControl.TabIndex = 1;
            this.jazzGraphTabControl.SelectedIndexChanged += new System.EventHandler(this.jazzGraphSelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 509);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(194, 6);
            // 
            // exportAllScriptsToolStripMenuItem
            // 
            this.exportAllScriptsToolStripMenuItem.Enabled = false;
            this.exportAllScriptsToolStripMenuItem.Name = "exportAllScriptsToolStripMenuItem";
            this.exportAllScriptsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exportAllScriptsToolStripMenuItem.Text = "Export all scripts...";
            this.exportAllScriptsToolStripMenuItem.Click += new System.EventHandler(this.exportAllSource_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator5,
            this.selectAllToolStripMenuItem,
            this.selectAllNodesToolStripMenuItem,
            this.selectAllLinksToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.editToolStripMenuItem.Enabled = false;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Enabled = false;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undo_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redo_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNone_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(161, 6);
            // 
            // selectAllNodesToolStripMenuItem
            // 
            this.selectAllNodesToolStripMenuItem.Name = "selectAllNodesToolStripMenuItem";
            this.selectAllNodesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllNodesToolStripMenuItem.Text = "Select All Nodes";
            this.selectAllNodesToolStripMenuItem.Click += new System.EventHandler(this.selectAllNodes_Click);
            // 
            // selectAllLinksToolStripMenuItem
            // 
            this.selectAllLinksToolStripMenuItem.Name = "selectAllLinksToolStripMenuItem";
            this.selectAllLinksToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectAllLinksToolStripMenuItem.Text = "Select All Links";
            this.selectAllLinksToolStripMenuItem.Click += new System.EventHandler(this.selectAllLinks_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 562);
            this.Controls.Add(this.jazzGraphTabControl);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.Text = "Freeform Jazz";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.jazzGraphTabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageIntoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packageAllIntoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl jazzGraphTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStripSeparator packageGraphToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportScriptStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutStateNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem layoutDGNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizeStatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeStatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewTransitionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTransitionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decisionGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewDGNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDGNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewDGLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDGLinksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shuffleStatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shuffleDGNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportAllScriptsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem selectAllNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllLinksToolStripMenuItem;
    }
}

