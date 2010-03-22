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
namespace MD3ReaderTest
{
    partial class View
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
            this.glControl1 = new OpenTK.GLControl();
            this.torsoList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SlowMo = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.LegTorsoSpin = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.LegTorsoTilt = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LegsFrame = new System.Windows.Forms.TextBox();
            this.legsForceLoop = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.legsList = new System.Windows.Forms.ComboBox();
            this.LegsLoopFrame = new System.Windows.Forms.TextBox();
            this.LegsStartFrame = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.LegsEndFrame = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TorsoFrame = new System.Windows.Forms.TextBox();
            this.torsoForceLoop = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TorsoLoopFrame = new System.Windows.Forms.TextBox();
            this.TorsoStartFrame = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TorsoEndFrame = new System.Windows.Forms.TextBox();
            this.FrameTime = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LegTorsoSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LegTorsoTilt)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(0, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(890, 642);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseClick);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // torsoList
            // 
            this.torsoList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.torsoList.FormattingEnabled = true;
            this.torsoList.Location = new System.Drawing.Point(6, 35);
            this.torsoList.Name = "torsoList";
            this.torsoList.Size = new System.Drawing.Size(121, 21);
            this.torsoList.TabIndex = 1;
            this.torsoList.SelectedIndexChanged += new System.EventHandler(this.torsoList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sequence";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.FrameTime);
            this.panel1.Controls.Add(this.SlowMo);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.LegTorsoSpin);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.LegTorsoTilt);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(896, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(163, 642);
            this.panel1.TabIndex = 3;
            // 
            // SlowMo
            // 
            this.SlowMo.AutoSize = true;
            this.SlowMo.Location = new System.Drawing.Point(11, 587);
            this.SlowMo.Name = "SlowMo";
            this.SlowMo.Size = new System.Drawing.Size(67, 17);
            this.SlowMo.TabIndex = 15;
            this.SlowMo.Text = "Slow-Mo";
            this.SlowMo.UseVisualStyleBackColor = true;
            this.SlowMo.CheckedChanged += new System.EventHandler(this.SlowMo_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 529);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Leg-Torso Rotoate";
            // 
            // LegTorsoSpin
            // 
            this.LegTorsoSpin.Location = new System.Drawing.Point(11, 545);
            this.LegTorsoSpin.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.LegTorsoSpin.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.LegTorsoSpin.Name = "LegTorsoSpin";
            this.LegTorsoSpin.Size = new System.Drawing.Size(120, 20);
            this.LegTorsoSpin.TabIndex = 13;
            this.LegTorsoSpin.ValueChanged += new System.EventHandler(this.LegTorsoSpin_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 490);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Leg-Torso Fore-Back";
            // 
            // LegTorsoTilt
            // 
            this.LegTorsoTilt.Location = new System.Drawing.Point(11, 506);
            this.LegTorsoTilt.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.LegTorsoTilt.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.LegTorsoTilt.Name = "LegTorsoTilt";
            this.LegTorsoTilt.Size = new System.Drawing.Size(120, 20);
            this.LegTorsoTilt.TabIndex = 11;
            this.LegTorsoTilt.ValueChanged += new System.EventHandler(this.LegTorsoTilt_ValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LegsFrame);
            this.groupBox2.Controls.Add(this.legsForceLoop);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.legsList);
            this.groupBox2.Controls.Add(this.LegsLoopFrame);
            this.groupBox2.Controls.Add(this.LegsStartFrame);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.LegsEndFrame);
            this.groupBox2.Location = new System.Drawing.Point(11, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 241);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legs";
            // 
            // LegsFrame
            // 
            this.LegsFrame.Location = new System.Drawing.Point(14, 206);
            this.LegsFrame.Name = "LegsFrame";
            this.LegsFrame.ReadOnly = true;
            this.LegsFrame.Size = new System.Drawing.Size(100, 20);
            this.LegsFrame.TabIndex = 11;
            // 
            // legsForceLoop
            // 
            this.legsForceLoop.AutoSize = true;
            this.legsForceLoop.Location = new System.Drawing.Point(14, 183);
            this.legsForceLoop.Name = "legsForceLoop";
            this.legsForceLoop.Size = new System.Drawing.Size(80, 17);
            this.legsForceLoop.TabIndex = 10;
            this.legsForceLoop.Text = "Force Loop";
            this.legsForceLoop.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Sequence";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Loop Frame";
            // 
            // legsList
            // 
            this.legsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.legsList.FormattingEnabled = true;
            this.legsList.Location = new System.Drawing.Point(6, 35);
            this.legsList.Name = "legsList";
            this.legsList.Size = new System.Drawing.Size(121, 21);
            this.legsList.TabIndex = 1;
            this.legsList.SelectedIndexChanged += new System.EventHandler(this.legsList_SelectedIndexChanged);
            // 
            // LegsLoopFrame
            // 
            this.LegsLoopFrame.Location = new System.Drawing.Point(13, 157);
            this.LegsLoopFrame.Name = "LegsLoopFrame";
            this.LegsLoopFrame.ReadOnly = true;
            this.LegsLoopFrame.Size = new System.Drawing.Size(113, 20);
            this.LegsLoopFrame.TabIndex = 7;
            // 
            // LegsStartFrame
            // 
            this.LegsStartFrame.Location = new System.Drawing.Point(14, 79);
            this.LegsStartFrame.Name = "LegsStartFrame";
            this.LegsStartFrame.ReadOnly = true;
            this.LegsStartFrame.Size = new System.Drawing.Size(113, 20);
            this.LegsStartFrame.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "End Frame";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Start Frame";
            // 
            // LegsEndFrame
            // 
            this.LegsEndFrame.Location = new System.Drawing.Point(14, 118);
            this.LegsEndFrame.Name = "LegsEndFrame";
            this.LegsEndFrame.ReadOnly = true;
            this.LegsEndFrame.Size = new System.Drawing.Size(113, 20);
            this.LegsEndFrame.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TorsoFrame);
            this.groupBox1.Controls.Add(this.torsoForceLoop);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.torsoList);
            this.groupBox1.Controls.Add(this.TorsoLoopFrame);
            this.groupBox1.Controls.Add(this.TorsoStartFrame);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TorsoEndFrame);
            this.groupBox1.Location = new System.Drawing.Point(11, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 237);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Torso";
            // 
            // TorsoFrame
            // 
            this.TorsoFrame.Location = new System.Drawing.Point(14, 207);
            this.TorsoFrame.Name = "TorsoFrame";
            this.TorsoFrame.ReadOnly = true;
            this.TorsoFrame.Size = new System.Drawing.Size(100, 20);
            this.TorsoFrame.TabIndex = 10;
            // 
            // torsoForceLoop
            // 
            this.torsoForceLoop.AutoSize = true;
            this.torsoForceLoop.Location = new System.Drawing.Point(14, 184);
            this.torsoForceLoop.Name = "torsoForceLoop";
            this.torsoForceLoop.Size = new System.Drawing.Size(80, 17);
            this.torsoForceLoop.TabIndex = 9;
            this.torsoForceLoop.Text = "Force Loop";
            this.torsoForceLoop.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Loop Frame";
            // 
            // TorsoLoopFrame
            // 
            this.TorsoLoopFrame.Location = new System.Drawing.Point(13, 157);
            this.TorsoLoopFrame.Name = "TorsoLoopFrame";
            this.TorsoLoopFrame.ReadOnly = true;
            this.TorsoLoopFrame.Size = new System.Drawing.Size(113, 20);
            this.TorsoLoopFrame.TabIndex = 7;
            // 
            // TorsoStartFrame
            // 
            this.TorsoStartFrame.Location = new System.Drawing.Point(14, 79);
            this.TorsoStartFrame.Name = "TorsoStartFrame";
            this.TorsoStartFrame.ReadOnly = true;
            this.TorsoStartFrame.Size = new System.Drawing.Size(113, 20);
            this.TorsoStartFrame.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End Frame";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Start Frame";
            // 
            // TorsoEndFrame
            // 
            this.TorsoEndFrame.Location = new System.Drawing.Point(14, 118);
            this.TorsoEndFrame.Name = "TorsoEndFrame";
            this.TorsoEndFrame.ReadOnly = true;
            this.TorsoEndFrame.Size = new System.Drawing.Size(113, 20);
            this.TorsoEndFrame.TabIndex = 5;
            // 
            // FrameTime
            // 
            this.FrameTime.Location = new System.Drawing.Point(11, 614);
            this.FrameTime.Name = "FrameTime";
            this.FrameTime.ReadOnly = true;
            this.FrameTime.Size = new System.Drawing.Size(113, 20);
            this.FrameTime.TabIndex = 12;
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1058, 646);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.glControl1);
            this.Name = "View";
            this.Text = "View";
            this.Load += new System.EventHandler(this.View_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LegTorsoSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LegTorsoTilt)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.ComboBox torsoList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TorsoLoopFrame;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TorsoEndFrame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TorsoStartFrame;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox legsList;
        private System.Windows.Forms.TextBox LegsLoopFrame;
        private System.Windows.Forms.TextBox LegsStartFrame;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LegsEndFrame;
        private System.Windows.Forms.CheckBox legsForceLoop;
        private System.Windows.Forms.CheckBox torsoForceLoop;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown LegTorsoSpin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown LegTorsoTilt;
        private System.Windows.Forms.TextBox LegsFrame;
        private System.Windows.Forms.TextBox TorsoFrame;
        private System.Windows.Forms.CheckBox SlowMo;
        private System.Windows.Forms.TextBox FrameTime;
    }
}