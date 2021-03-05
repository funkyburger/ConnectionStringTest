
namespace ConnectionStringTest.UI
{
    partial class MainTestControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.connectionStringBox = new System.Windows.Forms.TextBox();
            this.fireTestButton = new System.Windows.Forms.Button();
            this.testResultLabel = new System.Windows.Forms.Label();
            this.statusIcon = new System.Windows.Forms.PictureBox();
            this.timeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.statusIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // connectionStringBox
            // 
            this.connectionStringBox.Location = new System.Drawing.Point(3, 3);
            this.connectionStringBox.Name = "connectionStringBox";
            this.connectionStringBox.Size = new System.Drawing.Size(776, 20);
            this.connectionStringBox.TabIndex = 1;
            this.connectionStringBox.TextChanged += new System.EventHandler(this.connectionStringBox_TextChanged);
            // 
            // fireTestButton
            // 
            this.fireTestButton.Location = new System.Drawing.Point(3, 29);
            this.fireTestButton.Name = "fireTestButton";
            this.fireTestButton.Size = new System.Drawing.Size(75, 23);
            this.fireTestButton.TabIndex = 2;
            this.fireTestButton.Text = "Test";
            this.fireTestButton.UseVisualStyleBackColor = true;
            this.fireTestButton.Click += new System.EventHandler(this.fireTestButton_Click);
            // 
            // testResultLabel
            // 
            this.testResultLabel.AutoSize = true;
            this.testResultLabel.Location = new System.Drawing.Point(84, 34);
            this.testResultLabel.Name = "testResultLabel";
            this.testResultLabel.Size = new System.Drawing.Size(87, 13);
            this.testResultLabel.TabIndex = 3;
            this.testResultLabel.Text = "-Test result label-";
            // 
            // statusIcon
            // 
            this.statusIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.statusIcon.Location = new System.Drawing.Point(762, 32);
            this.statusIcon.Name = "statusIcon";
            this.statusIcon.Size = new System.Drawing.Size(17, 17);
            this.statusIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.statusIcon.TabIndex = 4;
            this.statusIcon.TabStop = false;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(712, 34);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(34, 13);
            this.timeLabel.TabIndex = 5;
            this.timeLabel.Text = "0.000";
            // 
            // MainTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.statusIcon);
            this.Controls.Add(this.testResultLabel);
            this.Controls.Add(this.fireTestButton);
            this.Controls.Add(this.connectionStringBox);
            this.Name = "MainTestControl";
            this.Size = new System.Drawing.Size(785, 60);
            ((System.ComponentModel.ISupportInitialize)(this.statusIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connectionStringBox;
        private System.Windows.Forms.Button fireTestButton;
        private System.Windows.Forms.Label testResultLabel;
        private System.Windows.Forms.PictureBox statusIcon;
        private System.Windows.Forms.Label timeLabel;
    }
}
