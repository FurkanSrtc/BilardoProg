using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BilardoProg
{
    public partial class MasaBilgisi : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Bilardo;Integrated Security=True");

        int masaNo;
        string kullaniciAdi;

      

        public MasaBilgisi(int mNo,string kAdi)
        {
            kullaniciAdi = kAdi;
            masaNo = mNo;
            InitializeComponent();
        }
        Boolean masaDurumu;

        private void kontrol()
        {
            if (masaDurumu == true)
            {
                lblMasaDurumu.Text = "Açık";
                button1.Enabled = false;
                button2.Enabled = true;

                label2.Visible = true;
                label3.Visible = true;
                lblbasSaati.Visible = true;
                lblMasaUcret.Text = "" ;
            }
            else
            {
                button1.Enabled = true;
                button2.Enabled = false;
                lblMasaDurumu.Text = "Kapalı";

                label2.Visible = true;
                label3.Visible = true;
                lblbasSaati.Visible = true;
                lblMasaUcret.Visible = true;
            }

        }
        private void MasaBilgisi_Load(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Green;
            button2.ForeColor = Color.Red;
            label1.Text = "Masa "+masaNo;
            DateTime baslangicSaat=DateTime.Now;
            SqlCommand cmd2 = new SqlCommand("select * from MasaBilgisi where KullaniciAdi='"+kullaniciAdi+"' and MasaNo='"+masaNo+"'", conn);
            conn.Open();
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                // lblbasSaati.Text = dr2["BaslangicSaat"].ToString();
                baslangicSaat= Convert.ToDateTime(dr2["BaslangicSaat"]);
                masaDurumu = Convert.ToBoolean(dr2["Durum"]);
                lblMasaUcret.Text = dr2["Ucret"].ToString();
            }
            conn.Close();
            lblbasSaati.Text = baslangicSaat.ToShortTimeString();
            kontrol();

            SqlCommand cmd = new SqlCommand("select SaatlikUcret from Kullanicilar where KullaniciAdi='" + kullaniciAdi +"'", conn);
            conn.Open();
            numericUpDown1.Value = Convert.ToInt16(cmd.ExecuteScalar());
                conn.Close();
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string format = "yyyy-MM-dd HH:mm:ss";
           
            masaDurumu = false;
            kontrol();

            SqlCommand cmd = new SqlCommand("select BaslangicSaat from MasaBilgisi where KullaniciAdi='" + kullaniciAdi + "' and MasaNo='" + masaNo + "' and Durum=1", conn);
            conn.Open();
             DateTime baslangic =Convert.ToDateTime(cmd.ExecuteScalar());
           // DateTime baslangic = new DateTime(2018,04,30,22,56,00); //Kontrol için. Orjinali üst satırda
            TimeSpan sonuc = DateTime.Now - baslangic;
            int dakika =Convert.ToInt32(sonuc.TotalMinutes);
            decimal fiyat = numericUpDown1.Value / 60;
            decimal ucret =Convert.ToDecimal(fiyat * dakika);
             ucret = Convert.ToDecimal(Math.Round(ucret, 2));

            String ucret2 = ucret.ToString().Replace(",", ".");

           
            lblMasaUcret.Text=(ucret2+" TL");
            conn.Close();


         

            conn.Open();
            SqlCommand cmd2 = new SqlCommand("update MasaBilgisi set Ucret = '" + ucret2 +"', Durum=0,BitisSaat='" + time.ToString(format) + "' where KullaniciAdi='"+kullaniciAdi+"' and MasaNo = "+masaNo+"", conn);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd2);
            da.Fill(dt2);
            conn.Close();


            Masalar masa = (Masalar)Application.OpenForms["Masalar"];
            masa.yesilYap(masaNo);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            masaDurumu = true;
            kontrol();
      
           DateTime time = DateTime.Now;       
            string format = "yyyy-MM-dd HH:mm:ss";
            //time.ToString(format)
            lblbasSaati.Text = time.ToShortTimeString();

            lblMasaUcret.Text = "";
            
            conn.Open();
            SqlCommand cmd = new SqlCommand("insert into MasaBilgisi (BaslangicSaat,KullaniciAdi,Durum,MasaNo) values ('"+time.ToString(format) + "','"+kullaniciAdi+"',1,'"+masaNo+"')", conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();

            Masalar masa = (Masalar)Application.OpenForms["Masalar"];
            masa.yenile(); //

           
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

       
    }
}
