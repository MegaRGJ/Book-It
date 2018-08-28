namespace BookIt_Desktop
{
    partial class Login
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
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblUserWarn = new System.Windows.Forms.Label();
            this.lblPassWarn = new System.Windows.Forms.Label();
            this.lblInvalid = new System.Windows.Forms.Label();
            this.lblCreate = new System.Windows.Forms.Label();
            this.lblConsole = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(97, 61);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(192, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextChanged += new System.EventHandler(this.TxtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(22, 64);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            // 
            // lblHelp
            // 
            this.lblHelp.AutoSize = true;
            this.lblHelp.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblHelp.Location = new System.Drawing.Point(337, 9);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(35, 13);
            this.lblHelp.TabIndex = 2;
            this.lblHelp.Text = "Help?";
            this.lblHelp.Click += new System.EventHandler(this.LblHelp_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(22, 115);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(97, 112);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(192, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(266, 163);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // lblUserWarn
            // 
            this.lblUserWarn.AutoSize = true;
            this.lblUserWarn.ForeColor = System.Drawing.Color.Red;
            this.lblUserWarn.Location = new System.Drawing.Point(260, 84);
            this.lblUserWarn.Name = "lblUserWarn";
            this.lblUserWarn.Size = new System.Drawing.Size(122, 13);
            this.lblUserWarn.TabIndex = 6;
            this.lblUserWarn.Text = "*Field must not be empty";
            this.lblUserWarn.Visible = false;
            // 
            // lblPassWarn
            // 
            this.lblPassWarn.AutoSize = true;
            this.lblPassWarn.ForeColor = System.Drawing.Color.Red;
            this.lblPassWarn.Location = new System.Drawing.Point(260, 135);
            this.lblPassWarn.Name = "lblPassWarn";
            this.lblPassWarn.Size = new System.Drawing.Size(122, 13);
            this.lblPassWarn.TabIndex = 7;
            this.lblPassWarn.Text = "*Field must not be empty";
            this.lblPassWarn.Visible = false;
            // 
            // lblInvalid
            // 
            this.lblInvalid.AutoSize = true;
            this.lblInvalid.ForeColor = System.Drawing.Color.Red;
            this.lblInvalid.Location = new System.Drawing.Point(12, 9);
            this.lblInvalid.Name = "lblInvalid";
            this.lblInvalid.Size = new System.Drawing.Size(172, 13);
            this.lblInvalid.TabIndex = 8;
            this.lblInvalid.Text = "Invalid credentials, please try again";
            this.lblInvalid.Visible = false;
            // 
            // lblCreate
            // 
            this.lblCreate.AutoSize = true;
            this.lblCreate.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblCreate.Location = new System.Drawing.Point(12, 194);
            this.lblCreate.Name = "lblCreate";
            this.lblCreate.Size = new System.Drawing.Size(81, 13);
            this.lblCreate.TabIndex = 9;
            this.lblCreate.Text = "Create Account";
            this.lblCreate.Click += new System.EventHandler(this.LblCreate_Click);
            // 
            // lblConsole
            // 
            this.lblConsole.AutoSize = true;
            this.lblConsole.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblConsole.Location = new System.Drawing.Point(332, 196);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(45, 13);
            this.lblConsole.TabIndex = 10;
            this.lblConsole.Text = "Console";
            this.lblConsole.Click += new System.EventHandler(this.LblConsole_Click);
            // 
            // Login
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(384, 216);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.lblCreate);
            this.Controls.Add(this.lblInvalid);
            this.Controls.Add(this.lblPassWarn);
            this.Controls.Add(this.lblUserWarn);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblUserWarn;
        private System.Windows.Forms.Label lblPassWarn;
        private System.Windows.Forms.Label lblInvalid;
        private System.Windows.Forms.Label lblCreate;
        private System.Windows.Forms.Label lblConsole;
    }
}

