namespace PortalEdit
{
    partial class CellZSSet
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
            this.ForceDepth = new System.Windows.Forms.CheckBox();
            this.ZLabel = new System.Windows.Forms.Label();
            this.ZValue = new System.Windows.Forms.NumericUpDown();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.PresetList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ZValue)).BeginInit();
            this.SuspendLayout();
            // 
            // ForceDepth
            // 
            this.ForceDepth.AutoSize = true;
            this.ForceDepth.Location = new System.Drawing.Point(158, 12);
            this.ForceDepth.Name = "ForceDepth";
            this.ForceDepth.Size = new System.Drawing.Size(85, 17);
            this.ForceDepth.TabIndex = 0;
            this.ForceDepth.Text = "Force Depth";
            this.ForceDepth.UseVisualStyleBackColor = true;
            // 
            // ZLabel
            // 
            this.ZLabel.AutoSize = true;
            this.ZLabel.Location = new System.Drawing.Point(12, 9);
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(14, 13);
            this.ZLabel.TabIndex = 1;
            this.ZLabel.Text = "Z";
            // 
            // ZValue
            // 
            this.ZValue.DecimalPlaces = 4;
            this.ZValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.ZValue.Location = new System.Drawing.Point(32, 7);
            this.ZValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ZValue.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.ZValue.Name = "ZValue";
            this.ZValue.Size = new System.Drawing.Size(120, 20);
            this.ZValue.TabIndex = 2;
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(90, 69);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 3;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(173, 69);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // PresetList
            // 
            this.PresetList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PresetList.FormattingEnabled = true;
            this.PresetList.Location = new System.Drawing.Point(32, 34);
            this.PresetList.Name = "PresetList";
            this.PresetList.Size = new System.Drawing.Size(216, 21);
            this.PresetList.TabIndex = 5;
            this.PresetList.SelectedIndexChanged += new System.EventHandler(this.PresetList_SelectedIndexChanged);
            // 
            // CellZSSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 113);
            this.Controls.Add(this.PresetList);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.ZValue);
            this.Controls.Add(this.ZLabel);
            this.Controls.Add(this.ForceDepth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CellZSSet";
            this.Text = "Set Z Value";
            ((System.ComponentModel.ISupportInitialize)(this.ZValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ComboBox PresetList;
        public System.Windows.Forms.NumericUpDown ZValue;
        public System.Windows.Forms.CheckBox ForceDepth;
    }
}