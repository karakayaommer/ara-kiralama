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
using AracKiralama;


namespace AracKiralama
{
    public partial class frmAraclar : Form
    {
        SqlConnection baglanti;
        SqlCommand komut = new SqlCommand();
        private bool _secim;

        public frmAraclar()
        {
            InitializeComponent();
        }

        public frmAraclar(params object[] prm)
        {
            InitializeComponent();
            _secim = (bool)prm[0];
        }

        void baglan()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
        }
        //----------------------------------------------------------------------------------------------
        private void frmAraclar_Load(object sender, EventArgs e)
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = Form1.baglantiBilgisi;
            komut.Connection = baglanti;
            VeriGetir();

        }
        //-----------------------------------------------------------------------------------------------
        void VeriGetir()
        {
            try
            {
                DataTable dt_Adresler = new DataTable("Araclar");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from Araclar", baglanti);
                baglan();
                adaptor.Fill(dt_Adresler);
                DgridAraclar.DataSource = dt_Adresler;
                //DgridAraclar.ItemsSource = dt_Adresler.DefaultView;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }
        //----------------------------------------------------------------------------------------
        private void btnKayitEkle_Click(object sender, EventArgs e)
        {
         
            komut.CommandText = "insert into Araclar(AracNo,Marka,Model,Plaka) values(@AracNo,@Marka,@Model,@Plaka)";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@AracNo", txtAracNo.Text);
            komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
            komut.Parameters.AddWithValue("@Model", txtModel.Text);
            komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            baglan();
            komut.ExecuteNonQuery();
            baglanti.Close();
            VeriGetir();
            verileriTemizle();

        }
        //----------------------------------------------------------------------------------------------
        void verileriTemizle()
        {
            txtAracNo.Clear();
            txtMarka.Clear();
            txtModel.Clear();
            txtPlaka.Clear();
            txtAracNo.Focus();
        }
        //----------------------------------------------------------------------------------------------
        private void btnKayitGuncelle_Click(object sender, EventArgs e)
        {
            komut.CommandText = "Update Araclar set Marka=@Marka,Model=@Model,Plaka=@Plaka Where AracNo=@AracNo";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
            komut.Parameters.AddWithValue("@Model", txtModel.Text);
            komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            komut.Parameters.AddWithValue("@AracNo", seciliAracNo);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Güncellendi");
            else
                MessageBox.Show("Kayıt  Güncellenemedi");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();
            DgridAraclar.Focus();

        }
        //----------------------------------------------------------------------------------------------
        private void DgridAraclar_DoubleClick(object sender, EventArgs e)
        {
            if (_secim)
            {
                KiralaForm.AracId = Convert.ToInt32(DgridAraclar.Rows[seciliKayit].Cells[0].Value);
                this.Close();
            }

            txtAracNo.Text = DgridAraclar.Rows[seciliKayit].Cells[0].Value.ToString();
            txtMarka.Text = DgridAraclar.Rows[seciliKayit].Cells[1].Value.ToString();
            txtModel.Text = DgridAraclar.Rows[seciliKayit].Cells[2].Value.ToString();
            txtPlaka.Text = DgridAraclar.Rows[seciliKayit].Cells[3].Value.ToString();
            seciliAracNo= DgridAraclar.Rows[seciliKayit].Cells[0].Value.ToString();

        }
        //----------------------------------------------------------------------------------------------
        int seciliKayit;
        string seciliAracNo;
        private void DgridAraclar_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            seciliKayit = e.RowIndex;
        }
        //----------------------------------------------------------------------------------------------
        private void btnKayitSil_Click(object sender, EventArgs e)
        {
            komut.CommandText = "Delete From Araclar Where AracNo=@AracNo";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@AracNo", txtAracNo.Text);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Silindi");
            else
                MessageBox.Show("Kayıt Bulunamadı.");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();

        }
        //-------------------------------------------------------------------------------------------
        private void txtFilitre_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_Adresler = new DataTable("Araclar");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from Araclar Where Marka like @Marka", baglanti);
                adaptor.SelectCommand.Parameters.AddWithValue("@Marka", txtFilitre.Text + "%");
                baglan();
                adaptor.Fill(dt_Adresler);
                DgridAraclar.DataSource = dt_Adresler;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }
    }
}
