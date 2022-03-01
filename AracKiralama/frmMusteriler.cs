using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AracKiralama
{
    public partial class frmMusteriler : Form
    {
        private bool _secim;
        public frmMusteriler()
        {
            InitializeComponent();
        }
        public frmMusteriler(params object[] prm)
        {
            InitializeComponent();
            _secim = (bool)prm[0];
        }


        SqlConnection baglanti;
        SqlCommand komut = new SqlCommand();
        private void frmMusteriler_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = Form1.baglantiBilgisi;
            komut.Connection = baglanti;
            veriGetir();
        }
        void baglan()
        {
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();
        }
        void veriGetir()
        {
            try
            {
                DataTable dt_Musteriler = new DataTable("MUSTERI");
                SqlDataAdapter adapter = new
                    SqlDataAdapter("Select MKod,TcNo,Ad,Soyad,Tel,Adres from MUSTERI", baglanti);
                baglan();
                adapter.Fill(dt_Musteriler);
                DgridMusteriler.DataSource = dt_Musteriler;
                baglanti.Close();
            }
            catch (Exception hata)
            {

                MessageBox.Show("Hata Oluştu" + hata.Message);
            }
        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtAdres_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnKytEkle_Click(object sender, EventArgs e)
        {
            komut.CommandText = "insert into MUSTERI(MKod,TcNo,Ad,Soyad,Adres,Tel) values(@MKod,@TcNO,@Ad,@Soyad,@Adres,@Tel)";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@MKod", txtMusKod.Text);
            komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Tel", txtTelefon.Text);

            baglan();
            komut.ExecuteNonQuery();
            baglanti.Close();
            veriGetir();
            verileriTemizle();
        }
        void verileriTemizle()
        {
            txtMusKod.Clear(); txtTcNo.Clear(); txtAd.Clear();
            txtSoyad.Clear(); txtAdres.Clear(); txtTelefon.Clear();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            komut.CommandText =
                "Update MUSTERI set TcN=@TcNo,Ad=@Ad,Soyad=@Soyad" +
                "Adres=@Adres,Tel=@Tel Where MKod=qMKod";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Tel", txtTelefon.Text);
            komut.Parameters.AddWithValue("@MKod", txtMusKod.Text);

            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Güncellendi");
            else
                MessageBox.Show("Kayıt Güncellenemedi");
            baglanti.Close();
            veriGetir();
            verileriTemizle();
            DgridMusteriler.Focus();




        }
        int secilKayit;
        private void DgridMusteriler_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            secilKayit = e.RowIndex;
        }

        private void DgridMusteriler_DoubleClick(object sender, EventArgs e)
        {
            if (_secim)
            {
                KiralaForm.MusteriId = Convert.ToInt32(DgridMusteriler.Rows[secilKayit].Cells["MKod"].Value);
                this.Close();
            }

            txtMusKod.Text = DgridMusteriler.Rows[secilKayit].Cells["MKod"].Value.ToString();
            txtTcNo.Text = DgridMusteriler.Rows[secilKayit].Cells["Tcno"].Value.ToString();
            txtAd.Text = DgridMusteriler.Rows[secilKayit].Cells["Ad"].Value.ToString();
            txtSoyad.Text = DgridMusteriler.Rows[secilKayit].Cells["Soyad"].Value.ToString();
            txtAdres.Text = DgridMusteriler.Rows[secilKayit].Cells["Adres"].Value.ToString();
            txtTelefon.Text = DgridMusteriler.Rows[secilKayit].Cells["Tel"].Value.ToString();
        }
    }
}
