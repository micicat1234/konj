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
    public partial class Menjava_gesla : Form
    {
        string connect = BazaConn.connect();

        public Menjava_gesla()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM izpisimen()", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    comboBox1.Items.Add(reader.GetString(0));
                }

                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            string ime = comboBox1.SelectedItem.ToString();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM izpispriimkov('"+ ime +"')", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    comboBox2.Items.Add(reader.GetString(0));
                }

                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT menjavagesla('" + comboBox1.SelectedItem.ToString() + "', '" + comboBox2.SelectedItem.ToString() + "', '" + Geslo.Text + "')", con);
                com.ExecuteNonQuery();
                com.Dispose();
                con.Close();

                Prijava prijava = new Prijava();
                prijava.Show();
                this.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Prijava prijava = new Prijava();
            prijava.Show();
            this.Hide();
        }
    }
}
