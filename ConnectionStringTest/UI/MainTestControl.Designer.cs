
using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainTestControl));
            this.connectionStringBox = new System.Windows.Forms.TextBox();
            this.testResultLabel = DiContainer.Resolve<ResultLabel>();
            this.statusIcon = new System.Windows.Forms.PictureBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.actionButton = new ConnectionStringTest.UI.ActionButton();
            ((System.ComponentModel.ISupportInitialize)(this.statusIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // connectionStringBox
            // 
            this.connectionStringBox.Location = new System.Drawing.Point(3, 3);
            this.connectionStringBox.Name = "connectionStringBox";
            this.connectionStringBox.Size = new System.Drawing.Size(753, 20);
            this.connectionStringBox.TabIndex = 1;
            this.connectionStringBox.TextChanged += new System.EventHandler(this.connectionStringBox_TextChanged);
            
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
            // actionButton
            // 
            this.actionButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("actionButton.BackgroundImage")));
            this.actionButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.actionButton.CausesValidation = false;
            this.actionButton.CurrentAction = ConnectionStringTest.UI.ActionButton.Action.FireTest;
            this.actionButton.Location = new System.Drawing.Point(757, 2);
            this.actionButton.Margin = new System.Windows.Forms.Padding(0);
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(22, 22);
            this.actionButton.TabIndex = 6;
            this.actionButton.UseVisualStyleBackColor = true;
            this.actionButton.Click += new System.EventHandler(this.actionButtonClicked);
            // 
            // MainTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actionButton);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.statusIcon);
            this.Controls.Add(this.testResultLabel);
            this.Controls.Add(this.connectionStringBox);
            this.Name = "MainTestControl";
            this.Size = new System.Drawing.Size(785, 60);
            ((System.ComponentModel.ISupportInitialize)(this.statusIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connectionStringBox;
        private ResultLabel testResultLabel;
        private System.Windows.Forms.PictureBox statusIcon;
        private System.Windows.Forms.Label timeLabel;
        private ActionButton actionButton;
    }
}
