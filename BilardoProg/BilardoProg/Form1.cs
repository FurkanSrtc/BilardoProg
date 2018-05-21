using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilardoProg
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Bilardo;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kontrol();
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * from Kullanicilar where KullaniciAdi='" + txtKadi1.Text + "' and Sifre='" + txtSifre.Text + "'", conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);


            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Kullanıcı Adı veya Parolanız hatalı");
            }

            else
            {

                string kullaniciAdi = txtKadi1.Text;
                SqlCommand cmd2 = new SqlCommand("select MasaSayisi from Kullanicilar where KullaniciAdi='" + txtKadi1.Text + "'", conn);
                int masaSayisi = Convert.ToInt32(cmd2.ExecuteScalar().ToString());
                Masalar m = new Masalar(kullaniciAdi, masaSayisi);
                m.Show();
                this.Hide();


            }
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            conn.Open();
            SqlCommand komut = new SqlCommand("select * from hatirla where id=1;", conn);
            try
            {
                 komut.ExecuteNonQuery();
                 SqlDataReader dr = komut.ExecuteReader();
             if (dr.Read())
            {
                txtKadi1.Text = dr["kullaniciadi"].ToString();
                txtSifre.Text= dr["sifre"].ToString();
            }
            conn.Close();

                if (txtKadi1.Text != "")
                {
                    checkBox1.Checked = true;
                }
                else checkBox1.Checked = false;
            }

            catch (Exception)
            {


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UyeOl uye = new UyeOl();
            uye.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            kontrol();
        }

        private void kontrol()
        {
            if (checkBox1.Checked)
            {
                conn.Open();


                SqlCommand komut = new SqlCommand("update hatirla set kullaniciadi = @kAdi, Sifre=@Sifre, id=1 ", conn);
                komut.Parameters.AddWithValue("@kAdi", txtKadi1.Text);
                komut.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                komut.ExecuteNonQuery();

                conn.Close();
            }
            else
            {
                conn.Open();


                SqlCommand komut = new SqlCommand("update hatirla set kullaniciadi = @kAdi, Sifre=@Sifre, id=0 ", conn);
                komut.Parameters.AddWithValue("@kAdi", txtKadi1.Text);
                komut.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                komut.ExecuteNonQuery();

                conn.Close();
            }
        }

    }
}
