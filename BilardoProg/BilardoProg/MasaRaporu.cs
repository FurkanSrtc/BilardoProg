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
    public partial class MasaRaporu : Form
    {
        string kullaniciAdi;
        int masaSayisi;
        public MasaRaporu(string kAdi,int mSayisi)
        {
            kullaniciAdi = kAdi;
            masaSayisi = mSayisi;
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection("Data Source =.; Initial Catalog = Bilardo; Integrated Security = True");
        private void MasaRaporu_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bilardoDataSet.MasaBilgisi' table. You can move, or remove it, as needed.

            label5.Text = (dateTimeBaslangic.Value.ToShortDateString()+"-"+dateTimeBitis.Value.ToShortDateString())+" Tarihleri Arasında";


            comboBox1.Items.Add("Tümü");
            for (int i = 1; i <= masaSayisi; i++)
            {
                comboBox1.Items.Add("Masa "+i);
            }

            yenile();

        }
        decimal toplamUcret;
        DateTime baslangic;
        DateTime bitis;
        string format = "yyyy-MM-dd";

        private void yenile()
        {
            baslangic = dateTimeBaslangic.Value;

            bitis = dateTimeBitis.Value;

            conn.Open();
            SqlCommand cmd = new SqlCommand("select ('Masa '+Cast(MasaNo as varchar)) as 'Masa Numarası',Ucret as 'Ücret',CAST(BaslangicSaat AS time) as 'Başlangıç Saati', CAST(BitisSaat AS time) as 'Bitiş Saati' ,CAST(BaslangicSaat AS date) as 'Tarih' from MasaBilgisi where KullaniciAdi='" + kullaniciAdi+ "'  and BaslangicSaat >='" + baslangic.ToString(format) + "' and BitisSaat <= '" + bitis.ToString(format) + "'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;



            SqlCommand cmd2 = new SqlCommand("select Sum(Ucret) from MasaBilgisi where  KullaniciAdi='" + kullaniciAdi + "' and BaslangicSaat >='" + baslangic.ToString(format) + "' and BitisSaat <= '" + bitis.ToString(format) + "'", conn);
            try
            {
                toplamUcret = Convert.ToDecimal(cmd2.ExecuteScalar().ToString());
            }
            catch (Exception)
            {
                toplamUcret = 0;
            }
            
            conn.Close();

            label4.Text = ("Toplam kazanç: "+ toplamUcret.ToString());

            conn.Close();
         
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label5.Text = (dateTimeBaslangic.Value.ToShortDateString() + "-" + dateTimeBitis.Value.ToShortDateString()) + " Tarihleri Arasında";

            if (comboBox1.SelectedIndex == 0)
            {
                yenile();


            }
            else
            {
                baslangic = dateTimeBaslangic.Value;

                bitis = dateTimeBitis.Value;
                string format = "yyyy-MM-dd";

                conn.Open();
                SqlCommand cmd = new SqlCommand("select ('Masa '+Cast(MasaNo as varchar)) as 'Masa Numarası',Ucret as 'Ücret',CAST(BaslangicSaat AS time) as 'Başlangıç Saati', CAST(BitisSaat AS time) as 'Bitiş Saati' ,CAST(BaslangicSaat AS date) as 'Tarih' from MasaBilgisi where KullaniciAdi='" + kullaniciAdi + "'and MasaNo='" + comboBox1.SelectedIndex + "'  and BaslangicSaat >='" + baslangic.ToString(format) + "' and BitisSaat <= '" + bitis.ToString(format) + "'", conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;




                SqlCommand cmd2 = new SqlCommand("select Sum(Ucret) from MasaBilgisi where KullaniciAdi='" + kullaniciAdi + "' and MasaNo='" + comboBox1.SelectedIndex + "' and BaslangicSaat >='" + baslangic.ToString(format) + "' and BitisSaat <= '" + bitis.ToString(format) + "' ", conn);
                try
                {
                    toplamUcret = Convert.ToDecimal(cmd2.ExecuteScalar().ToString());

                    label4.Text = ("Masa " + comboBox1.SelectedIndex + " için toplam kazanç: " + toplamUcret.ToString());
                }
                catch (Exception)
                {
                    label4.Text = ("Masa " + comboBox1.SelectedIndex + " için sonuç bulunamadı.");
                }

                conn.Close();


            }

            /*baslangic = dateTimeBaslangic.Value;

            bitis =dateTimeBitis.Value;
           string format = "yyyy-MM-dd";

           SqlCommand cmd = new SqlCommand("select ('Masa '+Cast(MasaNo as varchar)) as 'Masa Numarası',Ucret as 'Ücret',CAST(BaslangicSaat AS time) as 'Başlangıç Saati', CAST(BitisSaat AS time) as 'Bitiş Saati' ,CAST(BaslangicSaat AS date) as 'Tarih'from MasaBilgisi where KullaniciAdi='" + kullaniciAdi + "'and BaslangicSaat >='" + baslangic.ToString(format) + "' and BitisSaat <= '"+ bitis.ToString(format) + "'", conn);
           conn.Open();
           SqlDataAdapter da = new SqlDataAdapter(cmd);
           DataTable dt = new DataTable();
           da.Fill(dt);
           dataGridView1.DataSource = dt;
           conn.Close();*/
        }
    }
}
