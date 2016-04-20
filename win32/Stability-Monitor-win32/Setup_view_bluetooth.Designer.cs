namespace Stability_Monitor_win32
{
    partial class Setup_view_bluetooth
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
            this.Filepath_label = new System.Windows.Forms.TextBox();
            this.Filepath_tb = new System.Windows.Forms.TextBox();
            this.Name_of_device_tb = new System.Windows.Forms.TextBox();
            this.Name_of_device_label = new System.Windows.Forms.TextBox();
            this.Uuid_tb = new System.Windows.Forms.TextBox();
            this.Uuid_label = new System.Windows.Forms.TextBox();
            this.Add_test_bt = new System.Windows.Forms.Button();
            this.Cancel_bt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Filepath_label
            // 
            this.Filepath_label.Location = new System.Drawing.Point(12, 12);
            this.Filepath_label.Name = "Filepath_label";
            this.Filepath_label.ReadOnly = true;
            this.Filepath_label.Size = new System.Drawing.Size(62, 20);
            this.Filepath_label.TabIndex = 1;
            this.Filepath_label.TabStop = false;
            this.Filepath_label.Text = "FILEPATH:";
            // 
            // Filepath_tb
            // 
            this.Filepath_tb.Location = new System.Drawing.Point(80, 12);
            this.Filepath_tb.Name = "Filepath_tb";
            this.Filepath_tb.Size = new System.Drawing.Size(187, 20);
            this.Filepath_tb.TabIndex = 0;
            this.Filepath_tb.Text = "C:\\Stability-Monitor-win32\\";
            // 
            // Name_of_device_tb
            // 
            this.Name_of_device_tb.Location = new System.Drawing.Point(120, 53);
            this.Name_of_device_tb.Name = "Name_of_device_tb";
            this.Name_of_device_tb.Size = new System.Drawing.Size(147, 20);
            this.Name_of_device_tb.TabIndex = 1;
            // 
            // Name_of_device_label
            // 
            this.Name_of_device_label.Location = new System.Drawing.Point(12, 53);
            this.Name_of_device_label.Name = "Name_of_device_label";
            this.Name_of_device_label.ReadOnly = true;
            this.Name_of_device_label.Size = new System.Drawing.Size(102, 20);
            this.Name_of_device_label.TabIndex = 19;
            this.Name_of_device_label.TabStop = false;
            this.Name_of_device_label.Text = "NAME OF DEVICE:";
            // 
            // Uuid_tb
            // 
            this.Uuid_tb.Location = new System.Drawing.Point(120, 79);
            this.Uuid_tb.Name = "Uuid_tb";
            this.Uuid_tb.Size = new System.Drawing.Size(147, 20);
            this.Uuid_tb.TabIndex = 2;
            // 
            // Uuid_label
            // 
            this.Uuid_label.Location = new System.Drawing.Point(12, 79);
            this.Uuid_label.Name = "Uuid_label";
            this.Uuid_label.ReadOnly = true;
            this.Uuid_label.Size = new System.Drawing.Size(102, 20);
            this.Uuid_label.TabIndex = 17;
            this.Uuid_label.TabStop = false;
            this.Uuid_label.Text = "UUID:";
            // 
            // Add_test_bt
            // 
            this.Add_test_bt.Location = new System.Drawing.Point(147, 105);
            this.Add_test_bt.Name = "Add_test_bt";
            this.Add_test_bt.Size = new System.Drawing.Size(120, 30);
            this.Add_test_bt.TabIndex = 3;
            this.Add_test_bt.Text = "ADD TEST";
            this.Add_test_bt.UseVisualStyleBackColor = true;
            this.Add_test_bt.Click += new System.EventHandler(this.Add_test_bt_Click);
            // 
            // Cancel_bt
            // 
            this.Cancel_bt.Location = new System.Drawing.Point(12, 105);
            this.Cancel_bt.Name = "Cancel_bt";
            this.Cancel_bt.Size = new System.Drawing.Size(120, 30);
            this.Cancel_bt.TabIndex = 4;
            this.Cancel_bt.Text = "CANCEL";
            this.Cancel_bt.UseVisualStyleBackColor = true;
            this.Cancel_bt.Click += new System.EventHandler(this.Cancel_bt_Click);
            // 
            // Setup_view_bluetooth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 149);
            this.Controls.Add(this.Cancel_bt);
            this.Controls.Add(this.Name_of_device_tb);
            this.Controls.Add(this.Name_of_device_label);
            this.Controls.Add(this.Uuid_tb);
            this.Controls.Add(this.Uuid_label);
            this.Controls.Add(this.Add_test_bt);
            this.Controls.Add(this.Filepath_tb);
            this.Controls.Add(this.Filepath_label);
            this.Name = "Setup_view_bluetooth";
            this.Text = "Setup view bluetooth";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Filepath_label;
        private System.Windows.Forms.TextBox Filepath_tb;
        private System.Windows.Forms.TextBox Name_of_device_label;
        private System.Windows.Forms.Button Add_test_bt;
        private System.Windows.Forms.TextBox Uuid_label;
        private System.Windows.Forms.TextBox Uuid_tb;
        private System.Windows.Forms.TextBox Name_of_device_tb;
        private System.Windows.Forms.Button Cancel_bt;
    }
}