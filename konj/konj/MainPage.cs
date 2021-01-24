using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace konj
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kraji japjap = new Kraji();
            japjap.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ordinacije japjap = new Ordinacije();
            japjap.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pregledi japjap = new Pregledi();
            japjap.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Zobozdravniki japjap = new Zobozdravniki();
            japjap.Show();
            this.Hide();
        }
    }
}
