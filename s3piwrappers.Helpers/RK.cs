using System;
using System.Text;
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
                + ":0x" + this.GID.ToString("X8")
                + ":0x" + this.IID.ToString("X16");
        }
    }
}
