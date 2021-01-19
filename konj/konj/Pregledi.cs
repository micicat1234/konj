using System;
using System.Globalization;
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
        int zdravnik, ordinacija;
        int zdrv;
        int ordn;

        public Pregledi()
        {
            InitializeComponent();
        }

        private void Pregledi_Load(object sender, EventArgs e)
        {
            textBox4.Enabled = false;

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand ahaa = new NpgsqlCommand("SELECT * FROM izpisimennn()", con);
                NpgsqlDataReader pff = ahaa.ExecuteReader();
                while (pff.Read())
                {
                    comboBox1.Items.Add(pff.GetString(0));
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahha = new NpgsqlCommand("SELECT * FROM izpisimen()", con);
                NpgsqlDataReader lja = ahha.ExecuteReader();
                while (lja.Read())
                {
                    comboBox2.Items.Add(lja.GetString(0));
                    comboBox5.Items.Add(lja.GetString(0));
                }

                con.Close();

                con.Open();

                NpgsqlCommand ss = new NpgsqlCommand("SELECT * FROM priimki()", con);
                NpgsqlDataReader aa = ss.ExecuteReader();
                while (aa.Read())
                {
                    comboBox3.Items.Add(aa.GetString(0));
                }

                con.Close();

                con.Open();

                NpgsqlCommand sss = new NpgsqlCommand("SELECT * FROM izpisordinacij()", con);
                NpgsqlDataReader aaa = sss.ExecuteReader();
                while (aaa.Read())
                {
                    comboBox4.Items.Add(aaa.GetString(0));
                }

                con.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT dobitidzdravnika('" + comboBox2.SelectedItem + "', '" + comboBox3.SelectedItem + "')", con);
                NpgsqlDataReader pff = aha.ExecuteReader();
                while (pff.Read())
                {
                    zdravnik = pff.GetInt32(0);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahaa = new NpgsqlCommand("SELECT dobitidordinacije('" + comboBox4.SelectedItem + "')", con);
                NpgsqlDataReader pffa = ahaa.ExecuteReader();
                while (pffa.Read())
                {
                    ordinacija = pffa.GetInt32(0);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahaaa = new NpgsqlCommand("SELECT uredipregled('" + comboBox1.SelectedItem + "', '" + textBox1.Text + "', '" + Convert.ToDouble(textBox2.Text) + "', '" + zdravnik + "', '" + ordinacija + "')", con);
                ahaaa.ExecuteNonQuery();
                ahaaa.Dispose();
                con.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
               
                con.Open();

                NpgsqlCommand ahaaa = new NpgsqlCommand("SELECT * FROM dobitpregled('" + textBox1.Text + "')", con);
                NpgsqlDataReader pffaa = ahaaa.ExecuteReader();
                while (pffaa.Read())
                {
                    textBox2.Text = pffaa.GetDouble(0).ToString();
                    textBox4.Text = pffaa.GetDateTime(3).ToString();

                    zdrv = pffaa.GetInt32(1);
                    ordn = pffaa.GetInt32(2);

                }

                con.Close();

                con.Open();

                NpgsqlCommand ahaaaa = new NpgsqlCommand("SELECT * FROM dobitzdravnika('" + zdrv + "')", con);
                NpgsqlDataReader pffaaa = ahaaaa.ExecuteReader();
                while (pffaaa.Read())
                {
                    comboBox2.SelectedItem = pffaaa.GetString(0);
                    comboBox3.SelectedItem = pffaaa.GetString(1);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahaaaaa = new NpgsqlCommand("SELECT dobitordinacijoime('" + ordn + "')", con);
                NpgsqlDataReader pffaaaa = ahaaaaa.ExecuteReader();
                while (pffaaaa.Read())
                {
                    comboBox4.SelectedItem = pffaaaa.GetString(0);
                }

                con.Close();
            }
        }
    }
}
