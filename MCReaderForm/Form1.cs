using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MCReader;

using static MCReaderForm.FancyTreeView;
using static MCReaderForm.FormUtilities;


namespace MCReaderForm
{
    public partial class Form1 : Form
    {
        private List<Chunk> _chunks = new List<Chunk>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        public void DrawTree(List<INBTTag> list)
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();

            TreeNode tn = EnumerateList(list);
            tree.Nodes.Add(tn);

            tree.EndUpdate();

            tree.Nodes[0].Expand();
        }



        private void LoadAndShowLevelDat(Stream input)
        {
            NBTReader r = new NBTReader(input);
            List<INBTTag> list = r.ReadAll();

            DrawTree(list);
        }

        private void LoadAndShowRegion(Stream input)
        {
            try
            {
                RegionFile rf = new RegionFile(input);
                _chunks = rf.ReadChunks();
                listViewChunks.Items.Clear();

                foreach (Chunk c in _chunks)
                {
                    string[] row = { c.coordX.ToString(), c.coordZ.ToString() };
                    ListViewItem item = new ListViewItem(row);
                    item.Tag = c;
                    listViewChunks.Items.Add(item);
                }

                listViewChunks.Items[0].Selected = true;

                Chunk sel = ((Chunk)listViewChunks.SelectedItems[0].Tag);
                DrawTree(sel.NBT);
                
            }
            catch (Exception ex)
            {
                Log("LoadAndShowRegion failed!");
                Log(ex);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            SetTreeViewTheme(tree.Handle);
        }

        private void buttonOpenLevelDat_Click(object sender, EventArgs e)
        {
            if (openFileDialogLevelDat.ShowDialog() == DialogResult.OK)
            {
                Stream input = OpenFile(openFileDialogLevelDat.FileName);
                LoadAndShowLevelDat(input);
            }
        }

        private async void buttonCopyMCPath_Click(object sender, EventArgs e)
        {
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string normalPath = appData + @"\.minecraft\saves";
                if (Directory.Exists(normalPath))
                {
                    Clipboard.SetText(normalPath);

                    string temp = buttonCopyMCPath.Text;
                    buttonCopyMCPath.Text = "Copied!";

                    await Task.Delay(1000);

                    buttonCopyMCPath.Text = temp;
                }
            }
            catch
            {
                string temp = buttonCopyMCPath.Text;
                buttonCopyMCPath.Text = "Failed!";

                await Task.Delay(1000);

                buttonCopyMCPath.Text = temp;
            }
        }

        private void buttonOpenRegion_Click(object sender, EventArgs e)
        {
            if (openFileDialogRegion.ShowDialog() == DialogResult.OK)
            {
                Stream input = OpenFile(openFileDialogRegion.FileName);
                LoadAndShowRegion(input);
            }
        }

        private void listViewChunks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewChunks.SelectedItems.Count != 0)
            {
                Chunk sel = (Chunk) listViewChunks.SelectedItems[0].Tag;
                DrawTree(sel.NBT);
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                Log("Files: " + files.ToString());
                string file = files[0];
                if (file.EndsWith("dat"))
                {
                    Stream input = OpenFile(file);
                    LoadAndShowLevelDat(input);
                }
                else if (file.EndsWith("mca") || file.EndsWith("mcr"))
                {
                    Stream input = OpenFile(file);
                    LoadAndShowRegion(input);
                }
                else
                {
                    Log("File " + file + " is unsupported");
                }
            }
            catch (Exception ex)
            {
                Log("Failed drag and drop for DragEventArgs = " + e.ToString());
                Log(ex.ToString());
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        public static TreeNode EnumerateList(List<INBTTag> list)
        {
            TreeNode res = new TreeNode();
            for (int i = 0; i < list.Count; i++)
            {
                INBTTag tag = list[i];
                TreeNode temp;
                if ((tag is TAG_Compound) || (tag is TAG_List))
                {
                    List<INBTTag> tagList = ((List<INBTTag>)tag.Data());
                    temp = EnumerateList(tagList);
                    temp.Text = tag.Name();
                }
                else if ((tag is TAG_Int_Array) || (tag is TAG_Long_Array))
                {
                    INBTTag[] tagArr = ((INBTTag[])tag.Data());
                    temp = EnumerateList(tagArr.ToList());
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
    }

}