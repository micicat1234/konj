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
using System.Security.Cryptography;

namespace konj
{
    public partial class Menjava_gesla : Form
    {
        string connect = BazaConn.connect();
        string hash = "f0xle@rn";

        public Menjava_gesla()
        {
            InitializeComponent();
        }

        private string kriptiraj(string geslo)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(geslo);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string vrni = Convert.ToBase64String(results, 0, results.Length);
                    return vrni;
                }
            }
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

                NpgsqlCommand com = new NpgsqlCommand("SELECT menjavagesla('" + comboBox1.SelectedItem.ToString() + "', '" + comboBox2.SelectedItem.ToString() + "', '" + kriptiraj(Geslo.Text) + "')", con);
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
