using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public class Villager
    {
        private static string[] LevelStrings = new string[] { "", "Novice", "Apprentice", "Journeyman", "Expert", "Master" };

        public string profession;
        public string CustomName;
        public int level;

        public double coordX;
        public double coordY;
        public double coordZ;

        public bool hasTrades;

        public List<TAG_Compound> Offers = new List<TAG_Compound>();
        private TAG_Compound _tag;

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

            var offers = tag.GetChildData("Offers");
            if (offers != null)
            {
                List<INBTTag> tradesTag = ((List<INBTTag>)((List<INBTTag>)offers)[0].Data());
                Offers = tradesTag.Cast<TAG_Compound>().ToList();
                hasTrades = true;
            }

            _tag = tag;
        }

        public string GetTradeItemEnchants(TAG_Compound itTag)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(Enchantments: ");

            //  For enchantment book enchantments.
            object enchants = itTag.GetChildData("StoredEnchantments");
            if (enchants != null)
            {
                List<INBTTag> enchantments = (List<INBTTag>)enchants;
                foreach (TAG_Compound bookEnchant in enchantments)
                {
                    sb.Append(bookEnchant.GetChildData("id") + " " + bookEnchant.GetChildData("lvl") + ", ");
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
                        sb.Append(bookEnchant.GetChildData("id") + " " + bookEnchant.GetChildData("lvl") + ", ");
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
                    sb.Append(GetTradeItemEnchants(itTag));
                }

                //  If a villager only wants 1 item, the game treats them as also wanting "Count:0" of "minecraft:air"
                if ((sbyte)buyB.GetChildData("Count") != 0 && !StringsEqual((string) buyB.GetChildData("id"), "minecraft:air"))
                {
                    sb.Append(" + " + (sbyte)buyB.GetChildData("Count") + " " + buyB.GetChildData("id"));
                    itTag = (TAG_Compound)buyB.GetChildTag("tag");
                    if (itTag != null)
                    {
                        sb.Append(GetTradeItemEnchants(itTag));
                    }
                }

                sb.Append(" for ");
                sb.Append((sbyte)sell.GetChildData("Count") + " " + sell.GetChildData("id"));
                itTag = (TAG_Compound)sell.GetChildTag("tag");
                if (itTag != null)
                {
                    sb.Append(GetTradeItemEnchants(itTag));
                }

                list.Add(sb.ToString());
            }

            return list;
        }

        public override string ToString()
        {
            if (!StringsEqual(profession, "minecraft:none"))
            {
                return LevelStrings[level] + " " + profession + " at " + coordX.ToString("0.#") + ", " + coordY.ToString("0.#") + ", " + coordZ.ToString("0.#") + " has " + Offers.Count + " trades";
            }
            else
            {
                return "Unemployed villager at " + coordX.ToString("0.#") + ", " + coordY.ToString("0.#") + ", " + coordZ.ToString("0.#");
            }
        }
    }
}
