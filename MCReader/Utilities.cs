using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO.Compression;

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
