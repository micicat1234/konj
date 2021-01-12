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
    public partial class Registracija : Form
    {

        int id_kraja;
        string connect = BazaConn.connect();

        public Registracija()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM izpiskrajev()", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    comboBox1.Items.Add(reader.GetString(0));
                }

                con.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Prijava prijava = new Prijava();
            prijava.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Registracija

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                //id kraja
                string ime_kraja = comboBox1.SelectedItem.ToString();

                con.Open();
                NpgsqlCommand com = new NpgsqlCommand("SELECT DobitKrajid('" + ime_kraja + "')", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    id_kraja = reader.GetInt32(0);
                }
                con.Close();

                //vpiše notr
                con.Open();
                NpgsqlCommand aha = new NpgsqlCommand("SELECT Registracija('" + Ime.Text + "', '" + Geslo.Text + "', '" + Priimek.Text + "','" + id_kraja + "')", con);
                aha.ExecuteNonQuery();
                aha.Dispose();
                con.Close();
            }

            Prijava prijava = new Prijava();
            prijava.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
