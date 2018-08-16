using BookItDependancies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookIt_Desktop
{
    public partial class AccountCreate : Form
    {
        private bool validateSuccess = false;
        private List<Label> errorLabels = null;
        public AccountCreate()
        {
            InitializeComponent();
            CreateErrorLabels();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            Validation.ValidateInput("Username", txtUsername.Text);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
            Close();

        }
        #region Input validation
        private void CreateErrorLabels()
        {
            errorLabels = new List<Label>();
            //TODO - doesnt yet dispaly error labels
            Font f = new Font(lblInfo.Font.Name, lblInfo.Font.Size - 1);
            for (int n = 0; n < 8; n++)
            {
                Label la = new Label()
                {
                    Name = "errorLabel" + n.ToString(),
                    Font = f,
                    Text = "*test line here",
                    ForeColor = Color.Red,
                    //Visible = false,
                    Location = new Point(370, 50 + (40 * n)),
                    MaximumSize = new Size(this.Size.Width - 370 - 10,0),
                    AutoSize = true,
                    Visible = false
                };                
                la.TextChanged += new EventHandler(ErrorLabelUpdated);
                Controls.Add(la);
                errorLabels.Add(la);
            }
        }
        private void ValidateEntry()
        {
            //Username            
            //Password (both)            
            //Name
            //Email
            //Address (not required)
            //Postcode
            //Phone
        }

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Username", txtUsername.Text);
            if (validate != "") errorLabels[0].Text = "*" + validate;
            else errorLabels[0].Text = "";
            
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Password", txtPassword.Text);
            if (validate != "") errorLabels[1].Text = "*" + validate;
            else errorLabels[1].Text = "";
        }

        private void TxtREPassword_TextChanged(object sender, EventArgs e)
        {
            string validate = "";
            if (txtPassword.Text != txtREPassword.Text)
                validate = "*Passwords must match!";
            else validate = "";
            if (validate != "") errorLabels[2].Text = "*" + validate;
            else errorLabels[2].Text = "";

        }

        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Name", txtName.Text);
            if (validate != "") errorLabels[3].Text = "*" + validate;
            else errorLabels[3].Text = "";
        }

        private void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Email", txtEmail.Text);
            if (validate != "") errorLabels[4].Text = "*" + validate;
            else errorLabels[4].Text = "";
        }

        private void TxtAddress_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Address", txtAddress.Text);
            if (validate != "") errorLabels[5].Text = "*" + validate;
            else errorLabels[5].Text = "";
        }

        private void TxtPostcode_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Postcode", txtPostcode.Text);
            if (validate != "") errorLabels[6].Text = "*" + validate;
            else errorLabels[6].Text = "";
        }

        private void TxtPhone_TextChanged(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Phone", txtPhone.Text);
            if (validate != "") errorLabels[7].Text = "*" + validate;
            else errorLabels[7].Text = "";
        }

        private void ErrorLabelUpdated(object sender, EventArgs e)
        {
            Label l = (Label)sender;

            for (int i = 0; i < 8; i++)
            {
                if (l.Name == "errorLabel" + i)
                {
                    if (l.Text == "") l.Visible = false;
                    else { l.Visible = true; l.BringToFront(); }
                    break;
                }
            }

        }

        #endregion
    }
}
