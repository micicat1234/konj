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
    public partial class Zobozdravniki : Form
    {
        string hash = "f0xle@rn";
        int id_kraja, krajj;
        string connect = BazaConn.connect();

        public Zobozdravniki()
        {
            InitializeComponent();
        }

        private string dekriptiraj(string geslo)
        {
            byte[] data = Convert.FromBase64String(geslo);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripleDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    string vrni = UTF8Encoding.UTF8.GetString(results);
                    return vrni;
                }
            }
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

        private void Updatee()
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

                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpiskrajev()", con);
                NpgsqlDataReader pff = aha.ExecuteReader();
                while (pff.Read())
                {
                    comboBox3.Items.Add(pff.GetString(0));
                }

                con.Close();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Ime.Enabled = false;
            Priimek.Enabled = false;
            Geslo.Enabled = false;

            Updatee();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpispriimkov('" + comboBox1.SelectedItem + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    comboBox2.Items.Add(ja.GetString(0));
                }

                con.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ime.Enabled = true;
            Priimek.Enabled = true;
            Geslo.Enabled = true;

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpiszobozdravnikov('" + comboBox1.SelectedItem + "', '" + comboBox2.SelectedItem + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    Ime.Text = ja.GetString(0);
                    Priimek.Text = ja.GetString(1);
                    Geslo.Text = dekriptiraj(ja.GetString(2));
                    krajj = ja.GetInt32(3);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahdaa = new NpgsqlCommand("SELECT dobitimekraja('" + krajj + "')", con);
                NpgsqlDataReader jaa = ahdaa.ExecuteReader();
                while (jaa.Read())
                {
                    comboBox3.SelectedItem = jaa.GetString(0);
                }

                con.Close();

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand ahda = new NpgsqlCommand("SELECT dobitkrajid('" + comboBox3.SelectedItem + "')", con);
                NpgsqlDataReader ja = ahda.ExecuteReader();
                while (ja.Read())
                {
                    id_kraja = ja.GetInt32(0);
                }

                con.Close();

                con.Open();
                NpgsqlCommand aha = new NpgsqlCommand("SELECT posodobizobozdravnika('" + comboBox1.SelectedItem + "', '" + Ime.Text + "', '" + comboBox2.SelectedItem + "', '" + Priimek.Text + "', '" + kriptiraj(Geslo.Text) + "', '" + id_kraja + "')", con);
                aha.ExecuteNonQuery();
                aha.Dispose();
                con.Close();
            }
            comboBox1.Items.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            Ime.Text = "";
            Priimek.Text = "";
            Geslo.Text = "";
            Updatee();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();
                NpgsqlCommand aha = new NpgsqlCommand("SELECT izbrisizobozdravnika('" + comboBox1.SelectedItem + "', '" + comboBox2.SelectedItem + "') ", con);
                aha.ExecuteNonQuery();
                aha.Dispose();
                con.Close();
            }

            comboBox1.Items.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            Ime.Text = "";
            Priimek.Text = "";
            Geslo.Text = "";
            Updatee();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainPage japjap = new MainPage();
            japjap.Show();
            this.Hide();
        }
    }
}
