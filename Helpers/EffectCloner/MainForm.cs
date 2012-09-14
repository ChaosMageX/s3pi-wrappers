using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.SWB;
using s3piwrappers.EffectCloner.Swarm;

namespace s3piwrappers.EffectCloner
{
    public partial class MainForm : Form
    {
        private const string kEffectDialogFilter 
            = "Effect File (*.effects)|*.effects|Effect Package (*.package)|*.package";

        private const uint kEffectTID = 0xEA5118B0;
        private const uint kEffectGID = 0x0051185B;
        private const string kEffectS3piNameHead = "S3_EA5118B0_0051185B_";
        private const string kEffectS3piNameTail = "%%+_SWB.effects";

        private s3pi.Package.Package mEffectPackage;
        private EffectResource[] mEffectResources;
        private EffectResourceBuilder mOutputEffectResBuilder;

        public static void ShowException(Exception ex)
        {
            ShowException(ex, "", "Program Exception");
        }

        public static void ShowException(Exception ex, string prefix, string caption)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(prefix);
            for (Exception exception = ex; exception != null; exception = exception.InnerException)
            {
                builder.Append("\nSource: " + exception.Source);
                builder.Append("\nAssembly: " + exception.TargetSite.DeclaringType.Assembly.FullName);
                builder.Append("\n" + exception.Message);
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, false);
                builder.Append("\n" + trace);
                builder.Append("\n-----");
            }
            CopyableMessageBox.Show(builder.ToString(), caption, CopyableMessageBoxButtons.OK, 
                CopyableMessageBoxIcon.Error, 0);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(params string[] args)
            : this()
        {
            if (args.Length == 1)
            {
                string fileName = args[0];
                if (File.Exists(fileName))
                {
                    this.OpenInputEffect(fileName);
                    this.openInputBTN.Enabled = false;
                }
            }
        }

        private class VisualEffectHandleContainer
        {
            public VisualEffectHandle Handle;
            public EffectResource Owner;
            //public List<MetaparticleEffect> Dependents;
            public string OriginalName;

            public VisualEffectHandleContainer(VisualEffectHandle handle, EffectResource owner)
            {
                this.Handle = handle;
                this.Owner = owner;
                //this.Dependents = new List<MetaparticleEffect>();
                this.OriginalName = handle.EffectName;
            }

            public void SetEffectName(string newName)
            {
                /*string oldName = this.Handle.EffectName;
                if (this.Dependents != null)
                {
                    MetaparticleEffect metaEffect;
                    for (int i = 0; i < this.Dependents.Count; i++)
                    {
                        metaEffect = this.Dependents[i];
                        if (metaEffect.BaseEffectName == oldName)
                            metaEffect.BaseEffectName = newName;
                        if (metaEffect.DeathEffectName == oldName)
                            metaEffect.DeathEffectName = newName;
                    }
                }/**/
                this.Handle.EffectName = newName;
            }

            public void AddDependent(MetaparticleEffect metaEffect)
            {
                bool add = false;
                if (metaEffect.BaseEffectName == this.OriginalName)
                {
                    metaEffect.BaseEffectName = this.Handle.EffectName;
                    add = true;
                }
                if (metaEffect.DeathEffectName == this.OriginalName)
                {
                    metaEffect.DeathEffectName = this.Handle.EffectName;
                    add = true;
                }
                if (add)
                {
                    //this.Dependents.Add(metaEffect);
                }
            }

            public override string ToString()
            {
                return this.Handle.EffectName;
            }
        }

        private void openInput_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Open Effect Resource(s)";
                dialog.Filter = kEffectDialogFilter;
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.OpenInputEffect(dialog.FileName);
                }
            }
        }

        private static bool IsEffectResource(IResourceIndexEntry entry)
        {
            return entry.ResourceType == kEffectTID;
        }

        private void OpenInputEffect(string fileName)
        {
            if (this.outputEffectLST.Items.Count > 0)
            {
                DialogResult result = MessageBox.Show("Discard unsaved changes?", "Alert", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.No)
                    return;
            }
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (extension == ".effects")
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    this.mEffectResources = new EffectResource[1];
                    this.mEffectResources[0] = new EffectResource(0, fileStream);
                }
            }
            else if (extension == ".package")
            {
                this.mEffectPackage = s3pi.Package.Package.OpenPackage(0, fileName) as s3pi.Package.Package;
                List<IResourceIndexEntry> effectResources = this.mEffectPackage.FindAll(
                    new Predicate<IResourceIndexEntry>(IsEffectResource));
                if (effectResources == null || effectResources.Count == 0)
                {
                    MessageBox.Show(string.Concat(Path.GetFileName(fileName),
                        " does not contain any effect resources."));
                    return;
                }
                this.mEffectResources = new EffectResource[effectResources.Count];
                for (int k = 0; k < effectResources.Count; k++)
                {
                    this.mEffectResources[k] = new EffectResource(0, 
                        this.mEffectPackage.GetResource(effectResources[k]));
                }
            }
            else
            {
                MessageBox.Show(string.Concat("Unknown file type: *", extension));
                return;
            }
            this.inputEffectLST.Items.Clear();
            this.outputEffectLST.Items.Clear();
            this.mOutputEffectResBuilder = new EffectResourceBuilder(/*this.mEffectResource/**/);

            EffectResource effectResource;
            int i, j, count;
            for (i = 0; i < this.mEffectResources.Length; i++)
            {
                effectResource = this.mEffectResources[i];
                count = effectResource.VisualEffectHandles.Count;
                for (j = 0; j < count; j++)
                    this.inputEffectLST.Items.Add(new VisualEffectHandleContainer(
                        effectResource.VisualEffectHandles[j], effectResource));
            }
            this.printInputBTN.Enabled = true;
        }

        private string PromptForEffectName(string originalName)
        {
            SimpleStringPrompt prompt = new SimpleStringPrompt("Enter Effect Name", originalName);
            DialogResult result = DialogResult.Cancel;
            string effectLC, effectName = originalName;
            int index;
            while (result != DialogResult.OK)
            {
                result = prompt.ShowDialog();
                effectName = EffectHelper.CreateSafeEffectName(prompt.Result, false);
                effectLC = effectName.ToLowerInvariant();
                index = effectLC.Equals(originalName.ToLowerInvariant()) ? 0 : -1;
                for (int i = 0; i < this.mEffectResources.Length && index < 0; i++)
                    index = EffectHelper.BinarySearchForEffectHandle(effectLC, this.mEffectResources[i]);
                if (index >= 0)
                {
                    MessageBox.Show(effectName + " already exists. Try a different one.", "Alert");
                    result = DialogResult.Cancel;
                }
                if (result == DialogResult.OK)
                {
                    foreach (VisualEffectHandleContainer container in this.outputEffectLST.Items)
                    {
                        if (container.Handle.EffectName.ToLowerInvariant().Equals(effectLC))
                        {
                            MessageBox.Show(effectName + " already exists. Try a different one.", "Alert");
                            result = DialogResult.Cancel;
                            break;
                        }
                    }
                }
            }
            return effectName;
        }

        private void setEffectNameTSMI_Click(object sender, EventArgs e)
        {
            VisualEffectHandleContainer container = this.outputEffectLST.SelectedItem as VisualEffectHandleContainer;
            if (container != null)
            {
                container.SetEffectName(this.PromptForEffectName(container.Handle.EffectName));
                int index = this.outputEffectLST.SelectedIndex;
                this.outputEffectLST.Items.RemoveAt(index);
                this.outputEffectLST.Items.Insert(index, container);/**/
                //this.outputEffectLST.Invalidate();
            }
        }

        private void LocateMetaReferences(VisualEffectHandle handle, EffectResource resource,
            List<VisualEffectHandleContainer> references)
        {
            MetaparticleEffect[] metaEffects = EffectHelper.FindEffects<MetaparticleEffect>(handle, resource);
            if (metaEffects != null && metaEffects.Length > 0)
            {
                int i, j;
                VisualEffectHandle dep;
                string effectName;
                bool flag = false;
                for (i = 0; i < metaEffects.Length; i++)
                {
                    // compensate for variations in casing
                    effectName = metaEffects[i].BaseEffectName.ToLowerInvariant();
                    for (j = 0; j < references.Count && !flag; j++)
                    {
                        if (references[j].OriginalName == effectName)
                        {
                            //references[j].Dependents.Add(metaEffects[i]);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        dep = EffectHelper.FindVisualEffectHandle(effectName, resource);
                        if (dep != null)
                        {
                            references.Add(new VisualEffectHandleContainer(
                                new VisualEffectHandle(0, null, dep), resource));
                            this.LocateMetaReferences(dep, resource, references);
                        }
                    }
                    flag = false;
                    // compensate for variations in casing
                    effectName = metaEffects[i].DeathEffectName.ToLowerInvariant();
                    for (j = 0; j < references.Count && !flag; j++)
                    {
                        if (references[j].OriginalName == effectName)
                        {
                            //references[j].Dependents.Add(metaEffects[i]);
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        dep = EffectHelper.FindVisualEffectHandle(effectName, resource);
                        if (dep != null)
                        {
                            references.Add(new VisualEffectHandleContainer(
                                new VisualEffectHandle(0, null, dep), resource));
                            this.LocateMetaReferences(dep, resource, references);
                        }
                    }
                    flag = false;
                }
            }
        }

        private void LocateSeqReferences(VisualEffectHandle handle, EffectResource resource,
            List<VisualEffectHandleContainer> references)
        {
            SequenceEffect[] seqEffects = EffectHelper.FindEffects<SequenceEffect>(handle, resource);
            if (seqEffects != null && seqEffects.Length > 0)
            {
                int i, j, k;
                VisualEffectHandle dep;
                SequenceEffect seqEffect;
                string effectName;
                bool flag = false;
                for (i = 0; i < seqEffects.Length; i++)
                {
                    seqEffect = seqEffects[i];
                    for (j = 0; j < seqEffect.Elements.Count; j++)
                    {
                        // compensate for variations in casing
                        effectName = seqEffect.Elements[j].EffectName.ToLowerInvariant();
                        for (k = 0; k < references.Count && !flag; k++)
                        {
                            if (references[k].OriginalName == effectName)
                            {
                                //references[j].Dependents.Add(seqEffects[i]);
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            dep = EffectHelper.FindVisualEffectHandle(effectName, resource);
                            if (dep != null)
                            {
                                references.Add(new VisualEffectHandleContainer(
                                    new VisualEffectHandle(0, null, dep), resource));
                                this.LocateSeqReferences(dep, resource, references);
                            }
                        }
                        flag = false;
                    }
                }
            }
        }

        private void cloneEffect_Click(object sender, EventArgs e)
        {
            VisualEffectHandle handle = (this.inputEffectLST.SelectedItem as VisualEffectHandleContainer).Handle;
            EffectResource owner = (this.inputEffectLST.SelectedItem as VisualEffectHandleContainer).Owner;
            if (handle != null)
            {
                VisualEffectHandleContainer con;
                foreach (VisualEffectHandleContainer container in this.outputEffectLST.Items)
                {
                    if (container.Handle.Index == handle.Index)
                    {
                        MessageBox.Show("Selected effect has already been cloned.");
                        return;
                    }
                }
                con = new VisualEffectHandleContainer(new VisualEffectHandle(0, null, handle), owner);
                con.SetEffectName(this.PromptForEffectName(con.OriginalName));
                this.outputEffectLST.Items.Add(con);

                List<VisualEffectHandleContainer> references = new List<VisualEffectHandleContainer>();
                this.LocateMetaReferences(handle, owner, references);
                int metaCount = references.Count;
                this.LocateSeqReferences(handle, owner, references);
                int seqCount = references.Count - metaCount;
                if (references.Count > 0)
                {
                    string format;
                    if (metaCount > 0)
                        if (seqCount > 0)
                            format = "{0} has metaparticle and sequence effects.\n";
                        else
                            format = "{0} has metaparticle effects.\n";
                    else
                        format = "{0} has sequence effects.\n";
                    DialogResult result = MessageBox.Show(
                        //string.Concat(handle.EffectName, " has metaparticle and/or sequence effects.\n",
                        string.Concat(string.Format(format, handle.EffectName),
                        "Would you like to clone all effects referenced by them?"),
                        "Clone Referenced Effects", MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        for (int i = 0; i < references.Count; i++)
                        {
                            con = references[i];
                            con.SetEffectName(this.PromptForEffectName(con.OriginalName));
                            this.outputEffectLST.Items.Add(con);
                        }
                    }
                }
                this.saveOutputBTN.Enabled = true;
                this.printOutputBTN.Enabled = true;
            }
        }

        private void removeEffect_Click(object sender, EventArgs e)
        {
            int index = this.outputEffectLST.SelectedIndex;
            if (index >= 0)
                this.outputEffectLST.Items.RemoveAt(index);
            if (this.outputEffectLST.Items.Count == 0)
            {
                this.saveOutputBTN.Enabled = false;
                this.printOutputBTN.Enabled = false;
            }
        }

        private void saveOutput_Click(object sender, EventArgs e)
        {
            /*if (this.mEffectResource == null)
            {
                MessageBox.Show("Please open an effect file to clone effects from it.");
                return;
            }
            if (this.outputEffectLST.Items.Count == 0)
            {
                MessageBox.Show("Please select at least one effect from the left list to clone.");
                return;
            }/**/
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Save Effect Clone";
                dialog.Filter = kEffectDialogFilter;
                dialog.AddExtension = true;
                dialog.OverwritePrompt = true;
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string resName = Path.GetFileNameWithoutExtension(dialog.FileName);
                    ulong resHash = System.Security.Cryptography.FNV64.GetHash(resName);

                    string fileName = dialog.FileName;
                    string extension = Path.GetExtension(fileName).ToLowerInvariant();

                    if (extension == ".effects")
                    {
                        resName = kEffectS3piNameHead + resHash.ToString("X16") + "_" + resName
                            + kEffectS3piNameTail;
                        string prompt = "Would you like to save the effect file as:\n\n" + resName
                            + "\n\ninstead of:\n\n" + Path.GetFileName(fileName);
                        result = MessageBox.Show(prompt, "Make S3PI File Name?", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            fileName = dialog.FileName.Replace(Path.GetFileNameWithoutExtension(fileName), resName);
                        else
                            fileName = dialog.FileName;
                    }
                    
                    VisualEffectHandleContainer con;
                    int i, count = this.outputEffectLST.Items.Count;
                    for (i = 0; i < count; i++)
                    {
                        con = this.outputEffectLST.Items[i] as VisualEffectHandleContainer;
                        this.mOutputEffectResBuilder.AddVisualEffect(con.Handle, con.Owner);
                    }
                    this.mOutputEffectResBuilder.FlushEffects();
                    
                    for (i = 0; i < count; i++)
                    {
                        con = this.outputEffectLST.Items[i] as VisualEffectHandleContainer;
                        this.mOutputEffectResBuilder.SetAllEffectReferences(con.OriginalName, con.Handle.EffectName);
                    }
                    //EffectResource output = this.mInputEffectResBuilder.BuildResource();
                    //this.mInputEffectResBuilder.ResetOld();

                    if (extension == ".effects")
                    {
                        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                        {
                            this.mOutputEffectResBuilder.UnParse(fileStream);
                            /*Stream stream = output.Stream;
                            int length = (int)stream.Length;
                            byte[] buffer = new byte[length];
                            stream.Read(buffer, 0, length);
                            fileStream.Write(buffer, 0, length);/**/
                        }
                    }
                    else if (extension == ".package")
                    {
                        // Fail-safe to prevent accidental collision with existing SWB resources
                        //resHash = System.Security.Cryptography.FNV64.GetHash(resName + DateTime.Now.ToString());
                        TGIBlock effectKey = new TGIBlock(0, null, kEffectTID, kEffectGID, resHash);
                        MemoryStream mStream = new MemoryStream();
                        this.mOutputEffectResBuilder.UnParse(mStream);

                        TGIBlock nameMapKey = new TGIBlock(0, null, 0x0166038C, 0, 0L);
                        NameMapResource.NameMapResource nameMap
                            = new NameMapResource.NameMapResource(0, null);
                        nameMap.Add(resHash, resName);

                        IPackage effectPackage = s3pi.Package.Package.NewPackage(0);
                        effectPackage.AddResource(effectKey, mStream, false);
                        effectPackage.AddResource(nameMapKey, nameMap.Stream, false);
                        effectPackage.SaveAs(fileName);
                    }
                    this.outputEffectLST.Items.Clear();
                    this.mOutputEffectResBuilder = new EffectResourceBuilder();
                }
            }
        }

        private void printInput_Click(object sender, EventArgs e)
        {
            /*if (this.mEffectResource == null)
            {
                MessageBox.Show("Please open an effect file to print a list of effects it contains.");
                return;
            }/**/
            if (this.mEffectResources != null && this.mEffectResources.Length > 0)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "Print Input Effect Names";
                    dialog.Filter = "Text File|*.txt";
                    dialog.AddExtension = true;
                    dialog.OverwritePrompt = true;
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        using (FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create))
                        {
                            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.ASCII))
                            {
                                for (int i = 0; i < this.mEffectResources.Length; i++)
                                {
                                    EffectHelper.WriteEffectNameList(this.mEffectResources[i], writer);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void printOutput_Click(object sender, EventArgs e)
        {
            if (this.outputEffectLST.Items.Count > 0)
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Title = "Print Output Effect Names";
                    dialog.Filter = "Text File|*.txt";
                    dialog.AddExtension = true;
                    dialog.OverwritePrompt = true;
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        int i, index, count = this.outputEffectLST.Items.Count;
                        string eName;
                        List<string> effectNames = new List<string>(count);
                        effectNames.Add(this.outputEffectLST.Items[0].ToString());
                        for (i = 1; i < count; i++)
                        {
                            eName = this.outputEffectLST.Items[i].ToString();
                            index = effectNames.BinarySearch(eName);
                            // index should always be < 0 or there is something wrong with PromptForEffectName
                            if (index >= 0)
                                throw new Exception("Duplicate output visual effect name");
                            effectNames.Insert(~index, eName);
                        }
                        using (FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create))
                        {
                            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.ASCII))
                            {
                                
                                for (i = 0; i < count; i++)
                                {
                                    writer.WriteLine(effectNames[i]);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void showItemBStringsTSMI_Click(object sender, EventArgs e)
        {
            VisualEffectHandleContainer container = this.inputEffectLST.SelectedItem as VisualEffectHandleContainer;
            if (container != null)
            {
                string[] itemBStrings = EffectHelper.GetItemBStrings(container.Handle, container.Owner);
                StringBuilder builder = new StringBuilder();
                if (itemBStrings.Length == 0)
                {
                    builder.Append("No Item B Strings");
                }
                else
                {
                    for (int i = 0; i < itemBStrings.Length; i++)
                    {
                        builder.AppendLine(itemBStrings[i]);
                    }
                }
                MessageBox.Show(builder.ToString(), "Item B Strings");
            }
        }
    }
}
