namespace Stability_Monitor_win32
{
    partial class Main_view
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
            this.Start_tests_bt = new System.Windows.Forms.Button();
            this.Stop_tests_bt = new System.Windows.Forms.Button();
            this.Clear_list_of_tests_bt = new System.Windows.Forms.Button();
            this.List_of_tests_tb = new System.Windows.Forms.TextBox();
            this.Logs_tb = new System.Windows.Forms.TextBox();
            this.Add_new_test_cb = new System.Windows.Forms.ComboBox();
            this.List_of_tests_label = new System.Windows.Forms.TextBox();
            this.Logs_label = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Start_tests_bt
            // 
            this.Start_tests_bt.Location = new System.Drawing.Point(12, 12);
            this.Start_tests_bt.Name = "Start_tests_bt";
            this.Start_tests_bt.Size = new System.Drawing.Size(200, 30);
            this.Start_tests_bt.TabIndex = 0;
            this.Start_tests_bt.Text = "START TESTS";
            this.Start_tests_bt.UseVisualStyleBackColor = true;
            this.Start_tests_bt.Click += new System.EventHandler(this.Start_tests_bt_Click);
            // 
            // Stop_tests_bt
            // 
            this.Stop_tests_bt.Location = new System.Drawing.Point(12, 48);
            this.Stop_tests_bt.Name = "Stop_tests_bt";
            this.Stop_tests_bt.Size = new System.Drawing.Size(200, 30);
            this.Stop_tests_bt.TabIndex = 1;
            this.Stop_tests_bt.Text = "STOP TESTS";
            this.Stop_tests_bt.UseVisualStyleBackColor = true;
            this.Stop_tests_bt.Click += new System.EventHandler(this.Stop_tests_bt_Click);
            // 
            // Clear_list_of_tests_bt
            // 
            this.Clear_list_of_tests_bt.Location = new System.Drawing.Point(12, 306);
            this.Clear_list_of_tests_bt.Name = "Clear_list_of_tests_bt";
            this.Clear_list_of_tests_bt.Size = new System.Drawing.Size(200, 30);
            this.Clear_list_of_tests_bt.TabIndex = 3;
            this.Clear_list_of_tests_bt.Text = "CLEAR LIST OF TESTS";
            this.Clear_list_of_tests_bt.UseVisualStyleBackColor = true;
            this.Clear_list_of_tests_bt.Click += new System.EventHandler(this.Clear_list_of_tests_bt_Click);
            // 
            // List_of_tests_tb
            // 
            this.List_of_tests_tb.Location = new System.Drawing.Point(12, 137);
            this.List_of_tests_tb.Multiline = true;
            this.List_of_tests_tb.Name = "List_of_tests_tb";
            this.List_of_tests_tb.ReadOnly = true;
            this.List_of_tests_tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.List_of_tests_tb.Size = new System.Drawing.Size(200, 163);
            this.List_of_tests_tb.TabIndex = 4;
            this.List_of_tests_tb.TabStop = false;
            // 
            // Logs_tb
            // 
            this.Logs_tb.Location = new System.Drawing.Point(218, 38);
            this.Logs_tb.Multiline = true;
            this.Logs_tb.Name = "Logs_tb";
            this.Logs_tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Logs_tb.Size = new System.Drawing.Size(615, 298);
            this.Logs_tb.TabIndex = 5;
            this.Logs_tb.TabStop = false;
            // 
            // Add_new_test_cb
            // 
            this.Add_new_test_cb.FormattingEnabled = true;
            this.Add_new_test_cb.Items.AddRange(new object[] {
            "Test 1 - Wi-Fi/Send",
            "Test 2 - Wi-Fi/Receive",
            "Test 3 - Bluetooth/Send",
            "Test 4 - Bluetooth/Receive"});
            this.Add_new_test_cb.Location = new System.Drawing.Point(12, 84);
            this.Add_new_test_cb.Name = "Add_new_test_cb";
            this.Add_new_test_cb.Size = new System.Drawing.Size(200, 21);
            this.Add_new_test_cb.TabIndex = 2;
            this.Add_new_test_cb.Text = "                   ADD NEW TEST";
            this.Add_new_test_cb.SelectedIndexChanged += new System.EventHandler(this.Add_new_test_cb_SelectedIndexChanged);
            // 
            // List_of_tests_label
            // 
            this.List_of_tests_label.Location = new System.Drawing.Point(12, 111);
            this.List_of_tests_label.Name = "List_of_tests_label";
            this.List_of_tests_label.ReadOnly = true;
            this.List_of_tests_label.Size = new System.Drawing.Size(200, 20);
            this.List_of_tests_label.TabIndex = 7;
            this.List_of_tests_label.TabStop = false;
            this.List_of_tests_label.Text = "LIST OF TESTS";
            this.List_of_tests_label.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Logs_label
            // 
            this.Logs_label.Location = new System.Drawing.Point(218, 12);
            this.Logs_label.Name = "Logs_label";
            this.Logs_label.ReadOnly = true;
            this.Logs_label.Size = new System.Drawing.Size(615, 20);
            this.Logs_label.TabIndex = 8;
            this.Logs_label.TabStop = false;
            this.Logs_label.Text = "LOGS";
            this.Logs_label.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Main_view
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 348);
            this.Controls.Add(this.Logs_label);
            this.Controls.Add(this.List_of_tests_label);
            this.Controls.Add(this.Add_new_test_cb);
            this.Controls.Add(this.Logs_tb);
            this.Controls.Add(this.List_of_tests_tb);
            this.Controls.Add(this.Clear_list_of_tests_bt);
            this.Controls.Add(this.Stop_tests_bt);
            this.Controls.Add(this.Start_tests_bt);
            this.Name = "Main_view";
            this.Text = "Main view";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start_tests_bt;
        private System.Windows.Forms.Button Stop_tests_bt;
        private System.Windows.Forms.Button Clear_list_of_tests_bt;
        public System.Windows.Forms.TextBox List_of_tests_tb;
        public System.Windows.Forms.TextBox Logs_tb;
        private System.Windows.Forms.ComboBox Add_new_test_cb;
        private System.Windows.Forms.TextBox List_of_tests_label;
        private System.Windows.Forms.TextBox Logs_label;
    }
}

