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
    public partial class Kraji : Form
    {
        string connect = BazaConn.connect();
        public Kraji()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT dobitiposto('"+ textBox1.Text + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    textBox2.Text = ja.GetInt32(0).ToString();
                }

                con.Close();

            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Kraji_Load(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpiskrajev()", con);
                NpgsqlDataReader pff = aha.ExecuteReader();
                while (pff.Read())
                {
                    comboBox1.Items.Add(pff.GetString(0));
                }

                con.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                int posta = Convert.ToInt32(textBox2.Text);

                NpgsqlCommand ahda = new NpgsqlCommand("SELECT posodobikraj('" + comboBox1.SelectedItem + "', '" + textBox1.Text + "', '" + posta + "')", con);
                ahda.ExecuteNonQuery();
                ahda.Dispose();
                con.Close();
            }

            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                int posta = Convert.ToInt32(textBox2.Text);

                NpgsqlCommand ahda = new NpgsqlCommand("SELECT izbrisikraj('" + comboBox1.SelectedItem + "')", con);
                ahda.ExecuteNonQuery();
                ahda.Dispose();
                con.Close();
            }

            comboBox1.SelectedIndex = -1;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                int posta = Convert.ToInt32(textBox3.Text);

                NpgsqlCommand ahda = new NpgsqlCommand("SELECT vpiskraja('" + textBox4.Text + "', '" + posta + "')", con);
                ahda.ExecuteNonQuery();
                ahda.Dispose();
                con.Close();
            }

            textBox3.Text = "";
            textBox4.Text = "";
        }
    }
}
