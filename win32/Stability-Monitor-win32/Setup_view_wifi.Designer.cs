namespace Stability_Monitor_win32
{
    partial class Setup_view_wifi
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
            this.Add_test_bt = new System.Windows.Forms.Button();
            this.Port_tb = new System.Windows.Forms.TextBox();
            this.Port_label = new System.Windows.Forms.TextBox();
            this.Name_of_wifi_tb = new System.Windows.Forms.TextBox();
            this.Name_of_wifi_label = new System.Windows.Forms.TextBox();
            this.Ip_address_tb = new System.Windows.Forms.TextBox();
            this.Ip_address_label = new System.Windows.Forms.TextBox();
            this.Filepath_tb = new System.Windows.Forms.TextBox();
            this.Filepath_label = new System.Windows.Forms.TextBox();
            this.Cancel_bt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Add_test_bt
            // 
            this.Add_test_bt.Location = new System.Drawing.Point(137, 154);
            this.Add_test_bt.Name = "Add_test_bt";
            this.Add_test_bt.Size = new System.Drawing.Size(110, 30);
            this.Add_test_bt.TabIndex = 4;
            this.Add_test_bt.Text = "ADD TEST";
            this.Add_test_bt.UseVisualStyleBackColor = true;
            this.Add_test_bt.Click += new System.EventHandler(this.Add_test_bt_Click);
            // 
            // Port_tb
            // 
            this.Port_tb.Location = new System.Drawing.Point(107, 104);
            this.Port_tb.Name = "Port_tb";
            this.Port_tb.Size = new System.Drawing.Size(140, 20);
            this.Port_tb.TabIndex = 3;
            // 
            // Port_label
            // 
            this.Port_label.Location = new System.Drawing.Point(12, 104);
            this.Port_label.Name = "Port_label";
            this.Port_label.ReadOnly = true;
            this.Port_label.Size = new System.Drawing.Size(89, 20);
            this.Port_label.TabIndex = 14;
            this.Port_label.TabStop = false;
            this.Port_label.Text = "PORT:";
            // 
            // Name_of_wifi_tb
            // 
            this.Name_of_wifi_tb.Location = new System.Drawing.Point(107, 52);
            this.Name_of_wifi_tb.Name = "Name_of_wifi_tb";
            this.Name_of_wifi_tb.Size = new System.Drawing.Size(140, 20);
            this.Name_of_wifi_tb.TabIndex = 1;
            // 
            // Name_of_wifi_label
            // 
            this.Name_of_wifi_label.Location = new System.Drawing.Point(12, 52);
            this.Name_of_wifi_label.Name = "Name_of_wifi_label";
            this.Name_of_wifi_label.ReadOnly = true;
            this.Name_of_wifi_label.Size = new System.Drawing.Size(89, 20);
            this.Name_of_wifi_label.TabIndex = 12;
            this.Name_of_wifi_label.TabStop = false;
            this.Name_of_wifi_label.Text = "NAME OF WI-FI:";
            // 
            // Ip_address_tb
            // 
            this.Ip_address_tb.Location = new System.Drawing.Point(107, 78);
            this.Ip_address_tb.Name = "Ip_address_tb";
            this.Ip_address_tb.Size = new System.Drawing.Size(140, 20);
            this.Ip_address_tb.TabIndex = 2;
            // 
            // Ip_address_label
            // 
            this.Ip_address_label.Location = new System.Drawing.Point(12, 78);
            this.Ip_address_label.Name = "Ip_address_label";
            this.Ip_address_label.ReadOnly = true;
            this.Ip_address_label.Size = new System.Drawing.Size(89, 20);
            this.Ip_address_label.TabIndex = 10;
            this.Ip_address_label.TabStop = false;
            this.Ip_address_label.Text = "IP ADDRESS:";
            // 
            // Filepath_tb
            // 
            this.Filepath_tb.Location = new System.Drawing.Point(80, 12);
            this.Filepath_tb.Name = "Filepath_tb";
            this.Filepath_tb.Size = new System.Drawing.Size(167, 20);
            this.Filepath_tb.TabIndex = 0;
            this.Filepath_tb.Text = "C:\\Stability-Monitor-win32\\";
            // 
            // Filepath_label
            // 
            this.Filepath_label.Location = new System.Drawing.Point(12, 12);
            this.Filepath_label.Name = "Filepath_label";
            this.Filepath_label.ReadOnly = true;
            this.Filepath_label.Size = new System.Drawing.Size(62, 20);
            this.Filepath_label.TabIndex = 5;
            this.Filepath_label.TabStop = false;
            this.Filepath_label.Text = "FILEPATH:";
            // 
            // Cancel_bt
            // 
            this.Cancel_bt.Location = new System.Drawing.Point(12, 154);
            this.Cancel_bt.Name = "Cancel_bt";
            this.Cancel_bt.Size = new System.Drawing.Size(110, 30);
            this.Cancel_bt.TabIndex = 5;
            this.Cancel_bt.Text = "CANCEL";
            this.Cancel_bt.UseVisualStyleBackColor = true;
            this.Cancel_bt.Click += new System.EventHandler(this.Cancel_bt_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "TCP: 1 - 9999 / UDP: 10000 - 65535";
            // 
            // Setup_view_wifi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 196);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel_bt);
            this.Controls.Add(this.Filepath_tb);
            this.Controls.Add(this.Filepath_label);
            this.Controls.Add(this.Port_tb);
            this.Controls.Add(this.Port_label);
            this.Controls.Add(this.Name_of_wifi_tb);
            this.Controls.Add(this.Name_of_wifi_label);
            this.Controls.Add(this.Ip_address_tb);
            this.Controls.Add(this.Ip_address_label);
            this.Controls.Add(this.Add_test_bt);
            this.Name = "Setup_view_wifi";
            this.Text = "Setup view wi-fi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Add_test_bt;
        private System.Windows.Forms.TextBox Port_tb;
        private System.Windows.Forms.TextBox Port_label;
        private System.Windows.Forms.TextBox Name_of_wifi_tb;
        private System.Windows.Forms.TextBox Name_of_wifi_label;
        private System.Windows.Forms.TextBox Ip_address_tb;
        private System.Windows.Forms.TextBox Ip_address_label;
        private System.Windows.Forms.TextBox Filepath_tb;
        private System.Windows.Forms.TextBox Filepath_label;
        private System.Windows.Forms.Button Cancel_bt;
        private System.Windows.Forms.Label label1;
    }
}