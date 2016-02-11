namespace Stability_Monitor_win32
{
    partial class GUI_form
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
            this.Test_1_runner = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Test_1_runner
            // 
            this.Test_1_runner.Location = new System.Drawing.Point(12, 12);
            this.Test_1_runner.Name = "Test_1_runner";
            this.Test_1_runner.Size = new System.Drawing.Size(75, 23);
            this.Test_1_runner.TabIndex = 0;
            this.Test_1_runner.Text = "Run test 1";
            this.Test_1_runner.UseVisualStyleBackColor = true;
            this.Test_1_runner.Click += new System.EventHandler(this.Test_1_runner_Click);
            // 
            // GUI_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Test_1_runner);
            this.Name = "GUI_form";
            this.Text = "GUI_form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Test_1_runner;
    }
}

