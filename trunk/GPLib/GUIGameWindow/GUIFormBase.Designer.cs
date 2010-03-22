/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
namespace GUIGameWindow
{
    partial class GUIFormBase
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
            this.GUIRootPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // GUIRootPanel
            // 
            this.GUIRootPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GUIRootPanel.BackColor = System.Drawing.Color.Transparent;
            this.GUIRootPanel.Location = new System.Drawing.Point(0, 0);
            this.GUIRootPanel.Name = "GUIRootPanel";
            this.GUIRootPanel.Size = new System.Drawing.Size(350, 150);
            this.GUIRootPanel.TabIndex = 0;
            // 
            // GUIFormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 150);
            this.Controls.Add(this.GUIRootPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "GUIFormBase";
            this.Text = "GUIFormBase";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel GUIRootPanel;
    }
}