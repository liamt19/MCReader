namespace MCReaderForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.buttonOpenLevelDat = new System.Windows.Forms.Button();
            this.openFileDialogLevelDat = new System.Windows.Forms.OpenFileDialog();
            this.buttonCopyMCPath = new System.Windows.Forms.Button();
            this.buttonOpenRegion = new System.Windows.Forms.Button();
            this.openFileDialogRegion = new System.Windows.Forms.OpenFileDialog();
            this.listViewChunks = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageNBT = new System.Windows.Forms.TabPage();
            this.tabPageItemFinder = new System.Windows.Forms.TabPage();
            this.buttonChestFinderSearchFolder = new System.Windows.Forms.Button();
            this.buttonChestFinderSearchRegion = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxFinderEntities = new System.Windows.Forms.CheckBox();
            this.checkBoxFinderBlockEntities = new System.Windows.Forms.CheckBox();
            this.cbFinderInternalName = new System.Windows.Forms.ComboBox();
            this.cbFinderName = new System.Windows.Forms.ComboBox();
            this.tbSearchResults = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFinderID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonChestFinderSearchSelected = new System.Windows.Forms.Button();
            this.folderBrowserSearch = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonOpenEntityRegionFile = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageNBT.SuspendLayout();
            this.tabPageItemFinder.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList1;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(601, 411);
            this.tree.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "tag_none.png");
            this.imageList1.Images.SetKeyName(1, "tag_byte.png");
            this.imageList1.Images.SetKeyName(2, "tag_short.png");
            this.imageList1.Images.SetKeyName(3, "tag_int.png");
            this.imageList1.Images.SetKeyName(4, "tag_long.png");
            this.imageList1.Images.SetKeyName(5, "tag_float.png");
            this.imageList1.Images.SetKeyName(6, "tag_double.png");
            this.imageList1.Images.SetKeyName(7, "tag_byte_array.png");
            this.imageList1.Images.SetKeyName(8, "tag_string.png");
            this.imageList1.Images.SetKeyName(9, "tag_list.png");
            this.imageList1.Images.SetKeyName(10, "tag_compound.png");
            this.imageList1.Images.SetKeyName(11, "tag_int_array.png");
            this.imageList1.Images.SetKeyName(12, "tag_long_array.png");
            // 
            // buttonOpenLevelDat
            // 
            this.buttonOpenLevelDat.Location = new System.Drawing.Point(493, 12);
            this.buttonOpenLevelDat.Name = "buttonOpenLevelDat";
            this.buttonOpenLevelDat.Size = new System.Drawing.Size(111, 30);
            this.buttonOpenLevelDat.TabIndex = 1;
            this.buttonOpenLevelDat.Text = "Open level.dat";
            this.buttonOpenLevelDat.UseVisualStyleBackColor = true;
            this.buttonOpenLevelDat.Click += new System.EventHandler(this.buttonOpenLevelDat_Click);
            // 
            // openFileDialogLevelDat
            // 
            this.openFileDialogLevelDat.DefaultExt = "dat";
            this.openFileDialogLevelDat.FileName = "level.dat";
            this.openFileDialogLevelDat.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            // 
            // buttonCopyMCPath
            // 
            this.buttonCopyMCPath.Location = new System.Drawing.Point(619, 12);
            this.buttonCopyMCPath.Name = "buttonCopyMCPath";
            this.buttonCopyMCPath.Size = new System.Drawing.Size(129, 30);
            this.buttonCopyMCPath.TabIndex = 2;
            this.buttonCopyMCPath.Text = "Copy .minecraft path";
            this.buttonCopyMCPath.UseVisualStyleBackColor = true;
            this.buttonCopyMCPath.Click += new System.EventHandler(this.buttonCopyMCPath_Click);
            // 
            // buttonOpenRegion
            // 
            this.buttonOpenRegion.Location = new System.Drawing.Point(12, 10);
            this.buttonOpenRegion.Name = "buttonOpenRegion";
            this.buttonOpenRegion.Size = new System.Drawing.Size(125, 30);
            this.buttonOpenRegion.TabIndex = 4;
            this.buttonOpenRegion.Text = "Open region file";
            this.buttonOpenRegion.UseVisualStyleBackColor = true;
            this.buttonOpenRegion.Click += new System.EventHandler(this.buttonOpenRegion_Click);
            // 
            // openFileDialogRegion
            // 
            this.openFileDialogRegion.DefaultExt = "mca";
            this.openFileDialogRegion.FileName = "r.0.0.mca";
            this.openFileDialogRegion.Filter = "mca files (*.mca)|*.mca|All files (*.*)|*.*";
            // 
            // listViewChunks
            // 
            this.listViewChunks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewChunks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewChunks.FullRowSelect = true;
            this.listViewChunks.Location = new System.Drawing.Point(12, 46);
            this.listViewChunks.Name = "listViewChunks";
            this.listViewChunks.Size = new System.Drawing.Size(125, 436);
            this.listViewChunks.TabIndex = 5;
            this.listViewChunks.UseCompatibleStateImageBehavior = false;
            this.listViewChunks.View = System.Windows.Forms.View.Details;
            this.listViewChunks.SelectedIndexChanged += new System.EventHandler(this.listViewChunks_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Chunk X";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Z";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageNBT);
            this.tabControl1.Controls.Add(this.tabPageItemFinder);
            this.tabControl1.Location = new System.Drawing.Point(143, 46);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(609, 439);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPageNBT
            // 
            this.tabPageNBT.Controls.Add(this.tree);
            this.tabPageNBT.Location = new System.Drawing.Point(4, 24);
            this.tabPageNBT.Name = "tabPageNBT";
            this.tabPageNBT.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNBT.Size = new System.Drawing.Size(601, 411);
            this.tabPageNBT.TabIndex = 0;
            this.tabPageNBT.Text = "NBT Viewer";
            this.tabPageNBT.UseVisualStyleBackColor = true;
            // 
            // tabPageItemFinder
            // 
            this.tabPageItemFinder.Controls.Add(this.buttonChestFinderSearchFolder);
            this.tabPageItemFinder.Controls.Add(this.buttonChestFinderSearchRegion);
            this.tabPageItemFinder.Controls.Add(this.panel1);
            this.tabPageItemFinder.Controls.Add(this.cbFinderInternalName);
            this.tabPageItemFinder.Controls.Add(this.cbFinderName);
            this.tabPageItemFinder.Controls.Add(this.tbSearchResults);
            this.tabPageItemFinder.Controls.Add(this.label3);
            this.tabPageItemFinder.Controls.Add(this.label2);
            this.tabPageItemFinder.Controls.Add(this.tbFinderID);
            this.tabPageItemFinder.Controls.Add(this.label1);
            this.tabPageItemFinder.Controls.Add(this.buttonChestFinderSearchSelected);
            this.tabPageItemFinder.Location = new System.Drawing.Point(4, 24);
            this.tabPageItemFinder.Name = "tabPageItemFinder";
            this.tabPageItemFinder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageItemFinder.Size = new System.Drawing.Size(601, 411);
            this.tabPageItemFinder.TabIndex = 1;
            this.tabPageItemFinder.Text = "Item Finder";
            this.tabPageItemFinder.UseVisualStyleBackColor = true;
            // 
            // buttonChestFinderSearchFolder
            // 
            this.buttonChestFinderSearchFolder.Location = new System.Drawing.Point(190, 109);
            this.buttonChestFinderSearchFolder.Name = "buttonChestFinderSearchFolder";
            this.buttonChestFinderSearchFolder.Size = new System.Drawing.Size(129, 23);
            this.buttonChestFinderSearchFolder.TabIndex = 14;
            this.buttonChestFinderSearchFolder.Text = "Search folder";
            this.buttonChestFinderSearchFolder.UseVisualStyleBackColor = true;
            this.buttonChestFinderSearchFolder.Click += new System.EventHandler(this.buttonChestFinderSearchFolder_Click);
            // 
            // buttonChestFinderSearchRegion
            // 
            this.buttonChestFinderSearchRegion.Location = new System.Drawing.Point(467, 82);
            this.buttonChestFinderSearchRegion.Name = "buttonChestFinderSearchRegion";
            this.buttonChestFinderSearchRegion.Size = new System.Drawing.Size(129, 23);
            this.buttonChestFinderSearchRegion.TabIndex = 13;
            this.buttonChestFinderSearchRegion.Text = "Search region";
            this.buttonChestFinderSearchRegion.UseVisualStyleBackColor = true;
            this.buttonChestFinderSearchRegion.Click += new System.EventHandler(this.buttonChestFinderSearchRegion_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxFinderEntities);
            this.panel1.Controls.Add(this.checkBoxFinderBlockEntities);
            this.panel1.Location = new System.Drawing.Point(328, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(129, 49);
            this.panel1.TabIndex = 12;
            // 
            // checkBoxFinderEntities
            // 
            this.checkBoxFinderEntities.AutoSize = true;
            this.checkBoxFinderEntities.Location = new System.Drawing.Point(3, 26);
            this.checkBoxFinderEntities.Name = "checkBoxFinderEntities";
            this.checkBoxFinderEntities.Size = new System.Drawing.Size(64, 19);
            this.checkBoxFinderEntities.TabIndex = 1;
            this.checkBoxFinderEntities.Text = "Entities";
            this.checkBoxFinderEntities.UseVisualStyleBackColor = true;
            // 
            // checkBoxFinderBlockEntities
            // 
            this.checkBoxFinderBlockEntities.AutoSize = true;
            this.checkBoxFinderBlockEntities.Checked = true;
            this.checkBoxFinderBlockEntities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFinderBlockEntities.Location = new System.Drawing.Point(3, 3);
            this.checkBoxFinderBlockEntities.Name = "checkBoxFinderBlockEntities";
            this.checkBoxFinderBlockEntities.Size = new System.Drawing.Size(96, 19);
            this.checkBoxFinderBlockEntities.TabIndex = 0;
            this.checkBoxFinderBlockEntities.Text = "Block entities";
            this.checkBoxFinderBlockEntities.UseVisualStyleBackColor = true;
            // 
            // cbFinderInternalName
            // 
            this.cbFinderInternalName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbFinderInternalName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbFinderInternalName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFinderInternalName.FormattingEnabled = true;
            this.cbFinderInternalName.Location = new System.Drawing.Point(237, 28);
            this.cbFinderInternalName.Name = "cbFinderInternalName";
            this.cbFinderInternalName.Size = new System.Drawing.Size(220, 23);
            this.cbFinderInternalName.TabIndex = 11;
            // 
            // cbFinderName
            // 
            this.cbFinderName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbFinderName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbFinderName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFinderName.FormattingEnabled = true;
            this.cbFinderName.Location = new System.Drawing.Point(7, 28);
            this.cbFinderName.Name = "cbFinderName";
            this.cbFinderName.Size = new System.Drawing.Size(220, 23);
            this.cbFinderName.TabIndex = 10;
            this.cbFinderName.SelectedValueChanged += new System.EventHandler(this.cbFinderName_SelectedValueChanged);
            // 
            // tbSearchResults
            // 
            this.tbSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearchResults.Location = new System.Drawing.Point(2, 138);
            this.tbSearchResults.Multiline = true;
            this.tbSearchResults.Name = "tbSearchResults";
            this.tbSearchResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSearchResults.Size = new System.Drawing.Size(596, 271);
            this.tbSearchResults.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Internal Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(467, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "ID";
            // 
            // tbFinderID
            // 
            this.tbFinderID.Location = new System.Drawing.Point(467, 28);
            this.tbFinderID.Name = "tbFinderID";
            this.tbFinderID.Size = new System.Drawing.Size(129, 23);
            this.tbFinderID.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name";
            // 
            // buttonChestFinderSearchSelected
            // 
            this.buttonChestFinderSearchSelected.Location = new System.Drawing.Point(467, 109);
            this.buttonChestFinderSearchSelected.Name = "buttonChestFinderSearchSelected";
            this.buttonChestFinderSearchSelected.Size = new System.Drawing.Size(129, 23);
            this.buttonChestFinderSearchSelected.TabIndex = 2;
            this.buttonChestFinderSearchSelected.Text = "Search selected";
            this.buttonChestFinderSearchSelected.UseVisualStyleBackColor = true;
            this.buttonChestFinderSearchSelected.Click += new System.EventHandler(this.buttonChestFinderSearchSelected_Click);
            // 
            // buttonOpenEntityRegionFile
            // 
            this.buttonOpenEntityRegionFile.Location = new System.Drawing.Point(143, 10);
            this.buttonOpenEntityRegionFile.Name = "buttonOpenEntityRegionFile";
            this.buttonOpenEntityRegionFile.Size = new System.Drawing.Size(125, 30);
            this.buttonOpenEntityRegionFile.TabIndex = 7;
            this.buttonOpenEntityRegionFile.Text = "Open entity file";
            this.buttonOpenEntityRegionFile.UseVisualStyleBackColor = true;
            this.buttonOpenEntityRegionFile.Click += new System.EventHandler(this.buttonOpenEntityRegionFile_Click);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 494);
            this.Controls.Add(this.buttonOpenEntityRegionFile);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listViewChunks);
            this.Controls.Add(this.buttonOpenRegion);
            this.Controls.Add(this.buttonCopyMCPath);
            this.Controls.Add(this.buttonOpenLevelDat);
            this.Name = "Form1";
            this.Text = "Level Data Reader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.tabControl1.ResumeLayout(false);
            this.tabPageNBT.ResumeLayout(false);
            this.tabPageItemFinder.ResumeLayout(false);
            this.tabPageItemFinder.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public TreeView tree;
        private ImageList imageList1;
        private Button buttonOpenLevelDat;
        private OpenFileDialog openFileDialogLevelDat;
        private Button buttonCopyMCPath;
        private Button buttonOpenRegion;
        private OpenFileDialog openFileDialogRegion;
        private ListView listViewChunks;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TabControl tabControl1;
        private TabPage tabPageNBT;
        private TabPage tabPageItemFinder;
        private Button buttonChestFinderSearchSelected;
        private Label label3;
        private Label label2;
        private TextBox tbFinderID;
        private Label label1;
        private TextBox tbSearchResults;
        private ComboBox cbFinderInternalName;
        private ComboBox cbFinderName;
        private Panel panel1;
        private CheckBox checkBoxFinderEntities;
        private CheckBox checkBoxFinderBlockEntities;
        private Button buttonChestFinderSearchRegion;
        private Button buttonChestFinderSearchFolder;
        private FolderBrowserDialog folderBrowserSearch;
        private Button buttonOpenEntityRegionFile;
    }
}