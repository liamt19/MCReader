
global using static MCReader.Utilities;
global using MCReader.NBT;
global using MCReader.NBT.Tags;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace MCReader.NBT
{
    public class NBTReader
    {
        private BinaryReader br;
        public NBTReader(Stream input)
        {
            br = new BinaryReader(input);
        }

        [MethodImpl(Inline)]
        public List<INBTTag> ReadAll()
        {
            List<INBTTag> list = new List<INBTTag>();

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                INBTTag tag = ReadNext();
                list.Add(tag);
            }

            return list;
        }

        [MethodImpl(Inline)]
        public INBTTag ReadNext()
        {
            try
            {
                byte id = br.ReadByte();
                INBTTag tag = INBTTag.Create(id);
                if (id == (int)TagType.TAG_Compound && br.BaseStream.Position == 1)
                {
                    ((TAG_Compound)tag).MakeRoot();
                }
                tag.Read(this);

                return tag;
            }
            catch (Exception e)
            {
                if (e is EndOfStreamException)
                {
                    Log("NBTReader's stream ended, ReadNext is returning INBTTag.END");
                }
                else
                {
                    Log("ReadNext() failed -> " + e.ToString());
                }
            }
            return INBTTag.END;
        }

        [MethodImpl(Inline)]
        public byte ReadByte() => br.ReadByte();

        [MethodImpl(Inline)]
        public sbyte ReadSByte() => br.ReadSByte();

        [MethodImpl(Inline)]
        public ushort ReadUShort()
        {
            byte[] arr = br.ReadBytes(2);
            Array.Reverse(arr);
            return BitConverter.ToUInt16(arr);
        }

        [MethodImpl(Inline)]
        public short ReadShort()
        {
            byte[] arr = br.ReadBytes(2);
            Array.Reverse(arr);
            return BitConverter.ToInt16(arr);
        }

        [MethodImpl(Inline)]
        public int ReadInt()
        {
            byte[] arr = br.ReadBytes(4);
            Array.Reverse(arr);
            return BitConverter.ToInt32(arr);
        }

        [MethodImpl(Inline)]
        public long ReadLong()
        {
            byte[] arr = br.ReadBytes(8);
            Array.Reverse(arr);
            return BitConverter.ToInt64(arr);
        }

        [MethodImpl(Inline)]
        public float ReadFloat()
        {
            byte[] arr = br.ReadBytes(4);
            Array.Reverse(arr);
            return BitConverter.ToSingle(arr);
        }

        [MethodImpl(Inline)]
        public double ReadDouble()
        {
            byte[] arr = br.ReadBytes(8);
            Array.Reverse(arr);
            return BitConverter.ToDouble(arr);
        }

        [MethodImpl(Inline)]
        public byte[] ReadByteArray()
        {
            int size = ReadInt();
            byte[] arr = br.ReadBytes(size);
            return arr;
        }

        [MethodImpl(Inline)]
        public string ReadString()
        {
            ushort length = ReadUShort();
            byte[] arr = br.ReadBytes(length);
            string utf8 = Encoding.UTF8.GetString(arr, 0, arr.Length);
            return utf8;
        }

        [MethodImpl(Inline)]
        public List<INBTTag> ReadList()
        {
            byte id = ReadByte();
            int payload = INBTTag.Create(id).PayloadSize();
            int size = ReadInt();
            if (size == 0)
            {
                //Log("In ReadList for id " + id + " and payload " + payload + ", size is 0?");
            }
            List<INBTTag> list = new List<INBTTag>(size);
            for (int i = 0; i < size; i++)
            {
                INBTTag tag = INBTTag.Create(id);
                tag.Read(this, true);
                list.Add(tag);
            }

            return list;
        }

        [MethodImpl(Inline)]
        public List<INBTTag> ReadCompound()
        {
            List<INBTTag> list = new List<INBTTag>();

            byte id = ReadByte();
            while (id != (int)TagType.TAG_End)
            {
                INBTTag tag = INBTTag.Create(id);
                tag.Read(this);
                list.Add(tag);
                id = ReadByte();
            }

            return list;
        }

        [MethodImpl(Inline)]
        public TAG_Int[] ReadIntArray()
        {
            int size = ReadInt();
            TAG_Int[] arr = new TAG_Int[size];
            for (int i = 0; i < size; i++)
            {
                int data = ReadInt();
                TAG_Int tag = new TAG_Int(data);
                arr[i] = tag;
            }

            return arr;
        }

        [MethodImpl(Inline)]
        public TAG_Long[] ReadLongArray()
        {
            int size = ReadInt();
            TAG_Long[] arr = new TAG_Long[size];
            for (int i = 0; i < size; i++)
            {
                long data = ReadLong();
                TAG_Long tag = new TAG_Long(data);
                arr[i] = tag;
            }

            return arr;
        }

        
    }
}
