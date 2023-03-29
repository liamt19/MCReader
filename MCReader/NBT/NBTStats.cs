using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.NBT
{
    public struct NBTStats
    {
        public int numEnds;
        public int numBytes;
        public int numShorts;
        public int numInts;
        public int numLongs;
        public int numFloats;
        public int numDoubles;
        public int numByteArrays;
        public int numStrings;
        public int numLists;
        public int numCompounds;
        public int numIntArrays;
        public int numLongArrays;

        public int numCharsInStrings;
        public int numByteArraysBytes;
        public int numIntArraysInts;
        public int numLongArraysLongs;

    }
}
