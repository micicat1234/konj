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
    public partial class Form2 : Form
    {
        string connect = BazaConn.connect();

        public Form2()
        {
            InitializeComponent();
        }

        //IZPIS IMEN V COMBOBOX
        public void IzpisImen()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT * FROM IzpisImen()", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {

                    comboBox1.Items.Add(reader.GetString(0));
                }

                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            IzpisImen();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 prijava = new Form1();
            prijava.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ime = comboBox1.SelectedItem.ToString();
            string geslo = Geslo.Text;

            //Registracija ali menjanje gesla
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT Registracija('" + ime + "', '" + geslo + "')", con);
                com.ExecuteNonQuery();
                com.Dispose();
                con.Close();
            }
        }
    }
}
