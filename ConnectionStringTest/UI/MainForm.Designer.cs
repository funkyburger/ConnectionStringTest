﻿
namespace ConnectionStringTest.UI
{
    partial class MainForm
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
            this.mainTestControl = new ConnectionStringTest.UI.MainTestControl();
            this.SuspendLayout();
            // 
            // mainTestControl
            // 
            this.mainTestControl.Location = new System.Drawing.Point(3, 12);
            this.mainTestControl.Name = "mainTestControl";
            this.mainTestControl.Size = new System.Drawing.Size(785, 60);
            this.mainTestControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 71);
            this.Controls.Add(this.mainTestControl);
            this.MaximumSize = new System.Drawing.Size(802, 110);
            this.MinimumSize = new System.Drawing.Size(802, 110);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private MainTestControl mainTestControl;
    }
}