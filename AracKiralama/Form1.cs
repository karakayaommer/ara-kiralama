using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AracKiralama;




namespace AracKiralama
{
    public partial class Form1 : Form
    {
        public static string baglantiBilgisi =
            "Data Source =(localdb)\\v11.0;Initial Catalog=ARACKIRALAMA;" +
            "uid=sa;pwd=123";
        public Form1()
        {
            InitializeComponent();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        private void araçlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAraclar Araclar = new frmAraclar();
            Araclar.ShowDialog();
        }

        private void müşterilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteriler musteri = new frmMusteriler();
            musteri.ShowDialog();
        }

        private void araçKiralamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KiralaForm form = new KiralaForm();
            form.ShowDialog();
        }
    }
}
