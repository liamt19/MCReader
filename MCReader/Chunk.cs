using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MCReader.Data;
using MCReader.NBT;

namespace MCReader
{
    public class Chunk
    {
        public static int chestsSearched = 0;
        public static int blockEntitiesNoItemTag = 0;

        public List<INBTTag> RegionNBT;
        public List<INBTTag> EntityNBT;

        public bool IsPre_1_17 = false;

        public int numBlockEntities = 0;

        public int location;
        public int sectorCount;
        public int timestamp;

        public int coordX;
        public int coordZ;
        public int chunkX() => (int)Math.Floor(coordX / 32.0);
        public int chunkZ() => (int)Math.Floor(coordZ / 32.0);

        public string folderPath;
        public bool isRegion;

        public Chunk(bool isRegion, string folderPath) 
        {
            RegionNBT = new List<INBTTag>();
            EntityNBT = new List<INBTTag>();

            this.isRegion = isRegion;
            this.folderPath = folderPath;
        }

        public bool FindInChests(string item, out List<(TAG_Compound chestNBT, int numInChest)> results)
        {
            results = new List<(TAG_Compound, int)>();

            if (numBlockEntities > 0)
            {
                List<INBTTag> blockEntities;
                try
                {
                    //  Yikes
                    if (IsPre_1_17)
                    {
                        //  Pre 1.17 has a "Level" tag under the root, so this needs to be indexed at 0 again.
                        //  root -> Level -> TileEntities.Data() is a list of TAG_Compounds.
                        List<INBTTag> rootTags = (List<INBTTag>) RegionNBT[0].Data();
                        List<INBTTag> levelTag = (List<INBTTag>) rootTags[0].Data();
                        for (int i = 0; i < rootTags.Count; i++)
                        {
                            if (rootTags[i].Name() == "Level")
                            {
                                levelTag = (List<INBTTag>) rootTags[i].Data();
                                break;
                            }
                        }
                        
                        blockEntities = (List<INBTTag>) levelTag[10].Data();
                    }
                    else
                    {
                        blockEntities = (List<INBTTag>)((List<INBTTag>)(RegionNBT[0].Data()))[2].Data();
                    }
                }
                catch(Exception ex)
                {
                    return false;
                }

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

                    bool skipEntity = false;
                    foreach (INBTTag t in itemsListTags)
                    {
                        if (t is not TAG_Compound)
                        {
                            skipEntity = true;
                        }
                    }

                    if (skipEntity)
                    {
                        Log("Skipping " + chestTag.ToString() + " because it has children tag(s) that aren't compounds.");
                        continue;
                    }

                    foreach (TAG_Compound chestItem in itemsListTags.Cast<TAG_Compound>())
                    {
                        //Log("Item in chest: " + chestItem.ToString());
                        var itemNBT = (List<INBTTag>)chestItem.Data();

                        if (itemNBT[1].Data().GetType() != typeof(string))
                        {
                            if (itemNBT[1].Data().GetType() == typeof(List<INBTTag>))
                            {
                                Log("itemNBT[1] was " + itemNBT[1].Data().GetType().ToString() + " instead of string!: <<<");
                                foreach (INBTTag t in (List<INBTTag>)itemNBT[1].Data())
                                {
                                    Log("\t" + t.ToString());
                                }
                                Log(">>>");
                            }
                            else
                            {
                                Log("itemNBT[1] was " + itemNBT[1].Data().GetType().ToString() + " instead of string!: " + itemNBT.ToString());
                            }
                            continue;
                        }

                        string itemName = (string)(itemNBT[1].Data());
                        sbyte itemCount = (sbyte)(itemNBT[2].Data());

                        //  We found it if there is an exact match, or if we find an item that should be a match but has a mod's prefix on it.
                        //  I.E. searching for "apple" should match "minecraft:apple" too.
                        if (itemName == item || itemName.EndsWith(":" + item))
                        {
                            countInThisChest += itemCount;
                        }
                    }

                    if (countInThisChest > 0)
                    {
                        results.Add((chestTag, countInThisChest));
                    }
                }
            }
            
            return results.Count != 0;
        }

        public bool FindInEntities(string it, out List<(TAG_Compound entityNBT, int numInEntity)> results)
        {
            results = new List<(TAG_Compound, int)>();

            TAG_Compound root = (TAG_Compound)(RegionNBT[0]);
            object entityTag = root.GetChildData("Entities");
            if (entityTag != null)
            {
                var entityList = ((List<INBTTag>)entityTag).Cast<TAG_Compound>();
                foreach (TAG_Compound entity in entityList)
                {
                    int countInThisEntity = 0;
                    string entityId = (string)entity.GetChildData("id");
                    if (entity.GetChildData("Items") != null)
                    {
                        var itemList = ((List<INBTTag>)entity.GetChildData("Items")).Cast<TAG_Compound>();
                        //Log("Entity in chunk: " + entityId);
                        foreach (TAG_Compound item in itemList)
                        {
                            //Log("Item in entity: " + entityId);

                            string itemName = (string)item.GetChildData("id");
                            sbyte itemCount = (sbyte)item.GetChildData("Count");

                            //  We found it if there is an exact match, or if we find an item that should be a match but has a mod's prefix on it.
                            //  I.E. searching for "apple" should match "minecraft:apple" too.
                            if (itemName == it || itemName.EndsWith(":" + it))
                            {
                                countInThisEntity += itemCount;
                            }
                        }
                    }
                    if (countInThisEntity > 0)
                    {
                        results.Add((entity, countInThisEntity));
                    }
                }
            }

            return results.Count != 0;
        }

        public bool UncompressRegion(byte[] rawData)
        {
            Stream nbtStream = UnzipArray(rawData);
            NBTReader r = new NBTReader(nbtStream);
            RegionNBT = r.ReadAll();

            try
            {
                bool hasCoordinateTags = false;
                List<INBTTag> data = (List<INBTTag>)RegionNBT[0].Data();
                Fixed_Pre_1_17:
                foreach (INBTTag tag in data.ToArray())
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
                    else if (numBlockEntities == 0 && IsPre_1_17 && tag.Name() == "TileEntities")
                    {
                        numBlockEntities = ((TAG_List)tag).PayloadSize();
                    }
                    else if (tag.Name() == "Level")
                    {
                        IsPre_1_17 = true;
                        data = ((List<INBTTag>) tag.Data());
                        goto Fixed_Pre_1_17;
                    }
                }

                if (coordX == 0 && coordZ == 0 && !hasCoordinateTags)
                {
                    object posTag = ((TAG_Compound)RegionNBT[0]).GetChildData("Position");
                    if (posTag != null)
                    {
                        TAG_Int[] intArr = ((TAG_Int[])posTag);

                        coordX = (int)(intArr[0].Data());
                        coordZ = (int)(intArr[1].Data());
                    }
                    else
                    {
                        Log("Couldn't fix coords for chunk " + this.ToString());
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Log("Failed decoding X/Z for chunk -> " + ToString());
                return false;
            }

            return true;
        }

        public bool UncompressEntities(byte[] rawData)
        {
            Stream nbtStream = UnzipArray(rawData);
            NBTReader r = new NBTReader(nbtStream);
            EntityNBT = r.ReadAll();

            try
            {
                object posTag = ((TAG_Compound)EntityNBT[0]).GetChildData("Position");
                if (posTag != null)
                {
                    TAG_Int[] intArr = ((TAG_Int[])posTag);

                    coordX = (int)(intArr[0].Data());
                    coordZ = (int)(intArr[1].Data());
                }
                else if (((TAG_Compound)EntityNBT[0]).Name() == "Level")
                {
                    IsPre_1_17 = true;
                    //coordX = 
                }
                else
                {
                    Log("Couldn't fix coords for chunk " + this.ToString());
                    return false;
                }
            }
            catch (Exception e)
            {
                Log("Failed decoding X/Z for chunk -> " + ToString());
                return false;
            }

            return true;
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
