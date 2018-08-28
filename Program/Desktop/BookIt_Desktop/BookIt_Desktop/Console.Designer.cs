namespace BookIt_Desktop
{
    partial class Console
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
            this.txtConsoleDisplay = new System.Windows.Forms.RichTextBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.btnEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtConsoleDisplay
            // 
            this.txtConsoleDisplay.Location = new System.Drawing.Point(12, 12);
            this.txtConsoleDisplay.Name = "txtConsoleDisplay";
            this.txtConsoleDisplay.ReadOnly = true;
            this.txtConsoleDisplay.Size = new System.Drawing.Size(500, 340);
            this.txtConsoleDisplay.TabIndex = 0;
            this.txtConsoleDisplay.Text = "";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(12, 360);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(436, 20);
            this.txtConsole.TabIndex = 1;
            // 
            // btnEnter
            // 
            this.btnEnter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnEnter.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnEnter.Location = new System.Drawing.Point(454, 360);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(53, 20);
            this.btnEnter.TabIndex = 2;
            this.btnEnter.Text = "Enter";
            this.btnEnter.UseVisualStyleBackColor = false;
            this.btnEnter.Click += new System.EventHandler(this.BtnEnter_Click);
            // 
            // Console
            // 
            this.AcceptButton = this.btnEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(524, 391);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.txtConsoleDisplay);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(540, 430);
            this.Name = "Console";
            this.Text = "Console";
            this.ResizeEnd += new System.EventHandler(this.Console_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtConsoleDisplay;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Button btnEnter;
    }
}