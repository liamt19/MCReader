using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MCReader;
using MCReader.Data;

using Microsoft.VisualBasic.Logging;

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

        private string lastFolderPath;

        public bool WriteToLogFile = true;

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

            if (cbExpandNBTTree.Checked)
            {
                tree.ExpandAll();
            }
            else
            {
                tree.Nodes[0].Expand();
                foreach (TreeNode rootNode in tree.Nodes[0].Nodes)
                {
                    rootNode.Expand();
                }
            }
            
            //File.WriteAllText(@".\output.txt", TreeToString(tree.Nodes[0]));
        }



        private void LoadAndShowLevelDat(Stream input)
        {
            reader = new NBTReader(input);
            List<INBTTag> list = reader.ReadAll();
            //list.PrintStats();
            DrawTree(list);
        }

        private void LoadAndShowRegion(Stream input)
        {
            try
            {
                loadedRegion = new RegionFile(input, lastFolderPath);

                _chunks.Clear();
                _chunks = loadedRegion.ReadChunks();
                listViewChunks.Items.Clear();

                _chunks = _chunks.OrderBy(a => a.coordX).ThenBy(a => a.coordZ).ToList();
                //_chunks = _chunks.OrderByDescending(a => a.numBlockEntities).ThenBy(a => a.coordX).ThenBy(a => a.coordZ).ToList();

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
                DrawTree(sel.RegionNBT);
                
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
                loadedEntityRegion = new EntityFile(input, lastFolderPath);

                _chunks.Clear();
                bool result = loadedEntityRegion.ReadChunks(out _chunks);
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
                DrawTree(sel.RegionNBT);

            }
            catch (Exception ex)
            {
                Log("LoadAndShowEntityRegion failed!");
                Log(ex);
            }
        }


        private async void SearchForItemAsync(string item, List<Chunk> toSearch, bool checkBlockEntities, bool checkEntities)
        {
            Chunk.chestsSearched = 0;
            Chunk.blockEntitiesNoItemTag = 0;

            foreach (Chunk c in toSearch)
            {
                if (checkEntities && c.FindInEntities(item, out var entityResults))
                {
                    Log("Found '" + item + "' in " + c.ToString());
                    foreach (var (entityNBT, numInChest) in entityResults)
                    {
                        Log(numInChest + " in entity: " + entityNBT.ToString());
                        tbSearchResults.AppendText(numInChest + " in entity: " + entityNBT.ToString() + Environment.NewLine);
                    }
                }

                if (checkBlockEntities && c.FindInChests(item, out var results))
                {
                    Log("Found '" + item + "' in " + c.ToString());
                    Log("NBT: " + results[0].chestNBT.ToString());
                    foreach (var (chestNBT, numInChest) in results)
                    {
                        //Log(numInChest + " in " + chestNBT.ToString());
                        int xCoord = (int)chestNBT.GetChildData("x");
                        int yCoord = (int)chestNBT.GetChildData("y");
                        int zCoord = (int)chestNBT.GetChildData("z");

                        string containerName = (string)chestNBT.GetChildData("id");
                        if (containerName == null)
                        {
                            containerName = "(container)";
                        }

                        Log(numInChest + " in " + containerName + " at [" + xCoord + ", " + yCoord + ", " + zCoord + "]");
                        Log("INFO: " + chestNBT.ToString());
                        tbSearchResults.AppendText(numInChest + " in " + containerName + " at [" + xCoord + ", " + yCoord + ", " + zCoord + "]" + Environment.NewLine);
                    }
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
                lastFolderPath = Path.GetDirectoryName(openFileDialogRegion.FileName);
                Stream input = OpenFile(openFileDialogRegion.FileName);
                LoadAndShowRegion(input);
            }
        }

        private void listViewChunks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewChunks.SelectedItems.Count != 0)
            {
                Chunk sel = (Chunk) listViewChunks.SelectedItems[0].Tag;
                Log("Selected " + listViewChunks.SelectedIndices[0]);
                DrawTree(sel.RegionNBT);
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

            string itemName = cbFinderName.Text;
            if (cbFinderName.SelectedItem != null)
            {
                Item item = (Item)cbFinderName.SelectedItem;
                itemName = item.identifier;
            }
            SearchForItemAsync(itemName, toSearch, checkBoxFinderBlockEntities.Checked, checkBoxFinderEntities.Checked);
        }

        private void buttonChestFinderSearchRegion_Click(object sender, EventArgs e)
        {
            tbSearchResults.AppendText("Searching for '" + cbFinderName.Text + "' in " + _chunks.Count + " chunks (entire region)" + Environment.NewLine);

            string itemName = cbFinderName.Text;
            if (cbFinderName.SelectedItem != null)
            {
                Item item = (Item)cbFinderName.SelectedItem;
                itemName = item.identifier;
            }
            SearchForItemAsync(itemName, _chunks, checkBoxFinderBlockEntities.Checked, checkBoxFinderEntities.Checked);
        }


        private void cbFinderName_SelectedValueChanged(object sender, EventArgs e)
        {
            Item it = (Item)cbFinderName.SelectedItem;
            tbFinderID.Text = it.numeric_id.ToString();
            Log("Selected " + cbFinderName.SelectedIndex + " = '" + it.display_name + "'");
        }

        private void buttonChestFinderSearchFolder_Click(object sender, EventArgs e)
        {
            string itemName = cbFinderName.Text;
            if (cbFinderName.SelectedItem != null)
            {
                Item item = (Item)cbFinderName.SelectedItem;
                itemName = item.identifier;
            }
            
            Stream input;
            if (folderBrowserSearch.ShowDialog() == DialogResult.OK)
            {
                lastFolderPath = folderBrowserSearch.SelectedPath;
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
                    input = OpenFile(file);
                    loadedRegion = new RegionFile(input, lastFolderPath);
                    _chunks = loadedRegion.ReadChunks();
                    SearchForItemAsync(itemName, _chunks, true, true);

                }
            }

            tbSearchResults.AppendText("Folder search finished." + Environment.NewLine);
        }

        private void buttonOpenEntityRegionFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogRegion.ShowDialog() == DialogResult.OK)
            {
                lastFolderPath = Path.GetDirectoryName(openFileDialogRegion.FileName);
                Stream input = OpenFile(openFileDialogRegion.FileName);
                LoadAndShowRegion(input);
            }
        }

        private void buttonVillagerTradesSearchFolder_Click(object sender, EventArgs e)
        {
            Stream input;
            if (folderBrowserSearch.ShowDialog() == DialogResult.OK)
            {
                lastFolderPath = folderBrowserSearch.SelectedPath;
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
                tbVillagerTradesResults.AppendText("Starting folder search with " + nonzero.Count + " regions." + Environment.NewLine);

                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    input = OpenFile(file);
                    loadedEntityRegion = new EntityFile(input, lastFolderPath);
                    bool result = loadedEntityRegion.ReadChunks(out _chunks);
                    if (!result)
                    {
                        Log("Chunk files provided weren't entity files, or were pre 1.17!");
                        input.Close();
                        goto Fixed_Pre_1_17;
                    }
                    DoVillagerSearch(_chunks, cbVillagerTradesWhitelist.Checked);
                }

                tbVillagerTradesResults.AppendText("Folder search finished." + Environment.NewLine);
                return;

                Fixed_Pre_1_17:
                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    input = OpenFile(file);
                    loadedRegion = new RegionFile(input, lastFolderPath);
                    _chunks = loadedRegion.ReadChunks();
                    DoVillagerSearch(_chunks, cbVillagerTradesWhitelist.Checked);
                }

                tbVillagerTradesResults.AppendText("Folder search finished." + Environment.NewLine);
                return;
            }
            else
            {
                tbVillagerTradesResults.AppendText("Folder search finished." + Environment.NewLine);
            }
        }

        private void buttonVillagerTradesSearchRegion_Click(object sender, EventArgs e)
        {
            List<Chunk> toSearch = new List<Chunk>();
            for (int i = 0; i < listViewChunks.SelectedItems.Count; i++)
            {
                toSearch.Add((Chunk)listViewChunks.SelectedItems[i].Tag);
            }
            DoVillagerSearch(toSearch, cbVillagerTradesWhitelist.Checked);
        }

        private void DoVillagerSearch(List<Chunk> toSearch, bool includeAllProfessions = false)
        {
            if (toSearch == null)
            {
                Log("DoVillagerSearch called with a null List<Chunk> toSearch");
                return;
            }
            else if (toSearch.Count == 0)
            {
                Log("DoVillagerSearch called with an empty List<Chunk> toSearch");
                return;
            }

            List<Villager> villagers = EntityFile.GetVillagers(toSearch);
            foreach (Villager v in villagers)
            {
                if (v.hasTrades)
                {
                    if (includeAllProfessions || (v.profession == "minecraft:librarian" || v.profession == "minecraft:smith"))
                    {
                        Log(Environment.NewLine + v.ToString());
                        tbVillagerTradesResults.AppendText(v.ToString() + Environment.NewLine);

                        List<string> trades = v.GetTrades();
                        foreach (string trade in trades)
                        {
                            tbVillagerTradesResults.AppendText("\t" + trade + Environment.NewLine);
                        }
                    }
                }
                else if (v.profession == "minecraft:librarian")
                {
                    //  Log unmet librarians anyways.
                    Log(Environment.NewLine + v.ToString());
                    tbVillagerTradesResults.AppendText(v.ToString() + Environment.NewLine);
                }
            }
        }

        private void cbExpandNBTTree_CheckedChanged(object sender, EventArgs e)
        {
            if (cbExpandNBTTree.Checked)
            {
                tree.ExpandAll();
            }
            else
            {
                tree.CollapseAll();
                tree.Nodes[0].Expand();
            }
        }

        private void checkBoxFinderBlockEntities_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFinderBlockEntities.Checked)
            {
                checkBoxFinderEntities.Checked = false;
            }
            else
            {
                checkBoxFinderEntities.Checked = true;
            }
        }

        private void checkBoxFinderEntities_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFinderEntities.Checked)
            {
                checkBoxFinderBlockEntities.Checked = false;
            }
            else
            {
                checkBoxFinderBlockEntities.Checked = true;
            }
            
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void buttonEntitiesSearchFolder_Click(object sender, EventArgs e)
        {
            Stream input;
            if (folderBrowserSearch.ShowDialog() == DialogResult.OK)
            {
                lastFolderPath = folderBrowserSearch.SelectedPath;
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
                tbEnumerateEntities.AppendText("Starting folder search with " + nonzero.Count + " regions." + Environment.NewLine);

                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    input = OpenFile(file);
                    loadedEntityRegion = new EntityFile(input, lastFolderPath);
                    bool result = loadedEntityRegion.ReadChunks(out _chunks);
                    if (!result)
                    {
                        Log("Chunk files provided weren't entity files, or were pre 1.17!");
                        input.Close();
                        goto Fixed_Pre_1_17;
                    }
                    List<TAG_Compound> list = EntityFile.GetAllEntities(_chunks);
                    if (list.Count > 0)
                    {
                        Log("Region " + file + ": ");
                        tbEnumerateEntities.AppendText("Region " + file + ": " + Environment.NewLine);
                        foreach (TAG_Compound entity in list)
                        {
                            Log("\t" + (string)entity.GetChildData("id"));
                            tbEnumerateEntities.AppendText("\t" + (string)entity.GetChildData("id") + Environment.NewLine);
                        }
                    }
                    
                }

                tbEnumerateEntities.AppendText("Folder search finished." + Environment.NewLine);
                return;

            Fixed_Pre_1_17:
                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    input = OpenFile(file);
                    loadedRegion = new RegionFile(input, lastFolderPath);
                    _chunks = loadedRegion.ReadChunks();
                    List<TAG_Compound> list = EntityFile.GetAllEntities(_chunks);
                    if (list.Count > 0)
                    {
                        Log("Region " + file + ": ");
                        tbEnumerateEntities.AppendText("Region " + file + ": " + Environment.NewLine);
                        foreach (TAG_Compound entity in list)
                        {
                            Log("\t" + (string)entity.GetChildData("id"));
                            tbEnumerateEntities.AppendText("\t" + (string)entity.GetChildData("id") + Environment.NewLine);
                        }
                    }
                }

                tbEnumerateEntities.AppendText("Folder search finished." + Environment.NewLine);
                return;
            }
            else
            {
                tbEnumerateEntities.AppendText("Folder search finished." + Environment.NewLine);
            }
        }

        private void buttonChestsSearchFolder_Click(object sender, EventArgs e)
        {
            Stream input;
            if (folderBrowserSearch.ShowDialog() == DialogResult.OK)
            {
                lastFolderPath = folderBrowserSearch.SelectedPath;
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
                tbEnumerateChests.AppendText("Starting folder search with " + nonzero.Count + " regions." + Environment.NewLine);

                foreach (var file in files)
                {
                    Log("Looking in " + file.Substring(folderBrowserSearch.SelectedPath.Length + 1));
                    input = OpenFile(file);
                    loadedRegion = new RegionFile(input, lastFolderPath);
                    _chunks = loadedRegion.ReadChunks();
                    List<INBTTag> list = RegionFile.GetAllChests(_chunks);
                    if (list.Count > 0)
                    {
                        Log("\nRegion " + file + ": ");
                        tbEnumerateChests.AppendText("\nRegion " + file + ": " + Environment.NewLine);

                        if (WriteToLogFile)
                        {
                            File.AppendAllText(@".\output.txt", "\nRegion " + file + ": " + Environment.NewLine);
                        }
                            
                        foreach (TAG_Compound chest in list)
                        {
                            var itemListTags = chest.GetChildData("Items");
                            if (itemListTags == null)
                            {
                                continue;
                            }

                            Log("\tChest at [" + chest.GetChildData("x") + ", " + chest.GetChildData("y") + ", " + chest.GetChildData("z") + "]");
                            tbEnumerateChests.AppendText("\tChest at [" + chest.GetChildData("x") + ", " + chest.GetChildData("y") + ", " + chest.GetChildData("z") + "]" + Environment.NewLine);
                            
                            if (WriteToLogFile)
                            {
                                File.AppendAllText(@".\output.txt", "\tChest at [" + chest.GetChildData("x") + ", " + chest.GetChildData("y") + ", " + chest.GetChildData("z") + "]" + Environment.NewLine);
                            }

                            List<INBTTag> chestItems = (List <INBTTag>)itemListTags;

                            foreach (INBTTag chestItemI in chestItems)
                            {
                                if (chestItemI.GetType() != typeof(TAG_Compound))
                                {
                                    //  Some mods have weird formats, skip these.
                                    continue;
                                }

                                TAG_Compound chestItem = (TAG_Compound)chestItemI;

                                TAG_Compound itTag = (TAG_Compound)chestItem.GetChildTag("tag");
                                string s;
                                if (itTag != null && !StringsEqual((string) chestItem.GetChildData("id"), "potion"))
                                {
                                    string ench = Villager.GetItemEnchantments(itTag);
                                    s = "\t\t" + chestItem.GetChildData("id") + ", " + ench + Environment.NewLine;
                                }
                                else
                                {
                                    s = "\t\t" + chestItem.GetChildData("id") + Environment.NewLine;
                                }
                                Log(s);
                                tbEnumerateChests.AppendText(s);
                                if (WriteToLogFile)
                                {
                                    File.AppendAllText(@".\output.txt", s);
                                }
                            }
                        }
                    }
                }

                tbEnumerateChests.AppendText("Folder search finished." + Environment.NewLine);
                return;
            }
            else
            {
                tbEnumerateChests.AppendText("Folder search finished." + Environment.NewLine);
            }
            
        }
    }

}