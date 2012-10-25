using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph
{
    public class TextRKContainer : RKContainer
    {
        public static bool ParseRK(string value, out RK result)
        {
            result = null;
            string[] entries = value.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
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

        public static string PrintRK(IResourceKey key)
        {
            return string.Format("{0:x8}:{1:x8}:{2:x16}",
                                 key.ResourceType, key.ResourceGroup, key.Instance);
        }

        private class RKPos
        {
            public readonly List<int> Positions = new List<int>();
            public readonly RK OldResourceKey;
            public RK NewResourceKey;

            public RKPos(RK rk)
            {
                OldResourceKey = NewResourceKey = rk;
            }
        }

        private readonly List<RKPos> oldToNewRKs = new List<RKPos>();
        private readonly List<string> textLines = new List<string>();
        private readonly List<IResourceConnection> owners = new List<IResourceConnection>();

        public IResourceConnection[] Owners
        {
            get { return owners.ToArray(); }
        }

        public TextRKContainer(string fieldPath, AApiVersionedFields rootField,
                               TextReader reader, string absolutePath, Predicate<IResourceKey> validate,
                               bool checkAltKeyFields = false)
            : base(fieldPath, rootField, absolutePath, validate)
        {
            SlurpReferenceRKs(reader, checkAltKeyFields);
        }

        public override bool SetRK(IResourceKey oldKey, IResourceKey newKey)
        {
            RKPos rkPos;
            int foundIndex = -1;
            for (int i = 0; i < oldToNewRKs.Count; i++)
            {
                rkPos = oldToNewRKs[i];
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
            ReplaceReferenceRKs();
            SetRKInField(FlushReferenceRKs(),
                         base.contentFieldPath, base.rootField);
            return true;
        }

        private void ReplaceReferenceRKs()
        {
            const int keyLen = 8 + 1 + 8 + 1 + 16; //TTTTTTTT:GGGGGGGG:IIIIIIIIIIIIIIII
            int i, j;
            RKPos rkPos;
            List<int> pos;
            string line;
            for (i = 0; i < oldToNewRKs.Count; i++)
            {
                rkPos = oldToNewRKs[i];
                pos = rkPos.Positions;
                for (j = 0; j < pos.Count; j += 2)
                {
                    line = textLines[pos[j]];
                    textLines[pos[j]] = line.Substring(0, pos[j + 1])
                        + PrintRK(rkPos.NewResourceKey)
                        + line.Substring(pos[j + 1] + keyLen);
                }
            }
        }

        private TextReader FlushReferenceRKs()
        {
            var builder = new StringBuilder();
            for (int i = 0; i < textLines.Count; i++)
            {
                builder.AppendLine(textLines[i]);
            }
            return new StringReader(builder.ToString());
        }

        private void SlurpReferenceRKs(TextReader reader, bool checkAltKeyFields)
        {
            const int keyLen = 8 + 1 + 8 + 1 + 16; //TTTTTTTT:GGGGGGGG:IIIIIIIIIIIIIIII
            string absoluteName, line = reader.ReadLine();
            int linePos = 0;
            int index = -1;
            int i, foundIndex, keyOffset;

            while (line != null)
            {
                textLines.Add(line);
                keyOffset = 4;
                index = line.IndexOf("key:", linePos);
                if (index == -1 && checkAltKeyFields)
                {
                    index = line.IndexOf("key=\"", linePos);
                    if (index != -1) keyOffset = 5;
                }
                while (index == -1)
                {
                    line = reader.ReadLine();
                    if (line == null)
                        break;
                    textLines.Add(line);

                    linePos = 0;
                    index = line.IndexOf("key:", linePos);
                    if (index == -1 && checkAltKeyFields)
                    {
                        index = line.IndexOf("key=\"", linePos);
                        if (index != -1) keyOffset = 5;
                    }
                }
                linePos += keyOffset + keyLen;
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
                    for (i = 0; i < oldToNewRKs.Count && foundIndex < 0; i++)
                    {
                        rkPos = oldToNewRKs[i];
                        if (rkPos.OldResourceKey.Equals(rk))
                            foundIndex = i;
                    }
                    if (foundIndex == -1 && (base.validate == null || base.validate(rk)))
                    {
                        rkPos = new RKPos(rk);
                        absoluteName = absolutePath + string.Format("{txt:{0},{1}}",
                                                                    textLines.Count - 1, index + keyOffset);
                        owners.Add(new DefaultConnection(rk, this, ResourceDataActions.FindWrite, absoluteName));
                    }
                    rkPos.Positions.Add(textLines.Count - 1);
                    rkPos.Positions.Add(index + keyOffset);
                    oldToNewRKs.Add(rkPos);
                }
                else
                {
                    linePos -= keyLen;
                }
            }
        }
    }
}
