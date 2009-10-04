namespace PortalEdit
{
    partial class MapImageSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ImageFileName = new System.Windows.Forms.TextBox();
            this.BrowseImage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ImageInfo = new System.Windows.Forms.Label();
            this.PixelsPerUnit = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CenterY = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.CenterX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CenterY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CenterX)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File";
            // 
            // ImageFileName
            // 
            this.ImageFileName.Location = new System.Drawing.Point(35, 13);
            this.ImageFileName.Name = "ImageFileName";
            this.ImageFileName.Size = new System.Drawing.Size(224, 20);
            this.ImageFileName.TabIndex = 1;
            this.ImageFileName.TextChanged += new System.EventHandler(this.ImageFileName_TextChanged);
            // 
            // BrowseImage
            // 
            this.BrowseImage.Location = new System.Drawing.Point(265, 10);
            this.BrowseImage.Name = "BrowseImage";
            this.BrowseImage.Size = new System.Drawing.Size(26, 23);
            this.BrowseImage.TabIndex = 2;
            this.BrowseImage.Text = "...";
            this.BrowseImage.UseVisualStyleBackColor = true;
            this.BrowseImage.Click += new System.EventHandler(this.BrowseImage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Pixels Per Unit";
            // 
            // ImageInfo
            // 
            this.ImageInfo.AutoSize = true;
            this.ImageInfo.Location = new System.Drawing.Point(8, 38);
            this.ImageInfo.Name = "ImageInfo";
            this.ImageInfo.Size = new System.Drawing.Size(53, 13);
            this.ImageInfo.TabIndex = 4;
            this.ImageInfo.Text = "No Image";
            // 
            // PixelsPerUnit
            // 
            this.PixelsPerUnit.Location = new System.Drawing.Point(87, 24);
            this.PixelsPerUnit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PixelsPerUnit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PixelsPerUnit.Name = "PixelsPerUnit";
            this.PixelsPerUnit.Size = new System.Drawing.Size(59, 20);
            this.PixelsPerUnit.TabIndex = 5;
            this.PixelsPerUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CenterY);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.CenterX);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(4, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(123, 100);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Origin";
            // 
            // CenterY
            // 
            this.CenterY.DecimalPlaces = 2;
            this.CenterY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.CenterY.Location = new System.Drawing.Point(26, 58);
            this.CenterY.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.CenterY.Name = "CenterY";
            this.CenterY.Size = new System.Drawing.Size(77, 20);
            this.CenterY.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Y";
            // 
            // CenterX
            // 
            this.CenterX.DecimalPlaces = 2;
            this.CenterX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.CenterX.Location = new System.Drawing.Point(26, 32);
            this.CenterX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.CenterX.Name = "CenterX";
            this.CenterX.Size = new System.Drawing.Size(77, 20);
            this.CenterX.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Image Center";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.PixelsPerUnit);
            this.groupBox2.Location = new System.Drawing.Point(133, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 61);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scale";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ImageInfo);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.ImageFileName);
            this.groupBox3.Controls.Add(this.BrowseImage);
            this.groupBox3.Location = new System.Drawing.Point(4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(300, 64);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Image";
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(229, 145);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 9;
            this.OK.Text = "Apply";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // MapImageSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 180);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MapImageSetup";
            this.Text = "Map Image Underlay Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapImageSetup_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CenterY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CenterX)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ImageFileName;
        private System.Windows.Forms.Button BrowseImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ImageInfo;
        private System.Windows.Forms.NumericUpDown PixelsPerUnit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown CenterY;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown CenterX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button OK;
    }
}