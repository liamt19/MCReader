using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public struct Entity
    {
        //  Generally the same as "name", but capitalized
        public string display_name;
        public int id;
        public string name;

        public Entity(string display_name, int id, string name)
        {
            this.display_name = display_name;
            this.id = id;
            this.name = name;
        }
    }
}
