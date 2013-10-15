using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3piwrappers.Helpers.Cryptography;
using s3piwrappers.Helpers.Resources;

namespace s3piwrappers.JazzGraph
{
    public class NamespaceMap
    {
        private StateMachine mStateMachine;
        /// <summary>
        /// Source File -> Namespace -> Actor
        /// </summary>
        private SortedDictionary<string, 
            SortedDictionary<string, ActorDefinition>> mSrcFile2Ns2Actor;
        private SortedDictionary<IResourceKey, string> mKeyToFilenameMap;

        public NamespaceMap(StateMachine stateMachine)
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException("stateMachine");
            }
            this.mStateMachine = stateMachine;

            this.mSrcFile2Ns2Actor = new SortedDictionary<string, 
                SortedDictionary<string, ActorDefinition>>();
            this.mKeyToFilenameMap = new SortedDictionary<IResourceKey, string>();
        }

        public void SetNamespaceMap(string sourceFile, string nameSpace, 
            ActorDefinition actor)
        {
            if (sourceFile == null)
                throw new ArgumentNullException("sourceFile");
            if (nameSpace == null)
                throw new ArgumentNullException("namespaces");
            SortedDictionary<string, ActorDefinition> dict;
            if (!this.mSrcFile2Ns2Actor.TryGetValue(sourceFile, out dict))
            {
                dict = new SortedDictionary<string, ActorDefinition>();
                this.mSrcFile2Ns2Actor.Add(sourceFile, dict);
            }
            dict[nameSpace] = actor;
        }

        public void RegisterNamespaces(string sourceFile, string[] nameSpaces)
        {
            if (sourceFile == null)
                throw new ArgumentNullException("sourceFile");
            if (nameSpaces == null)
                throw new ArgumentNullException("namespaces");
            SortedDictionary<string, ActorDefinition> dict;
            bool flag = this.mSrcFile2Ns2Actor.TryGetValue(
                sourceFile, out dict);
            if (!flag)
            {
                dict = new SortedDictionary<string, ActorDefinition>();
                this.mSrcFile2Ns2Actor.Add(sourceFile, dict);
            }
            int i;
            string str;
            for (i = 0; i < nameSpaces.Length; i++)
            {
                str = nameSpaces[i];
                if (str != null && !dict.ContainsKey(str))
                {
                    dict.Add(str, null);
                }
            }
            if (flag)
            {
                int index, count = dict.Count;
                string[] keys = new string[count];
                dict.Keys.CopyTo(keys, 0);
                for (i = 0; i < count; i++)
                {
                    index = Array.IndexOf(nameSpaces, keys[i], 0, count);
                    if (index >= 0)
                    {
                        dict.Remove(keys[i]);
                    }
                }
            }
        }

        public class AnimNamespace
        {
            public readonly string Name;
            public readonly ActorDefinition Actor;

            public AnimNamespace(string name, ActorDefinition Actor)
            {
                this.Name = name;
                this.Actor = Actor;
            }
        }

        public class AnimSource
        {
            public readonly string FileName;
            public readonly AnimNamespace[] Namespaces;

            public AnimSource(string fileName, AnimNamespace[] namespaces)
            {
                this.FileName = fileName;
                this.Namespaces = namespaces;
            }
        }

        public AnimSource[] GetSourceToNamespaceToActorArray()
        {
            AnimNamespace[] namespaces;
            AnimSource source;
            AnimSource[] sources 
                = new AnimSource[this.mSrcFile2Ns2Actor.Count];
            int j, i = 0;
            foreach (KeyValuePair<string,
                SortedDictionary<string, ActorDefinition>> dict
                in this.mSrcFile2Ns2Actor)
            {
                if (dict.Value == null)
                {
                    source = new AnimSource(dict.Key, null);
                }
                else
                {
                    j = 0;
                    namespaces = new AnimNamespace[dict.Value.Count];
                    foreach (KeyValuePair<string, ActorDefinition> pair
                        in dict.Value)
                    {
                        namespaces[j++] 
                            = new AnimNamespace(pair.Key, pair.Value);
                    }
                    source = new AnimSource(dict.Key, namespaces);
                }
                sources[i++] = source;
            }
            return sources;
        }

        public SortedDictionary<IResourceKey, string> KeyToFilenameMap
        {
            get { return this.mKeyToFilenameMap; }
        }

        public void UpdateKeyToFilenameMap(
            IDictionary<ulong, string> keyNameMap)
        {
            if (keyNameMap == null)
            {
                throw new ArgumentNullException("keyNameMap");
            }
            this.mKeyToFilenameMap.Clear();
            IResourceKey rk;
            List<IResourceKey> rks = this.mStateMachine.SlurpReferencedRKs();
            for (int i = rks.Count - 1; i >= 0; i--)
            {
                rk = rks[i];
                if (rk.ResourceType != 0x6b20c4f3)
                {
                    rks.RemoveAt(i);
                }
            }
            string name;
            for (int i = 0; i < rks.Count; i++)
            {
                rk = rks[i];
                if (keyNameMap.TryGetValue(rk.Instance, out name) ||
                    KeyNameReg.TryFindName(rk.Instance, out name))
                {
                    this.mKeyToFilenameMap[rk] = name;
                }
            }
        }

        public void ExportToList(JazzStateMachine.AnimationList animList,
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            if (animList == null)
            {
                throw new ArgumentNullException("animList");
            }
            if (nameMap == null)
            {
                throw new ArgumentNullException("nameMap");
            }
            string srcFile;
            foreach (KeyValuePair<string,
                SortedDictionary<string, ActorDefinition>> dict
                in this.mSrcFile2Ns2Actor)
            {
                srcFile = dict.Key;
                foreach (KeyValuePair<string, ActorDefinition> pair 
                    in dict.Value)
                {
                    animList.Add(this.ExportEntry(srcFile, 
                        pair.Key, pair.Value, nameMap, exportAllNames));
                }
            }
            uint code;
            SortedDictionary<string, ActorDefinition> table;
            foreach (KeyValuePair<IResourceKey, string> key 
                in this.mKeyToFilenameMap)
            {
                srcFile = key.Value;
                code = (uint)((key.Key.Instance >> 0x20) 
                     ^ (key.Key.Instance & 0xffffffffUL));
                if (this.mSrcFile2Ns2Actor.TryGetValue(srcFile, out table))
                {
                    foreach (KeyValuePair<string, ActorDefinition> pair 
                        in table)
                    {
                        animList.Add(this.ExportEntry(code, 
                            pair.Key, pair.Value, nameMap, exportAllNames));
                    }
                }
            }
        }

        private JazzStateMachine.Animation ExportEntry(string sourceFile,
            string nameSpace, ActorDefinition actor,
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            return this.ExportEntry(sourceFile, 0, true, 
                nameSpace, actor, nameMap, exportAllNames);
        }

        private JazzStateMachine.Animation ExportEntry(uint code,
            string nameSpace, ActorDefinition actor,
            IDictionary<ulong, string> nameMap, bool exportAllNames)
        {
            return this.ExportEntry(null, code, false, 
                nameSpace, actor, nameMap, exportAllNames);
        }

        private JazzStateMachine.Animation ExportEntry(
            string sourceFile, uint code, bool useSourceFile,
            string nameSpace, ActorDefinition actor,
            IDictionary<ulong, string> nameMap, bool ean)
        {
            System.IO.Stream s = null;
            uint hash;
            string name;
            JazzStateMachine.Animation animation 
                = new JazzStateMachine.Animation(0, null, s);
            if (useSourceFile)
            {
                // TODO: sourceFile should not allow parsable numbers
                // Try to create a better brute force unhasher for
                // reading animation source files.
                if (!sourceFile.StartsWith("0x") ||
                    !uint.TryParse(sourceFile.Substring(2),
                        System.Globalization.NumberStyles.HexNumber,
                        null, out hash))
                {
                    hash = FNVHash.HashString32(sourceFile);
                    if (!nameMap.TryGetValue(hash, out name) &&
                        (ean || !KeyNameReg.TryFindName(hash, out name)))
                    {
                        nameMap[hash] = sourceFile;
                    }
                }
                animation.NameHash = hash;
            }
            else
            {
                animation.NameHash = code;
            }
            if (nameSpace.StartsWith("0x") && 
                uint.TryParse(nameSpace.Substring(2),
                    System.Globalization.NumberStyles.HexNumber, 
                    null, out hash))
            {
                animation.Actor1Hash = hash;
            }
            else
            {
                hash = FNVHash.HashString32(nameSpace);
                if (!nameMap.TryGetValue(hash, out name) &&
                    (ean || !KeyNameReg.TryFindName(hash, out name)))
                {
                    nameMap[hash] = sourceFile;
                }
                animation.Actor1Hash = hash;
            }
            
            animation.Actor2Hash = actor == null ? 0 : actor.NameHash;

            return animation;
        }
    }
}
