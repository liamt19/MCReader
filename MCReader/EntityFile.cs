using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MCReader.Data;

namespace MCReader
{
    public class EntityFile
    {
        private BinaryReader br;
        public string folderPath;

        public EntityFile(Stream input, string folderPath)
        {
            br = new BinaryReader(input);
            this.folderPath = folderPath;
        }

        public bool ReadChunks(out List<Chunk> generated)
        {
            List<Chunk> allChunks = new List<Chunk>();
            generated = new List<Chunk>();
            for (int i = 0; i < 1024; i++)
            {
                allChunks.Add(new Chunk(false, this.folderPath));
            }

            if (br.BaseStream.Length == 0)
            {
                Log("BinaryReader's base stream had a length of 0! Is file size 0?");
                return false;
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

                byte[] buff = new byte[size];
                br.Read(buff, 0, size);
                if (!c.UncompressEntities(buff))
                {
                    Log("EntityFile.ReadChunks was called using region files instead of entity files!");
                    return false;
                    break;
                }

                //Log("Read chunk " + i);
            }

            return true;
        }


        public static List<Villager> GetVillagers(List<Chunk> list)
        {
            List<Villager> villagers = new List<Villager>();

            foreach (Chunk c in list)
            {
                if (c.RegionNBT.Count == 0)
                {
                    continue;
                }

                TAG_Compound root = (TAG_Compound)(c.RegionNBT[0]);

                if (c.IsPre_1_17)
                {
                    root = (TAG_Compound) ((List<INBTTag>) root.Data())[0];
                }

                object entityTag = root.GetChildData("Entities");
                if (entityTag != null)
                {
                    var entityList = ((List<INBTTag>)entityTag).Cast<TAG_Compound>();
                    foreach (TAG_Compound entity in entityList)
                    {
                        string entityId = (string) entity.GetChildData("id");
                        if (StringsEqual(entityId, "minecraft:villager"))
                        {
                            Villager v = new Villager(entity);
                            villagers.Add(v);
                        }
                    }
                }
            }

            return villagers;
        }

        public static List<TAG_Compound> GetAllEntities(List<Chunk> list, bool ignoreItems = true)
        {
            List<TAG_Compound> entities = new List<TAG_Compound>();
            foreach (Chunk c in list)
            {
                if (c.RegionNBT.Count == 0)
                {
                    continue;
                }

                TAG_Compound root = (TAG_Compound)(c.RegionNBT[0]);

                if (c.IsPre_1_17)
                {
                    root = (TAG_Compound)((List<INBTTag>)root.Data())[0];
                }

                object entityTag = root.GetChildData("Entities");
                if (entityTag != null)
                {
                    var entityList = ((List<INBTTag>)entityTag).Cast<TAG_Compound>();
                    foreach (TAG_Compound entity in entityList)
                    {
                        if (!(ignoreItems && StringsEqual((string)entity.GetChildData("id"), "minecraft:item")))
                        {
                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

    }
}
