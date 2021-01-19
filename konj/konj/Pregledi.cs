using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Configuration;

namespace konj
{
    public partial class Pregledi : Form
    {
        string connect = BazaConn.connect();
        public Pregledi()
        {
            InitializeComponent();
        }

        private void Pregledi_Load(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpisdatuma()", con);
                NpgsqlDataReader pff = aha.ExecuteReader();
                while (pff.Read())
                {
                    comboBox1.Items.Add(pff.GetDateTime(0).ToString());
                }

                con.Close();
            }
        }
    }
}
