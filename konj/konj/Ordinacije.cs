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
        int kraj, zdravnik, krajj, zdravnikk;
        public Ordinacije()
        {
            InitializeComponent();
        }

        public void dobitajdija()
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT dobitidzdravnika('" + comboBox2.SelectedItem + "', '" + comboBox3.SelectedItem + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    zdravnikk = ja.GetInt32(0);
                }

                con.Close();

                con.Open();

                NpgsqlCommand ahaa = new NpgsqlCommand("SELECT dobitkrajid('" + comboBox4.SelectedItem + "')", con);
                NpgsqlDataReader jaa = ahaa.ExecuteReader();
                while (jaa.Read())
                {
                    krajj = jaa.GetInt32(0);
                }

                con.Close();
            }
        }


        private void Ordinacije_Load(object sender, EventArgs e)
        {
            linkLabel2.Visible = false;
            label7.Visible = false;
            Posta.Visible = false;
            Krajtextbox.Visible = false;

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
                    comboBox7.Items.Add(rreader.GetString(0));
                }

                con.Close();

                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT * FROM izpisimen()", con);
                NpgsqlDataReader lja = aha.ExecuteReader();
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

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT izpispriimkov('" + comboBox5.SelectedItem + "')", con);
                NpgsqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    comboBox6.Items.Add(reader.GetString(0));
                }

                con.Close();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label7.Visible = true;
            Posta.Visible = true;
            Krajtextbox.Visible = true;
            comboBox7.Visible = false;
            linkLabel1.Visible = false;
            linkLabel2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand ahda = new NpgsqlCommand("SELECT izbrisiordinacijo('" + comboBox1.SelectedItem + "');", con);
                ahda.ExecuteNonQuery();
                ahda.Dispose();
                con.Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand aha = new NpgsqlCommand("SELECT dobitidzdravnika('" + comboBox5.SelectedItem + "', '" + comboBox6.SelectedItem + "')", con);
                NpgsqlDataReader ja = aha.ExecuteReader();
                while (ja.Read())
                {
                    zdravnikk = ja.GetInt32(0);
                }

                con.Close();

            }

            if (Posta.Text == "")
            {

                using (NpgsqlConnection con = new NpgsqlConnection(connect))
                {

                    con.Open();

                    NpgsqlCommand ahaa = new NpgsqlCommand("SELECT dobitkrajid('" + comboBox7.SelectedItem + "')", con);
                    NpgsqlDataReader jaa = ahaa.ExecuteReader();
                    while (jaa.Read())
                    {
                        krajj = jaa.GetInt32(0);
                    }

                    con.Close();

                    con.Open();

                    NpgsqlCommand ahda = new NpgsqlCommand("SELECT dodatordinacijo('" + Ime.Text + "', '" + zdravnikk + "', '" + krajj + "');", con);
                    ahda.ExecuteNonQuery();
                    ahda.Dispose();
                    con.Close();
                }

            }
            else
            {
                using (NpgsqlConnection con = new NpgsqlConnection(connect))
                {
                    con.Open();

                    NpgsqlCommand ahda = new NpgsqlCommand("SELECT dodatordinacijoinkraj('" + Ime.Text + "', '" + zdravnikk + "', '" + Krajtextbox.Text + "', '" +Convert.ToInt32(Posta.Text) + "');", con);
                    ahda.ExecuteNonQuery();
                    ahda.Dispose();
                    con.Close();
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MainPage japjap = new MainPage();
            japjap.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label7.Visible = false;
            Posta.Visible = false;
            Posta.Text = "";
            Krajtextbox.Visible = false;
            comboBox7.Visible = true;
            linkLabel1.Visible = true;
            linkLabel2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dobitajdija();

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                string select = "SELECT posodobiordinacijo('" + comboBox1.SelectedItem + "', '" + textBox1.Text + "', " + krajj + "," + zdravnikk + ");";
                MessageBox.Show(select);

                NpgsqlCommand ahda = new NpgsqlCommand(select , con);

                ahda.ExecuteNonQuery();
                ahda.Dispose();
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

                con.Open();

                NpgsqlCommand ahsaa = new NpgsqlCommand("SELECT * FROM dobitzdravnika('" + zdravnik + "')", con);
                NpgsqlDataReader jdaa = ahsaa.ExecuteReader();
                while (jdaa.Read())
                {
                    comboBox2.SelectedItem = jdaa.GetString(0);
                    comboBox3.SelectedItem = jdaa.GetString(1);

                }

                con.Close();

            }

        }
    }
}
