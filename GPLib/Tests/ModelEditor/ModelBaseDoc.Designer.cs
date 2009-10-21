namespace ModelEditor
{
    partial class ModelBaseDoc
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
            this.View = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // View
            // 
            this.View.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.View.BackColor = System.Drawing.Color.Black;
            this.View.Location = new System.Drawing.Point(2, 40);
            this.View.Name = "View";
            this.View.Size = new System.Drawing.Size(491, 295);
            this.View.TabIndex = 0;
            this.View.VSync = false;
            // 
            // ModelBaseDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 337);
            this.Controls.Add(this.View);
            this.Name = "ModelBaseDoc";
            this.Text = "ModelBaseDoc";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl View;
    }
}