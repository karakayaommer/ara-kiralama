using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AracKiralama
{
    public partial class KiralaForm : Form
    {
        public static int AracId;
        public static int MusteriId;
        
        public KiralaForm()
        {
            InitializeComponent();
        }

        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();

        private void Connect()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        private void VerileriGetir()
        {
            try
            {
                DataTable dataTable = new DataTable("ViewAracKira");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from ViewAracKira", connection);
                Connect();
                adaptor.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                connection.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }
        private void MusteriBilgileriGetir(int musteriId)
        {
            try
            {
                Connect();
                var idYeGoreGetir = "Select * From Musteri Where MKod=" + musteriId;
                var komut = new SqlCommand(idYeGoreGetir, connection);
                var dataReader = komut.ExecuteReader();
                while (dataReader.Read())
                {
                    txtAdi.Text = dataReader["Ad"].ToString();
                    txtSoyadi.Text = dataReader["Soyad"].ToString();
                    txtTcNo.Text = dataReader["Tcno"].ToString();
                    txtTel.Text = dataReader["Tel"].ToString();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Meydana Geldi:\n" + ex.Message);
            }
        }

        private void AracBilgileriGetir(int aracId)
        {
            try
            {
                Connect();
                var idYeGoreGetir = "Select * From Araclar Where AracNo=" + aracId;
                var komut = new SqlCommand(idYeGoreGetir, connection);
                var dataReader = komut.ExecuteReader();
                while (dataReader.Read())
                {
                    txtMarka.Text = dataReader["Marka"].ToString();
                    txtModel.Text = dataReader["Model"].ToString();
                    txtPlaka.Text = dataReader["Plaka"].ToString();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir Hata Meydana Geldi:\n" + ex.Message);
            }
        }

        private void Temizle()
        {
            txtAdi.Clear();
            txtSoyadi.Clear();
            txtTcNo.Clear();
            txtTel.Clear();
            txtMarka.Clear();
            txtModel.Clear();
            txtPlaka.Clear();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtAdi_Click(object sender, EventArgs e)
        {
            frmMusteriler form = new frmMusteriler(true);
            form.ShowDialog();
            MusteriBilgileriGetir(MusteriId);
            txtMarka.Enabled = MusteriId > 0;
        }

        private void txtMarka_Click(object sender, EventArgs e)
        {
            frmAraclar form = new frmAraclar(true);
            form.ShowDialog();
            AracBilgileriGetir(AracId);
            btnKirala.Enabled = AracId > 0;
        }

        private void KiralaForm_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = Form1.baglantiBilgisi;
            command.Connection = connection;
            VerileriGetir();
            txtBaslama.Value = DateTime.Now.Date;
            txtTeslim.Value = DateTime.Now.Date;
        }

        private void btnKirala_Click(object sender, EventArgs e)
        {
            var nowDate = DateTime.Now.Date;

            for (int i = 0; i < dataGridView.RowCount; i++)
            {
                var plaka = (string)dataGridView.Rows[i].Cells["Plaka"].Value;
                var tarih = Convert.ToDateTime(dataGridView.Rows[i].Cells["TesTarih"].Value);
                if (plaka == txtPlaka.Text)
                {
                    if (nowDate <= tarih)
                    {
                        MessageBox.Show(
                            $"Araç Başka Bir Müşteriye Kiralanmış. Tekrar Kiralama İşlemi Yapılamaz.\nAraç Teslim Tarihi:{tarih.ToShortDateString()}",
                            "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            var dialogMessage = MessageBox.Show(
                $"{txtAdi.Text} {txtSoyadi.Text} Adlı Müşteriye {txtMarka.Text} Markalı {txtModel.Text} Model Aracı {txtBaslama.Text} Tarihinden {txtTeslim.Text} Tarihine Kadar Kiralamak Üzeresiniz. İşlemi Onaylıyor Musunuz?", "Kiralama Onay",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogMessage == DialogResult.Yes)
            {
                Connect();
                command.CommandText = "insert into Kiralama values(@MKod,@AracNo,@Tarih,@TesTarih)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@MKod", MusteriId);
                command.Parameters.AddWithValue("@AracNo", AracId);
                command.Parameters.AddWithValue("@Tarih", txtBaslama.Value);
                command.Parameters.AddWithValue("@TesTarih", txtTeslim.Value);
                MessageBox.Show(command.ExecuteNonQuery() >= 1 ? "Kiralama İşlemi Başarılı" : "Bir Hata Meydana Geldi");
                connection.Close();
                VerileriGetir();
                Temizle();
                txtAdi.Focus();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
