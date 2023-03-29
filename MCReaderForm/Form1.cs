using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MCReader;
using MCReader.Data;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static MCReaderForm.FancyTreeView;
using static MCReaderForm.FormUtilities;


namespace MCReaderForm
{
    public partial class Form1 : Form
    {
        private List<Chunk> _chunks = new List<Chunk>();
        private NBTReader reader;
        private RegionFile loadedRegion;
        private EntityFile loadedEntityRegion;

        public Form1()
        {
            InitializeComponent();

            cbFinderName.DataSource = DataLists.Items;
            cbFinderName.DisplayMember = "display_name";

            cbFinderInternalName.DataSource = DataLists.Items;
            cbFinderInternalName.DisplayMember = "identifier";
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
            File.WriteAllText(@".\output.txt", TreeToString(tree.Nodes[0]));
        }



        private void LoadAndShowLevelDat(Stream input)
        {
            reader = new NBTReader(input);
            List<INBTTag> list = reader.ReadAll();
            list.PrintStats();
            DrawTree(list);
        }

        private void LoadAndShowRegion(Stream input)
        {
            try
            {
                loadedRegion = new RegionFile(input);

                _chunks.Clear();
                _chunks = loadedRegion.ReadChunks();
                listViewChunks.Items.Clear();

                _chunks = _chunks.OrderBy(a => a.coordX).ThenBy(a => a.coordZ).ToList();

                foreach (Chunk c in _chunks)
                {
                    string[] row = { c.coordX.ToString(), c.coordZ.ToString() };
                    ListViewItem item = new ListViewItem(row);
                    item.Tag = c;
                    listViewChunks.Items.Add(item);
                }


                listViewChunks.Sort();

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

        private void LoadAndShowEntityRegion(Stream input)
        {
            try
            {
                loadedEntityRegion = new EntityFile(input);

                _chunks.Clear();
                _chunks = loadedEntityRegion.ReadChunks();
                listViewChunks.Items.Clear();

                _chunks = _chunks.OrderBy(a => a.coordX).ThenBy(a => a.coordZ).ToList();

                foreach (Chunk c in _chunks)
                {
                    string[] row = { c.coordX.ToString(), c.coordZ.ToString() };
                    ListViewItem item = new ListViewItem(row);
                    item.Tag = c;
                    listViewChunks.Items.Add(item);
                }


                listViewChunks.Sort();

                listViewChunks.Items[0].Selected = true;

                Chunk sel = ((Chunk)listViewChunks.SelectedItems[0].Tag);
                DrawTree(sel.NBT);

            }
            catch (Exception ex)
            {
                Log("LoadAndShowEntityRegion failed!");
                Log(ex);
            }
        }


        private async void SearchForItemAsync(Item item, List<Chunk> toSearch, bool checkBlockEntities, bool checkEntities)
        {
            Chunk.chestsSearched = 0;
            Chunk.blockEntitiesNoItemTag = 0;

            foreach (Chunk c in toSearch)
            {
                if (c.FindInChests(item, out var results))
                {
                    Log("Found '" + item.display_name + "' in " + c.ToString());
                    foreach (var (chestNBT, numInChest) in results)
                    {
                        //Log(numInChest + " in " + chestNBT.ToString());
                        int xCoord = (int)chestNBT.GetChildData("x");
                        int yCoord = (int)chestNBT.GetChildData("y");
                        int zCoord = (int)chestNBT.GetChildData("z");

                        Log(numInChest + " in chest at [" + xCoord + ", " + yCoord + ", " + zCoord + "]");
                        tbSearchResults.AppendText(numInChest + " in chest at [" + xCoord + ", " + yCoord + ", " + zCoord + "]" + Environment.NewLine);
                    }
                }
                else
                {
                    //Log("No '" + item.display_name + "' in " + c.ToString());
                }
            }

            Log("Search finished, checked " + Chunk.chestsSearched + " block entities");
            //tbSearchResults.AppendText("Search finished, checked " + Chunk.chestsSearched + " block entities" + Environment.NewLine);
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


        private void buttonChestFinderSearchSelected_Click(object sender, EventArgs e)
        {
            List<Chunk> toSearch = new List<Chunk>();
            if (listViewChunks.SelectedItems.Count == 1)
            {
                toSearch.Add((Chunk)listViewChunks.SelectedItems[0].Tag);
                tbSearchResults.AppendText("Searching for '" + cbFinderName.Text + "' in " + listViewChunks.SelectedItems.Count + " chunks" + Environment.NewLine);
            }
            else
            {
                for (int i = 0; i < listViewChunks.SelectedItems.Count; i++)
                {
                    toSearch.Add((Chunk)listViewChunks.SelectedItems[i].Tag);
                }
                tbSearchResults.AppendText("Searching for '" + cbFinderName.Text + "' in " + listViewChunks.SelectedItems.Count + " chunks" + Environment.NewLine);
            }

            Item item = (Item)cbFinderName.SelectedItem;
            SearchForItemAsync(item, toSearch, checkBoxFinderBlockEntities.Checked, checkBoxFinderEntities.Checked);
        }

        private void buttonChestFinderSearchRegion_Click(object sender, EventArgs e)
        {
            tbSearchResults.AppendText("Searching for '" + cbFinderName.Text + "' in " + _chunks.Count + " chunks (entire region)" + Environment.NewLine);

            Item item = (Item)cbFinderName.SelectedItem;
            SearchForItemAsync(item, _chunks, checkBoxFinderBlockEntities.Checked, checkBoxFinderEntities.Checked);
        }


        private void cbFinderName_SelectedValueChanged(object sender, EventArgs e)
        {
            Item it = (Item)cbFinderName.SelectedItem;
            tbFinderID.Text = it.numeric_id.ToString();
        }

        private void buttonChestFinderSearchFolder_Click(object sender, EventArgs e)
        {
            Item item = (Item)cbFinderName.SelectedItem;
            Stream input;
            if (folderBrowserSearch.ShowDialog() == DialogResult.OK)
            {
                var files = Directory.GetFiles(folderBrowserSearch.SelectedPath);

                List<string> nonzero = new List<string>();
                foreach (var file in files)
                {
                    if (new FileInfo(file).Length == 0)
                    {
                        //Log("Skipping " + file.Substring(folderBrowserSearch.SelectedPath.Length) + " because it's size is 0");
                        continue;
                    }
                    nonzero.Add(file);
                }

                files = nonzero.ToArray();
                tbSearchResults.AppendText("Starting folder search with " + nonzero.Count + " regions." + Environment.NewLine);

                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    if (new FileInfo(file).Length == 0)
                    {
                        Log("Skipping " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1) + " because it's size is 0");
                        continue;
                    }
                    input = OpenFile(file);
                    loadedRegion = new RegionFile(input);
                    _chunks = loadedRegion.ReadChunks();
                    SearchForItemAsync(item, _chunks, true, true);
                }
            }

            tbSearchResults.AppendText("Folder search finished." + Environment.NewLine);
        }

        private void buttonOpenEntityRegionFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogRegion.ShowDialog() == DialogResult.OK)
            {
                Stream input = OpenFile(openFileDialogRegion.FileName);
                LoadAndShowRegion(input);
            }
        }
    }

}