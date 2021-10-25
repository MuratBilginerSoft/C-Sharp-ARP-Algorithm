using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ARP_Algorithm_v2._0
{
    public partial class GenetikAlgoritmalar : Form
    {

        public static DataTable TabloT = new DataTable();
        DataTable TümVeriler = new DataTable();
        DataTable TümVeriler2 = new DataTable();
        DataTable TümVeriler3 = new DataTable();
        DataTable TümVeriler4 = new DataTable();

        int[] RotaSeçim = new int[TabloT.Rows.Count];
        int[] RotaTakip = new int[TabloT.Rows.Count];
        int[] EnİyiRotaSeçim = new int[TabloT.Rows.Count];
        int[] RotaTakip2 = new int[TabloT.Rows.Count];
        string[] Rotam;

        Controllers BL = new Controllers();
        Başlangıç BForm = new Başlangıç();

        public GenetikAlgoritmalar()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState=FormWindowState.Minimized;
        }

        private void GenetikAlgoritmalar_Load(object sender, EventArgs e)
        {
            if (Başlangıç.VeriKaynağı == "Sabit Lokasyon")
            {
                TabloT = Controllers.Tablo1;
            }

            TümVeriler.Columns.Add("Sıra", typeof(int));
            TümVeriler.Columns.Add("Rota", typeof(string));
            TümVeriler.Columns.Add("Mesafe", typeof(double));

            TümVeriler2.Columns.Add("Sıra", typeof(int));
            TümVeriler2.Columns.Add("Rota", typeof(string));
            TümVeriler2.Columns.Add("Mesafe", typeof(double));

            TümVeriler3.Columns.Add("Sıra", typeof(int));
            TümVeriler3.Columns.Add("Rota", typeof(string));
            TümVeriler3.Columns.Add("Mesafe", typeof(double));

            TümVeriler4.Columns.Add("Sıra", typeof(int));
            TümVeriler4.Columns.Add("Rota", typeof(string));
            TümVeriler4.Columns.Add("Mesafe", typeof(double));
        }

        int X1;
        int Id = 0;
        private void AlgoritmaBaşlat_Click(object sender, EventArgs e)
        {
            
            GridLokasyon.DataSource = TabloT;

            double PB = Convert.ToDouble(NUDPB.Value);

            Array.Resize(ref RotaSeçim, TabloT.Rows.Count);
            Array.Resize(ref EnİyiRotaSeçim, TabloT.Rows.Count);
            Array.Resize(ref RotaTakip, TabloT.Rows.Count);
            Array.Resize(ref RotaTakip2, TabloT.Rows.Count);

            for (int i = 0; i < PB; i++)
            {
                BL.RastgeleDeğerÜrettt(RotaSeçim, EnİyiRotaSeçim,TabloT, X1);

                BL.MesafeHesapla(TabloT, RotaSeçim);

                TümVeriler.Rows.Add(i+1, Controllers.Sıralama, Controllers.Toplam1);
            }

            GridİlkPopülasyon.DataSource = TümVeriler;

            var query2 = from C in TümVeriler.AsEnumerable()
                         orderby C.Field<double>("Mesafe") ascending
                         select C;

            foreach (var q in query2)
            {
                TümVeriler2.Rows.Add(q.Field<int>("Sıra"), q.Field<string>("Rota"), q.Field<double>("Mesafe"));
            }

            GridPopulasyonSıralı.DataSource = TümVeriler2;

            for (int i = 0; i < 2000; i++)
            {
                BL.RastgeleDeğerÜrettt(RotaSeçim, EnİyiRotaSeçim,TabloT, X1);

                BL.MesafeHesapla(TabloT, RotaSeçim);

                TümVeriler3.Rows.Add(i+1, Controllers.Sıralama, Controllers.Toplam1);
            }

            GridÇözüm.DataSource = TümVeriler3;

            var query3 = from C in TümVeriler3.AsEnumerable()
                         orderby C.Field<double>("Mesafe") ascending
                         select C;

            foreach (var q in query3)
            {
                TümVeriler4.Rows.Add(q.Field<int>("Sıra"), q.Field<string>("Rota"), q.Field<double>("Mesafe"));
            }

            

            GridSıralıÇözüm.DataSource = TümVeriler4;

            BL.SabitLokosyan2("GenetikDeney", "Genetik Algoritma.accdb");

            Random r = new Random();


            int v1 = r.Next(1, Controllers.Tablo3.Rows.Count);

            TextSıralama.Text = TümVeriler4.Rows[0]["Rota"].ToString();
            TextMesafe1.Text = Controllers.Tablo3.Rows[1]["Mesafe"].ToString();

            BL.GenetikDeney("Genetik Algoritma.accdb", NUDPB.Value.ToString(), ComboÇO.Text, ComboMD.Text, ComboÇY.Text, NUDDK.Value.ToString(), Convert.ToDouble(TextMesafe1.Text));

           


        }

        private void PicRotaÇiz_Click(object sender, EventArgs e)
        {
            GridLokasyon.DataSource = null;
            GridSıralıÇözüm.DataSource = null;
            TümVeriler.Rows.Clear();
            TümVeriler2.Rows.Clear();
            TümVeriler3.Rows.Clear();
            TümVeriler4.Rows.Clear();
        }
    }
}
