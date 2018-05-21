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
    public partial class Masalar : Form
    {
        string kullaniciAdi;
        int masaSayisi;

      

        public Masalar(string kAdi,int mSayisi)
        {
            kullaniciAdi = kAdi;
            masaSayisi = mSayisi;
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=Bilardo;Integrated Security=True");

      
        private void Masalar_Load(object sender, EventArgs e)
        {
           

            for (int i = 1; i <= masaSayisi; i++)
            {
                Button button = new Button();
            
                button.Name = (i).ToString();
                button.Width = 130;
                button.Height = 75;
                button.Text = "Masa "+(i).ToString();
             
            
                button.BackgroundImage = Resource1.yesil;
                button.BackgroundImageLayout = ImageLayout.Stretch;
                
                this.Controls.Add(button);
                flowLayoutPanel1.Controls.Add(button);
                button.Click += Button_Click;  
            }
            yenile();
     
        }
        List<int> acikMasalar = new List<int>();
        public void yenile()
        {
            acikMasalar.Clear();

            SqlCommand cmd = new SqlCommand("select MasaNo,Durum from MasaBilgisi where kullaniciAdi='" + kullaniciAdi + "' and Durum=1", conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                acikMasalar.Add((int)dr["MasaNo"]);
            }
            conn.Close();

            foreach (Button kontrol in flowLayoutPanel1.Controls)
            {
                    foreach (var item in acikMasalar)
                    {
                        if (kontrol.Name == item.ToString())
                        {

                            kontrol.BackgroundImage = Resource1.kirmizi;
                            kontrol.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                         
                    }
            }

        }

        public void yesilYap(int MasaNo)
        {
            foreach (Button kontrol in flowLayoutPanel1.Controls)
            {
                        if (kontrol.Name == MasaNo.ToString())
                        {
                            kontrol.BackgroundImage = Resource1.yesil;
                            kontrol.BackgroundImageLayout = ImageLayout.Stretch;
                        }
            }
        }

            private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
           
           // btn.BackgroundImage=... resim ekleyebilirsin
            int masaNo=Convert.ToInt16(btn.Name.ToString());
            MasaBilgisi mb = new MasaBilgisi(masaNo,kullaniciAdi);
            mb.Show();
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MasaRaporu masaRaporu = new MasaRaporu(kullaniciAdi,masaSayisi);
            masaRaporu.Show();
        }
    }
}
