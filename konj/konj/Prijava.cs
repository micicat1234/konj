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
    public partial class Prijava : Form
    {
        string hash = "f0xle@rn";
        string connect = BazaConn.connect();

        public Prijava()
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registracija registracija = new Registracija();
            registracija.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ime = Ime.Text;
            string priimek = Priimek.Text;
            string geslo = kriptiraj(Geslo.Text);

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT Prijava('" + ime + "', '" + geslo + "', '" + priimek + "')", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    int stevilo = reader.GetInt32(0);

                    if(stevilo == 0)
                    {
                        MessageBox.Show("Podatki se ne ujemajo!");
                    }
                    else if(stevilo ==1)
                    {
                        MainPage japjap = new MainPage();
                        japjap.Show();
                        this.Hide();
                    }
                }

                con.Close();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Menjava_gesla japjap = new Menjava_gesla();
            japjap.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }
    }
    

}
