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
    public partial class TabuArama : Form
    {
        #region Tanımlamalar

        Controllers BL = new Controllers();
        Başlangıç BForm = new Başlangıç();
        Stopwatch Sw = new Stopwatch();

        Random r = new Random();

        public static DataTable TabloT = new DataTable();
        public static DataTable TabloS = new DataTable();

        #region Çizim Araçları

        Graphics ÇizimYap; // Çizim yapacak değişkenimiz.
        Pen Kalem1 = new System.Drawing.Pen(Color.White, 2); // Güzergah için çizgi çizecek olan kalemimiz.
        Brush Dolgu1 = new SolidBrush(Color.Black); // Elipsleri dolduracak olan fırçamızı tanımladık.
        Font Yazı1 = new Font("Georgia", 12, FontStyle.Bold); // Lokasyonları yazacak olan grafik nesnesi.
        Brush Dolgu2 = new SolidBrush(Color.White); // Lokasyon isimlerini yazacak olan fırçamız

        DataTable TümVeriler = new DataTable();
        DataTable TabloDöngü = new DataTable();
        DataTable TabuList = new DataTable();
        DataTable EnİyiRota = new DataTable();
        DataTable DeneyEnİyi = new DataTable();
        DataTable DeneyEnİyi2 = new DataTable();
        #endregion

        #endregion

        #region Değişkenler
        
        int AdımSayısı;
        double EnİyiMesafe;
        double İterasyonBaşlangıç;
        double İterasyonSabit;
        double İterasyonA;
        int TabuBBoyutu, TabuDBBoyutu;
        int TekrarSayısı;
        int Id = 0;

        string Tspİsmi;
        string RotaSeçimi;
        string Yöntem;
        string TabloAdı3;

        int X1 = 0;

        bool TabuDurumu = false;

        double EnİyiÇözüm = 0;

        double Çözüm = 0;

        string DosyaYolu = "SezgizelSonuc.accdb";

        TimeSpan Süre1;
        int İterasyonSayısı = 0;

        int L1, L2, L3, L4;

        int zet1 = 0; //Tabunun o anki döngüsünün rotasayısını tutacak
        int zet3 = 0;
        int zet2 = 0; // TabuListteki değeri kontrol ediyor
        int zet4 = 0;
        int[] RotaSeçim = new int[TabloT.Rows.Count];
        int[] RotaTakip = new int[TabloT.Rows.Count];
        int[] EnİyiRotaSeçim = new int[TabloT.Rows.Count];
        int[] RotaTakip2 = new int[TabloT.Rows.Count];
        string[] Rotam;
        #endregion

        #region Ana Form İşlemleri

        public TabuArama()
        {
            InitializeComponent();
        }

        private void TabuArama_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            

            #region Tablo Aktrımı
            if (Başlangıç.VeriKaynağı == "Sabit Lokasyon")
            {
                TabloT = Controllers.Tablo1;
            }

            else if (Başlangıç.VeriKaynağı == "Dış Veri")
            {
                if (Başlangıç.DosyaUzantısı == "accdb")
                {
                    TabloT = Controllers.Tablo1;
                }

                else
                {
                    TabloT = Başlangıç.Tablo1;
                }

            }

            #endregion

            #region Kontrol Tablosu Oluştur

            TümVeriler.Columns.Add("Sıra", typeof(int));
            TümVeriler.Columns.Add("Rota", typeof(string));
            TümVeriler.Columns.Add("Mesafe", typeof(double));
            TümVeriler.Columns.Add("ÇizimSüre", typeof(TimeSpan));
            TümVeriler.Columns.Add("Delta", typeof(double));
            TümVeriler.Columns.Add("Durum 1", typeof(double));
            TümVeriler.Columns.Add("R. Değer", typeof(double));
            TümVeriler.Columns.Add("Durum 2", typeof(double));
            TümVeriler.Columns.Add("Değişim", typeof(string));

            TabloDöngü.Columns.Add("Sıra", typeof(int));
            TabloDöngü.Columns.Add("Rota", typeof(string));
            TabloDöngü.Columns.Add("Mesafe", typeof(double));

            TabuList.Columns.Add("Sıra", typeof(int));
            TabuList.Columns.Add("Rota", typeof(string));
            TabuList.Columns.Add("Mesafe", typeof(double));

            EnİyiRota.Columns.Add("Sıra", typeof(int));
            EnİyiRota.Columns.Add("Rota", typeof(string));
            EnİyiRota.Columns.Add("Mesafe", typeof(double));

            DeneyEnİyi.Columns.Add("DeneyNo", typeof(string));
            DeneyEnİyi.Columns.Add("Rota", typeof(string));
            DeneyEnİyi.Columns.Add("Mesafe", typeof(double));

            DeneyEnİyi2.Columns.Add("DeneyNo", typeof(string));
            DeneyEnİyi2.Columns.Add("Rota", typeof(string));
            DeneyEnİyi2.Columns.Add("Mesafe", typeof(double));


            #endregion
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

        #region MenüItem İşlemleri

        private void MenüÇıkış_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region ComboBox İşlemleri

        private void ComboDurdurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboDurdurma.Text == "Belirli Adım Sayısı")
            {
                NumericAdımSayısı.Enabled = true;
               
                Comboİterasyon.Enabled = true;
            }

            else if (ComboDurdurma.Text == "En İyi Çözüm")
            {
                NumericAdımSayısı.Enabled = false;
                
                Comboİterasyon.Enabled = true;
            }

            else
            {
                NumericAdımSayısı.Enabled = true;
               
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

            NumericTBelleği.Enabled = true;
            NumericDTBelleği.Enabled = true;
            AlgoritmaBaşlat.Enabled = true;
            NumericTekrar.Enabled = true;
        }

        #endregion

        private void AlgoritmaBaşlat_Click(object sender, EventArgs e)
        {
            
           
            #region Temizle

            GridLokasyon.DataSource = TabloT;
            TabuList.Clear();
            TümVeriler.Clear();
            TabloDöngü.Clear();
            DeneyEnİyi.Clear();
            EnİyiRota.Clear();

            GridÇözüm.DataSource = null;
            dataGridView1.DataSource = null;
            DataEnİyi.DataSource = null;

            #endregion

            #region Değişken Alımları

            #region Yöntem Seçimi Değişkenleri

            Tspİsmi = TextDosyaİsmi.Text;
            RotaSeçimi = ComboRotaSeçimi.Text;
            Yöntem = "TabuArama";
            TabloAdı3="TabuEnİyi";

            #endregion

            #region Durdurma Kriteri

            if (ComboDurdurma.Text == "En İyi Çözüm")
            {
                AdımSayısı = r.Next(10, 51);
                
            }

            else
            {
                AdımSayısı = Convert.ToInt32(NumericAdımSayısı.Value);
              
            }

            #endregion

            #region Tabu Değerleri

            TabuBBoyutu = Convert.ToInt32(NumericTBelleği.Value);
            TabuDBBoyutu = Convert.ToInt32(NumericDTBelleği.Value);

            #endregion

            #region Dizi Yeni Boyutlandır

            Array.Resize(ref RotaSeçim, TabloT.Rows.Count);
            Array.Resize(ref EnİyiRotaSeçim, TabloT.Rows.Count);
            Array.Resize(ref RotaTakip, TabloT.Rows.Count);
            Array.Resize(ref RotaTakip2, TabloT.Rows.Count);

            #endregion

            TekrarSayısı = Convert.ToInt32(NumericTekrar.Value);

            #endregion

            #region Tabu Arama Çalıştır

            if (NumericDTBelleği.Value <= NumericTBelleği.Value)
            {
                for (int i1 = 0; i1 < TekrarSayısı; i1++)
                {
                    #region Değişken İlk Durumu

                    TabuList.Clear();
                    TabuDurumu = false;

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

                    #endregion

                    #region Rota Seçme Kriteri

                    #region En iyi Rota Kullanımı

                    if (ComboRotaSeçimi.Text == "En İyi Rota")
                    {
                        BL.SezgiselEnİyi(TabloAdı3, DosyaYolu, Tspİsmi);
                        Rotam = Controllers.Tablo2.Rows[0]["EnİyiYol"].ToString().Split('-');

                        Array.Resize(ref RotaSeçim, Rotam.Length);

                        for (int i = 0; i < Rotam.Length; i++)
                        {
                            RotaSeçim[i] = Convert.ToInt32(Rotam[i].ToString());
                        }
                    }

                    #endregion

                    #region Rastgele Rota Kullanımı

                    else if (ComboRotaSeçimi.Text == "Rastgele Rota")
                    {
                        #region İlk Değer Atama

                        BL.RastgeleDeğerÜrett(RotaSeçim, EnİyiRotaSeçim, TabloT, X1);

                        #endregion
                    }

                    #endregion

                    #endregion

                    #region İlk Değer İlk İşlem

                    Id = 0; // Toplamda o Adımdaki toplam rota sayısını belirleyecek.

                    TümVeriler.Rows.Add(i1 + 1, DateTime.Now.ToString() + "   <----- Deneme Sayısı---->  " + (int)(i1 + 1));

                    Id++;

                    for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                    {
                        EnİyiRotaSeçim[i] = RotaSeçim[i];
                        RotaTakip[i] = RotaSeçim[i];
                        RotaTakip2[i] = RotaSeçim[i];
                    }

                    BL.MesafeHesapla(TabloT, RotaSeçim);

                    TümVeriler.Rows.Add(Id, Controllers.Sıralama, Controllers.Toplam1, Süre1);

                    Çözüm = Controllers.Toplam1;
                    EnİyiÇözüm = Çözüm;

                    #endregion

                    #region Adım Sayısı

                    for (int i3 = 0; i3 < AdımSayısı; i3++)
                    {
                        #region Değişkenler İlk Durumu

                        zet1 = 0;  zet4 = 0;

                        TabloDöngü.Clear();

                        #endregion

                        #region İterasyon Sayısı

                        for (int İS = İterasyonSayısı; İS > 0; İS--)
                        {
                            zet1++; Id++;

                            #region Tabuda Eleman Yokken

                            if (TabuDurumu == false)
                            {
                                do
                                {
                                    L1 = r.Next(1, TabloT.Rows.Count - 1);
                                    L2 = r.Next(1, TabloT.Rows.Count - 1);
                                } while (L1 == L2);

                                L3 = RotaSeçim[L2];
                                L4 = RotaSeçim[L1];

                                RotaSeçim[L1] = L3;
                                RotaSeçim[L2] = L4;

                                BL.RotaSırala(RotaSeçim, TabloT);

                                BL.MesafeHesapla(TabloT, RotaSeçim);

                                Çözüm = Controllers.Toplam1;

                                TabloDöngü.Rows.Add(zet1, Controllers.Sıralama2, Çözüm);
                                TümVeriler.Rows.Add(Id, Controllers.Sıralama2, Çözüm);

                                if (Çözüm < EnİyiÇözüm)
                                {
                                    zet4++;
                                    EnİyiÇözüm = Çözüm;

                                    for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                                    {
                                        EnİyiRotaSeçim[i] = RotaSeçim[i];
                                    }

                                    BL.RotaSırala(EnİyiRotaSeçim, TabloT);
                                    EnİyiRota.Rows.Add(Id, Controllers.Sıralama2, Çözüm);
                                }

                                for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                                {
                                    RotaSeçim[i] = RotaTakip[i];
                                }
                            }

                            #endregion

                            #region Tabuda Eleman Varken

                            else if (TabuDurumu == true)
                            {
                            git:
                                for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                                {
                                    RotaSeçim[i] = RotaTakip2[i];
                                }

                                do
                                {
                                    L1 = r.Next(1, TabloT.Rows.Count - 1);
                                    L2 = r.Next(1, TabloT.Rows.Count - 1);
                                } while (L1 == L2);

                                L3 = RotaSeçim[L2];
                                L4 = RotaSeçim[L1];

                                RotaSeçim[L1] = L3;
                                RotaSeçim[L2] = L4;

                                BL.RotaSırala(RotaSeçim, TabloT);

                                for (int i6 = 0; i6 < TabuList.Rows.Count; i6++)
                                {
                                    if (TabuList.Rows[i6]["Rota"].ToString().Trim() == Controllers.Sıralama2)
                                    {
                                        goto git;
                                        break;
                                    }
                                }



                                BL.MesafeHesapla(TabloT, RotaSeçim);

                                Çözüm = Controllers.Toplam1;
                                TabloDöngü.Rows.Add(zet1, Controllers.Sıralama2, Controllers.Toplam1);
                                TümVeriler.Rows.Add(Id, Controllers.Sıralama2, Çözüm);

                                if (Çözüm < EnİyiÇözüm)
                                {
                                    zet4++;
                                    EnİyiÇözüm = Çözüm;

                                    for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                                    {
                                        EnİyiRotaSeçim[i] = RotaSeçim[i];
                                    }

                                    BL.RotaSırala(EnİyiRotaSeçim, TabloT);
                                    EnİyiRota.Rows.Add(Id, Controllers.Sıralama2, Çözüm);
                                }

                            }
                            #endregion
                        }

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

                        else if (Comboİterasyon.Text == "Üstel")
                        {
                            İterasyonSayısı = Convert.ToInt16(Math.Pow(İterasyonSayısı, (1 / İterasyonA)));
                        }

                        #endregion

                        #region Tabuya Değer Yazdırma

                        #region Tabuda Hiç Değer Yoksa

                        if (TabuDurumu == false)
                        {
                            var query = from C in TabloDöngü.AsEnumerable()
                                        orderby C.Field<double>("Mesafe") ascending
                                        select C;

                            int zet2 = 0;

                            foreach (var q in query)
                            {
                                zet2++;
                                TabuList.Rows.Add(q.Field<int>("Sıra"), q.Field<string>("Rota"), q.Field<double>("Mesafe"));

                                if (zet2 == TabuDBBoyutu)
                                {
                                    break;
                                }

                            }


                            TabuDurumu = true;
                        }

                        #endregion

                        #region Tabuda Değer Varsa

                        else
                        {
                            var query = from C in TabloDöngü.AsEnumerable()
                                        orderby C.Field<double>("Mesafe") ascending
                                        select C;

                            zet2 = 0;

                            foreach (var q in query)
                            {
                                zet2++;

                                if (TabuList.Rows.Count < TabuBBoyutu)
                                {
                                    TabuList.Rows.Add(q.Field<int>("Sıra"), q.Field<string>("Rota"), q.Field<double>("Mesafe"));
                                    if (zet2 == TabuDBBoyutu)
                                    {
                                        break;
                                    }
                                }

                                else if (TabuList.Rows.Count >= TabuBBoyutu)
                                {
                                    TabuList.Rows[zet3]["Sıra"] = q.Field<int>("Sıra");
                                    TabuList.Rows[zet3]["Rota"] = q.Field<string>("Rota");
                                    TabuList.Rows[zet3]["Mesafe"] = q.Field<double>("Mesafe");
                                    zet3++;

                                    if (zet3 == TabuBBoyutu)
                                    {
                                        zet3 = 0;
                                    }

                                    if (zet2 == TabuDBBoyutu)
                                    {
                                        break;
                                    }
                                }

                            }
                        }

                        #endregion

                        #endregion

                        #region Yeni Eniyi Çözümü Belirle

                        #region En iyi Çözüm Bulunamamışsa

                        if (zet4 == 0)
                        {
                            Id++;
                            TümVeriler.Rows.Add(Id, "En iyi çözüm bulunamadığı için değişimle yeni çözüm belirlendi.");

                            Id++;

                            for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                            {
                                RotaSeçim[i] = RotaTakip[i];
                            }

                            do
                            {
                                L1 = r.Next(1, TabloT.Rows.Count - 1);
                                L2 = r.Next(1, TabloT.Rows.Count - 1);
                            } while (L1 == L2);

                            L3 = RotaSeçim[L2];
                            L4 = RotaSeçim[L1];

                            RotaSeçim[L1] = L3;
                            RotaSeçim[L2] = L4;

                            BL.RotaSırala(RotaSeçim, TabloT);

                            BL.MesafeHesapla(TabloT, RotaSeçim);

                            Çözüm = Controllers.Toplam1;

                            TümVeriler.Rows.Add(Id, Controllers.Sıralama2, Çözüm);

                            for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                            {
                                RotaTakip[i] = RotaSeçim[i];
                                RotaTakip2[i] = RotaSeçim[i];
                            }
                        }

                        #endregion

                        #region En İyi Çözüm Bulunabildiyse

                        else
                        {
                            Id++;
                            TümVeriler.Rows.Add(Id, "Daha İyi bir çözüm bulundu.");

                            BL.RotaSırala(EnİyiRotaSeçim, TabloT);

                            BL.MesafeHesapla(TabloT, EnİyiRotaSeçim);

                            EnİyiÇözüm = Controllers.Toplam1;

                            TümVeriler.Rows.Add(Id, Controllers.Sıralama2, EnİyiÇözüm);

                            for (int i = 0; i < TabloT.Rows.Count - 1; i++)
                            {
                                RotaSeçim[i] = EnİyiRotaSeçim[i];
                                RotaTakip[i] = EnİyiRotaSeçim[i];
                                RotaTakip2[i] = EnİyiRotaSeçim[i];
                            }
                        }

                        #endregion

                        #endregion

                        #endregion

                    } // Adım Sayısı Döngü Sonu

                    #region Rota Yazdır

                    Id++;
                    TümVeriler.Rows.Add(Id, "Döngü Tamamlandı");

                    BL.RotaSırala(EnİyiRotaSeçim, TabloT);
                    BL.MesafeHesapla(TabloT, EnİyiRotaSeçim);

                    TümVeriler.Rows.Add(Id, Controllers.Sıralama2, Controllers.Toplam1);

                    TextSıralama.Text = Controllers.Sıralama2.ToString();
                    TextMesafe1.Text = Controllers.Toplam1.ToString();
                    TextSıralama2.Text = Controllers.Sıralama2.ToString();
                    TextMesafe2.Text = Controllers.Toplam1.ToString();
                    GridÇözüm.DataSource = TümVeriler;
                    GridTabu.DataSource = TabuList;
                    dataGridView1.DataSource = EnİyiRota;

                    DeneyEnİyi.Rows.Add(DateTime.Now.ToString() + "   <----- Deneme Sayısı---->  " + (int)(i1 + 1), Controllers.Sıralama2.ToString(), Controllers.Toplam1.ToString());
                    #endregion

                    #endregion

                }

                #region Rota Sırala

                var query2 = from C in DeneyEnİyi.AsEnumerable()
                             orderby C.Field<double>("Mesafe") ascending
                             select C;

                foreach (var q in query2)
                {
                    DeneyEnİyi2.Rows.Add(q.Field<string>("DeneyNo"), q.Field<string>("Rota"), q.Field<double>("Mesafe"));
                }

                DataEnİyi.DataSource = DeneyEnİyi2;
                PicRotaÇiz.Enabled = true;

                #endregion

                #region EniyiSıra Yenile

               
                #endregion



                BL.TabuDeney("TabuArama.accdb", ComboDurdurma.Text, NumericAdımSayısı.Value.ToString(), Comboİterasyon.Text, NumericİterasyonB.Value.ToString(), NumericİterasyonSabit.Value.ToString(), ComboİterasyonA.Text, NumericTBelleği.Value.ToString(), NumericDTBelleği.Value.ToString(), Convert.ToDouble(TextMesafe2.Text));

            }
            #endregion

            #region Tabu Arama Başlamadı

            else
            {
                MessageBox.Show("Tabu Döngü Belleği Boyutu Tabu Belleği Boyutundan Büyük Olamaz");
            }

            #endregion



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
            BL.LokasyonYerleştir(ÇizimYap, PicHarita, TabloT, Dolgu1, Dolgu2, Yazı1);

            Pen Kalem = new System.Drawing.Pen(Color.Black, 2);
            ÇizimYap = PicHarita.CreateGraphics();
            for (int i6 = 0; i6 < RotaSeçim.Length - 1; i6++)
            {
                double v3 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6]]["X"]);
                double v4 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6]]["Y"]);
                double v5 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6 + 1]]["X"]);
                double v6 = Convert.ToDouble(TabloT.Rows[EnİyiRotaSeçim[i6 + 1]]["Y"]);
                int v1 = Convert.ToInt32(v3);
                int v2 = Convert.ToInt32(v4);
                int v7 = Convert.ToInt32(v5);
                int v8 = Convert.ToInt32(v6);


                ÇizimYap.DrawLine(Kalem, v1, v2, v7, v8);
            }

          
        }

        private void PicVeriAktar_Click(object sender, EventArgs e)
        {
            string[] yol;
            string[] yol3 = new string[20];
            string Sıralama3;

            if (RadioYeni.Checked==true)
            {
                yol = DeneyEnİyi2.Rows[0]["Rota"].ToString().Split('-');
                Array.Resize(ref yol3, yol.Length);

                yol3[0] = "0";
                yol3[yol.Length - 1] = "0";

                for (int i = 1; i < yol.Length - 1; i++)
                {
                    yol3[i] = yol[i].Trim();
                }

                Sıralama3 = "0";

                for (int i = 1; i < yol3.Length; i++)
                {
                    Sıralama3 += "-" + yol3[i];
                }

                BL.SezgiselVeriEkle(DosyaYolu, Tspİsmi, Sıralama3, DeneyEnİyi2.Rows[0]["Mesafe"].ToString());
                MessageBox.Show("Kayıt Başarılı");
            }

            else
            {
                if (ComboRotaSeçimi.Text == "En İyi Rota")
                {
                    if (Convert.ToDouble(Controllers.Tablo2.Rows[0]["Mesafe"].ToString()) > Convert.ToDouble(DeneyEnİyi2.Rows[0]["Mesafe"].ToString()))
                    {
                        yol = DeneyEnİyi2.Rows[0]["Rota"].ToString().Split('-');
                        Array.Resize(ref yol3, yol.Length);

                        yol3[0] = "0";
                        yol3[yol.Length - 1] = "0";

                        for (int i = 1; i < yol.Length - 1; i++)
                        {
                            yol3[i] = yol[i].Trim();
                        }

                        Sıralama3 = "0";

                        for (int i = 1; i < yol3.Length; i++)
                        {
                            Sıralama3 += "-" + yol3[i];
                        }

                        BL.SezgiselVeriGüncelle(DosyaYolu, Tspİsmi, Sıralama3, DeneyEnİyi2.Rows[0]["Mesafe"].ToString());
                        MessageBox.Show("Güncelleme Başarılı");
                    }
                }
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            DataEnİyi.DataSource = null;
            DeneyEnİyi2.Rows.Clear();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}