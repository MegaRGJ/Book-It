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
            CreateToolTips();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            RunAllValidation(sender, e);
            if (DataValidated()) {
                //Update database for new user
                
            }
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

        private void RunAllValidation(object sender, EventArgs e)
        {
            TxtUsername_TextChanged(sender, e);
            TxtPassword_TextChanged(sender, e);
            TxtREPassword_TextChanged(sender, e);
            TxtName_TextChanged(sender, e);
            TxtEmail_LostFocus(sender, e);
            TxtAddress_LostFocus(sender, e);
            TxtPostcode_LostFocus(sender, e);
            TxtPhone_LostFocus(sender, e);

        }

        private bool DataValidated() {
            foreach (Label l in errorLabels) {
                if (l.Visible)
                    return false; //If any error labels are showing then data must have failed validation
            }
            return true;
        }

        private void CreateToolTips()
        {
            new ToolTip().SetToolTip(lblUsername, "The name of your account");
            new ToolTip().SetToolTip(lblPassword, "Try to avoid common passwords");
            new ToolTip().SetToolTip(lblREPassword, "Must match your password");
            new ToolTip().SetToolTip(lblName, "Your first and last name, use a hyphen or underscore for multi-barreled surnames");
            new ToolTip().SetToolTip(lblEmail, "Your email address");
            new ToolTip().SetToolTip(lblAddress, "House number and street only");
            new ToolTip().SetToolTip(lblPostcode, "Your postcode");
            new ToolTip().SetToolTip(lblPhone, "Your UK phone number");
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

        private void TxtEmail_LostFocus(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Email", txtEmail.Text);
            if (validate != "") errorLabels[4].Text = "*" + validate;
            else errorLabels[4].Text = "";
        }

        private void TxtAddress_LostFocus(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Email", txtEmail.Text);
            if (validate != "") errorLabels[4].Text = "*" + validate;
            else errorLabels[5].Text = "";
        }

        private void TxtPostcode_LostFocus(object sender, EventArgs e)
        {
            string validate = Validation.ValidateInput("Postcode", txtEmail.Text);
            if (validate != "") errorLabels[4].Text = "*" + validate;
            else errorLabels[6].Text = "";
        }

        private void TxtPhone_LostFocus(object sender, EventArgs e)
        {
            string phone = txtEmail.Text;
            if (phone[0] == '0') phone = phone.Substring(1,phone.Length - 1);
            string validate = Validation.ValidateInput("Phone", "+44" + phone);
            if (validate != "") errorLabels[4].Text = "*" + validate;
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
