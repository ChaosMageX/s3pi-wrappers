using System;
using System.Globalization;
using s3pi.Interfaces;

namespace s3piwrappers.Helpers
{
    public struct RK : IResourceKey, IEquatable<RK>, IComparable<RK>
    {
        public uint TID;
        public uint GID;
        public ulong IID;

        public RK(uint tid, uint gid, ulong iid)
        {
            this.TID = tid;
            this.GID = gid;
            this.IID = iid;
        }

        public RK(IResourceKey rk)
        {
            if (rk == null)
            {
                this.TID = 0;
                this.GID = 0;
                this.IID = 0;
            }
            else
            {
                this.TID = rk.ResourceType;
                this.GID = rk.ResourceGroup;
                this.IID = rk.Instance;
            }
        }

        public ulong Instance
        {
            get { return this.IID; }
            set { this.IID = value; }
        }

        public uint ResourceGroup
        {
            get { return this.GID; }
            set { this.GID = value; }
        }

        public uint ResourceType
        {
            get { return this.TID; }
            set { this.TID = value; }
        }

        public bool Equals(IResourceKey x, IResourceKey y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(IResourceKey obj)
        {
            return (int)(this.TID ^ this.GID ^ this.IID);
        }

        public bool Equals(IResourceKey other)
        {
            return other != null &&
                this.IID == other.Instance &&
                this.GID == other.ResourceGroup &&
                this.TID == other.ResourceType;
        }

        public bool Equals(RK other)
        {
            return this.IID == other.IID &&
                   this.GID == other.GID &&
                   this.TID == other.TID;
        }

        public int CompareTo(IResourceKey other)
        {
            if (other == null)
                return 1;
            int num = this.TID.CompareTo(other.ResourceType);
            if (num != 0)
                return num;
            num = this.GID.CompareTo(other.ResourceGroup);
            if (num != 0)
                return num;
            return this.IID.CompareTo(other.Instance);
        }

        public int CompareTo(RK other)
        {
            int num = this.TID.CompareTo(other.TID);
            if (num != 0)
                return num;
            num = this.GID.CompareTo(other.GID);
            if (num != 0)
                return num;
            return this.IID.CompareTo(other.IID);
        }

        public override string ToString()
        {
            return "0x" + this.TID.ToString("X8")
                + "-0x" + this.GID.ToString("X8")
                + "-0x" + this.IID.ToString("X16");
        }

        private static readonly NumberFormatInfo sNFI 
            = NumberFormatInfo.CurrentInfo;

        public static bool TryParseHex32(string s, out uint num)
        {
            num = 0;
            if (s == null || s.Length == 0)
            {
                return false;
            }
            s = s.TrimStart();
            if (s.Length == 0)
            {
                return false;
            }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                uint.TryParse(s.Substring(2, s.Length - 2),
                              NumberStyles.HexNumber, sNFI, out num))
            {
                return true;
            }
            return false;
        }

        public static bool TryParseUInt32(string s, out uint num)
        {
            num = 0;
            if (s == null || s.Length == 0)
            {
                return false;
            }
            s = s.TrimStart();
            if (s.Length == 0)
            {
                return false;
            }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return uint.TryParse(s.Substring(2, s.Length - 2),
                                     NumberStyles.HexNumber, sNFI, out num);
            }
            return uint.TryParse(s, NumberStyles.Integer, sNFI, out num);
        }

        public static bool TryParseHex64(string s, out ulong num)
        {
            num = 0;
            if (s == null || s.Length == 0)
            {
                return false;
            }
            s = s.TrimStart();
            if (s.Length == 0)
            {
                return false;
            }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                ulong.TryParse(s.Substring(2, s.Length - 2),
                               NumberStyles.HexNumber, sNFI, out num))
            {
                return true;
            }
            return false;
        }

        public static bool TryParseUInt64(string s, out ulong num)
        {
            num = 0;
            if (s == null || s.Length == 0)
            {
                return false;
            }
            s = s.TrimStart();
            if (s.Length == 0)
            {
                return false;
            }
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return ulong.TryParse(s.Substring(2, s.Length - 2),
                                      NumberStyles.HexNumber, sNFI, out num);
            }
            return ulong.TryParse(s, NumberStyles.Integer, sNFI, out num);
        }

        public static bool TryParse(string s, out RK result)
        {
            result = new RK();
            if (s == null || s.Length == 0)
            {
                return false;
            }
            // Two formats 0x{X8}-0x{X8}-0x{X16} and (key:){X8}:{X8}:{X16}
            string[] tgi;
            if (s.IndexOf("-", 0, s.Length,
                          StringComparison.OrdinalIgnoreCase) >= 0)
            {
                tgi = s.Split(new char[] { '-' }, 0x7fffffff, 
                              StringSplitOptions.None);
            }
            else
            {
                tgi = s.Split(new char[] { ':' }, 0x7fffffff,
                              StringSplitOptions.None);
            }
            if (tgi.Length < 3)
            {
                return false;
            }
            int phase = 0;
            for (int i = 0; i < tgi.Length && phase < 3; i++)
            {
                s = tgi[i].TrimStart();
                if (s.Length == 0)
                {
                    continue;
                }
                if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    s = s.Substring(2, s.Length - 2);
                }
                switch (phase)
                {
                    case 0:
                        if (uint.TryParse(s, NumberStyles.HexNumber, 
                                          sNFI, out result.TID))
                        {
                            phase++;
                        }
                        break;
                    case 1:
                        if (uint.TryParse(s, NumberStyles.HexNumber, 
                                          sNFI, out result.GID))
                        {
                            phase++;
                        }
                        break;
                    case 2:
                        if (ulong.TryParse(s, NumberStyles.HexNumber, 
                                           sNFI, out result.IID))
                        {
                            phase++;
                        }
                        break;
                }
            }
            return phase == 3;
        }
    }
}
