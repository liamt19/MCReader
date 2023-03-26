using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MCReader.NBT;

namespace MCReader
{
    public class Chunk
    {
        public List<byte> data;
        public List<INBTTag> NBT;

        public int location;
        public int sectorCount;
        public int timestamp;

        public int coordX;
        public int coordZ;

        public Chunk() 
        {
            data = new List<byte>();
            NBT = new List<INBTTag>();
        }

        public void Uncompress()
        {
            Stream nbtStream = UnzipArray(data.ToArray());
            NBTReader r = new NBTReader(nbtStream);
            NBT = r.ReadAll();

            try
            {
                List<INBTTag> data = (List<INBTTag>)NBT[0].Data();
                foreach (INBTTag tag in data)
                {
                    if (tag.Name() == "xPos")
                    {
                        coordX = (int)tag.Data();
                    }
                    else if (tag.Name() == "zPos")
                    {
                        coordZ = (int) tag.Data();
                    }
                }
            }
            catch {
                Log("Failed decoding X/Z for chunk -> " + ToString());
            }
        }

        public override string ToString()
        {
            string s = DateTime.UnixEpoch.AddSeconds(timestamp).ToShortDateString();
            return sectorCount + " sectors at offset 0x" + (location * 4096).ToString("X") + " timestamp " + s;
        }
    }
}
