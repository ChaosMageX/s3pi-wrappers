using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.Package;
using s3piwrappers.CustomForms;
using s3piwrappers.FreeformJazz.Widgets;
using s3piwrappers.Helpers;
using s3piwrappers.Helpers.Resources;
using s3piwrappers.JazzGraph;

namespace s3piwrappers.FreeformJazz
{
    public partial class MainForm : Form
    {
        public const string kName = "Freeform Jazz";

        public const string kPackageExt = ".package";

        private static readonly string kJazzPackageDialogFilter;
            //= "Sims 3 Packages (*.package)|*.package|All Files (*.*)|*.*";
        private static readonly string kJazzScriptDialogFilter;
            //= "Jazz Scripts (*.jazz)|*.jazz|All Files (*.*)|*.*";
        private static readonly string kKeyNameMapDialogFilter;
            //= "Key Name Maps (*.keyname)|*.keyname|All Files (*.*)|*.*";
        
        private const uint kKeyNameMapTID = 0x0166038C;

        private static bool sExportAllNames = false;

        static MainForm()
        {
            kJazzPackageDialogFilter = "Sims 3 Packages (*" +
                kPackageExt + ")|*" + 
                kPackageExt + "|All Files (*.*)|*.*";
            kJazzScriptDialogFilter = "Jazz Scripts (*" + 
                GlobalManager.kJazzExt + ")|*" + 
                GlobalManager.kJazzExt + "|All Files (*.*)|*.*";
            kKeyNameMapDialogFilter = "Key Name Maps (*" +
                KeyNameMap.NameMapExt + ")|*" +
                KeyNameMap.NameMapExt + "|All Files (*.*)|*.*";
        }

        public static void ShowException(Exception ex)
        {
            ShowException(ex, "", kName + ": Program Exception");
        }

        public static void ShowException(Exception ex, 
            string prefix, string caption)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(prefix))
            {
                builder.AppendLine(prefix);
            }
            builder.AppendLine("== START ==");
            for (Exception e = ex; e != null; e = e.InnerException)
            {
                builder.AppendLine("Source: " + e.Source);
                builder.AppendLine("Assembly: " 
                    + e.TargetSite.DeclaringType.Assembly.FullName);
                builder.AppendLine(e.Message);
                System.Diagnostics.StackTrace trace 
                    = new System.Diagnostics.StackTrace(ex, false);
                builder.AppendLine(trace.ToString());
                builder.AppendLine("-----");
            }
            builder.Append("== END ==");
            CopyableMessageBox.Show(builder.ToString(), caption, 
                CopyableMessageBoxButtons.OK,
                CopyableMessageBoxIcon.Error, 0);
        }

        private static string[] sInvalidPathOldValues;
        private static string[] sInvalidPathNewValues;

        private static void InitInvalidPathValues()
        {
            char[] ifChars = Path.GetInvalidFileNameChars();
            char[] ipChars = Path.GetInvalidPathChars();
            sInvalidPathOldValues 
                = new string[ifChars.Length + ipChars.Length + 1];
            sInvalidPathNewValues
                = new string[ifChars.Length + ipChars.Length + 1];
            int i, j = 0;
            for (i = 0; i < ifChars.Length; i++)
            {
                sInvalidPathOldValues[j] 
                    = new string(new char[] { ifChars[i] });
                sInvalidPathNewValues[j++]
                    = string.Format("%{0:x2}", (uint)ifChars[i]);
            }
            for (i = 0; i < ipChars.Length; i++)
            {
                sInvalidPathOldValues[j]
                    = new string(new char[] { ipChars[i] });
                sInvalidPathNewValues[j++]
                    = string.Format("%{0:x2}", (uint)ipChars[i]);
            }
            sInvalidPathOldValues[j] = "-";
            sInvalidPathNewValues[j] = string.Format("%{0:x2}", (uint)'-');
        }

        private static string EscapePathString(string path)
        {
            if (sInvalidPathOldValues == null)
            {
                InitInvalidPathValues();
            }
            for (int i = sInvalidPathOldValues.Length - 1; i >= 0; i--)
            {
                path = path.Replace(sInvalidPathOldValues[i], 
                                    sInvalidPathNewValues[i]);
            }
            return path;
        }

        private static string UnescapePathString(string path)
        {
            int i;
            byte b;
            string bad, rep;
            for (i = path.IndexOf('%'); i >= 0 && (i + 2) < path.Length;
                 i = path.IndexOf('%'))
            {
                bad = path.Substring(i + 1, 2);
                b = Convert.ToByte(bad, 16);
                rep = new string(new char[] { (char)b });
                path = path.Replace(string.Concat("%", bad), rep);
            }
            return path;
        }

        private static string CreateJazzScriptFileName(
            IResourceKey key, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Format(
                    "S3_{0:X8}_{1:X8}_{2:X16}%%+{3}",
                    GlobalManager.kJazzTID, key.ResourceGroup, key.Instance, 
                    GlobalManager.kJazzS3End);
            }
            else
            {
                return string.Format(
                    "S3_{0:X8}_{1:X8}_{2:X16}_{3}%%+{4}",
                    GlobalManager.kJazzTID, key.ResourceGroup, key.Instance,
                    EscapePathString(name), GlobalManager.kJazzS3End);
            }
        }

        private static string CreateKeyNameMapFileName(
            IResourceKey key, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Format(
                    "S3_{0:X8}_{1:X8}_{2:X16}%%+{3}",
                    KeyNameMap.NameMapTID, key.ResourceGroup, key.Instance,
                    KeyNameMap.NameMapS3End);
            }
            else
            {
                return string.Format(
                    "S3_{0:X8}_{1:X8}_{2:X16}_{3}%%+{4}",
                    KeyNameMap.NameMapTID, key.ResourceGroup, key.Instance,
                    EscapePathString(name), KeyNameMap.NameMapS3End);
            }
        }

        public static string CreateTitle(string packagePath, bool readOnly)
        {
            string fn = packagePath;
            if (fn.Length > 120)
            {
                int i = fn.IndexOf(Path.DirectorySeparatorChar);
                if (i < 0)
                {
                    i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                }
                while (i >= 0 && fn.Length > 120)
                {
                    fn = fn.Substring(i + 1);
                    i = fn.IndexOf(Path.DirectorySeparatorChar);
                    if (i < 0)
                    {
                        i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                    }
                }
                fn = Path.Combine("...", fn);
            }
            return (readOnly ? ": [RO] " : ": [RW] ") + fn;
        }

        private static string CreateTitle(
            string jazzScriptPath, string keyNameMapPath)
        {
            string fn = jazzScriptPath;
            if (fn.Length > 60)
            {
                int i = fn.IndexOf(Path.DirectorySeparatorChar);
                if (i < 0)
                {
                    i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                }
                while (i >= 0 && fn.Length > 60)
                {
                    fn = fn.Substring(i + 1);
                    i = fn.IndexOf(Path.DirectorySeparatorChar);
                    if (i < 0)
                    {
                        i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                    }
                }
                fn = Path.Combine("...", fn);
            }
            string result = kName + ": " + fn;
            if (!string.IsNullOrEmpty(keyNameMapPath))
            {
                fn = keyNameMapPath;
                if (fn.Length > 60)
                {
                    int i = fn.IndexOf(Path.DirectorySeparatorChar);
                    if (i < 0)
                    {
                        i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                    }
                    while (i >= 0 && fn.Length > 60)
                    {
                        fn = fn.Substring(i + 1);
                        i = fn.IndexOf(Path.DirectorySeparatorChar);
                        if (i < 0)
                        {
                            i = fn.IndexOf(Path.AltDirectorySeparatorChar);
                        }
                    }
                    fn = Path.Combine("...", fn);
                }
                result = result + ", " + fn;
            }
            return result;
        }

        private static bool isJazz(IResourceIndexEntry rie)
        {
            return rie.ResourceType == GlobalManager.kJazzTID;
        }
        private static readonly Predicate<IResourceIndexEntry> sIsJazz
            = new Predicate<IResourceIndexEntry>(isJazz);

        private class JazzRIEFinder
        {
            public uint GID;
            public ulong IID;

            public Predicate<IResourceIndexEntry> Match;

            public bool IsMatch(IResourceIndexEntry rie)
            {
                return rie.ResourceType == GlobalManager.kJazzTID
                    && rie.ResourceGroup == this.GID
                    && rie.Instance == this.IID;
            }

            public JazzRIEFinder(uint gid, ulong iid)
            {
                this.GID = gid;
                this.IID = iid;
                this.Match = new Predicate<IResourceIndexEntry>(
                    this.IsMatch);
            }
        }

        private List<JazzPackage> mOpenPackages
            = new List<JazzPackage>();
        private List<JazzGraphContainer> mOpenGraphs
            = new List<JazzGraphContainer>();

        private OpenFileDialog mOpenPackageDialog;
        private OpenFileDialog mOpenJazzScriptDialog;
        private OpenFileDialog mOpenKeyNameMapDialog;
        private SaveFileDialog mSavePackageDialog;
        private SaveFileDialog mSaveJazzScriptDialog;
        private SaveFileDialog mSaveKeyNameMapDialog;
        private SettingsDialog mSettingsDialog;

        public MainForm()
        {
            InitializeComponent();
            this.jazzGraphTabControl.TabPages.Clear();

            this.layoutStateNodesToolStripMenuItem.ToolTipText =
                "Start automatic state node layout\n" +
                "when state node(s) moved by mouse";
            this.layoutDGNodesToolStripMenuItem.ToolTipText =
                "Start automatic DG node layout\n" +
                "when DG node(s) moved by mouse";

            this.mOpenPackageDialog = new OpenFileDialog();
            this.mOpenPackageDialog.AddExtension = true;
            this.mOpenPackageDialog.CheckFileExists = true;
            this.mOpenPackageDialog.CheckPathExists = true;
            //this.mOpenPackageDialog.DefaultExt = kPackageExt;
            this.mOpenPackageDialog.Filter = kJazzPackageDialogFilter;
            this.mOpenPackageDialog.ShowReadOnly = true;
            this.mOpenPackageDialog.SupportMultiDottedExtensions = true;
            this.mOpenPackageDialog.Title = "Open Package";

            this.mOpenJazzScriptDialog = new OpenFileDialog();
            this.mOpenJazzScriptDialog.AddExtension = true;
            this.mOpenJazzScriptDialog.CheckFileExists = true;
            this.mOpenJazzScriptDialog.CheckPathExists = true;
            //this.mOpenJazzScriptDialog.DefaultExt = GlobalManager.kJazzExt;
            this.mOpenJazzScriptDialog.Filter = kJazzScriptDialogFilter;
            this.mOpenJazzScriptDialog.SupportMultiDottedExtensions = true;
            this.mOpenJazzScriptDialog.Title = "Import Jazz Script";

            this.mOpenKeyNameMapDialog = new OpenFileDialog();
            this.mOpenKeyNameMapDialog.AddExtension = true;
            this.mOpenKeyNameMapDialog.CheckFileExists = true;
            this.mOpenKeyNameMapDialog.CheckPathExists = true;
            //this.mOpenKeyNameMapDialog.DefaultExt = KeyNameMap.NameMapExt;
            this.mOpenKeyNameMapDialog.Filter = kKeyNameMapDialogFilter;
            this.mOpenKeyNameMapDialog.SupportMultiDottedExtensions = true;
            this.mOpenKeyNameMapDialog.Title = "Import Key Name Map";

            this.mSavePackageDialog = new SaveFileDialog();
            this.mSavePackageDialog.AddExtension = true;
            //this.mSavePackageDialog.DefaultExt = kPackageExt;
            this.mSavePackageDialog.Filter = kJazzPackageDialogFilter;
            this.mSavePackageDialog.OverwritePrompt = true;
            this.mSavePackageDialog.SupportMultiDottedExtensions = true;
            this.mSavePackageDialog.Title = "Save Package As";

            this.mSaveJazzScriptDialog = new SaveFileDialog();
            this.mSaveJazzScriptDialog.AddExtension = true;
            //this.mSaveJazzScriptDialog.DefaultExt = GlobalManager.kJazzExt;
            this.mSaveJazzScriptDialog.Filter = kJazzScriptDialogFilter;
            this.mSaveJazzScriptDialog.OverwritePrompt = true;
            this.mSaveJazzScriptDialog.SupportMultiDottedExtensions = true;
            this.mSaveJazzScriptDialog.Title = "Export Jazz Script As";

            this.mSaveKeyNameMapDialog = new SaveFileDialog();
            this.mSaveKeyNameMapDialog.AddExtension = true;
            //this.mSaveKeyNameMapDialog.DefaultExt = KeyNameMap.NameMapExt;
            this.mSaveKeyNameMapDialog.Filter = kKeyNameMapDialogFilter;
            this.mSaveKeyNameMapDialog.OverwritePrompt = true;
            this.mSaveKeyNameMapDialog.SupportMultiDottedExtensions = true;
            this.mSaveKeyNameMapDialog.Title = "Export Key Name Map As";

            this.mSettingsDialog = new SettingsDialog(this);

            this.Text = kName;

            SplashLauncher.Launch();
        }

        public MainForm(params string[] args)
            : this()
        {
            if (args.Length > 0)
            {
                string path = Path.GetFullPath(args[0]);
                if (path.EndsWith(GlobalManager.kJazzExt, true, null))
                {
                    this.ImportJazzGraphScript(path);
                }
                else
                {
                    this.OpenJazzGraph(path);
                }
            }
        }

        public void InvalidateJazzGraphs()
        {
            foreach (JazzGraphContainer jgc in this.mOpenGraphs)
            {
                if (jgc.Scene.StateView != null)
                {
                    jgc.Scene.StateView.BackColor 
                        = StateMachineScene.BackColor;
                    jgc.Scene.StateView.Invalidate();
                }
            }
        }

        public void RefreshJazzGraphs(bool refreshDecisionGraphs)
        {
            int i, j, k;
            DGEdge dgEdge;
            DGNode dgNode;
            AnchorPoint ap;
            StateEdge stateEdge;
            StateNode stateNode;
            GraphForms.GraphElement[] children;
            GraphForms.Algorithms.Digraph<DGNode, DGEdge> decisionGraph;
            GraphForms.Algorithms.Digraph<StateNode, StateEdge> stateGraph;
            foreach (JazzGraphContainer jgc in this.mOpenGraphs)
            {
                if (refreshDecisionGraphs)
                {
                    jgc.Scene.StateMachine.RefreshHashNames();
                }
                stateGraph = jgc.Scene.StateGraph;
                for (i = stateGraph.NodeCount - 1; i >= 0; i--)
                {
                    stateNode = stateGraph.NodeAt(i);
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
                for (i = stateGraph.EdgeCount - 1; i >= 0; i--)
                {
                    stateEdge = stateGraph.EdgeAt(i);
                    stateEdge.Update();
                }
                if (jgc.Scene.StateView != null)
                {
                    jgc.Scene.StateView.BackColor 
                        = StateMachineScene.BackColor;
                    jgc.Scene.StateView.Invalidate();
                }
            }
        }

        #region File Menu Event Handlers

        private void new_Click(object sender, EventArgs e)
        {
            this.CreateNewJazzGraph(null);
        }

        private void open_Click(object sender, EventArgs e)
        {
            this.OpenJazzGraph(null);
        }

        private void load_Click(object sender, EventArgs e)
        {
            this.LoadJazzGraph();
        }

        private void close_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            this.CloseJazzGraph(index);
        }

        private void closeAll_Click(object sender, EventArgs e)
        {
            this.CloseAllJazzGraphs();
        }

        private void save_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            this.SaveJazzGraph(index, true);
        }

        private void saveAll_Click(object sender, EventArgs e)
        {
            this.SaveAllJazzGraphs(false);
        }

        private void packageInto_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            this.PackageJazzGraphInto(index, null);
        }

        private void packageAllInto_Click(object sender, EventArgs e)
        {
            this.PackageAllJazzGraphsInto();
        }

        private void importScript_Click(object sender, EventArgs e)
        {
            this.ImportJazzGraphScript(null);
        }

        private void importNameMap_Click(object sender, EventArgs e)
        {
            this.ImportKeyNameMap(null);
        }

        private void exportScript_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            this.ExportJazzGraphScript(index, null);
        }

        private void exportAllScripts_Click(object sender, EventArgs e)
        {
            this.ExportAllJazzGraphScripts();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !this.CloseAllJazzGraphs();
        }

        #region Jazz Graph File Manipulation

        private bool CanModifyJazzGraph(int index)
        {
            return false;
        }

        private void ToggleJazzGraphMenuStrips()
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            if (0 > index || index >= this.mOpenGraphs.Count)
            {
                return;
            }
            bool crw = this.CanModifyJazzGraph(index);
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            this.Text = kName + (jgc.JP == null ? "" : jgc.JP.Title);

            this.saveToolStripMenuItem.Enabled
                = jgc.SaveState != JazzSaveState.Saved;

            bool flag = jgc.Scene.FocusedState == null;
            this.stateGraphToolStripMenuItem.Enabled = flag;
            this.decisionGraphToolStripMenuItem.Enabled = !flag;

            this.addNewStateToolStripMenuItem.Enabled = crw;
            flag = jgc.Scene.SelectedStateNodeCount > 0;
            this.minimizeStatesToolStripMenuItem.Enabled = flag;
            this.removeStatesToolStripMenuItem.Enabled = crw && flag;
            this.addNewTransitionsToolStripMenuItem.Enabled = crw;
            this.removeTransitionsToolStripMenuItem.Enabled
                = crw && jgc.Scene.SelectedStateEdgeCount > 0;

            if (flag)
            {
                index = 0;
                StateNode sNode;
                StateNode[] sNodes = jgc.Scene.SelectedStateNodes;
                for (int i = sNodes.Length - 1; i >= 0; i--)
                {
                    sNode = sNodes[i];
                    if (sNode.Minimized)
                    {
                        index++;
                    }
                }
                if (index == 0)
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Unchecked;
                }
                else if (index < sNodes.Length)
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Indeterminate;
                }
                else
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Checked;
                }
            }
            else
            {
                this.minimizeStatesToolStripMenuItem.CheckState
                    = CheckState.Unchecked;
            }

            this.addNewDGNodeToolStripMenuItem.Enabled = crw;
            this.removeDGNodesToolStripMenuItem.Enabled
                = crw && jgc.Scene.SelectedDGNodeCount > 0;
            this.addNewDGLinksToolStripMenuItem.Enabled = crw;
            this.removeDGLinksToolStripMenuItem.Enabled
                = crw && jgc.Scene.SelectedDGEdgeCount > 0;
        }

        private void jazzGraphSelectedIndexChanged(object sender, EventArgs e)
        {
            JazzGraphContainer jgc;
            int index = this.jazzGraphTabControl.SelectedIndex;
            // Pause all hidden layouts and unpause the visible one
            for (int i = this.mOpenGraphs.Count - 1; i >= 0; i--)
            {
                jgc = this.mOpenGraphs[i];
                if (i == index)
                {
                    jgc.Scene.LayoutPaused = false;
                }
                else
                {
                    jgc.Scene.LayoutPaused = true;
                }
            }
            this.ToggleJazzGraphMenuStrips();
        }

        private bool CreateNewJazzGraph(StateMachine sm)
        {
            // Initialize the opened jazz graph visually
            BufferedPanel view = new BufferedPanel();
            TabPage graphTab = new TabPage();
            Size gSize = this.jazzGraphTabControl.Size;
            Size iSize = this.jazzGraphTabControl.ItemSize;
            gSize.Width = gSize.Width - 8;
            gSize.Height = gSize.Height - iSize.Height - 8;

            graphTab.AutoScroll = true;
            graphTab.Location
                = new Point(4, iSize.Height + 4);
            graphTab.Size = gSize;
            graphTab.UseVisualStyleBackColor = true;

            view.Size = gSize;

            // Initialize the opened jazz graph internally
            JazzGraphContainer jgc;
            if (sm == null)
            {
                JazzGraphNamePrompt jgnp = new JazzGraphNamePrompt();
                if (jgnp.ShowDialog() != DialogResult.OK)
                {
                    view.Dispose();
                    graphTab.Dispose();
                    jgnp.Dispose();
                    return false;
                }
                jgnp.Dispose();
                jgc = new JazzGraphContainer(
                    this.mOpenGraphs.Count, jgnp.Name, view);
            }
            else
            {
                jgc = new JazzGraphContainer(
                    this.mOpenGraphs.Count, sm, view);
            }
            jgc.Scene.LayoutStateGraphOnNodeMoved
                = this.layoutStateNodesToolStripMenuItem.Checked;
            jgc.Scene.LayoutDecisionGraphOnNodeMoved
                = this.layoutDGNodesToolStripMenuItem.Checked;
            // Add the opened jazz graph internally
            this.mOpenGraphs.Add(jgc);
            jgc.Scene.StateNodeSelectionChanged +=
                new EventHandler(Scene_StateNodeSelectionChanged);
            jgc.Scene.StateEdgeSelectionChanged +=
                new EventHandler(Scene_StateEdgeSelectionChanged);
            jgc.Scene.DGNodeSelectionChanged +=
                new EventHandler(Scene_DGNodeSelectionChanged);
            jgc.Scene.DGEdgeSelectionChanged +=
                new EventHandler(Scene_DGEdgeSelectionChanged);
            jgc.Scene.FocusedStateChanged +=
                new EventHandler(Scene_FocusedStateChanged);
            // Add the opened jazz graph visually
            graphTab.Controls.Add(view);
            graphTab.Text = jgc.Name + " *";
            graphTab.SizeChanged +=
                new EventHandler(jgc.OnTabSizeChanged);
            this.jazzGraphTabControl.TabPages.Add(graphTab);

            if (this.mOpenGraphs.Count > 1)
            {
                // Switch to the already open jazz graph
                this.jazzGraphTabControl.SelectedIndex = jgc.Index;
                // TODO: Will this invoke SelectedIndexChanged ?
            }
            else
            {
                jgc.Scene.LayoutPaused = false;
                this.ToggleJazzGraphMenuStrips();
            }
            this.closeToolStripMenuItem.Enabled = true;
            this.closeAllToolStripMenuItem.Enabled = true;
            this.packageIntoToolStripMenuItem.Enabled = true;
            this.packageAllIntoToolStripMenuItem.Enabled = true;
            this.exportScriptStripMenuItem.Enabled = true;
            this.exportAllScriptsToolStripMenuItem.Enabled = true;
            this.editToolStripMenuItem.Enabled = true;
            return true;
        }

        private JazzPackage OpenPackage(string path,
            bool readOnly, bool allowReadOnly)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return null;
            }
            int i;
            JazzPackage jp;
            // Check if the package is already open
            for (i = this.mOpenPackages.Count - 1; i >= 0; i--)
            {
                jp = this.mOpenPackages[i];
                if (path.Equals(jp.Path) && jp.ReadOnly == readOnly)
                {
                    return jp;
                }
            }
            s3pi.Filetable.PathPackageTuple ppt;
            try
            {
                ppt = new s3pi.Filetable.PathPackageTuple(path, !readOnly);
            }
            catch (InvalidDataException ide)
            {
                if (ide.Message.Contains("magic tag"))
                {
                    CopyableMessageBox.Show(
                        "Could not open Sims 3 package:\n" + 
                        (readOnly ? "[RO] " : "[RW] ") + path +
                        "\n\nThis file does not contain the expected " +
                        "package identifier string tag in the header." +
                        "\nThis could be because it is a protected " +
                        "package (e.g. a Store item), a Sims3Pack or " +
                        "some other random file.\n\n---\nError message:\n" +
                        ide.Message, kName + ": Unable to Open File",
                        CopyableMessageBoxButtons.OK,
                        CopyableMessageBoxIcon.Error, 0);
                }
                else if (ide.Message.Contains("major version"))
                {
                    CopyableMessageBox.Show(
                        "Could not open Sims 3 package:\n" + 
                        (readOnly ? "[RO] " : "[RW] ") + path + 
                        "\n\nThis file does not contain the expected " +
                        "package major version number in the header." +
                        "\nThis could be because it is a package for " +
                        "The Sims 2 or Spore.\n\n---\nError message:\n" +
                        ide.Message, kName + ": Unable to Open File",
                        CopyableMessageBoxButtons.OK,
                        CopyableMessageBoxIcon.Error, 0);
                }
                else
                {
                    ShowException(ide,
                        "Could not open Sims 3 package:\n" + path + "\n",
                        kName + ": Unable to Open File");
                }
                return null;
            }
            catch (UnauthorizedAccessException uae)
            {
                if (readOnly)
                {
                    ShowException(uae,
                        "Could not open Sims 3 package:\n[RO] " + path + 
                        "\n", kName + ": Unable to Open File");
                    return null;
                }
                else if (allowReadOnly && 0 == CopyableMessageBox.Show(
                    "Could not open Sims 3 package:\n[RW] " + path +
                    "\n\nThe file could be write-protected, in which " +
                    "case it might be possible to open it read-only." +
                    "\n\n---\nError message:\n" + uae.Message,
                    kName + ": Unable to Open File",
                    CopyableMessageBoxIcon.Error,
                    new string[] { "&Open read-only", "&Cancel" }, 1, 1))
                {
                    return OpenPackage(path, true, allowReadOnly);
                }
                else
                {
                    ShowException(uae,
                        "Could not open Sims 3 package:\n[RW] " + path + 
                        "\n", kName + ": Unable to Open File");
                    return null;
                }
            }
            catch (IOException ioe)
            {
                if (0 == CopyableMessageBox.Show(
                    "Could not open Sims 3 package:\n" + path +
                    "\n\nThere may be another process with exclusive " +
                    "access to the file (e.g. The Sims 3). After exiting " +
                    "the other process, you can retry opening the package." +
                    "\n\n---\nError message:\n" + ioe.Message,
                    kName + ": Unable to Open File",
                    CopyableMessageBoxIcon.Error,
                    new string[] { "&Retry", "&Cancel" }, 1, 1))
                {
                    return OpenPackage(path, readOnly, allowReadOnly);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ShowException(ex,
                    "Could not open Sims 3 package:\n" + 
                    (readOnly ? "[RO] " : "[RW] ") + path + "\n",
                    kName + ": Unable to Open File");
                return null;
            }
            // Check if the package even contains any jazz graphs
            List<IResourceIndexEntry> rieList
                = ppt.Package.FindAll(sIsJazz);
            if (rieList == null || rieList.Count == 0)
            {
                CopyableMessageBox.Show(
                    "Could not open Sims 3 package:\n" + 
                    (readOnly ? "[RO] " : "[RW] ") + path +
                    "\n\nIt does not contain any Jazz Graphs.",
                    kName + ": Unable to Open File",
                    CopyableMessageBoxButtons.OK,
                    CopyableMessageBoxIcon.Error, 0);
                Package.ClosePackage(0, ppt.Package);
                return null;
            }
            // Refresh FileTableExt.Current
            FileTableExt.Current.Add(ppt);
            GlobalManager.RefreshCurrent();
            // Return the opened jazz package
            jp = new JazzPackage(path, ppt.Package, readOnly);
            this.mOpenPackages.Add(jp);
            return jp;
        }

        private bool OpenJazzGraph(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                this.mOpenPackageDialog.FileName = "";
                this.mOpenPackageDialog.FilterIndex = 0;
                if (this.mOpenPackageDialog.ShowDialog() != DialogResult.OK)
                {
                    return false;
                }
                path = Path.GetFullPath(this.mOpenPackageDialog.FileName);
            }
            JazzPackage jp = this.OpenPackage(path,
                this.mOpenPackageDialog.ReadOnlyChecked, true);
            if (jp == null)
            {
                return false;
            }
            ResourceMgr.ResPackage pkg = null;
            ResourceMgr.ResPackage[] pkgs
                = GlobalManager.JazzManager.Current;
            if (pkgs == null || pkgs.Length == 0)
            {
                if (jp.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jp);
                }
                return false;
            }
            int i;
            for (i = pkgs.Length - 1; i >= 0; i--)
            {
                pkg = pkgs[i];
                if (path.Equals(pkg.Path))
                {
                    break;
                }
            }
            if (i < 0)
            {
                if (jp.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jp);
                }
                return false;
            }
            using (SelectResourceDialog srd
                = new SelectResourceDialog(GlobalManager.JazzManager))
            {
                srd.Text = "Select a Jazz Graph";
                srd.SelectedCategory = SelectResourceDialog.Category.Current;
                srd.SelectCategoryEnabled = false;
                srd.SelectedPackage = pkg;
                srd.SelectPackageEnabled = false;
                switch (srd.ShowDialog())
                {
                    case DialogResult.OK:
                        this.LoadJazzGraph(pkg, srd.SelectedResource);
                        return true;
                    case DialogResult.Cancel:
                        if (jp.Graphs.Count == 0)
                        {
                            this.CloseAndRemovePackage(jp);
                        }
                        return false;
                }
            }
            return true;
        }

        private bool LoadJazzGraph()
        {
            using (SelectResourceDialog srd
                = new SelectResourceDialog(GlobalManager.JazzManager))
            {
                srd.Text = "Select a Jazz Graph";
                if (srd.ShowDialog() == DialogResult.OK)
                {
                    this.LoadJazzGraph(
                        srd.SelectedPackage, srd.SelectedResource);
                    return true;
                }
            }
            return false;
        }

        private void LoadJazzGraph(
            ResourceMgr.ResPackage rPackage, ResourceMgr.ResEntry rEntry)
        {
            int i, j;
            string path = rPackage.Path;
            bool b = true;
            JazzPackage jp = null;
            JazzGraphContainer jgc = null;
            // Check if jazz package and jazz graph are already open
            for (i = this.mOpenPackages.Count - 1; i >= 0 && b; i--)
            {
                jp = this.mOpenPackages[i];
                if (string.Equals(path, jp.Path))
                {
                    for (j = jp.Graphs.Count - 1; j >= 0 && b; j--)
                    {
                        jgc = jp.Graphs[j];
                        if (jgc.RIE.Instance == rEntry.IID &&
                            jgc.RIE.ResourceGroup == rEntry.GID)
                        {
                            // Inform user that they are attempting
                            // open a jazz graph that is already open
                            MessageBox.Show(
                                "Could not open " + rEntry.Name + 
                                " in package:\n" + 
                                (jp.ReadOnly ? "[RO] " : "[RW] ") + path +
                                "\n\nIt is already open.",
                                kName + ": Unable to Open Jazz Graph",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            b = false;
                            j++;//counteract j-- done before b checked
                        }
                    }
                    if (j < 0)
                    {
                        jgc = null;
                    }
                    b = false;
                    i++;//counteract i-- done before b checked
                }
            }
            // Open the jazz package read-only if it is not already open
            if (i < 0)
            {
                s3pi.Filetable.PathPackageTuple ppt
                    = new s3pi.Filetable.PathPackageTuple(path, false);
                jp = new JazzPackage(path, ppt.Package, true);
                this.mOpenPackages.Add(jp);
                FileTableExt.Current.Add(ppt);
                GlobalManager.RefreshCurrent();
                this.RefreshJazzGraphs(true);
            }
            if (jgc == null)
            {
                // Open the jazz graph from the jazz package
                JazzRIEFinder finder
                    = new JazzRIEFinder(rEntry.GID, rEntry.IID);
                IResourceIndexEntry rie
                    = jp.Package.Find(finder.Match);
                if (rie != null)
                {
                    // Initialize the opened jazz graph visually
                    BufferedPanel view = new BufferedPanel();
                    TabPage graphTab = new TabPage();
                    Size gSize = this.jazzGraphTabControl.Size;
                    Size iSize = this.jazzGraphTabControl.ItemSize;
                    gSize.Width = gSize.Width - 8;
                    gSize.Height = gSize.Height - iSize.Height - 8;

                    graphTab.AutoScroll = true;
                    graphTab.Location
                        = new Point(4, iSize.Height + 4);
                    graphTab.Size = gSize;
                    graphTab.UseVisualStyleBackColor = true;

                    view.Size = gSize;

                    // Initialize the opened jazz graph internally
                    jgc = new JazzGraphContainer(
                        this.mOpenGraphs.Count, jp, rie, view);
                    if (jgc.Scene == null)
                    {
                        if (jp.Graphs.Count == 0)
                        {
                            this.CloseAndRemovePackage(jp);
                        }
                        view.Dispose();
                        graphTab.Dispose();
                        return;
                    }
                    jgc.Scene.LayoutStateGraphOnNodeMoved
                        = this.layoutStateNodesToolStripMenuItem.Checked;
                    jgc.Scene.LayoutDecisionGraphOnNodeMoved
                        = this.layoutDGNodesToolStripMenuItem.Checked;
                    // Add the opened jazz graph internally
                    jp.Graphs.Add(jgc);
                    this.mOpenGraphs.Add(jgc);
                    jgc.Scene.StateNodeSelectionChanged +=
                        new EventHandler(Scene_StateNodeSelectionChanged);
                    jgc.Scene.StateEdgeSelectionChanged +=
                        new EventHandler(Scene_StateEdgeSelectionChanged);
                    jgc.Scene.DGNodeSelectionChanged +=
                        new EventHandler(Scene_DGNodeSelectionChanged);
                    jgc.Scene.DGEdgeSelectionChanged +=
                        new EventHandler(Scene_DGEdgeSelectionChanged);
                    jgc.Scene.FocusedStateChanged +=
                        new EventHandler(Scene_FocusedStateChanged);
                    // Add the opened jazz graph visually
                    graphTab.Controls.Add(view);
                    graphTab.Text = jgc.Name;
                    graphTab.SizeChanged += 
                        new EventHandler(jgc.OnTabSizeChanged);
                    this.jazzGraphTabControl.TabPages.Add(graphTab);
                }
            }
            if (this.mOpenGraphs.Count > 1)
            {
                // Switch to the already open jazz graph
                this.jazzGraphTabControl.SelectedIndex = jgc.Index;
                // TODO: Will this invoke SelectedIndexChanged ?
            }
            else
            {
                jgc.Scene.LayoutPaused = false;
                this.ToggleJazzGraphMenuStrips();
            }
            this.closeToolStripMenuItem.Enabled = true;
            this.closeAllToolStripMenuItem.Enabled = true;
            this.packageIntoToolStripMenuItem.Enabled = true;
            this.packageAllIntoToolStripMenuItem.Enabled = true;
            this.exportScriptStripMenuItem.Enabled = true;
            this.exportAllScriptsToolStripMenuItem.Enabled = true;

            this.editToolStripMenuItem.Enabled = true;
        }

        private bool ImportKeyNameMap(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                this.mOpenKeyNameMapDialog.FileName = "";
                this.mOpenKeyNameMapDialog.FilterIndex = 0;
                if (this.mOpenKeyNameMapDialog.ShowDialog() !=
                    DialogResult.OK)
                {
                    return false;
                }
                path = Path.GetFullPath(this.mOpenKeyNameMapDialog.FileName);
            }
            if (!File.Exists(path))
            {
                return false;
            }
            FileStream fs;
            NameMapResource.NameMapResource knm;
            try
            {
                fs = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite);
                knm = new NameMapResource.NameMapResource(0, fs);
            }
            catch (Exception ex)
            {
                ShowException(ex, "",
                    kName + ": Unable to Import Key Name Map File");
                return false;
            }
            KeyNameMap.ImportKeyNameMap(path, knm);
            this.RefreshJazzGraphs(true);
            return true;
        }

        private bool ImportJazzGraphScript(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                this.mOpenJazzScriptDialog.FileName = "";
                this.mOpenJazzScriptDialog.FilterIndex = 0;
                if (this.mOpenJazzScriptDialog.ShowDialog() != 
                    DialogResult.OK)
                {
                    return false;
                }
                path = Path.GetFullPath(this.mOpenJazzScriptDialog.FileName);
            }
            if (!File.Exists(path))
            {
                return false;
            }
            FileStream fs;
            GenericRCOLResource rcol = null;
            try
            {
                fs = new FileStream(path, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite);
                rcol = new GenericRCOLResource(0, fs);
            }
            catch (Exception ex)
            {
                ShowException(ex, "",
                    kName + ": Unable to Import Jazz Script File");
                return false;
            }
            StateMachine sm = new StateMachine(rcol);
            if (sm.NameIsHash)
            {
                path = Path.GetFileName(path);
                string name = path;
                int i = name.ToLower().IndexOf("s3_");
                if (i >= 0) name = name.Substring(i + 3);
                i = name.IndexOf("%%+");
                if (i >= 0) name = name.Substring(0, i);
                string[] parts = name.Split(
                    new char[] { '_', '-' }, 4);
                if (parts.Length == 4)
                {
                    name = UnescapePathString(parts[3]);
                }
                else
                {
                    name = Path.GetFileNameWithoutExtension(path);
                }
                sm.Name = name;
            }
            return this.CreateNewJazzGraph(sm);
        }

        private int OverwritePackagePrompt(string path)
        {
            int i;
            ResourceMgr.ResPackage[] packages;
            packages = GlobalManager.JazzManager.CustomContent;
            if (packages != null && packages.Length > 0)
            {
                for (i = packages.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(path, packages[i].Path))
                    {
                        return CopyableMessageBox.Show(
                            "You are about to overwrite the following " +
                            "Custom Content Sims 3 Package:\n" + path + 
                            "\n\nIf you intend to override existing " +
                            "behavior, it is better to create a new " +
                            "package. Would you like to pick a new " +
                            "location to save to now?", 
                            kName + ": Save Warning",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0);
                    }
                }
            }
            packages = GlobalManager.JazzManager.GameCore;
            if (packages != null && packages.Length > 0)
            {
                for (i = packages.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(path, packages[i].Path))
                    {
                        return CopyableMessageBox.Show(
                            "You are about to overwrite the following " +
                            "Game Core Sims 3 Package:\n" + path +
                            "\n\nIf you intend to override existing " +
                            "behavior, it is better to create a new " +
                            "package. Would you like to pick a new " +
                            "location to save to now?",
                            kName + ": Save Warning",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0);
                    }
                }
            }
            packages = GlobalManager.JazzManager.GameContent;
            if (packages != null && packages.Length > 0)
            {
                for (i = packages.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(path, packages[i].Path))
                    {
                        return CopyableMessageBox.Show(
                            "You are about to overwrite the following " +
                            "Game Content Sims 3 Package:\n" + path +
                            "\n\nIf you intend to override existing " +
                            "behavior, it is better to create a new " +
                            "package. Would you like to pick a new " +
                            "location to save to now?",
                            kName + ": Save Warning",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0);
                    }
                }
            }
            packages = GlobalManager.JazzManager.DDSImages;
            if (packages != null && packages.Length > 0)
            {
                for (i = packages.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(path, packages[i].Path))
                    {
                        return CopyableMessageBox.Show(
                            "You are about to overwrite the following " +
                            "DDS Image Sims 3 Package:\n" + path +
                            "\n\nIf you intend to override existing " +
                            "behavior, it is better to create a new " +
                            "package. Would you like to pick a new " +
                            "location to save to now?",
                            kName + ": Save Warning",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0);
                    }
                }
            }
            packages = GlobalManager.JazzManager.Thumbnails;
            if (packages != null && packages.Length > 0)
            {
                for (i = packages.Length - 1; i >= 0; i--)
                {
                    if (string.Equals(path, packages[i].Path))
                    {
                        return CopyableMessageBox.Show(
                            "You are about to overwrite the following " +
                            "Thumbnail Sims 3 Package:\n" + path +
                            "\n\nIf you intend to override existing " +
                            "behavior, it is better to create a new " +
                            "package. Would you like to pick a new " +
                            "location to save to now?",
                            kName + ": Save Warning",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0);
                    }
                }
            }
            return 1;
        }

        private static string[] sPackageScriptCancel
            = new string[] { "&Package", "&Script", "&Cancel" };

        private JazzPackage OpenPackageForSaving(string fileName)
        {
            if (fileName == null)
            {
                this.mSavePackageDialog.FileName = "";
            }
            else
            {
                this.mSavePackageDialog.FileName = fileName;
            }
            this.mSavePackageDialog.FilterIndex = 0;
            if (this.mSavePackageDialog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            int i;
            JazzPackage jp = null;
            string path = Path.GetFullPath(this.mSavePackageDialog.FileName);
            if (path.EndsWith(GlobalManager.kJazzExt, true, null))
            {
                switch (CopyableMessageBox.Show("Cannot save jazz " +
                    "graphs to the Sims 3 package: \n" + path +
                    "\n\nThe *" + GlobalManager.kJazzExt +
                    "extension cannot be used when saving jazz graphs " + 
                    "into a Sims 3 package. Would you like to try saving " + 
                    "it into a different Sims 3 package?",
                    kName + ": Unable to Save into Package",
                    CopyableMessageBoxButtons.OKCancel,
                    CopyableMessageBoxIcon.Error, 0))
                {
                    case 0:// OK
                        int ln = GlobalManager.kJazzExt.Length;
                        path = string.Concat(
                            path.Substring(0, path.Length - ln), 
                            kPackageExt);
                        return this.OpenPackageForSaving(path);
                    case 1:// Cancel
                        return null;
                }
            }
            if (File.Exists(path))
            {
                // Check if the package is already open
                for (i = this.mOpenPackages.Count - 1; i >= 0; i--)
                {
                    jp = this.mOpenPackages[i];
                    if (!jp.ReadOnly && path.Equals(jp.Path))
                    {
                        break;
                    }
                }
                if (i < 0)
                {
                    // Check if the package is part of the FileTable
                    switch (this.OverwritePackagePrompt(path))
                    {
                        case 0:// Yes
                            return this.OpenPackageForSaving(null);
                        case 1:// No
                            break;
                        case 2:// Cancel
                            return null;
                    }
                    // Try to open the package with writing capability
                    jp = this.OpenPackage(path, false, false);
                    if (jp == null)
                    {
                        return null;
                    }
                }
            }
            else
            {
                jp = new JazzPackage(path, Package.NewPackage(0), false);
                this.mOpenPackages.Add(jp);
            }
            return jp;
        }

        private void CloseAndRemovePackage(JazzPackage jp)
        {
            if (File.Exists(jp.Path))
            {
                for (int i = this.mOpenPackages.Count - 1; i >= 0; i--)
                {
                    if (jp.Equals(this.mOpenPackages[i]))
                    {
                        this.mOpenPackages.RemoveAt(i);
                        FileTableExt.Current.RemoveAt(i);
                        GlobalManager.RefreshCurrent();
                        Package.ClosePackage(0, jp.Package);
                        break;
                    }
                }
            }
            else
            {
                this.mOpenPackages.Remove(jp);
            }
        }

        private static readonly KeyNameReg.KNM sGameKNMs 
            = KeyNameReg.KNM.GameCore | KeyNameReg.KNM.GameContent 
            | KeyNameReg.KNM.DDSImages | KeyNameReg.KNM.Thumbnails;

        private bool PackageJazzGraphInto(int index, string fileName)
        {
            if (fileName == null)
            {
                this.mSavePackageDialog.FileName = "";
            }
            else
            {
                this.mSavePackageDialog.FileName = fileName;
            }
            this.mSavePackageDialog.FilterIndex = 0;
            if (this.mSavePackageDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            string path = Path.GetFullPath(this.mSavePackageDialog.FileName);
            if (path.EndsWith(GlobalManager.kJazzExt, true, null))
            {
                switch (CopyableMessageBox.Show(
                    "Cannot save " + jgc.Name +
                    " to the Sims 3 package:\n" + path +
                    "\n\nThe *" + GlobalManager.kJazzExt +
                    "extension cannot be used when saving a jazz graph " +
                    "into a Sims 3 package. Would you like to try saving " + 
                    "it into a Sims 3 package again or export it as the " +
                    "above jazz script file instead?", 
                    kName + ": Unable to Save into Package",
                    CopyableMessageBoxIcon.Error,
                    sPackageScriptCancel, 0, 2))
                {
                    case 0:// Package
                        int ln = GlobalManager.kJazzExt.Length;
                        path = string.Concat(
                            path.Substring(0, path.Length - ln), 
                            kPackageExt);
                        return this.PackageJazzGraphInto(index, path);
                    case 1:// Script
                        return this.ExportJazzGraphScript(index, path);
                    case 2:// Cancel
                        return false;
                }
            }
            int i;
            TabPage page;
            JazzPackage jp = null;
            IResourceIndexEntry rie = null;
            if (jgc.JP != null && !jgc.JP.ReadOnly && 
                path.Equals(jgc.JP.Path))
            {
                if (jgc.SaveState != JazzSaveState.Saved)
                {
                    jp = jgc.JP;
                    if (jgc.NameIsValid() && 
                        !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                    {
                        jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                    }
                    StateMachine sm2 = jgc.Scene.StateMachine;
                    KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
                    GenericRCOLResource rcol2 = sm2.ExportToResource(
                        jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                    KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
                    jp.Package.ReplaceResource(rie, rcol2);
                    jp.Package.SavePackage();
                    jgc.SaveState = JazzSaveState.Saved;
                }
                return true;
            }
            if (File.Exists(path))
            {
                // Check if the package is already open
                for (i = this.mOpenPackages.Count - 1; i >= 0; i--)
                {
                    jp = this.mOpenPackages[i];
                    if (!jp.ReadOnly && path.Equals(jp.Path))
                    {
                        break;
                    }
                }
                if (i < 0)
                {
                    // Check if the package is part of the FileTable
                    switch (this.OverwritePackagePrompt(path))
                    {
                        case 0:// Yes
                            return this.PackageJazzGraphInto(index, null);
                        case 1:// No
                            break;
                        case 2:// Cancel
                            return false;
                    }
                    // Try to open the package with writing capability
                    jp = this.OpenPackage(path, false, false);
                    if (jp == null)
                    {
                        return false;
                    }
                }
                // Prompt the user if there is a resource collision
                bool flag = true;
                JazzGraphNamePrompt jgnp 
                    = new JazzGraphNamePrompt(jgc.Name, jgc.Name);
                JazzRIEFinder rieFinder = new JazzRIEFinder(0, 0);
                page = this.jazzGraphTabControl.TabPages[index];
                while (flag)
                {
                    rieFinder.GID = jgc.Key.GID;
                    rieFinder.IID = jgc.Key.IID;
                    rie = jp.Package.Find(rieFinder.Match);
                    if (rie == null)
                    {
                        flag = false;
                    }
                    else
                    {
                        switch (CopyableMessageBox.Show("A jazz " + 
                            "graph with the name (resource key):\n" +
                            jgc.Name + " (" + jgc.Key.ToString() + 
                            ")\nAlready exists in the Sims 3 package\n:" +
                            path +
                            "\n\nThis jazz graph will be overwritten " +
                            "by the one you are about to save into the " +
                            "Sims 3 package. Do you want to give it a " +
                            "different name before saving it into that " + 
                            "Sims 3 package?", 
                            kName + ": Overwrite Jazz Graph?",
                            CopyableMessageBoxButtons.YesNoCancel,
                            CopyableMessageBoxIcon.Warning, 0))
                        {
                            case 0:// Yes
                                jgnp.BannedNames.Clear();
                                jgnp.BannedNames.Add(jgc.Name);
                                if (jgnp.ShowDialog() != DialogResult.OK)
                                {
                                    if (jp.Graphs.Count == 0)
                                    {
                                        this.CloseAndRemovePackage(jp);
                                    }
                                    jgnp.Dispose();
                                    return false;
                                }
                                jgc.Key = jgnp.Key;
                                jgc.Name = jgnp.Name;
                                jgc.SaveState = JazzSaveState.Dirty;
                                page.Text = jgc.Name + " *";
                                break;
                            case 1:// No
                                flag = false;
                                break;
                            case 2:// Cancel
                                if (jp.Graphs.Count == 0)
                                {
                                    this.CloseAndRemovePackage(jp);
                                }
                                jgnp.Dispose();
                                return false;
                        }
                    }
                }
                if (rie == null)
                {
                    rie = jp.Package.AddResource(jgc.Key, null, true);
                }
                jgnp.Dispose();
            }
            else
            {
                jp = new JazzPackage(path, Package.NewPackage(0), false);
                this.mOpenPackages.Add(jp);
                rie = jp.Package.AddResource(jgc.Key, null, true);
            }
            if (jgc.JP != null)
            {
                jgc.JP.RemoveJazzGraph(jgc);
                if (jgc.JP.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jgc.JP);
                }
            }
            jgc.JP = jp;
            jgc.RIE = rie;
            jp.Graphs.Add(jgc);
            if (jgc.NameIsValid() && !jp.KNMapRes.ContainsKey(jgc.Key.IID))
            {
                jp.KNMapRes[jgc.Key.IID] = jgc.Name;
            }
            StateMachine sm = jgc.Scene.StateMachine;
            KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
            GenericRCOLResource rcol = sm.ExportToResource(
                jgc.Key.IID, jp.KNMapRes, sExportAllNames);
            KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
            jp.Package.ReplaceResource(rie, rcol);
            jp.Package.ReplaceResource(jp.KNMapRIE, jp.KNMapRes);
            if (File.Exists(path))
            {
                jp.Package.SavePackage();
            }
            else
            {
                jp.Package.SaveAs(path);
                Package.ClosePackage(0, jp.Package);
                s3pi.Filetable.PathPackageTuple ppt
                    = new s3pi.Filetable.PathPackageTuple(path, true);
                jp.Package = ppt.Package;
                FileTableExt.Current.Add(ppt);
                GlobalManager.RefreshCurrent();
                // TODO: Does the RIE of every JazzGraphContainer
                // in jp.Graphs have to be reset after jp.Package
                // is reopened?
            }
            jgc.SaveState = JazzSaveState.Saved;
            if (index == this.jazzGraphTabControl.SelectedIndex)
            {
                this.Text = kName + jp.Title;
                this.saveToolStripMenuItem.Enabled = false;
            }
            page = this.jazzGraphTabControl.TabPages[index];
            page.Text = jgc.Name;//Remove asterisk
            bool saveAll = false;
            for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
            {
                jgc = this.mOpenGraphs[i];
                if (jgc.SaveState != JazzSaveState.Saved)
                {
                    saveAll = true;
                    break;
                }
            }
            this.saveAllToolStripMenuItem.Enabled = saveAll;
            return true;
        }

        private bool PackageAllJazzGraphsInto()
        {
            JazzPackage jp = this.OpenPackageForSaving(null);
            if (jp == null)
            {
                return false;
            }
            int i;
            // Prompt user for which unsaved jazz graphs to save,
            // and write the desired jazz graphs to the package
            INamedResourceIndexEntry[] toSave
                = new INamedResourceIndexEntry[this.mOpenGraphs.Count];
            for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
            {
                toSave[i] = this.mOpenGraphs[i];
            }
            using (SelectEditRIEDialog rieDialog
                = new SelectEditRIEDialog(toSave))
            {
                rieDialog.Text = kName
                    + ": Save Jazz Graphs To Sims 3 Package";
                rieDialog.Message = "Select the jazz graphs to save " +
                    "into the following Sims 3 package:\n" + jp.Path;
                rieDialog.AutomaticHashing
                    = SelectEditRIEDialog.Hashing.FNV64;
                rieDialog.DetectConflicts = true;
                rieDialog.EnablePasteResourceKeys = false;
                rieDialog.ReadonlyTID = true;
                rieDialog.ReadonlyGID = true;
                rieDialog.ReadonlyIID = true;
                rieDialog.ReadonlyCompressed = true;
                if (rieDialog.ShowDialog() == DialogResult.OK)
                {
                    toSave = rieDialog.SelectedResources;
                }
                else
                {
                    if (jp.Graphs.Count == 0)
                    {
                        this.CloseAndRemovePackage(jp);
                    }
                    return false;
                }
            }
            if (toSave == null || toSave.Length == 0)
            {
                if (jp.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jp);
                }
                return true;
            }
            JazzGraphContainer jgc;
            IResourceIndexEntry rie;
            string overwrites = "";
            JazzRIEFinder rieFinder = new JazzRIEFinder(0, 0);
            for (i = 0; i < toSave.Length; i++)
            {
                jgc = toSave[i] as JazzGraphContainer;
                if (!jp.Equals(jgc.JP))
                {
                    rieFinder.GID = jgc.Key.GID;
                    rieFinder.IID = jgc.Key.IID;
                    rie = jp.Package.Find(rieFinder.Match);
                    if (rie != null)
                    {
                        overwrites += jgc.Name + " (" + jgc.Key + ")\n";
                    }
                }
            }
            if (overwrites.Length > 0)
            {
                switch (CopyableMessageBox.Show("The following jazz " +
                    "jazz graphs already exist in the given Sims 3 " +
                    "package:\n" + jp.Path + "\n\n" + overwrites +
                    "\nDo you still want to save it into that Sims 3 " +
                    "package?", kName + ": Overwrite Jazz Graphs?",
                    CopyableMessageBoxButtons.YesNo,
                    CopyableMessageBoxIcon.Warning, 0))
                {
                    case 0:// Yes
                        break;
                    case 1:// No
                        if (jp.Graphs.Count == 0)
                        {
                            this.CloseAndRemovePackage(jp);
                        }
                        return false;
                }
            }
            bool update = false;
            TabPage page;
            StateMachine sm;
            GenericRCOLResource rcol;
            TabControl.TabPageCollection pages
                = this.jazzGraphTabControl.TabPages;
            KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
            for (i = toSave.Length - 1; i >= 0; i--)
            {
                jgc = toSave[i] as JazzGraphContainer;
                if (this.jazzGraphTabControl.SelectedIndex == jgc.Index)
                {
                    update = true;
                }
                if (jgc.JP != null)
                {
                    jgc.JP.RemoveJazzGraph(jgc);
                    if (jp.Path.Equals(jgc.JP.Path))
                    {
                        jgc.JP = jp;
                        if (jgc.NameIsValid() && 
                            !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                        {
                            jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                        }
                        sm = jgc.Scene.StateMachine;
                        rcol = sm.ExportToResource(
                            jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                        jp.Package.ReplaceResource(jgc.RIE, rcol);
                    }
                    else if (jgc.JP.Graphs.Count == 0)
                    {
                        this.CloseAndRemovePackage(jgc.JP);
                    }
                }
                else
                {
                    jgc.JP = jp;
                    if (jgc.NameIsValid() && 
                        !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                    {
                        jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                    }
                    sm = jgc.Scene.StateMachine;
                    rcol = sm.ExportToResource(
                        jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                    jp.Package.AddResource(jgc.Key, rcol.Stream, true);
                }
                jp.Graphs.Add(jgc);
                if (jgc.SaveState != JazzSaveState.Saved)
                {
                    page = pages[jgc.Index];
                    page.Text = jgc.Name;//Remove asterisk
                }
                jgc.SaveState = JazzSaveState.Saved;
            }
            KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
            jp.Package.ReplaceResource(jp.KNMapRIE, jp.KNMapRes);
            if (File.Exists(jp.Path))
            {
                jp.Package.SavePackage();
            }
            else
            {
                jp.Package.SaveAs(jp.Path);
                Package.ClosePackage(0, jp.Package);
                s3pi.Filetable.PathPackageTuple ppt
                    = new s3pi.Filetable.PathPackageTuple(jp.Path, true);
                jp.Package = ppt.Package;
                FileTableExt.Current.Add(ppt);
                GlobalManager.RefreshCurrent();
                // TODO: Does the RIE of every JazzGraphContainer
                // in jp.Graphs have to be reset after jp.Package
                // is reopened?
            }
            if (update)
            {
                this.Text = kName + jp.Title;
            }
            i = this.jazzGraphTabControl.SelectedIndex;
            jgc = this.mOpenGraphs[i];
            update = jgc.SaveState != JazzSaveState.Saved;
            this.saveToolStripMenuItem.Enabled = update;
            if (!update)
            {
                for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
                {
                    jgc = this.mOpenGraphs[i];
                    if (jgc.SaveState != JazzSaveState.Saved)
                    {
                        update = true;
                        break;
                    }
                }
            }
            this.saveAllToolStripMenuItem.Enabled = update;
            return true;
        }

        private bool ExportJazzGraphScript(int index, string fileName)
        {
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (fileName == null)
            {
                string dir = Path.GetDirectoryName(
                    this.mSaveJazzScriptDialog.FileName);
                this.mSaveJazzScriptDialog.FileName = Path.Combine(dir, 
                    CreateJazzScriptFileName(jgc.Key, jgc.Name));
            }
            else
            {
                this.mSaveJazzScriptDialog.FileName = fileName;
            }
            this.mSaveJazzScriptDialog.FilterIndex = 0;
            if (this.mSaveJazzScriptDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            string path = Path.GetFullPath(
                this.mSaveJazzScriptDialog.FileName);
            if (path.EndsWith(kPackageExt, true, null))
            {
                switch (CopyableMessageBox.Show(
                    "Cannot export " + jgc.Name +
                    " as a jazz script file: \n" + path + "\n\nThe *" + 
                    kPackageExt + " extension cannot be used when " +
                    "exporting jazz graphs as scripts. Would you " +
                    "like to try exporting it as a jazz script again " + 
                    "or write it into the above Sims 3 package file " + 
                    "instead?", kName + ": Unable to Export as Script",
                    CopyableMessageBoxIcon.Error,
                    sPackageScriptCancel, 1, 2))
                {
                    case 0:// Package
                        return this.PackageJazzGraphInto(index, path);
                    case 1:// Script
                        int ln = kPackageExt.Length;
                        path = string.Concat(
                            path.Substring(0, path.Length - ln), 
                            GlobalManager.kJazzExt);
                        return this.ExportJazzGraphScript(index, path);
                    case 2:// Cancel
                        return false;
                }
            }
            NameMapResource.NameMapResource knm
                = new NameMapResource.NameMapResource(0, null);
            if (jgc.NameIsValid())
            {
                knm[jgc.Key.IID] = jgc.Name;
            }
            StateMachine sm = jgc.Scene.StateMachine;
            KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
            GenericRCOLResource rcol = sm.ExportToResource(
                jgc.Key.IID, knm, sExportAllNames);
            KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;

            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
            }
            catch (Exception ex)
            {
                ShowException(ex, "", 
                    kName + ": Unable to Export Jazz Script File");
                return false;
            }
            Stream s = rcol.Stream;
            s.Position = 0;
            BinaryReader r = new BinaryReader(s);
            BinaryWriter w = new BinaryWriter(fs);
            w.Write(r.ReadBytes((int)s.Length));
            w.Close();

            if (0 == CopyableMessageBox.Show("Would you also like to " + 
                "export a key name map file containing the names " + 
                "referenced within the exported jazz script?",
                kName + ": Export Key Name Map?", 
                CopyableMessageBoxButtons.YesNo, 
                CopyableMessageBoxIcon.Question, 0))
            {
                string dir = Path.GetDirectoryName(
                    this.mSaveKeyNameMapDialog.FileName);
                this.mSaveKeyNameMapDialog.FileName = Path.Combine(dir, 
                    CreateKeyNameMapFileName(jgc.Key, jgc.Name));
                this.mSaveKeyNameMapDialog.FilterIndex = 0;
                if (this.mSaveKeyNameMapDialog.ShowDialog() == 
                    DialogResult.OK)
                {
                    fs = null;
                    path = Path.GetFullPath(
                        this.mSaveKeyNameMapDialog.FileName);
                    try
                    {
                        fs = new FileStream(path, FileMode.Create);
                    }
                    catch (Exception ex2)
                    {
                        ShowException(ex2, "",
                            kName + ": Unable to Export Key Name Map");
                    }
                    if (fs != null)
                    {
                        s = knm.Stream;
                        s.Position = 0;
                        r = new BinaryReader(s);
                        w = new BinaryWriter(fs);
                        w.Write(r.ReadBytes((int)s.Length));
                        w.Close();
                    }
                }
            }
            return true;
        }

        private bool ExportAllJazzGraphScripts()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select a Folder to Export Jazz Scripts into";
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() != DialogResult.OK)
            {
                fbd.Dispose();
                return false;
            }
            int i;
            string dir = fbd.SelectedPath;
            fbd.Dispose();
            INamedResourceIndexEntry[] toExport
                = new INamedResourceIndexEntry[this.mOpenGraphs.Count];
            for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
            {
                toExport[i] = this.mOpenGraphs[i];
            }
            using (SelectEditRIEDialog rieDialog
                = new SelectEditRIEDialog(toExport))
            {
                rieDialog.Text = kName
                    + ": Export Jazz Graphs to Script Files";
                rieDialog.Message 
                    = "Select the jazz graphs to export as individual *"
                    + GlobalManager.kJazzExt + " script files.";
                rieDialog.AutomaticHashing
                    = SelectEditRIEDialog.Hashing.FNV64;
                rieDialog.DetectConflicts = true;
                rieDialog.EnablePasteResourceKeys = false;
                rieDialog.ReadonlyTID = true;
                rieDialog.ReadonlyGID = true;
                rieDialog.ReadonlyIID = true;
                rieDialog.ReadonlyCompressed = true;
                if (rieDialog.ShowDialog() == DialogResult.OK)
                {
                    toExport = rieDialog.SelectedResources;
                }
                else
                {
                    return false;
                }
            }
            if (toExport == null || toExport.Length == 0)
            {
                return true;
            }
            string path;
            Stream s;
            FileStream fs;
            BinaryReader r;
            BinaryWriter w;
            JazzGraphContainer jgc;
            StateMachine sm;
            GenericRCOLResource rcol;
            NameMapResource.NameMapResource knm
                = new NameMapResource.NameMapResource(0, null);
            KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
            for (i = toExport.Length - 1; i >= 0; i--)
            {
                jgc = toExport[i] as JazzGraphContainer;
                if (jgc.NameIsValid())
                {
                    knm[jgc.Key.IID] = jgc.Name;
                }
                sm = jgc.Scene.StateMachine;
                rcol = sm.ExportToResource(
                    jgc.Key.IID, knm, sExportAllNames);
                path = Path.Combine(dir, 
                    CreateJazzScriptFileName(jgc.Key, jgc.Name));

                fs = null;
                try
                {
                    fs = new FileStream(path, FileMode.Create);
                }
                catch (Exception ex)
                {
                    ShowException(ex, "Could not export the jazz graph: " + 
                        jgc.Name + "\nAs the jazz script file: \n" + path,
                        kName + ": Unable to Export Jazz Script");
                }
                if (fs != null)
                {
                    s = rcol.Stream;
                    s.Position = 0;
                    r = new BinaryReader(s);
                    w = new BinaryWriter(fs);
                    w.Write(r.ReadBytes((int)s.Length));
                    w.Close();
                }
            }
            KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
            if (0 == CopyableMessageBox.Show("Would you also like to " +
                "export a key name map file containing the names " +
                "referenced within the exported jazz scripts?",
                kName + ": Export Key Name Map?",
                CopyableMessageBoxButtons.YesNo,
                CopyableMessageBoxIcon.Question, 0))
            {
                this.mSaveKeyNameMapDialog.FileName = Path.Combine(dir,
                    CreateKeyNameMapFileName(
                    new RK(KeyNameMap.NameMapTID, 0, 0), null));
                this.mSaveKeyNameMapDialog.FilterIndex = 0;
                if (this.mSaveKeyNameMapDialog.ShowDialog() ==
                    DialogResult.OK)
                {
                    fs = null;
                    path = Path.GetFullPath(
                        this.mSaveKeyNameMapDialog.FileName);
                    try
                    {
                        fs = new FileStream(path, FileMode.Create);
                    }
                    catch (Exception ex2)
                    {
                        ShowException(ex2, "",
                            kName + ": Unable to Export Key Name Map");
                    }
                    if (fs != null)
                    {
                        s = knm.Stream;
                        s.Position = 0;
                        r = new BinaryReader(s);
                        w = new BinaryWriter(fs);
                        w.Write(r.ReadBytes((int)s.Length));
                        w.Close();
                    }
                }
            }
            return true;
        }

        private bool SaveJazzGraphAs(int index)
        {
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            switch (CopyableMessageBox.Show("Would you like to save " +
                jgc.Name + " into a Sims 3 package file or export it " +
                "as a jazz script file?",
                kName + ": Save Jazz Graph as Type",
                CopyableMessageBoxIcon.Question, sPackageScriptCancel, 0, 2))
            {
                case 0:// Package
                    return this.PackageJazzGraphInto(index, null);
                case 1:// Script
                    return this.ExportJazzGraphScript(index, null);
                case 2:// Cancel
                    return false;
            }
            return false;
        }

        private bool SaveJazzGraph(int index, bool updateSaveAll)
        {
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.JP == null)
            {
                return this.PackageJazzGraphInto(index, null);
            }
            else if (jgc.JP.ReadOnly)
            {
                return this.PackageJazzGraphInto(index, null);
            }
            else
            {
                JazzPackage jp = jgc.JP;
                if (jgc.NameIsValid() && 
                    !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                {
                    jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                }
                StateMachine sm = jgc.Scene.StateMachine;
                KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
                GenericRCOLResource rcol = sm.ExportToResource(
                    jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
                jp.Package.ReplaceResource(jgc.RIE, rcol);
                jp.Package.ReplaceResource(jp.KNMapRIE, jp.KNMapRes);
                jp.Package.SavePackage();
                jgc.SaveState = JazzSaveState.Saved;
                if (index == this.jazzGraphTabControl.SelectedIndex)
                {
                    this.Text = kName + jgc.JP.Title;
                    this.saveToolStripMenuItem.Enabled = false;
                }
                TabPage page = this.jazzGraphTabControl.TabPages[index];
                page.Text = jgc.Name;//Remove asterisk
                if (updateSaveAll)
                {
                    bool saveAll = false;
                    for (int i = this.mOpenGraphs.Count - 1; i >= 0; i--)
                    {
                        jgc = this.mOpenGraphs[i];
                        if (jgc.SaveState != JazzSaveState.Saved)
                        {
                            saveAll = true;
                            break;
                        }
                    }
                    this.saveAllToolStripMenuItem.Enabled = saveAll;
                }
                return true;
            }
        }

        private bool SaveAllJazzGraphs(bool closing)
        {
            int i;
            JazzGraphContainer jgc;
            List<INamedResourceIndexEntry> unsaved
                = new List<INamedResourceIndexEntry>(this.mOpenGraphs.Count);
            for (i = 0; i < this.mOpenGraphs.Count; i++)
            {
                jgc = this.mOpenGraphs[i];
                if (jgc.SaveState != JazzSaveState.Saved)
                {
                    if (jgc.JP != null && !jgc.JP.ReadOnly)
                    {
                        this.SaveJazzGraph(i, false);
                    }
                    else
                    {
                        unsaved.Add(jgc);
                    }
                }
            }
            if (unsaved.Count == 0)
            {
                this.saveToolStripMenuItem.Enabled = false;
                this.saveAllToolStripMenuItem.Enabled = false;
                return true;
            }
            switch (CopyableMessageBox.Show("There are jazz graphs " +
                "with unsaved changes that were newly created or " +
                "opened read-only. Would you like to save one or " +
                "more of these jazz graphs into a single Sims 3" +
                "package" + (closing ? " before closing them" : "") + 
                "?", kName + ": Unsaved Jazz Graphs",
                closing ? CopyableMessageBoxButtons.YesNoCancel
                        : CopyableMessageBoxButtons.YesNo,
                closing ? CopyableMessageBoxIcon.Warning
                        : CopyableMessageBoxIcon.Question, 0))
            {
                case 1:// No
                    this.saveAllToolStripMenuItem.Enabled = true;
                    return true;
                case 2:// Cancel
                    this.saveAllToolStripMenuItem.Enabled = true;
                    return false;
            }
            JazzPackage jp = this.OpenPackageForSaving(null);
            if (jp == null)
            {
                return false;
            }
            // Prompt user for which unsaved jazz graphs to save,
            // and write the desired jazz graphs to the package
            INamedResourceIndexEntry[] toSave = null;
            using (SelectEditRIEDialog rieDialog 
                = new SelectEditRIEDialog(unsaved.ToArray()))
            {
                rieDialog.Text = kName 
                    + ": Save Jazz Graphs To Sims 3 Package";
                rieDialog.Message = "Select the jazz graphs to save " +
                    "into the following Sims 3 package" +
                    (closing ? " before they are closed" : "") + ":\n" +
                    jp.Path;
                rieDialog.AutomaticHashing 
                    = SelectEditRIEDialog.Hashing.FNV64;
                rieDialog.DetectConflicts = true;
                rieDialog.EnablePasteResourceKeys = false;
                rieDialog.ReadonlyTID = true;
                rieDialog.ReadonlyGID = true;
                rieDialog.ReadonlyIID = true;
                rieDialog.ReadonlyCompressed = true;
                if (rieDialog.ShowDialog() == DialogResult.OK)
                {
                    toSave = rieDialog.SelectedResources;
                }
                else
                {
                    if (jp.Graphs.Count == 0)
                    {
                        this.CloseAndRemovePackage(jp);
                    }
                    return false;
                }
            }
            if (toSave == null || toSave.Length == 0)
            {
                if (jp.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jp);
                }
                return true;
            }
            bool update = false;
            TabPage page;
            StateMachine sm;
            GenericRCOLResource rcol;
            TabControl.TabPageCollection pages
                = this.jazzGraphTabControl.TabPages;
            KeyNameReg.IncludedKeyNameMaps = sGameKNMs;
            for (i = toSave.Length - 1; i >= 0; i--)
            {
                jgc = toSave[i] as JazzGraphContainer;
                if (this.jazzGraphTabControl.SelectedIndex == jgc.Index)
                {
                    update = true;
                }
                if (jgc.JP != null)
                {
                    jgc.JP.RemoveJazzGraph(jgc);
                    if (jp.Path.Equals(jgc.JP.Path))
                    {
                        jgc.JP = jp;
                        if (jgc.NameIsValid() && 
                            !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                        {
                            jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                        }
                        sm = jgc.Scene.StateMachine;
                        rcol = sm.ExportToResource(
                            jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                        jp.Package.ReplaceResource(jgc.RIE, rcol);
                    }
                    else if (jgc.JP.Graphs.Count == 0)
                    {
                        this.CloseAndRemovePackage(jgc.JP);
                    }
                }
                else
                {
                    jgc.JP = jp;
                    if (jgc.NameIsValid() && 
                        !jp.KNMapRes.ContainsKey(jgc.Key.IID))
                    {
                        jp.KNMapRes[jgc.Key.IID] = jgc.Name;
                    }
                    sm = jgc.Scene.StateMachine;
                    rcol = sm.ExportToResource(
                        jgc.Key.IID, jp.KNMapRes, sExportAllNames);
                    jp.Package.AddResource(jgc.Key, rcol.Stream, true);
                }
                jp.Graphs.Add(jgc);
                if (jgc.SaveState != JazzSaveState.Saved)
                {
                    page = pages[jgc.Index];
                    page.Text = jgc.Name;//Remove asterisk
                }
                jgc.SaveState = JazzSaveState.Saved;
            }
            KeyNameReg.IncludedKeyNameMaps = KeyNameReg.KNM.All;
            jp.Package.ReplaceResource(jp.KNMapRIE, jp.KNMapRes);
            if (File.Exists(jp.Path))
            {
                jp.Package.SavePackage();
            }
            else
            {
                jp.Package.SaveAs(jp.Path);
                Package.ClosePackage(0, jp.Package);
                s3pi.Filetable.PathPackageTuple ppt
                    = new s3pi.Filetable.PathPackageTuple(jp.Path, true);
                jp.Package = ppt.Package;
                FileTableExt.Current.Add(ppt);
                GlobalManager.RefreshCurrent();
                // TODO: Does the RIE of every JazzGraphContainer
                // in jp.Graphs have to be reset after jp.Package
                // is reopened?
            }
            if (update)
            {
                this.Text = kName + jp.Title;
            }
            i = this.jazzGraphTabControl.SelectedIndex;
            jgc = this.mOpenGraphs[i];
            update = jgc.SaveState != JazzSaveState.Saved;
            this.saveToolStripMenuItem.Enabled = update;
            if (!update)
            {
                for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
                {
                    jgc = this.mOpenGraphs[i];
                    if (jgc.SaveState != JazzSaveState.Saved)
                    {
                        update = true;
                        break;
                    }
                }
            }
            this.saveAllToolStripMenuItem.Enabled = update;
            return true;
        }

        private bool CloseJazzGraph(int index)
        {
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            // Check if the jazz graph has any unsaved changes
            // and save them if necessary
            if (jgc.SaveState != JazzSaveState.Saved)
            {
                switch (CopyableMessageBox.Show(jgc.Name + " has unsaved " +
                    "changes. Would you like to save it before closing it?",
                    kName + ": Unsaved Jazz Graph",
                    CopyableMessageBoxButtons.YesNoCancel, 
                    CopyableMessageBoxIcon.Warning, 0))
                {
                    case 0:// Yes
                        if (!this.SaveJazzGraph(index, true))
                        {
                            return false;
                        }
                        break;
                    case 1:// No
                        break;
                    case 2:// Cancel
                        return false;
                }
            }
            int i;
            // Switch over to a tab page that isn't about to close
            if (this.jazzGraphTabControl.SelectedIndex == index)
            {
                i = this.mOpenGraphs.Count;
                if (i > 1 && index < (i - 1))
                {
                    this.jazzGraphTabControl.SelectedIndex = index + 1;
                }
                else if (i > 1)
                {
                    this.jazzGraphTabControl.SelectedIndex = index - 1;
                }
            }
            // Internally close the jazz graph's containing package
            // if it no longer has any jazz graphs associated with it
            if (jgc.JP != null)
            {
                jgc.JP.RemoveJazzGraph(jgc);
                if (jgc.JP.Graphs.Count == 0)
                {
                    this.CloseAndRemovePackage(jgc.JP);
                }
            }
            // Internally and visually close the jazz graph
            jgc.Scene.StateNodeSelectionChanged -=
                new EventHandler(this.Scene_StateNodeSelectionChanged);
            jgc.Scene.StateEdgeSelectionChanged -=
                new EventHandler(this.Scene_StateEdgeSelectionChanged);
            jgc.Scene.DGNodeSelectionChanged -=
                new EventHandler(this.Scene_DGNodeSelectionChanged);
            jgc.Scene.DGEdgeSelectionChanged -=
                new EventHandler(this.Scene_DGEdgeSelectionChanged);
            jgc.Scene.FocusedStateChanged -=
                new EventHandler(this.Scene_FocusedStateChanged);
            TabControl.TabPageCollection pages 
                = this.jazzGraphTabControl.TabPages;
            TabPage page = pages[index];
            page.SizeChanged -= new EventHandler(jgc.OnTabSizeChanged);
            for (i = this.mOpenGraphs.Count - 1; i > index; i--)
            {
                jgc = this.mOpenGraphs[i];
                jgc.Index--;
            }
            jgc = this.mOpenGraphs[index];
            this.mOpenGraphs.RemoveAt(index);
            pages.RemoveAt(index);
            page.Controls.Clear();
            jgc.Scene.Dispose();
            page.Dispose();
            if (this.mOpenGraphs.Count == 0)
            {
                this.closeToolStripMenuItem.Enabled = false;
                this.closeAllToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItem.Enabled = false;
                this.saveAllToolStripMenuItem.Enabled = false;
                this.packageIntoToolStripMenuItem.Enabled = false;
                this.packageAllIntoToolStripMenuItem.Enabled = false;
                this.exportScriptStripMenuItem.Enabled = false;
                this.exportAllScriptsToolStripMenuItem.Enabled = false;

                this.editToolStripMenuItem.Enabled = false;
                this.stateGraphToolStripMenuItem.Enabled = false;
                this.decisionGraphToolStripMenuItem.Enabled = false;

                this.Text = kName;
            }
            return true;
        }

        private bool CloseAllJazzGraphs()
        {
            // Check if any of the jazz graphs have any unsaved changes
            // and save them if necessary
            if (!this.SaveAllJazzGraphs(true))
            {
                return false;
            }
            int i;
            // Internally close all packages
            JazzPackage jp;
            for (i = this.mOpenPackages.Count - 1; i >= 0; i--)
            {
                jp = this.mOpenPackages[i];
                jp.Graphs.Clear();
                Package.ClosePackage(0, jp.Package);
            }
            this.mOpenPackages.Clear();
            FileTableExt.Current.Clear();
            GlobalManager.RefreshCurrent();
            // Internally and visually close all jazz graphs
            TabPage page;
            JazzGraphContainer jgc;
            TabControl.TabPageCollection pages
                = this.jazzGraphTabControl.TabPages;
            for (i = this.mOpenGraphs.Count - 1; i >= 0; i--)
            {
                jgc = this.mOpenGraphs[i];
                jgc.Scene.StateNodeSelectionChanged -=
                    new EventHandler(this.Scene_StateNodeSelectionChanged);
                jgc.Scene.StateEdgeSelectionChanged -=
                    new EventHandler(this.Scene_StateEdgeSelectionChanged);
                jgc.Scene.DGNodeSelectionChanged -=
                    new EventHandler(this.Scene_DGNodeSelectionChanged);
                jgc.Scene.DGEdgeSelectionChanged -=
                    new EventHandler(this.Scene_DGEdgeSelectionChanged);
                jgc.Scene.FocusedStateChanged -=
                    new EventHandler(this.Scene_FocusedStateChanged);
                page = pages[i];
                page.SizeChanged -= new EventHandler(jgc.OnTabSizeChanged);
                page.Controls.Clear();
                jgc.Scene.Dispose();
                page.Dispose();
            }
            this.mOpenGraphs.Clear();
            pages.Clear();

            this.closeToolStripMenuItem.Enabled = false;
            this.closeAllToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Enabled = false;
            this.saveAllToolStripMenuItem.Enabled = false;
            this.packageIntoToolStripMenuItem.Enabled = false;
            this.packageAllIntoToolStripMenuItem.Enabled = false;
            this.exportScriptStripMenuItem.Enabled = false;
            this.exportAllScriptsToolStripMenuItem.Enabled = false;
            
            this.editToolStripMenuItem.Enabled = false;
            this.stateGraphToolStripMenuItem.Enabled = false;
            this.decisionGraphToolStripMenuItem.Enabled = false;

            this.Text = kName;

            return true;
        }
        #endregion

        #region Jazz Graph Scene Event Handlers

        private void Scene_FocusedStateChanged(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            bool flag = jgc.Scene.FocusedState == null;
            this.stateGraphToolStripMenuItem.Enabled = flag;
            this.decisionGraphToolStripMenuItem.Enabled = !flag;
        }

        private void Scene_DGEdgeSelectionChanged(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            this.removeDGLinksToolStripMenuItem.Enabled 
                = this.CanModifyJazzGraph(index) && 
                    jgc.Scene.SelectedDGEdgeCount > 0;
        }

        private void Scene_DGNodeSelectionChanged(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            this.removeDGNodesToolStripMenuItem.Enabled
                = this.CanModifyJazzGraph(index) && 
                    jgc.Scene.SelectedDGNodeCount > 0;
        }

        private void Scene_StateEdgeSelectionChanged(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            this.removeTransitionsToolStripMenuItem.Enabled
                = this.CanModifyJazzGraph(index) && 
                    jgc.Scene.SelectedStateEdgeCount > 0;
        }

        private void Scene_StateNodeSelectionChanged(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            bool flag = jgc.Scene.SelectedStateNodeCount > 0;
            this.minimizeStatesToolStripMenuItem.Enabled = flag;
            this.removeStatesToolStripMenuItem.Enabled 
                = this.CanModifyJazzGraph(index) && flag;

            if (flag)
            {
                index = 0;
                StateNode sNode;
                StateNode[] sNodes = jgc.Scene.SelectedStateNodes;
                for (int i = sNodes.Length - 1; i >= 0; i--)
                {
                    sNode = sNodes[i];
                    if (sNode.Minimized)
                    {
                        index++;
                    }
                }
                if (index == 0)
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Unchecked;
                }
                else if (index < sNodes.Length)
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Indeterminate;
                }
                else
                {
                    this.minimizeStatesToolStripMenuItem.CheckState
                        = CheckState.Checked;
                }
            }
            else
            {
                this.minimizeStatesToolStripMenuItem.CheckState 
                    = CheckState.Unchecked;
            }
        }
        #endregion

        #region Edit Menu Event Handlers

        private void undo_Click(object sender, EventArgs e)
        {

        }

        private void redo_Click(object sender, EventArgs e)
        {

        }

        private void selectAll_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.Scene.FocusedState == null)
            {
                jgc.Scene.SelectAllStateNodes();
                jgc.Scene.SelectAllStateEdges();
            }
            else
            {
                jgc.Scene.SelectAllDGNodes();
                jgc.Scene.SelectAllDGEdges();
            }
        }

        private void selectAllNodes_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.Scene.FocusedState == null)
            {
                jgc.Scene.SelectAllStateNodes();
            }
            else
            {
                jgc.Scene.SelectAllDGNodes();
            }
        }

        private void selectAllLinks_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.Scene.FocusedState == null)
            {
                jgc.Scene.SelectAllStateEdges();
            }
            else
            {
                jgc.Scene.SelectAllDGEdges();
            }
        }

        private void selectNone_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.Scene.FocusedState == null)
            {
                jgc.Scene.ClearStateEdgeSelection();
                jgc.Scene.ClearStateNodeSelection();
            }
            else
            {
                jgc.Scene.ClearDGEdgeSelection();
                jgc.Scene.ClearDGNodeSelection();
            }
        }
        #endregion

        #region State Graph Menu Event Handlers

        private void shuffleStateNodes_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            if (jgc.Scene.Layout != null)
            {
                jgc.Scene.Layout.ShuffleNodes();
            }
        }

        private void addNewState_Click(object sender, EventArgs e)
        {

        }

        private void minimizeStates_Click(object sender, EventArgs e)
        {
            StateNode sNode;
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            StateNode[] sNodes = jgc.Scene.SelectedStateNodes;
            switch (this.minimizeStatesToolStripMenuItem.CheckState)
            {
                case CheckState.Unchecked:
                    for (index = sNodes.Length - 1; index >= 0; index--)
                    {
                        sNode = sNodes[index];
                        sNode.SetMinimized(true);
                    }
                    this.minimizeStatesToolStripMenuItem.CheckState 
                        = CheckState.Checked;
                    break;
                case CheckState.Indeterminate:
                case CheckState.Checked:
                    for (index = sNodes.Length - 1; index >= 0; index--)
                    {
                        sNode = sNodes[index];
                        sNode.SetMinimized(false);
                    }
                    this.minimizeStatesToolStripMenuItem.CheckState 
                        = CheckState.Unchecked;
                    break;
            }
        }

        private void removeStates_Click(object sender, EventArgs e)
        {

        }

        private void addNewTransitions_Click(object sender, EventArgs e)
        {

        }

        private void removeTransitions_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Decision Graph Menu Event Handlers

        private void shuffleDGNodes_Click(object sender, EventArgs e)
        {
            int index = this.jazzGraphTabControl.SelectedIndex;
            JazzGraphContainer jgc = this.mOpenGraphs[index];
            StateNode stateNode = jgc.Scene.FocusedState;
            if (stateNode != null && stateNode.DGLayout != null)
            {
                stateNode.DGLayout.ShuffleNodes();
            }
        }

        private void addNewDGNode_Click(object sender, EventArgs e)
        {

        }

        private void removeDGNodes_Click(object sender, EventArgs e)
        {

        }

        private void addNewDGLinks_Click(object sender, EventArgs e)
        {

        }

        private void removeDGLinks_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Settings Menu Event Handlers

        private void layoutStateNodesCheckedChanged(
            object sender, EventArgs e)
        {
            bool flag = this.layoutStateNodesToolStripMenuItem.Checked;
            foreach (JazzGraphContainer jgc in this.mOpenGraphs)
            {
                jgc.Scene.LayoutStateGraphOnNodeMoved = flag;
            }
        }

        private void layoutDGNodesCheckedChanged(
            object sender, EventArgs e)
        {
            bool flag = this.layoutDGNodesToolStripMenuItem.Checked;
            foreach (JazzGraphContainer jgc in this.mOpenGraphs)
            {
                jgc.Scene.LayoutStateGraphOnNodeMoved = flag;
            }
        }

        private void preferences_Click(object sender, EventArgs e)
        {
            DialogResult result = this.mSettingsDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
            }
        }
        #endregion
    }
}
