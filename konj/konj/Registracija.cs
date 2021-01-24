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
    public partial class Registracija : Form
    {
        string hash = "f0xle@rn";
        int id_kraja;
        string connect = BazaConn.connect();

        public Registracija()
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

                string kriptirano_geslo = kriptiraj(Geslo.Text);

                NpgsqlCommand aha = new NpgsqlCommand("SELECT Registracija('" + Ime.Text + "', '" + kriptirano_geslo + "', '" + Priimek.Text + "','" + id_kraja + "')", con);
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
