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
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList1;
            this.tree.Location = new System.Drawing.Point(143, 46);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(600, 440);
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
            this.buttonOpenLevelDat.Location = new System.Drawing.Point(143, 10);
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
            this.buttonCopyMCPath.Location = new System.Drawing.Point(614, 10);
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
            this.listViewChunks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewChunks.FullRowSelect = true;
            this.listViewChunks.Location = new System.Drawing.Point(12, 46);
            this.listViewChunks.MultiSelect = false;
            this.listViewChunks.Name = "listViewChunks";
            this.listViewChunks.Size = new System.Drawing.Size(125, 440);
            this.listViewChunks.TabIndex = 5;
            this.listViewChunks.UseCompatibleStateImageBehavior = false;
            this.listViewChunks.View = System.Windows.Forms.View.Details;
            this.listViewChunks.SelectedIndexChanged += new System.EventHandler(this.listViewChunks_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "X";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Z";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 498);
            this.Controls.Add(this.listViewChunks);
            this.Controls.Add(this.buttonOpenRegion);
            this.Controls.Add(this.buttonCopyMCPath);
            this.Controls.Add(this.buttonOpenLevelDat);
            this.Controls.Add(this.tree);
            this.Name = "Form1";
            this.Text = "Level Data Reader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
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
    }
}