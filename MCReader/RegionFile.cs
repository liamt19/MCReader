using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader
{
    public class RegionFile
    {
        private BinaryReader br;
        public string folderPath;

        public RegionFile(Stream input, string folderPath)
        {
            br = new BinaryReader(input);
            this.folderPath = folderPath;
        }

        public List<Chunk> ReadChunks()
        {
            List<Chunk> allChunks = new List<Chunk>();
            for (int i = 0; i < 1024; i++)
            {
                allChunks.Add(new Chunk(true, this.folderPath));
            }

            if (br.BaseStream.Length == 0)
            {
                Log("BinaryReader's base stream had a length of 0! Is file size 0?");
                return allChunks;
            }

            int generatedCount = 0;
            for (int i = 0; i < 1024; i++)
            {
                int t = (br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte());
                allChunks[i].location = t;
                allChunks[i].sectorCount = br.ReadByte();
                if (t != 0)
                {
                    generatedCount++;
                }
            }

            for (int i = 0; i < 1024; i++)
            {
                int t = (br.ReadByte() << 24 | br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte());
                allChunks[i].timestamp = t;
            }

            Log("Got " + generatedCount + " locations and timestamps");

            List<Chunk> generated = new List<Chunk>();
            foreach (var c in allChunks.OrderBy(o => o.location))
            {
                if (c.location != 0)
                {
                    generated.Add(c);
                }
            }

            for (int i = 0; i < generated.Count; i++)
            {
                Chunk c = generated[i];
                br.BaseStream.Seek(c.location * 4096, SeekOrigin.Begin);
                //Log("Seeking to 0x" + br.BaseStream.Position.ToString("X") + " for " + i);

                //  little endian -> big
                int size = (br.ReadByte() << 24 | br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte());

                byte compression = br.ReadByte();

                //size = 8092 - 5;

                byte[] buff = new byte[size];
                br.Read(buff, 0, size);
                if (!c.UncompressRegion(buff))
                {
                    Log("RegionFile.ReadChunks was called using entity files instead of region files!");
                    break;
                }

                //Log("Read chunk " + i);
            }

            return generated;
        }

        public static List<INBTTag> GetAllChests(List<Chunk> list, bool ignoreItems = true)
        {
            List<INBTTag> chests = new List<INBTTag>();
            foreach (Chunk c in list)
            {
                if (c.RegionNBT.Count == 0)
                {
                    continue;
                }

                TAG_Compound root = (TAG_Compound)(c.RegionNBT[0]);

                object entityTag;
                if (c.IsPre_1_17)
                {
                    root = (TAG_Compound)((List<INBTTag>)root.Data())[0];
                    entityTag = root.GetChildData("TileEntities");
                }
                else
                {
                    entityTag = root.GetChildData("Entities");
                }

                if (entityTag != null)
                {
                    var entityTagList = (List<INBTTag>) entityTag;
                    foreach (INBTTag entity in entityTagList)
                    {
                        chests.Add(entity);
                    }
                }
            }

            return chests;
        }

    }
}
