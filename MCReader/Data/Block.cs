using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public struct Block
    {
        public string display_name;
        public int numeric_id;
        public string identifier;

        public Block(string display_name, int numeric_id, string identifier)
        {
            this.display_name = display_name;
            this.numeric_id = numeric_id;
            this.identifier = identifier;
        }
    }
}
