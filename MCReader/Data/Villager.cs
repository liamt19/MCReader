using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public class Villager
    {
        //  Villagers that haven't been talked to yet might have a level of 0.  
        private static string[] LevelStrings = new string[] { "Unmet", "Novice", "Apprentice", "Journeyman", "Expert", "Master" };

        public string profession;
        public string CustomName;
        public int level;

        public double coordX;
        public double coordY;
        public double coordZ;

        public bool hasTrades;

        public List<TAG_Compound> Offers = new List<TAG_Compound>();
        private TAG_Compound _tag;

        public TAG_Compound NBT_tag => _tag;


        public Villager(TAG_Compound tag) 
        {
            CustomName = (string) tag.GetChildData("CustomName");

            var entityCoords = ((List<INBTTag>) tag.GetChildData("Pos")).Cast<TAG_Double>().ToList();
            coordX = (double) entityCoords[0].Data();
            coordY = (double) entityCoords[1].Data();
            coordZ = (double) entityCoords[2].Data();

            TAG_Compound VillagerData = (TAG_Compound)tag.GetChildTag("VillagerData");
            if (VillagerData != null)
            {
                profession = (string)VillagerData.GetChildData("profession");
                level = (int) VillagerData.GetChildData("level");
            }
            else
            {
                //  For Forge 1.12.2
                profession = (string)tag.GetChildData("ProfessionName");
                level = (int)tag.GetChildData("CareerLevel");

                //  Cartographers are distinguished from librarians by their 'Career' tag being 2, instead of 1
                if (profession == "minecraft:librarian" && tag.GetChildData("Career") != null && ((int)tag.GetChildData("Career") == 2))
                {
                    profession = "minecraft:cartographer";
                }
            }

            var offers = tag.GetChildData("Offers");
            if (offers != null)
            {
                List<INBTTag> tradesTag = ((List<INBTTag>)((List<INBTTag>)offers)[0].Data());
                Offers = tradesTag.Cast<TAG_Compound>().ToList();
                hasTrades = true;
            }

            _tag = tag;
        }

        public static string GetItemEnchantments(TAG_Compound itTag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(Enchantments: ");

            //  For enchantment book enchantments.
            object enchants = itTag.GetChildData("StoredEnchantments");

            if (enchants == null)
            {
                //  For Forge 1.12.2
                enchants = itTag.GetChildData("ench");
            }

            if (enchants != null)
            {
                List<INBTTag> enchantments = (List<INBTTag>)enchants;
                foreach (TAG_Compound bookEnchant in enchantments)
                {
                    object en = bookEnchant.GetChildData("id");
                    short enchId = -1;
                    if (en.GetType() == typeof(short))
                    {
                        enchId = (short)en;
                    }
                    var matchingEnchants = DataLists.Enchantments.Where(x => x.id == enchId).ToArray();
                    if (matchingEnchants.Length > 0)
                    {
                        sb.Append(matchingEnchants[0].internalName + " " + bookEnchant.GetChildData("lvl") + ", ");
                    }
                    else
                    {
                        sb.Append(enchId + " " + bookEnchant.GetChildData("lvl") + ", ");
                    }
                }
                sb.Remove(sb.Length - 2, 2);
            }
            else
            {
                //  For regular items.
                enchants = itTag.GetChildData("Enchantments");
                if (enchants != null)
                {
                    List<INBTTag> enchantments = (List<INBTTag>)enchants;
                    foreach (TAG_Compound bookEnchant in enchantments)
                    {
                        short enchId = (short)bookEnchant.GetChildData("id");
                        var matchingEnchants = DataLists.Enchantments.Where(x => x.id == enchId).ToArray();
                        if (matchingEnchants.Length > 0)
                        {
                            sb.Append(matchingEnchants[0].internalName + " " + bookEnchant.GetChildData("lvl") + ", ");
                        }
                        else
                        {
                            sb.Append(enchId + " " + bookEnchant.GetChildData("lvl") + ", ");
                        }
                    }
                    sb.Remove(sb.Length - 2, 2);
                }
                else
                {
                    Log("GetTradeItemEnchants didn't have a tag for 'StoredEnchantments' or 'Enchantments' when it should have: " + itTag.ToString());

                }
            }

            sb.Append(")");
            return sb.ToString();
        }

        public List<string> GetTrades()
        {
            StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>();

            if (Offers.Count == 6)
            {
                int z = 0;
            }

            foreach (TAG_Compound offer in Offers)
            {
                sb.Clear();

                TAG_Compound buy = (TAG_Compound) offer.GetChildTag("buy");
                TAG_Compound buyB = (TAG_Compound) offer.GetChildTag("buyB");
                TAG_Compound sell = (TAG_Compound) offer.GetChildTag("sell");

                sb.Append(buy.GetChildData("Count") + " " + buy.GetChildData("id"));

                TAG_Compound itTag = (TAG_Compound) buy.GetChildTag("tag");
                if (itTag != null)
                {
                    sb.Append(GetItemEnchantments(itTag));
                }

                //  If a villager only wants 1 item, the game treats them as also wanting "Count:0" of "minecraft:air"
                if (buyB != null && (sbyte)buyB.GetChildData("Count") != 0 && !StringsEqual((string) buyB.GetChildData("id"), "minecraft:air"))
                {
                    sb.Append(" + " + (sbyte)buyB.GetChildData("Count") + " " + buyB.GetChildData("id"));
                    itTag = (TAG_Compound)buyB.GetChildTag("tag");
                    if (itTag != null)
                    {
                        sb.Append(GetItemEnchantments(itTag));
                    }
                }

                sb.Append(" for ");
                sb.Append((sbyte)sell.GetChildData("Count") + " " + sell.GetChildData("id"));
                itTag = (TAG_Compound)sell.GetChildTag("tag");
                if (itTag != null)
                {
                    sb.Append(GetItemEnchantments(itTag));
                }

                list.Add(sb.ToString());
            }

            return list;
        }

        public override string ToString()
        {
            if (!StringsEqual(profession, "minecraft:none"))
            {
                return (level < LevelStrings.Length ? (LevelStrings[level]) : ("Level " + level + "?")) + " " + profession + " at " + coordX.ToString("0.#") + ", " + coordY.ToString("0.#") + ", " + coordZ.ToString("0.#") + " has " + Offers.Count + " trades";
            }
            else
            {
                return "Unemployed villager at " + coordX.ToString("0.#") + ", " + coordY.ToString("0.#") + ", " + coordZ.ToString("0.#");
            }
        }
    }
}
