using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCReader.NBT.Tags
{
    public class TAG_End : INBTTag
    {
        public int PayloadSize() => 0;
        private object _data = "NONE";
        private string _name = "END";

        public object Data()
        {
            return _data;
        }

        public string Name()
        {
            return _name;
        }

        public void Read(NBTReader r, bool noName)
        {
            

        }

        public TAG_End(object data = null)
        {

        }

        public override string ToString()
        {
            return _name + ": " + _data.ToString();
        }
    }
}
