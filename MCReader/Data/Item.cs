using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public struct Item
    {
        public string display_name { get; }
        public int numeric_id { get; }
        public string identifier { get; }

        public Item(string display_name, int numeric_id, string identifier)
        {
            this.display_name = display_name;
            this.numeric_id = numeric_id;
            this.identifier = identifier;
        }
    }
}
