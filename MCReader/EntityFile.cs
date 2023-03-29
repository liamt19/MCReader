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
        public EntityFile(Stream input)
        {
            br = new BinaryReader(input);
        }

        public List<Chunk> ReadChunks()
        {
            List<Chunk> allChunks = new List<Chunk>();
            for (int i = 0; i < 1024; i++)
            {
                allChunks.Add(new Chunk());
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
                c.Uncompress(buff);

                //Log("Read chunk " + i);
            }

            return generated;
        }


        public static List<Villager> GetVillagers(List<Chunk> list)
        {
            List<Villager> villagers = new List<Villager>();

            foreach (Chunk c in list)
            {
                TAG_Compound root = (TAG_Compound)(c.NBT[0]);
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
    }
}
