﻿using System;
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
    public partial class Form1 : Form
    {
        string connect = BazaConn.connect();

        public Form1()
        {
            InitializeComponent();
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
            Form2 registracija = new Form2();
            registracija.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ime = Ime.Text;
            string geslo = Geslo.Text;

            using (NpgsqlConnection con = new NpgsqlConnection(connect))
            {
                con.Open();

                NpgsqlCommand com = new NpgsqlCommand("SELECT Prijava('" + ime + "', '" + geslo + "')", con);
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
                        Form3 japjap = new Form3();
                        japjap.Show();
                        this.Hide();
                    }
                }

                con.Close();
            }
        }
    }
    

}
