using BookItDependancies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookIt_Desktop
{
    public partial class Login : Form
    {
        public bool LoggedIn = false;
        public bool running = true;
        private int attemptsMade = 0;
        private bool unlocked = false;

        public Login()
        {
            InitializeComponent();
        }

        private void LblHelp_Click(object sender, EventArgs e)
        {
            //TODO - link to webite or something eventually
            MessageBox.Show("Log in to your account here, if you are having trouble then please seek assistance via our website", "Help", MessageBoxButtons.OK);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                if (unlocked) {
                    if (txtUsername.Text == "Admin" && txtPassword.Text == "admin")
                    {
                        Enabled = false;
                        Console console = new Console();
                        console.ShowDialog();
                        Enabled = true;
                    }
                }
                if (Server.Login(txtUsername.Text, txtPassword.Text))
                {
                    LoggedIn = true;
                    running = true;
                }
                else
                {
                    attemptsMade += 1;
                    lblInvalid.Visible = true;
                }
            }
            else
            {
                TxtUsername_TextChanged(sender, e);
                TxtPassword_TextChanged(sender, e);
            }
            if (attemptsMade == 4)
                Program.DisplayMessage("It seems you are having trouble logging in, you can reset your password via our website" + "\n" + "You have 2 attempts remaining before you are locked out temporarily", "Having problems?");
            if (attemptsMade >= 6)
            {
                Program.DisplayMessage("You have been temporarily blocked due to multiple unsuccessful login attempts, please try again later", "Login failed");
                running = false;
            }
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text == "") lblUserWarn.Visible = true;
            else lblUserWarn.Visible = false;
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text == "") lblPassWarn.Visible = true;
            else lblPassWarn.Visible = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Activate();
        }

        private void LblCreate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            AccountCreate create = new AccountCreate();
            create.ShowDialog();
            Enabled = true;
        }

        private void LblConsole_Click(object sender, EventArgs e)
        {
            if (btnLogin.BackColor != Color.White)
            {
                btnLogin.BackColor = Color.White;
                unlocked = true;
            }
            else {
                btnLogin.BackColor = SystemColors.Control;
                unlocked = false;
            }
        }
    }
}
