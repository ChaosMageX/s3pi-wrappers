using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3pi.Filetable;

namespace s3piwrappers.SceneGraph
{
    public class TextRKContainer : RKContainer
    {
        private static bool ParseRK(string value, out RK result)
        {
            result = null;
            string[] entries = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (entries.Length < 3)
                return false;
            uint tid, gid;
            ulong iid;
            if (!uint.TryParse(entries[0], NumberStyles.HexNumber, null, out tid))
                return false;
            if (!uint.TryParse(entries[1], NumberStyles.HexNumber, null, out gid))
                return false;
            if (!ulong.TryParse(entries[2], NumberStyles.HexNumber, null, out iid))
                return false;
            result = new RK(RK.NULL);
            result.ResourceType = tid;
            result.ResourceGroup = gid;
            result.Instance = iid;
            return true;
        }

        private static string PrintRK(IResourceKey key)
        {
            return string.Format("{0:x8}:{1:x8}:{2:x16}",
                key.ResourceType, key.ResourceGroup, key.Instance);
        }

        private class RKPos
        {
            public List<int> Positions = new List<int>();
            public RK OldResourceKey;
            public RK NewResourceKey;
            public RKPos(RK rk) { this.OldResourceKey = rk; this.NewResourceKey = rk; }
        }

        private List<RKPos> oldToNewRKs = new List<RKPos>();
        private List<string> textLines = new List<string>();
        private List<IResourceConnection> owners = new List<IResourceConnection>();

        public IResourceConnection[] Owners
        {
            get { return this.owners.ToArray(); }
        }

        public TextRKContainer(string fieldPath, AApiVersionedFields rootField, 
            TextReader reader, string absolutePath) 
            : base(fieldPath, rootField, absolutePath)
        {
            this.SlurpReferenceRKs(reader, absolutePath);
        }

        public override bool SetRK(IResourceKey oldKey, IResourceKey newKey)
        {
            RKPos rkPos;
            int foundIndex = -1;
            for (int i = 0; i < this.oldToNewRKs.Count; i++)
            {
                rkPos = this.oldToNewRKs[i];
                if (rkPos.OldResourceKey.Equals(oldKey))
                {
                    rkPos.NewResourceKey = new RK(newKey);
                    foundIndex = i;
                }
            }
            return foundIndex != -1;
        }

        public override bool CommitChanges()
        {
            this.ReplaceReferenceRKs();
            RKContainer.SetRKInField(this.FlushReferenceRKs(),
                base.contentFieldPath, base.rootField);
            return true;
        }

        private void ReplaceReferenceRKs()
        {
            const int keyLen = 8 + 1 + 8 + 1 + 16;//TTTTTTTT:GGGGGGGG:IIIIIIIIIIIIIIII
            int i, j;
            RKPos rkPos;
            List<int> pos;
            string line;
            for (i = 0; i < this.oldToNewRKs.Count; i++)
            {
                rkPos = this.oldToNewRKs[i];
                pos = rkPos.Positions;
                for (j = 0; j < pos.Count; j += 2)
                {
                    line = this.textLines[pos[j]];
                    this.textLines[pos[j]] = line.Substring(0, pos[j + 1])
                        + PrintRK(rkPos.NewResourceKey)
                        + line.Substring(pos[j + 1] + keyLen);
                }
            }
        }

        private TextReader FlushReferenceRKs()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.textLines.Count; i++)
            {
                builder.AppendLine(this.textLines[i]);
            }
            return new StringReader(builder.ToString());
        }

        private void SlurpReferenceRKs(TextReader reader, string absolutePath)
        {
            const int keyLen = 8 + 1 + 8 + 1 + 16;//TTTTTTTT:GGGGGGGG:IIIIIIIIIIIIIIII
            string absoluteName, line = reader.ReadLine();
            int linePos = 0;
            int index = -1;
            int i, foundIndex, keyOffset;

            while (line != null)
            {
                this.textLines.Add(line);
                keyOffset = 4;
                index = line.IndexOf("key:", linePos);
                if (index == -1)
                {
                    index = line.IndexOf("key=\"", linePos);
                    if (index != -1) keyOffset = 5;
                }
                while (index == -1)
                {
                    line = reader.ReadLine();
                    if (line == null)
                        break;
                    this.textLines.Add(line);

                    linePos = 0;
                    index = line.IndexOf("key:", linePos);
                    if (index == -1)
                    {
                        index = line.IndexOf("key=\"", linePos);
                        if (index != -1) keyOffset = 5;
                    }
                }
                linePos += keyLen + keyOffset;
                if (linePos > line.Length)
                {
                    line = reader.ReadLine();
                    continue;
                }

                string rkStr = line.Substring(index + keyOffset, keyLen);
                RK rk;
                if (ParseRK(rkStr, out rk))
                {
                    RKPos rkPos = null;
                    foundIndex = -1;
                    for (i = 0; i < this.oldToNewRKs.Count && foundIndex < 0; i++)
                    {
                        rkPos = this.oldToNewRKs[i];
                        if (rkPos.OldResourceKey.Equals(rk))
                            foundIndex = i;
                    }
                    if (foundIndex == -1)
                    {
                        rkPos = new RKPos(rk);
                        absoluteName = absolutePath + string.Format("{txt:{0},{1}}",
                            this.textLines.Count - 1, index + keyOffset);
                        this.owners.Add(new DefaultConnection(rk, this, false, absoluteName));
                    }
                    rkPos.Positions.Add(this.textLines.Count - 1);
                    rkPos.Positions.Add(index + keyOffset);
                    this.oldToNewRKs.Add(rkPos);
                }
            }
        }
    }
}
