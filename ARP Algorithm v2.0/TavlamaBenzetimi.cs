using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ARP_Algorithm_v2._0
{
    public partial class TavlamaBenzetimi : Form
    {
        #region Tanımlamalar

        Controllers BL = new Controllers();
        Başlangıç BForm = new Başlangıç();
        Stopwatch Sw = new Stopwatch();

        Random r = new Random();

        public static DataTable TabloT = new DataTable();

        #region Çizim Araçları

        Graphics ÇizimYap; // Çizim yapacak değişkenimiz.
        Pen Kalem1 = new System.Drawing.Pen(Color.White, 2); // Güzergah için çizgi çizecek olan kalemimiz.
        Brush Dolgu1 = new SolidBrush(Color.Black); // Elipsleri dolduracak olan fırçamızı tanımladık.
        Font Yazı1 = new Font("Georgia", 12, FontStyle.Bold); // Lokasyonları yazacak olan grafik nesnesi.
        Brush Dolgu2 = new SolidBrush(Color.White); // Lokasyon isimlerini yazacak olan fırçamız

        DataTable Tablo2 = new DataTable();
        #endregion

        #endregion

        #region Değişkenler

        int BaşlangıçSıcaklığı;
        double SıcaklıkAzalışıSabit;
        double SıcaklıkAzalışıA;
        double İterasyonBaşlangıç;
        double İterasyonSabit;
        double İterasyonA;
        double BKÇKO, SKÇKO;
        double DuruşSıcaklık;
        double DuruşMesafe;
        int TekrarSayısı;

        int Id = 0;

        int X1 = 0;

        double EnİyiÇözüm = 0;

        double Çözüm = 0;

        double Delta;

        string Değişim = "Olmadı";

        TimeSpan Süre1;
        int İterasyonSayısı = 0;

        int L1, L2, L3, L4;

        int[] RotaSeçim = new int[TabloT.Rows.Count];
        int[] EnİyiRotaSeçim = new int[TabloT.Rows.Count];
        #endregion

        #region Ana Form İşlemleri

        public TavlamaBenzetimi()
        {
            InitializeComponent();
        }

       
        private void TavlamaBenzetimi_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            #region Tablo Aktrımı 
            if (Başlangıç.VeriKaynağı == "Sabit Lokasyon")
            {
                TabloT = Controllers.Tablo1;
            }

            else if (Başlangıç.VeriKaynağı=="Dış Veri")
            {
                if (Başlangıç.DosyaUzantısı=="accdb")
                {
                    TabloT = Controllers.Tablo1;
                }

                else
                {
                    TabloT = Başlangıç.Tablo1;
                }

            }

            #endregion

            Tablo2.Columns.Add("Sıra", typeof(int));
            Tablo2.Columns.Add("Rota", typeof(string));
            Tablo2.Columns.Add("Mesafe", typeof(double));
            Tablo2.Columns.Add("ÇizimSüre", typeof(TimeSpan));
            Tablo2.Columns.Add("Delta", typeof(double));
            Tablo2.Columns.Add("Durum 1", typeof(double));
            Tablo2.Columns.Add("R. Değer", typeof(double));
            Tablo2.Columns.Add("Durum 2", typeof(double));
            Tablo2.Columns.Add("Değişim", typeof(string));
        }

        #endregion

        #region PictureBox İşlemleri

        private void PicKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PicSimge_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region ComboBox İşlemleri

        private void ComboSıcaklıkAzalışı_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboSıcaklıkAzalışı.Text == "Aritmetik")
            {

                NumericAzalışSabit.Enabled = true;
                ComboAzalışA.Enabled = false;
                Comboİterasyon.Enabled = true;
            }

            else if (ComboSıcaklıkAzalışı.Text == "Geometrik")
            {

                NumericAzalışSabit.Enabled = false;
                ComboAzalışA.Enabled = true;
                Comboİterasyon.Enabled = true;
            }

            else if (ComboSıcaklıkAzalışı.Text == "Ters Fonksiyon")
            {

                NumericAzalışSabit.Enabled = true;
                ComboAzalışA.Enabled = false;
                Comboİterasyon.Enabled = true;
            }

            else if (ComboSıcaklıkAzalışı.Text == "Logaritmik")
            {

                NumericAzalışSabit.Enabled = true;
                ComboAzalışA.Enabled = false;
                Comboİterasyon.Enabled = true;
            }
        }

        private void Comboİterasyon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comboİterasyon.Text == "Sabit")
            {
                NumericİterasyonB.Enabled = false;
                NumericİterasyonSabit.Enabled = true;
            }

            else if (Comboİterasyon.Text == "Aritmetik" || Comboİterasyon.Text == "Logaritmik")
            {
                NumericİterasyonB.Enabled = true;
                NumericİterasyonSabit.Enabled = true;
            }

            else if (Comboİterasyon.Text == "Geometrik" || Comboİterasyon.Text == "Üstel")
            {
                NumericİterasyonB.Enabled = true;
                NumericİterasyonSabit.Enabled = false;
                ComboİterasyonA.Enabled = true;
            }

            ComboBKO.Enabled = true;
            ComboSKO.Enabled = true;
            ComboDuruş.Enabled = true;
            NumericDuruşSıcaklık.Enabled = true;
            
            NumericDeney.Enabled = true;
        }

        #endregion

        private void AlgoritmaBaşlat_Click(object sender, EventArgs e)
        {
            GridLokasyon.DataSource = TabloT;

            #region Sıcaklık Azalışı Durum Kontrolü

            if (ComboSıcaklıkAzalışı.Text == "Aritmetik" || ComboSıcaklıkAzalışı.Text == "Ters Fonksiyon" || ComboSıcaklıkAzalışı.Text == "Logaritmik")
            {
                if (NumericAzalışSabit.Value <= 0)
                {
                    MessageBox.Show("Sıcaklık Azalışı değeri 0 veya 0 dan küçük olamaz!!!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    SıcaklıkAzalışıSabit = Convert.ToDouble(NumericAzalışSabit.Value);
                }
            }

            else if (ComboSıcaklıkAzalışı.Text == "Geometrik")
            {
                if (ComboAzalışA.Text == "")
                {
                    MessageBox.Show("Sıcaklık Azalışı A için bir Değer Girmediniz!!!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {

                    SıcaklıkAzalışıA = (Convert.ToDouble(ComboAzalışA.Text)) * (0.01);
                }

            }

            #endregion

            #region İterasyon Sayısı Kontrol

            if (Comboİterasyon.Text == "Sabit")
            {
                İterasyonSabit = Convert.ToDouble(NumericİterasyonSabit.Value);
                İterasyonSayısı = Convert.ToInt16(İterasyonSabit);

            }

            else if (Comboİterasyon.Text == "Aritmetik")
            {
                İterasyonBaşlangıç = Convert.ToDouble(NumericİterasyonB.Value);
                İterasyonSabit = Convert.ToDouble(NumericİterasyonSabit.Value);
                İterasyonSayısı = Convert.ToInt16(İterasyonBaşlangıç);
            }

            else if (Comboİterasyon.Text == "Geometrik" || Comboİterasyon.Text == "Üstel")
            {
                İterasyonBaşlangıç = Convert.ToDouble(NumericİterasyonB.Value);
                İterasyonA = Convert.ToDouble(ComboİterasyonA.Text);
                İterasyonSayısı = Convert.ToInt16(İterasyonBaşlangıç);
            }

            else if (Comboİterasyon.Text == "Logaritmik")
            {
                İterasyonSabit = Convert.ToDouble(NumericİterasyonSabit.Value);
                İterasyonSayısı = Convert.ToInt16(İterasyonSabit);
            }

            #endregion

            #region Ortak Ana Kodlar

            #region Değişken Alımları

            BaşlangıçSıcaklığı = Convert.ToInt16(NumericSıcaklık.Value);

            #region Duruş Değişkenleri

            BKÇKO = Convert.ToDouble(ComboBKO.Text);
            SKÇKO = Convert.ToDouble(ComboSKO.Text);

            DuruşSıcaklık = Convert.ToDouble(NumericDuruşSıcaklık.Value);
            
            TekrarSayısı = Convert.ToInt16(NumericDeney.Value);

            #endregion

            #endregion

            #region Tavlama Benzetimi Uygulama

            string Sıralama = "";

            Array.Resize(ref RotaSeçim, TabloT.Rows.Count);
            Array.Resize(ref EnİyiRotaSeçim, TabloT.Rows.Count);

            for (int o1 = 0; o1 < TekrarSayısı; o1++) // Deney Analizi Döngüsü
            {

                Id = 0;

                #region İlk Değer Üret

                Tablo2.Rows.Add(o1 + 1, DateTime.Now.ToString() + "   <----- Deneme Sayısı---->  " + (int)(o1 + 1));

                Id++;

                Sw.Start();

                BL.RastgeleDeğerÜret(RotaSeçim, EnİyiRotaSeçim, TabloT, X1);

                BL.MesafeHesapla(TabloT, RotaSeçim);

                Süre1 = Sw.Elapsed;


                Tablo2.Rows.Add(Id, Controllers.Sıralama, Controllers.Toplam1, Süre1);

                EnİyiÇözüm = Controllers.Toplam1;

                #endregion

                for (double BS = BaşlangıçSıcaklığı; BS > DuruşSıcaklık; )
                {

                    for (double İS = İterasyonSayısı; İS > 0; İS--)
                    {

                        Sw.Start();

                        Id++;

                        Sıralama = "0";

                        Değişim = "Gerçekleşmedi";

                        do
                        {
                            L1 = r.Next(1, TabloT.Rows.Count - 1);
                            L2 = r.Next(1, TabloT.Rows.Count - 1);
                        } while (L1 == L2);

                        L3 = RotaSeçim[L2];
                        L4 = RotaSeçim[L1];

                        RotaSeçim[L1] = L3;
                        RotaSeçim[L2] = L4;

                        for (int i5 = 1; i5 < TabloT.Rows.Count - 1; i5++)
                        {
                            Sıralama += " - " + TabloT.Rows[RotaSeçim[i5]]["Lokasyon"].ToString();
                        }

                        Sıralama += " - " + TabloT.Rows[TabloT.Rows.Count - 1]["Lokasyon"].ToString();

                        BL.MesafeHesapla(TabloT, RotaSeçim);

                        Süre1 = Sw.Elapsed;

                        Çözüm = Controllers.Toplam1;

                        double Durum1 = 0, Durum2 = 0, Durum3 = 0;


                        if (Çözüm - EnİyiÇözüm < 0)
                        {
                            EnİyiRotaSeçim = RotaSeçim;
                            RotaSeçim = EnİyiRotaSeçim;
                            EnİyiÇözüm = Çözüm;
                            Değişim = "Gerçekleşti";
                        }

                        else
                        {
                            Delta = Çözüm - EnİyiÇözüm;
                            Durum1 = Math.Pow(2.718, (-(Delta / BaşlangıçSıcaklığı)));
                            Durum2 = Convert.ToDouble(r.Next(1, 10)) * 0.1;
                            Durum3 = (Durum2 * BKÇKO * SKÇKO) / 10;

                            if (Durum3 < Durum1)
                            {
                                Değişim = "Gerçekleşti";
                                EnİyiRotaSeçim = RotaSeçim;
                                RotaSeçim = EnİyiRotaSeçim;
                                EnİyiÇözüm = Çözüm;
                            }
                        }

                        Tablo2.Rows.Add(Id, Sıralama, Controllers.Toplam1, Süre1, Delta, Durum1, Durum2, Durum3, Değişim);
                    }



                    #region Sıcaklık Azalışı Durum Kontrolü

                    if (ComboSıcaklıkAzalışı.Text == "Aritmetik")
                    {
                        BS = Convert.ToInt64(BS - SıcaklıkAzalışıSabit);
                    }

                    else if (ComboSıcaklıkAzalışı.Text == "Geometrik")
                    {
                        BS = Convert.ToInt64(BS * SıcaklıkAzalışıA);
                    }

                    else if (ComboSıcaklıkAzalışı.Text == "Ters Fonksiyon")
                    {
                        BS = Convert.ToInt64(SıcaklıkAzalışıSabit / (1 + Id));

                    }

                    else if (ComboSıcaklıkAzalışı.Text == "Logaritmik")
                    {
                        BS = Convert.ToInt64(SıcaklıkAzalışıSabit / (Math.Log(1 + Id)));
                    }


                    #endregion

                    #region İterasyon Sayısı Belirle

                    if (Comboİterasyon.Text == "Sabit")
                    {
                        İterasyonSayısı = Convert.ToInt16(İterasyonSabit);
                    }

                    else if (Comboİterasyon.Text == "Aritmetik")
                    {
                        İterasyonSayısı = Convert.ToInt16(İterasyonSayısı + İterasyonSabit);
                    }

                    else if (Comboİterasyon.Text == "Geometrik")
                    {
                        İterasyonSayısı = Convert.ToInt16(İterasyonSayısı / İterasyonA);
                    }

                    else if (Comboİterasyon.Text == "Logaritmik")
                    {
                        İterasyonSayısı = Convert.ToInt16(İterasyonSabit / Math.Log(BS));
                    }

                    else if (Comboİterasyon.Text == "Üstel")
                    {
                        İterasyonSayısı = Convert.ToInt16(Math.Pow(İterasyonSayısı, (1 / İterasyonA)));
                    }

                    #endregion

                }

                Sıralama = "0";

                for (int i5 = 1; i5 < TabloT.Rows.Count - 1; i5++)
                {
                    Sıralama += " - " + TabloT.Rows[EnİyiRotaSeçim[i5]]["Lokasyon"].ToString();
                }

                Sıralama += " - " + TabloT.Rows[TabloT.Rows.Count-1]["Lokasyon"].ToString();

                TextSıralama.Text = Sıralama;
                TextSıralama2.Text = Sıralama;
                TextMesafe1.Text = EnİyiÇözüm.ToString();
                TextMesafe2.Text = EnİyiÇözüm.ToString();
                Tablo2.Rows.Add();
                Tablo2.Rows.Add(o1 + 1, Sıralama, EnİyiÇözüm);
                Tablo2.Rows.Add();

            }

            GridÇözüm.DataSource = null;
            GridÇözüm.DataSource = Tablo2;

            #endregion

            #endregion

            BL.TavlamaDenet("Tavlama Benzetimi.accdb", NumericSıcaklık.Value.ToString(), ComboSıcaklıkAzalışı.Text, NumericAzalışSabit.Value.ToString(), ComboAzalışA.Text, Comboİterasyon.Text, NumericİterasyonB.Value.ToString(), NumericİterasyonSabit.Value.ToString(), ComboİterasyonA.Text, ComboBKO.Text, ComboSKO.Text, NumericDuruşSıcaklık.Value.ToString(), EnİyiÇözüm);
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            BL.LokasyonYerleştir(ÇizimYap, PicHarita, TabloT, Dolgu1, Dolgu2, Yazı1);

            Pen Kalem = new System.Drawing.Pen(Color.Black, 2);
            ÇizimYap = PicHarita.CreateGraphics();
            for (int i6 = 0; i6 < RotaSeçim.Length - 1; i6++)
            {
                double v3 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6]]["X"]);
                double v4 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6]]["Y"]);
                double v5 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6 + 1]]["X"]);
                double v6 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6+1]]["Y"]);
                int v1 = Convert.ToInt32(v3);
                int v2 = Convert.ToInt32(v4);
                int v7 = Convert.ToInt32(v5);
                int v8 = Convert.ToInt32(v6);

                
                ÇizimYap.DrawLine(Kalem, v1, v2, v7, v8);
            }
           
        }
    }
}
