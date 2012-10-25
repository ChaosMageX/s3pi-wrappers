using System;
using System.Collections.Generic;
using s3pi.Filetable;
using s3pi.Interfaces;

namespace s3piwrappers.SceneGraph.Managers
{
    public class STBL
    {
        public enum Lang : byte
        {
            /// <summary>
            ///   en-US; English; United States
            /// </summary>
            ENG_US = 0x00,

            /// <summary>
            ///   zh-CN; Chinese (Simplified); China
            /// </summary>
            CHS_CN = 0x01,

            /// <summary>
            ///   zh-TW; Chinese (Traditional); Taiwan
            /// </summary>
            CHT_CN = 0x02,

            /// <summary>
            ///   cs-CZ; Czech; Czech Republic
            /// </summary>
            CZE_CZ = 0x03,

            /// <summary>
            ///   da-DK; Danish; Denmark
            /// </summary>
            DAN_DK = 0x04,

            /// <summary>
            ///   nl-NL; Dutch; Netherlands
            /// </summary>
            DUT_NL = 0x05,

            /// <summary>
            ///   fi-FI; Finnish; Finland
            /// </summary>
            FIN_FI = 0x06,

            /// <summary>
            ///   fr-FR; French; France
            /// </summary>
            FRE_FR = 0x07,

            /// <summary>
            ///   de-DE; German; Germany
            /// </summary>
            GER_DE = 0x08,

            /// <summary>
            ///   el-GR; Greek; Greece
            /// </summary>
            GRE_GR = 0x09,

            /// <summary>
            ///   hu-HU; Hungarian; Hungary
            /// </summary>
            HUN_HU = 0x0A,

            /// <summary>
            ///   it-IT; Italian; Italy
            /// </summary>
            ITA_IT = 0x0B,

            /// <summary>
            ///   ja-JP; Japanese; Japan
            /// </summary>
            JPN_JP = 0x0C,

            /// <summary>
            ///   ko-KR; Korean; South Korea
            /// </summary>
            KOR_KR = 0x0D,

            /// <summary>
            ///   no-NO; Norwegian; Norway
            /// </summary>
            NOR_NO = 0x0E,

            /// <summary>
            ///   pl-PL; Polish; Poland
            /// </summary>
            POL_PL = 0x0F,

            /// <summary>
            ///   pt-PT; Portuguese; Portugal
            /// </summary>
            POR_PT = 0x10,

            /// <summary>
            ///   pt-BR; Portuguese; Brazil
            /// </summary>
            POR_BR = 0x11,

            /// <summary>
            ///   ru-RU; Russian; Russia
            /// </summary>
            RUS_RU = 0x12,

            /// <summary>
            ///   es-ES; Spanish; Spain
            /// </summary>
            SPA_ES = 0x13,

            /// <summary>
            ///   es-MX; Spanish; Mexico
            /// </summary>
            SPA_MX = 0x14,

            /// <summary>
            ///   sv-SE; Swedish; Sweden
            /// </summary>
            SWE_SE = 0x15,

            /// <summary>
            ///   th-TH; Thai; Thailand
            /// </summary>
            THA_TH = 0x16
        }

        public const uint STBL_TID = 0x220557DA;
        public const ulong FNV64Blank = 0xCBF29CE484222325;

        public static bool LangSearch { get; set; }

        private static Dictionary<byte, List<SpecificResource>> _STBLs;

        public static void Reset()
        {
            _STBLs = null;
        }

        private static bool IsSTBL(IResourceIndexEntry rie)
        {
            return rie.ResourceType == STBL_TID;
        }

        public static Dictionary<byte, List<SpecificResource>> STBLs
        {
            get
            {
                if (_STBLs == null)
                {
                    _STBLs = new Dictionary<byte, List<SpecificResource>>();
                    if (FileTable.GameContent != null)
                        foreach (PathPackageTuple ppt in FileTable.GameContent)
                            foreach (SpecificResource sr in ppt.FindAll(IsSTBL))
                            {
                                //note that we do not want to actually run the resource wrapper here, it's too slow
                                var lang = (byte) (sr.ResourceIndexEntry.Instance >> 56);
                                if (!_STBLs.ContainsKey(lang)) _STBLs.Add(lang, new List<SpecificResource>());
                                _STBLs[lang].Add(sr);
                            }
                }
                return _STBLs;
            }
        }

        public static bool IsOK
        {
            get { return STBLs != null && STBLs.Count > 0; }
        }

        public delegate void Callback(byte lang);

        public static SpecificResource findStblFor(ulong guid, Callback callBack = null)
        {
            if (guid == FNV64Blank) return null;

            for (byte i = 0; i < (LangSearch ? 0x17 : 0x01); i++)
            {
                if (callBack != null) callBack(i);
                SpecificResource sr = findStblFor(guid, i);
                if (sr != null) return sr;
            }
            return null;
        }

        private class STBLFinder
        {
            private readonly ulong guid;

            public STBLFinder(ulong guid)
            {
                this.guid = guid;
            }

            public bool HasGUID(SpecificResource sr)
            {
                var stbl = sr.Resource as IDictionary<ulong, string>;
                if (stbl == null) return false;
                return stbl.ContainsKey(guid);
            }
        }

        public static SpecificResource findStblFor(ulong guid, byte lang)
        {
            if (guid == FNV64Blank) return null;
            var finder = new STBLFinder(guid);
            return STBLs.ContainsKey(lang) ? STBLs[lang].Find(finder.HasGUID) : null;
        }

        public static string StblLookup(ulong guid, int lang = -1, Callback callBack = null)
        {
            if (guid == FNV64Blank) return null;

            SpecificResource sr;
            if (lang < 0 || lang >= 0x17) sr = findStblFor(guid, callBack);
            else sr = findStblFor(guid, (byte) lang);
            return sr == null ? null : (sr.Resource as IDictionary<ulong, string>)[guid];
        }

        private static string language_fmt = "Strings_{0}_{1:x2}{2:x14}";

        private static readonly string[] languages = new[]
            {
                "ENG_US", "CHI_CN", "CHI_TW", "CZE_CZ",
                "DAN_DK", "DUT_NL", "FIN_FI", "FRE_FR",
                "GER_DE", "GRE_GR", "HUN_HU", "ITA_IT",
                "JAP_JP", "KOR_KR", "NOR_NO", "POL_PL",
                "POR_PT", "POR_BR", "RUS_RU", "SPA_ES",
                "SPA_MX", "SWE_SE", "THA_TH",
            };

        public static void AddSTBLToNameMap(IDictionary<ulong, string> nameMap, byte lang, ulong instance)
        {
            if (nameMap == null) throw new ArgumentNullException("nameMap");
            if (nameMap.ContainsKey(instance)) return;
            string value = String.Format(language_fmt, languages[lang], lang, instance & 0x00FFFFFFFFFFFFFF);
            nameMap.Add(instance, value);
        }
    }
}
