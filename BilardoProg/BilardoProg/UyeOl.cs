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
    public partial class UyeOl : Form
    {
        public UyeOl()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Bilardo; Integrated Security = True");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string kayit = "insert into Kullanicilar(KullaniciAdi,Sifre,MasaSayisi,SaatlikUcret) values (@kad,@sifre,@masasayisi,@saatlikucret)";

                SqlCommand komut = new SqlCommand(kayit, conn);

                komut.Parameters.AddWithValue("@kad", textBox1.Text);
                komut.Parameters.AddWithValue("@sifre", textBox2.Text);
                komut.Parameters.AddWithValue("@masasayisi", textBox3.Text);
                komut.Parameters.AddWithValue("@saatlikucret", textBox4.Text);
                komut.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Kayıt İşlemi Gerçekleşti");
            }
            catch (Exception)
            {
                MessageBox.Show("Birşeyler Ters Gitti :/");
            }
            
        }

      
    }
}
