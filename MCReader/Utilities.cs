using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO.Compression;
using MCReader.NBT.Tags;

namespace MCReader
{
    public static class Utilities
    {

        public static void Log(string s)
        {
            Console.WriteLine(s);
            Debug.WriteLine(s);
        }

        public static void Log(object s)
        {
            Console.WriteLine(s.ToString());
            Debug.WriteLine(s.ToString());
        }



        //  https://stackoverflow.com/questions/14401270/efficient-way-to-read-big-endian-data-in-c-sharp
        public static int ToInt32BigEndian(byte[] buf, int i)
        {
            return (buf[i] << 24) | (buf[i + 1] << 16) | (buf[i + 2] << 8) | buf[i + 3];
        }


        public static Stream OpenFile(string file)
        {
            Stream input;

            if (IsGZipped(file))
            {
                Log("File " + file + "' is GZipped");
                input = UnzipFile(file);
            }
            else
            {
                Log("File " + file + "' is plaintext");
                input = File.OpenRead(file);
            }

            return input;
        }

        public static Stream UnzipFile(string file)
        {
            using FileStream compressedFileStream = File.Open(file, FileMode.Open);
            using GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            MemoryStream memoryStream = new MemoryStream();
            decompressor.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public static Stream UnzipArray(byte[] arr)
        {
            using MemoryStream memoryStream = new MemoryStream(arr);
            using ZLibStream decompressor = new ZLibStream(memoryStream, CompressionMode.Decompress);
            MemoryStream unzippedStream = new MemoryStream();
            decompressor.CopyTo(unzippedStream);
            unzippedStream.Position = 0;
            return unzippedStream;
        }

        public static bool IsGZipped(string filePath)
        {
            byte[] magic = { 0x1F, 0x8B };
            byte DEFLATE = 0x08;
            bool IsDeflate = false;

            try
            {
                using FileStream fileStream = File.Open(filePath, FileMode.Open);
                if (fileStream.ReadByte() == magic[0] && fileStream.ReadByte() == magic[1])
                {
                    IsDeflate = (fileStream.ReadByte() == DEFLATE);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            return false;
        }

        public static void PrintStats(this List<INBTTag> list)
        {
            NBTStats stats = new NBTStats();
            PrintStats_rec(list, ref stats);
            Log("Ends: " + stats.numEnds);
            Log("Bytes: " + stats.numBytes);
            Log("Shorts: " + stats.numShorts);
            Log("Longs: " + stats.numLongs);
            Log("Floats: " + stats.numFloats);
            Log("Doubles: " + stats.numDoubles);
            Log(stats.numByteArraysBytes + " bytes in " + stats.numByteArrays + " byte arrays");
            Log(stats.numCharsInStrings + " characters in " + stats.numStrings + " strings");
            Log("Lists: " + stats.numLists);
            Log("Compounds: " + stats.numCompounds);
            Log(stats.numIntArraysInts + " ints in " + stats.numIntArrays + " int arrays");
            Log(stats.numLongArraysLongs + " longs in " + stats.numLongArrays + " long arrays");
        }

        private static void PrintStats_rec(List<INBTTag> list, ref NBTStats stats)
        {
            foreach (INBTTag tag in list)
            {
                if (tag.TagID() == (int)TagType.TAG_End)
                {
                    stats.numEnds++;
                }
                if (tag.TagID() == (int)TagType.TAG_Byte)
                {
                    stats.numBytes++;
                }
                if (tag.TagID() == (int)TagType.TAG_Short)
                {
                    stats.numShorts++;
                }
                if (tag.TagID() == (int)TagType.TAG_Int)
                {
                    stats.numInts++;
                }
                if (tag.TagID() == (int)TagType.TAG_Long)
                {
                    stats.numLongs++;
                }
                if (tag.TagID() == (int)TagType.TAG_Float)
                {
                    stats.numFloats++;
                }
                if (tag.TagID() == (int)TagType.TAG_Double)
                {
                    stats.numDoubles++;
                }
                if (tag.TagID() == (int)TagType.TAG_Byte_Array)
                {
                    stats.numByteArrays++;
                    stats.numByteArraysBytes += ((TAG_Byte[])tag.Data()).Length;
                }
                if (tag.TagID() == (int)TagType.TAG_String)
                {
                    stats.numStrings++;
                    stats.numCharsInStrings += ((string)tag.Data()).Length;
                }
                if (tag.TagID() == (int)TagType.TAG_List)
                {
                    stats.numLists++;
                    var children = ((List<INBTTag>)tag.Data());
                    PrintStats_rec(children, ref stats);
                }
                if (tag.TagID() == (int)TagType.TAG_Compound)
                {
                    stats.numCompounds++;
                    var children = ((List<INBTTag>)tag.Data());
                    PrintStats_rec(children, ref stats);
                }
                if (tag.TagID() == (int)TagType.TAG_Long_Array)
                {
                    stats.numLongArrays++;
                    stats.numLongArraysLongs += ((TAG_Long[])tag.Data()).Length;
                }
            }
        }


        public static string Stringify(this List<INBTTag> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToString() + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append("]");
            return sb.ToString();
        }


        public static string ToTree(this string s)
        {
            StringBuilder sb = new StringBuilder();
            int tabs = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ']')
                {
                    tabs = (tabs == 0) ? 0 : tabs - 1;
                    sb.AppendLine();
                    sb.Append('\t', tabs);
                }
                sb.Append(s[i]);
                if (s[i] == '[')
                {
                    tabs++;
                    sb.AppendLine();
                    sb.Append('\t', tabs);
                }
            }

            return sb.ToString();
        }
    }
}
