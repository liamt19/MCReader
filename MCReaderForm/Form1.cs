using System.Diagnostics;
using System.Runtime.InteropServices;

using static MCReaderForm.FancyTreeView;

namespace MCReaderForm
{
    public partial class Form1 : Form
    {
        public static string worldFolder = @"D:\Data\MCReader\wld\";
        public static string levelDat = worldFolder + @"level.dat";

        public static string regionFolder = worldFolder + @"region\";
        public static string regionFile = regionFolder + @"r.5.2.mca";

        public static string entityFolder = worldFolder + @"entities\";
        public static string entityFile = entityFolder + @"r.-3.1.mca";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Stream input = OpenFile(regionFile);
            Stream input = OpenFile(levelDat);

            NBTReader r = new NBTReader(input);
            List<INBTTag> list = r.ReadAll();

            DrawTree(list);
        }

        public void DrawTree(List<INBTTag> list)
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();

            TreeNode tn = EnumerateList(list);
            tree.Nodes.Add(tn);

            tree.EndUpdate();
        }

        public TreeNode EnumerateList(List<INBTTag> list)
        {
            TreeNode res = new TreeNode();
            for (int i = 0; i < list.Count; i++)
            {
                INBTTag tag = list[i];
                TreeNode temp;
                if (tag is TAG_Compound)
                {
                    List<INBTTag> tagList = ((List <INBTTag>)tag.Data());
                    temp = EnumerateList(tagList);
                    temp.Text = tag.Name();
                }
                else
                {
                    temp = new TreeNode(tag.ToString());
                }

                int imgIdx = (int)INBTTag.IDOf(tag);
                temp.ImageIndex = imgIdx;
                temp.SelectedImageIndex = imgIdx;

                if (i == 0 && tag is TAG_Compound && (tag as TAG_Compound).IsRoot)
                {
                    res = temp;
                }
                else
                {
                    res.Nodes.Add(temp);
                }
            }

            return res;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetTreeViewTheme(tree.Handle);
        }
    }



}