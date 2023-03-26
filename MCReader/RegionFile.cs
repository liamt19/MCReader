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

        public RegionFile(Stream input)
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
                c.data.AddRange(buff);
                c.Uncompress();

                //Log("Read chunk " + i);
            }

            Log("done");
            return generated;
        }

    }
}
