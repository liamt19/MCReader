using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCReader.Data
{
    public static class DataLists
    {
        private static string DataFile = @".\Data\BurgerData.json";
        private static string ItemFile = @".\Data\fmt\items.txt";
        private static string EntityFile = @".\Data\fmt\entities.txt";
        private static string BlockFile = @".\Data\fmt\blocks.txt";

        //  display_name, numeric_id, text_id
        public static List<Item> Items;

        //  display_name, id, name
        public static List<Entity> Entities;

        //  display_name, numeric_id, text_id
        public static List<Block> Blocks;

        static DataLists()
        {
            Items = new List<Item>();
            Entities = new List<Entity>();
            Blocks = new List<Block>();

            MakeLists();
        }

        private static void MakeLists()
        {
            foreach (string line in File.ReadAllLines(ItemFile))
            {
                string[] splits = line.Split('\t');
                Items.Add(new Item(splits[0], int.Parse(splits[1]), splits[2]));
            }

            Log("Loaded " + Items.Count + " items");

            foreach (string line in File.ReadAllLines(EntityFile))
            {
                string[] splits = line.Split('\t');
                Entities.Add(new Entity(splits[0], int.Parse(splits[1]), splits[2]));
            }

            Log("Loaded " + Entities.Count + " entities");

            foreach (string line in File.ReadAllLines(BlockFile))
            {
                string[] splits = line.Split('\t');
                Blocks.Add(new Block(splits[0], int.Parse(splits[1]), splits[2]));
            }

            Log("Loaded " + Blocks.Count + " blocks");

        }


    }
}
