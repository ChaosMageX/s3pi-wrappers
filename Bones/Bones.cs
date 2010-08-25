using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace s3piwrappers
{
    /// <summary>
    /// Provides a reverse lookup for hashed bone names.
    /// </summary>
    /// <remarks>
    /// Used in RIG, SLOT, CLIP resources.
    /// </remarks>
    public class Bones
    {
        private Dictionary<UInt32, String> sBoneMap;
        const string kFileName = "Bones.txt";
        public static Bones Instance = new Bones();
        private Bones()
        {
            LoadBoneMap();
        }
        void LoadBoneMap()
        {
            sBoneMap = new Dictionary<UInt32, String>();
            using (var sr = new StreamReader(File.OpenRead(kFileName)))
            {
                while (sr.BaseStream.Position != sr.BaseStream.Length)
                {
                    var line = sr.ReadLine().Split('=');
                    uint key = Convert.ToUInt32(line[0].TrimStart(new[] { '0', 'x' }), 16);
                    string val = line[1];
                    sBoneMap[key] = val;
                }
            }
        }

        public string this[uint key]
        {
            get
            {
                string val;
                if (sBoneMap.ContainsKey(key)) val = sBoneMap[key];
                else val = "0x" + key.ToString("X8");
                return val;
            }
            set
            {
                sBoneMap[key] = value;
            }
        }

       
    }
}
