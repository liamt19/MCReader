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
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imageList1;
            this.tree.Location = new System.Drawing.Point(12, 12);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 461);
            this.Controls.Add(this.tree);
            this.Name = "Form1";
            this.Text = "MCReaderForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        public TreeView tree;
        private ImageList imageList1;
    }
}