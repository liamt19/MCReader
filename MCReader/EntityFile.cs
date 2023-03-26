using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader
{
    public class EntityFile
    {
        private BinaryReader br;
        public EntityFile(Stream input)
        {
            br = new BinaryReader(input);
        }

        public void ReadAll()
        {
            List<int> locations = new List<int>();
            for (int i = 0; i < 1024; i++)
            {
                int t = br.ReadInt32();
                if (t != 0)
                {
                    locations.Add(t);
                }
            }

            List<int> timestamps = new List<int>();
            for (int i = 0; i < 1024; i++)
            {
                int t = br.ReadInt32();
                if (t != 0)
                {
                    timestamps.Add(t);
                }
            }

            Log("Got " + locations.Count + " locations and " + timestamps.Count + " timestamps");

            List<Chunk> chunks = new List<Chunk>(locations.Count);
            for (int i = 0; i < locations.Count; i++) 
            {
                br.BaseStream.Seek((i + 2) * 4096, SeekOrigin.Begin);

                //  little endian -> big
                int size = (br.ReadByte() << 24 | br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte());
                
                byte compression = br.ReadByte();

                Chunk c = new Chunk();
                byte[] buff = new byte[size];
                br.Read(buff, 0, size);
                c.data.AddRange(buff);
                c.Uncompress();

                Log("Read chunk " + i);
            }

            
            Log("at 0x" + br.BaseStream.Position.ToString("X"));
            Log("done");
        }

    }
}
