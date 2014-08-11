using System;
using System.IO;
using System.Text;

namespace s3piwrappers.Helpers.IO
{
    public sealed class BinaryStreamWrapper
    {
        private static readonly ByteOrder defaultByteOrder;
        private static readonly Encoding defaultEncoding;

        static BinaryStreamWrapper()
        {
            defaultByteOrder = ByteOrder.LittleEndian;
            defaultEncoding = Encoding.ASCII;
        }

        private readonly Stream mStream;
        private ByteOrder mByteOrder;
        private Encoding mCharEncoding;

        public Stream BaseStream
        {
            get { return mStream; }
        }

        public ByteOrder ByteOrder { get { return mByteOrder; } set { mByteOrder = value; } }

        public Encoding CharacterEncoding { get { return mCharEncoding; } set { mCharEncoding = value; } }

        public BinaryStreamWrapper(Stream s) : this(s, defaultEncoding, defaultByteOrder)
        {
        }

        public BinaryStreamWrapper(Stream s, Encoding encoding) : this(s, encoding, defaultByteOrder)
        {
        }

        public BinaryStreamWrapper(Stream s, ByteOrder order) : this(s, defaultEncoding, order)
        {
        }

        public BinaryStreamWrapper(Stream s, Encoding encoding, ByteOrder order)
        {
            mStream = s;
            mCharEncoding = encoding;
            mByteOrder = order;
        }

        public bool ReadBool8()
        {
            return mStream.ReadByte() > 0;
        }

        public Byte ReadByte()
        {
            return (Byte) mStream.ReadByte();
        }

        public Char ReadChar()
        {
            return ReadChar(mCharEncoding, mByteOrder);
        }

        public UInt16 ReadUInt16()
        {
            return ReadUInt16(mByteOrder);
        }

        public UInt32 ReadUInt32()
        {
            return ReadUInt32(mByteOrder);
        }

        public UInt64 ReadUInt64()
        {
            return ReadUInt64(mByteOrder);
        }

        public Int16 ReadInt16()
        {
            return ReadInt16(mByteOrder);
        }

        public Int32 ReadInt32()
        {
            return ReadInt32(mByteOrder);
        }

        public Int64 ReadInt64()
        {
            return ReadInt64(mByteOrder);
        }

        public float ReadFloat()
        {
            return ReadFloat(mByteOrder);
        }

        public double ReadDouble()
        {
            return ReadDouble(mByteOrder);
        }

        public void Read(out Byte[] output, int count)
        {
            output = ReadBytes(count);
        }

        public void Read(out UInt16 output)
        {
            output = ReadUInt16();
        }

        public void Read(out UInt32 output)
        {
            output = ReadUInt32();
        }

        public void Read(out UInt64 output)
        {
            output = ReadUInt64();
        }

        public void Read(out Int16 output)
        {
            output = ReadInt16();
        }

        public void Read(out Int32 output)
        {
            output = ReadInt32();
        }

        public void Read(out Int64 output)
        {
            output = ReadInt64();
        }

        public void Read(out float output)
        {
            output = ReadFloat();
        }

        public void Read(out double output)
        {
            output = ReadDouble();
        }

        public void Read(out Byte[] output, int count, ByteOrder order)
        {
            output = GetBytes(count, order);
        }

        public void Read(out bool output)
        {
            output = mStream.ReadByte() > 0;
        }

        public void Read(out Byte output)
        {
            output = (Byte) mStream.ReadByte();
        }

        public void Read(out UInt16 output, ByteOrder order)
        {
            output = ReadUInt16(order);
        }

        public void Read(out UInt32 output, ByteOrder order)
        {
            output = ReadUInt32(order);
        }

        public void Read(out UInt64 output, ByteOrder order)
        {
            output = ReadUInt64(order);
        }

        public void Read(out Int16 output, ByteOrder order)
        {
            output = ReadInt16(order);
        }

        public void Read(out Int32 output, ByteOrder order)
        {
            output = ReadInt32(order);
        }

        public void Read(out Int64 output, ByteOrder order)
        {
            output = ReadInt64(order);
        }

        public void Read(out float output, ByteOrder order)
        {
            output = ReadFloat(order);
        }

        public void Read(out double output, ByteOrder order)
        {
            output = ReadDouble(order);
        }

        public void Read(out String output, StringType type)
        {
            output = ReadString(type);
        }

        public void Read(out String output, StringType type, Encoding encoding)
        {
            output = ReadString(type, encoding);
        }

        public void Read(out String output, StringType type, ByteOrder order)
        {
            output = ReadString(type, order);
        }

        public void Read(out String output, StringType type, Encoding encoding, ByteOrder order)
        {
            output = ReadString(type, encoding, order);
        }

        public String ReadString(StringType type)
        {
            return ReadString(type, mCharEncoding, mByteOrder);
        }

        public String ReadString(StringType type, ByteOrder order)
        {
            return ReadString(type, mCharEncoding, order);
        }

        public String ReadString(StringType type, Encoding encoding)
        {
            return ReadString(type, encoding, mByteOrder);
        }

        public String ReadString(StringType type, Encoding encoding, ByteOrder order)
        {
            switch (type)
            {
            case StringType.ZeroDelimited:
                return ReadZDString(encoding, order);
            case StringType.Pascal8:
                return ReadPascalString(8, encoding, order);
            case StringType.Pascal16:
                return ReadPascalString(16, encoding, order);
            case StringType.Pascal32:
                return ReadPascalString(32, encoding, order);
            case StringType.Pascal64:
                return ReadPascalString(64, encoding, order);
            case StringType.SevenBit:
                return Read7BitString(encoding, order);
            default:
                throw new ArgumentException("Invalid string encoding", "type");
            }
        }

        public String Read7BitString(Encoding encoding, ByteOrder order)
        {
            String str = String.Empty;
            int length = 0;
            var b = (byte) mStream.ReadByte();
            length += b%0x80;
            while ((b & 0x80) == 0x80)
            {
                b = (byte) mStream.ReadByte();
                length += b%0x80;
            }
            for (int i = 0; i < length; i++)
                str += ReadChar(encoding, order);
            return str;
        }

        public String ReadZDString(Encoding encoding, ByteOrder order)
        {
            String str = String.Empty;
            char c = ReadChar();
            while (Convert.ToByte(c) != 0)
            {
                str += c;
                c = ReadChar();
            }
            return str;
        }

        public String ReadPascalString(int size, Encoding encoding, ByteOrder order)
        {
            string str = String.Empty;
            int charSize = encoding.GetMaxByteCount(1);
            ulong len = 0;
            switch (size)
            {
            case 8:
                len = (byte) mStream.ReadByte();
                break;
            case 16:
                len = ReadUInt16(order);
                break;
            case 32:
                len = ReadUInt32(order);
                break;
            case 64:
                len = ReadUInt64(order);
                break;
            default:
                throw new ArgumentException("Invalid bit size for string length!");
            }
            for (ulong i = 0; i < len; i++)
            {
                str += ReadChar(encoding, order);
            }
            return str;
        }

        public Char ReadChar(Encoding encoding, ByteOrder order)
        {
            int charSize = encoding is UnicodeEncoding ? 2 : 1;
            byte[] buffer = GetBytes(charSize, order);
            return encoding.GetChars(buffer)[0];
        }

        public UInt16 ReadUInt16(ByteOrder order)
        {
            return BitConverter.ToUInt16(GetBytes(2, order), 0);
        }

        public UInt32 ReadUInt32(ByteOrder order)
        {
            return BitConverter.ToUInt32(GetBytes(4, order), 0);
        }

        public UInt64 ReadUInt64(ByteOrder order)
        {
            return BitConverter.ToUInt64(GetBytes(8, order), 0);
        }

        public Int16 ReadInt16(ByteOrder order)
        {
            return BitConverter.ToInt16(GetBytes(2, order), 0);
        }

        public Int32 ReadInt32(ByteOrder order)
        {
            return BitConverter.ToInt32(GetBytes(4, order), 0);
        }

        public Int64 ReadInt64(ByteOrder order)
        {
            return BitConverter.ToInt64(GetBytes(8, order), 0);
        }

        public float ReadFloat(ByteOrder order)
        {
            return BitConverter.ToSingle(GetBytes(4, order), 0);
        }

        public double ReadDouble(ByteOrder order)
        {
            return BitConverter.ToDouble(GetBytes(8, order), 0);
        }

        public byte[] ReadBytes(int count)
        {
            return GetBytes(count, ByteOrder.LittleEndian);
        }

        private byte[] GetBytes(int count, ByteOrder order)
        {
            var bytes = new byte[count];
            switch (order)
            {
            case ByteOrder.LittleEndian:
                for (int i = 0; i < count; i++)
                    bytes[i] = (byte) mStream.ReadByte();
                break;
            case ByteOrder.BigEndian:
                for (int i = count; i > 0; i--)
                    bytes[i - 1] = (byte) mStream.ReadByte();
                break;
            default:
                throw new ArgumentException("Unsupported byte order: {0}", "order");
            }
            return bytes;
        }

        public void Write(bool input)
        {
            mStream.WriteByte((byte)(input ? 1 : 0));
        }

        public void Write(Byte input)
        {
            mStream.WriteByte(input);
        }

        public void Write(Byte[] input)
        {
            WriteBytes(input, ByteOrder.LittleEndian);
        }

        public void Write(UInt16 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(UInt32 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(UInt64 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(Int16 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(Int32 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(Int64 input)
        {
            Write(input, mByteOrder);
        }

        public void Write(float input)
        {
            Write(input, mByteOrder);
        }

        public void Write(double input)
        {
            Write(input, mByteOrder);
        }

        public void Write(UInt16 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(UInt32 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(UInt64 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(Int16 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(Int32 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(Int64 input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(float input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(double input, ByteOrder order)
        {
            WriteBytes(BitConverter.GetBytes(input), order);
        }

        public void Write(Char input)
        {
            Write(input, mCharEncoding, mByteOrder);
        }

        public void Write(Char input, Encoding encoding)
        {
            Write(input, encoding, mByteOrder);
        }

        public void Write(Char input, ByteOrder order)
        {
            Write(input, mCharEncoding, order);
        }

        public void Write(Char input, Encoding encoding, ByteOrder order)
        {
            WriteChar(input, encoding, order);
        }

        public void WriteString(String input, StringType type)
        {
            WriteString(input, type, mCharEncoding, mByteOrder);
        }

        public void WriteString(String input, StringType type, ByteOrder order)
        {
            WriteString(input, type, mCharEncoding, order);
        }

        public void WriteString(String input, StringType type, Encoding encoding)
        {
            WriteString(input, type, encoding, mByteOrder);
        }

        public void WriteString(String input, StringType type, Encoding encoding, ByteOrder order)
        {
            switch (type)
            {
            case StringType.ZeroDelimited:
                WriteZDString(input, encoding, order);
                break;
            case StringType.Pascal8:
                WritePascalString(input, 8, encoding, order);
                break;
            case StringType.Pascal16:
                WritePascalString(input, 16, encoding, order);
                break;
            case StringType.Pascal32:
                WritePascalString(input, 32, encoding, order);
                break;
            case StringType.Pascal64:
                WritePascalString(input, 64, encoding, order);
                break;
            case StringType.SevenBit:
                Write7BitString(input, encoding, order);
                break;
            default:
                throw new ArgumentException("Invalid string encoding", "type");
            }
        }

        public void Write(String input, StringType type)
        {
            Write(input, type, mCharEncoding, mByteOrder);
        }

        public void Write(String input, StringType type, Encoding encoding)
        {
            Write(input, type, encoding, mByteOrder);
        }

        public void Write(String input, StringType type, ByteOrder order)
        {
            Write(input, type, mCharEncoding, order);
        }

        public void Write(String input, StringType type, Encoding encoding, ByteOrder order)
        {
            switch (type)
            {
            case StringType.ZeroDelimited:
                WriteZDString(input, encoding, order);
                break;
            case StringType.Pascal8:
                WritePascalString(input, 8, encoding, order);
                break;
            case StringType.Pascal16:
                WritePascalString(input, 16, encoding, order);
                break;
            case StringType.Pascal32:
                WritePascalString(input, 32, encoding, order);
                break;
            case StringType.Pascal64:
                WritePascalString(input, 64, encoding, order);
                break;
            case StringType.SevenBit:
                Write7BitString(input, encoding, order);
                break;
            default:
                throw new ArgumentException("Invalid string encoding", "type");
            }
        }

        public void Write7BitString(String input, Encoding encoding, ByteOrder order)
        {
            int length = input.Length;
            while ((length & 0x80) == 0x80)
            {
                Write((byte) 0x80);
                length %= 0x80;
            }
            Write((byte) length);
            foreach (char c in input)
                WriteChar(c, encoding, order);
        }

        public void WriteZDString(String input, Encoding encoding, ByteOrder order)
        {
            if (!String.IsNullOrEmpty(input))
            {
                foreach (char c in input)
                    WriteChar(c, encoding, order);
            }
            Write((byte) 0);
        }

        public void WritePascalString(String input, int size, Encoding encoding, ByteOrder order)
        {
            int charSize = encoding.GetMaxByteCount(1);
            int len = input.Length;
            switch (size)
            {
            case 8:
                Write((byte) len);
                break;
            case 16:
                Write((UInt16) len, order);
                break;
            case 32:
                Write((UInt32) len, order);
                break;
            case 64:
                Write((UInt64) len, order);
                break;
            default:
                throw new ArgumentException("Invalid bit size for string length!");
            }
            foreach (char c in input)
                WriteChar(c, encoding, order);
        }

        private void WriteChar(Char c, Encoding encoding, ByteOrder order)
        {
            int charSize = encoding is UnicodeEncoding ? 2 : 1;
            byte[] buffer = encoding.GetBytes(new[] {c});
            Write(buffer);
        }

        private byte[] WriteBytes(byte[] bytes, ByteOrder order)
        {
            switch (order)
            {
            case ByteOrder.LittleEndian:
                for (int i = 0; i < bytes.Length; i++)
                    mStream.WriteByte(bytes[i]);
                break;
            case ByteOrder.BigEndian:
                for (int i = bytes.Length; i > 0; i--)
                    mStream.WriteByte(bytes[i - 1]);
                break;
            default:
                throw new ArgumentException("Unsupported byte order: {0}", "order");
            }
            return bytes;
        }
    }
}
