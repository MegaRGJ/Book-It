namespace BookIt_Desktop
{
    partial class AccountCreate
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtPostcode = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblPostcode = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblREPassword = new System.Windows.Forms.Label();
            this.txtREPassword = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRequired = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtPhoneArea = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(43, 21);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(174, 13);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Please enter your information below";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(151, 53);
            this.txtUsername.MaxLength = 50;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(212, 20);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.TextChanged += new System.EventHandler(this.TxtUsername_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(151, 93);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ShortcutsEnabled = false;
            this.txtPassword.Size = new System.Drawing.Size(212, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(151, 213);
            this.txtEmail.MaxLength = 150;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(212, 20);
            this.txtEmail.TabIndex = 6;
            this.txtEmail.Leave += new System.EventHandler(this.TxtEmail_LostFocus);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(151, 173);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(212, 20);
            this.txtName.TabIndex = 5;
            this.txtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(174, 333);
            this.txtPhone.MaxLength = 20;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(189, 20);
            this.txtPhone.TabIndex = 9;
            this.txtPhone.Leave += new System.EventHandler(this.TxtPhone_LostFocus);
            // 
            // txtPostcode
            // 
            this.txtPostcode.Location = new System.Drawing.Point(151, 293);
            this.txtPostcode.MaxLength = 20;
            this.txtPostcode.Name = "txtPostcode";
            this.txtPostcode.Size = new System.Drawing.Size(212, 20);
            this.txtPostcode.TabIndex = 8;
            this.txtPostcode.Leave += new System.EventHandler(this.TxtPostcode_LostFocus);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(151, 253);
            this.txtAddress.MaxLength = 200;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(212, 20);
            this.txtAddress.TabIndex = 7;
            this.txtAddress.Leave += new System.EventHandler(this.TxtAddress_LostFocus);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(56, 176);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(52, 13);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Full name";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(56, 216);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 10;
            this.lblEmail.Text = "Email";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(56, 256);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(45, 13);
            this.lblAddress.TabIndex = 11;
            this.lblAddress.Text = "Address";
            // 
            // lblPostcode
            // 
            this.lblPostcode.AutoSize = true;
            this.lblPostcode.Location = new System.Drawing.Point(56, 296);
            this.lblPostcode.Name = "lblPostcode";
            this.lblPostcode.Size = new System.Drawing.Size(52, 13);
            this.lblPostcode.TabIndex = 12;
            this.lblPostcode.Text = "Postcode";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(56, 336);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(38, 13);
            this.lblPhone.TabIndex = 13;
            this.lblPhone.Text = "Phone";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(56, 96);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 15;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(56, 56);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 14;
            this.lblUsername.Text = "Username";
            // 
            // lblREPassword
            // 
            this.lblREPassword.AutoSize = true;
            this.lblREPassword.Location = new System.Drawing.Point(56, 136);
            this.lblREPassword.Name = "lblREPassword";
            this.lblREPassword.Size = new System.Drawing.Size(89, 13);
            this.lblREPassword.TabIndex = 17;
            this.lblREPassword.Text = "Retype password";
            // 
            // txtREPassword
            // 
            this.txtREPassword.Location = new System.Drawing.Point(151, 133);
            this.txtREPassword.MaxLength = 50;
            this.txtREPassword.Name = "txtREPassword";
            this.txtREPassword.ShortcutsEnabled = false;
            this.txtREPassword.Size = new System.Drawing.Size(212, 20);
            this.txtREPassword.TabIndex = 4;
            this.txtREPassword.UseSystemPasswordChar = true;
            this.txtREPassword.TextChanged += new System.EventHandler(this.TxtREPassword_TextChanged);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(393, 377);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 10;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(48, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.lblRequired.Location = new System.Drawing.Point(370, 50);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(11, 13);
            this.lblRequired.TabIndex = 20;
            this.lblRequired.Text = "*";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label9.Location = new System.Drawing.Point(370, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "*";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label10.Location = new System.Drawing.Point(370, 176);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label11.Location = new System.Drawing.Point(370, 133);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label13.Location = new System.Drawing.Point(370, 210);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label14.Location = new System.Drawing.Point(370, 333);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(11, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label15.Location = new System.Drawing.Point(370, 290);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(11, 13);
            this.label15.TabIndex = 26;
            this.label15.Text = "*";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(453, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 9);
            this.label12.TabIndex = 28;
            this.label12.Text = "* Requried field";
            // 
            // txtPhoneArea
            // 
            this.txtPhoneArea.Enabled = false;
            this.txtPhoneArea.Location = new System.Drawing.Point(151, 333);
            this.txtPhoneArea.MaxLength = 20;
            this.txtPhoneArea.Name = "txtPhoneArea";
            this.txtPhoneArea.Size = new System.Drawing.Size(25, 20);
            this.txtPhoneArea.TabIndex = 29;
            this.txtPhoneArea.Text = "+44";
            // 
            // AccountCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 411);
            this.Controls.Add(this.txtPhoneArea);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblREPassword);
            this.Controls.Add(this.txtREPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblPostcode);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtPostcode);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblInfo);
            this.MaximumSize = new System.Drawing.Size(560, 450);
            this.MinimumSize = new System.Drawing.Size(560, 450);
            this.Name = "AccountCreate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account creation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtPostcode;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblPostcode;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblREPassword;
        private System.Windows.Forms.TextBox txtREPassword;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtPhoneArea;
    }
}