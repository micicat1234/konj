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
    public partial class Ordinacije : Form
    {
        string connect = BazaConn.connect();
        int kraj, zdravnik;
        public Ordinacije()
        {
            InitializeComponent();
        }

        private void Ordinacije_Load(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM izpisordinacij()", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(0));
                }

                con.Close();

                con.Open();

                NpgsqlCommand comm = new NpgsqlCommand("SELECT * FROM izpiskrajev()", con);
                NpgsqlDataReader rreader = comm.ExecuteReader();
                while (rreader.Read())
                {
                    comboBox4.Items.Add(rreader.GetString(0));
                }

                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM dobitordinacijo('" + comboBox1.SelectedItem + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    kraj = ja.GetInt32(0);
                    zdravnik = ja.GetInt32(1);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahsa = new NpgsqlCommand("SELECT dobitimekraja('" + kraj + "')", con);
                NpgsqlDataReader jda = ahsa.ExecuteReader();
                while (jda.Read())
                {
                    comboBox4.SelectedItem = jda.GetString(0);
                }

                con.Close();

            }

        }
    }
}
