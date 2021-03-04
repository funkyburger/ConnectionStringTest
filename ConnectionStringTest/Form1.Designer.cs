
namespace ConnectionStringTest
{
    partial class Form1
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
            this.connectionStringBox = new System.Windows.Forms.TextBox();
            this.fireTestButton = new System.Windows.Forms.Button();
            this.testResultLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connectionStringBox
            // 
            this.connectionStringBox.Location = new System.Drawing.Point(12, 12);
            this.connectionStringBox.Name = "connectionStringBox";
            this.connectionStringBox.Size = new System.Drawing.Size(776, 20);
            this.connectionStringBox.TabIndex = 0;
            // 
            // fireTestButton
            // 
            this.fireTestButton.Location = new System.Drawing.Point(13, 39);
            this.fireTestButton.Name = "fireTestButton";
            this.fireTestButton.Size = new System.Drawing.Size(75, 23);
            this.fireTestButton.TabIndex = 1;
            this.fireTestButton.Text = "Test";
            this.fireTestButton.UseVisualStyleBackColor = true;
            this.fireTestButton.Click += new System.EventHandler(this.testbutton_Click);
            // 
            // testResultLabel
            // 
            this.testResultLabel.AutoSize = true;
            this.testResultLabel.Location = new System.Drawing.Point(94, 44);
            this.testResultLabel.Name = "testResultLabel";
            this.testResultLabel.Size = new System.Drawing.Size(0, 13);
            this.testResultLabel.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 72);
            this.Controls.Add(this.testResultLabel);
            this.Controls.Add(this.fireTestButton);
            this.Controls.Add(this.connectionStringBox);
            this.Name = "Form1";
            this.Text = "Test connection string";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connectionStringBox;
        private System.Windows.Forms.Button fireTestButton;
        private System.Windows.Forms.Label testResultLabel;
    }
}

