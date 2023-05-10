using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public struct Enchantment
    {
        public string internalName;
        public string displayName;
        public int level;
        public int maxLevel;
        public bool isTreasure;
        public int id;

        public Enchantment(string internalName, string displayName, int level, int maxLevel, bool isTreasure, int id)
        {
            this.internalName = internalName;
            this.displayName = displayName;
            this.level = level;
            this.maxLevel = maxLevel;
            this.isTreasure = isTreasure;
            this.id = id;
        }
    }
}
