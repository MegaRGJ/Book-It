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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            lblLoggedName.Text = "Logged in as " + Server.ClientUsername;
            ResizeControls();
            Focus();
        }

        public void ResizeControls()
        {
            /*
            Default window size (900, 650)
            Default menu panel location and size : 10, 25 (205, 560)
            Default bookings panel location and size :220, 25 (650, 560)
            */
            lblLoggedName.Location = new Point(Width - (lblLoggedName.Width + (4*Server.ClientUsername.Length)), 10);            
            pnlMenu.Size = new Size(205, Height - 90);            
            pnlBookings.Size = new Size(Width - 245, Height - 90);

        }

        private void Main_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }
    }
}
