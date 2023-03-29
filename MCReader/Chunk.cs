using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MCReader.Data;
using MCReader.NBT;

namespace MCReader
{
    public class Chunk
    {
        public static int chestsSearched = 0;
        public static int blockEntitiesNoItemTag = 0;

        public List<INBTTag> NBT;

        public int numBlockEntities = 0;

        public int location;
        public int sectorCount;
        public int timestamp;

        public int coordX;
        public int coordZ;

        public Chunk() 
        {
            NBT = new List<INBTTag>();
        }

        public bool FindInChests(Item item, out List<(TAG_Compound chestNBT, int numInChest)> results)
        {
            results = new List<(TAG_Compound, int)>();

            if (numBlockEntities > 0)
            {
                //  Yikes
                List<INBTTag> blockEntities = (List<INBTTag>)((List<INBTTag>)(NBT[0].Data()))[2].Data();

                foreach (TAG_Compound chestTag in blockEntities.Cast<TAG_Compound>())
                {
                    chestsSearched++;

                    List<INBTTag> chestTags = ((List<INBTTag>)chestTag.Data());
                    INBTTag itemTag = null;
                    for (int i = 0; i < chestTags.Count; i++)
                    {
                        if (chestTags[i].Name() == "Items")
                        {
                            itemTag = chestTags[i];
                        }
                    }

                    if (itemTag == null)
                    {
                        blockEntitiesNoItemTag++;
                        continue;
                    }

                    int countInThisChest = 0;

                    List<INBTTag> itemsListTags = (List<INBTTag>)itemTag.Data();
                    foreach (TAG_Compound chestItem in itemsListTags.Cast<TAG_Compound>())
                    {
                        //Log("Item in chest: " + chestItem.ToString());
                        var itemNBT = (List<INBTTag>)chestItem.Data();

                        if (itemNBT[1].Data().GetType() != typeof(string))
                        {
                            Log("itemNBT[1] was " + itemNBT[1].Data().GetType().ToString() + " instead of string!: " + itemNBT.ToString());
                            continue;
                        }

                        string itemName = (string) (itemNBT[1].Data());
                        sbyte itemCount = (sbyte) (itemNBT[2].Data());

                        //  We found it if there is an exact match, or if we find an item that should be a match but has a mod's prefix on it.
                        //  I.E. searching for "apple" should match "minecraft:apple" too.
                        if (itemName == item.identifier || itemName.EndsWith(":" + item.identifier))
                        {
                            countInThisChest += itemCount;
                        }

                        /**
                        foreach (INBTTag itemNBTTag in (List<INBTTag>)chestItem.Data())
                        {
                            break;
                            if (itemNBTTag.Name() == "id") 
                            {
                            }
                        }
                         */
                    }

                    if (countInThisChest > 0)
                    {
                        results.Add((chestTag, countInThisChest));
                    }
                }
            }
            
            return results.Count != 0;
        }

        public void Uncompress(byte[] rawData)
        {
            Stream nbtStream = UnzipArray(rawData);
            NBTReader r = new NBTReader(nbtStream);
            NBT = r.ReadAll();

            try
            {
                bool hasCoordinateTags = false;
                List<INBTTag> data = (List<INBTTag>)NBT[0].Data();
                foreach (INBTTag tag in data)
                {
                    if (tag.Name() == "xPos")
                    {
                        coordX = (int)tag.Data();
                        hasCoordinateTags = true;
                    }
                    else if (tag.Name() == "zPos")
                    {
                        coordZ = (int) tag.Data();
                        hasCoordinateTags = true;
                    }
                    else if (tag.Name() == "block_entities")
                    {
                        numBlockEntities = ((TAG_List)tag).PayloadSize();
                    }
                }

                if (coordX == 0 && coordZ == 0 && !hasCoordinateTags)
                {
                    object posTag = ((TAG_Compound)NBT[0]).GetChildData("Position");
                    if (posTag != null)
                    {
                        TAG_Int[] intArr = ((TAG_Int[])posTag);

                        coordX = (int) (intArr[0].Data());
                        coordZ = (int) (intArr[1].Data());
                    }
                    else
                    {
                        Log("Couldn't fix coords for chunk " + this.ToString());
                    }
                }

                if (numBlockEntities != 0)
                {
                    //Log("Chunk at " + coordX + ", " + coordZ + " has " + numBlockEntities + " block entities");
                }
            }
            catch {
                Log("Failed decoding X/Z for chunk -> " + ToString());
            }
        }

        public string RawDataToString()
        {
            string s = DateTime.UnixEpoch.AddSeconds(timestamp).ToShortDateString();
            return sectorCount + " sectors at offset 0x" + (location * 4096).ToString("X") + " timestamp " + s;
        }

        public override string ToString()
        {
            return "Chunk at [" + coordX + ", " + coordZ + "]";
        }
    }
}
