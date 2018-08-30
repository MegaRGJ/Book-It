namespace BookIt_Desktop
{
    partial class Main
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
            this.lblLoggedName = new System.Windows.Forms.Label();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.pnlBookings = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblLoggedName
            // 
            this.lblLoggedName.AutoSize = true;
            this.lblLoggedName.Location = new System.Drawing.Point(799, 9);
            this.lblLoggedName.Name = "lblLoggedName";
            this.lblLoggedName.Size = new System.Drawing.Size(71, 13);
            this.lblLoggedName.TabIndex = 0;
            this.lblLoggedName.Text = "Logged in as ";
            // 
            // pnlMenu
            // 
            this.pnlMenu.Location = new System.Drawing.Point(10, 25);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(205, 560);
            this.pnlMenu.TabIndex = 1;
            // 
            // pnlBookings
            // 
            this.pnlBookings.Location = new System.Drawing.Point(220, 25);
            this.pnlBookings.Name = "pnlBookings";
            this.pnlBookings.Size = new System.Drawing.Size(650, 560);
            this.pnlBookings.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 611);
            this.Controls.Add(this.pnlBookings);
            this.Controls.Add(this.pnlMenu);
            this.Controls.Add(this.lblLoggedName);
            this.MaximumSize = new System.Drawing.Size(1080, 720);
            this.MinimumSize = new System.Drawing.Size(500, 200);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoggedName;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel pnlBookings;
    }
}