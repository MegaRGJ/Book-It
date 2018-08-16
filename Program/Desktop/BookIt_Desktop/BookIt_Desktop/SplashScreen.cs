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
    public partial class SplashScreen : Form
    {
        
        public SplashScreen()
        {
            InitializeComponent();

            PictureBox spashPictureBox = new PictureBox();
            spashPictureBox.Image = Properties.Resources.SplashScreen;
            spashPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            spashPictureBox.Dock = DockStyle.Fill;
            Controls.Add(spashPictureBox);
            
            StartPosition = FormStartPosition.CenterScreen;
            Activate();
        }
        
        private void SplashScreen_Load(object sender, EventArgs e)
        {
           
        }
        
    }
}
