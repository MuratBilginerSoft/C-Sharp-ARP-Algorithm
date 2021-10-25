using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ARP_Algorithm_v2._0
{
    public class Controllers
    {
        #region Tanımlamalar

        VeriKatmanı DL = new VeriKatmanı();

        OleDbCommand Komut;
        OleDbDataAdapter Kayıt;
        public static DataTable Tablo1 = new DataTable();
        public static DataTable Tablo2 = new DataTable();

       

        Random r = new Random();

        #endregion

        #region Değişkenler

        bool Durum = false;

       

        Point İlkKonum;

        public static string Sorgu1;
        public static string Sorgu2;
        public static string Sorgu4;

        #endregion  #region Form Aç

        #region Form Aç

        public void FormAç(Form Form1, Form Form2)
        {
            Form1.Hide();
            Form2.ShowDialog();
            Form1.Show();   
        }

        #endregion

        #region Form Taşıma

        public void MouseDown(Form Form1, MouseEventArgs mearg)
        {
            Durum = true;
            Form1.Cursor = Cursors.SizeAll;
            İlkKonum = mearg.Location;
        }

        public void MouseMove(Form Form1, MouseEventArgs mearg)
        {
            if (Durum)
            {
                Form1.Left = mearg.X + Form1.Left - (İlkKonum.X);
                Form1.Top = mearg.Y + Form1.Top - (İlkKonum.Y);
            }
        }

        public void MouseUp(Form Form1)
        {
            Durum = false;
            Form1.Cursor = Cursors.Default;
        }
        #endregion

        #region VeriTabanı İşlemleri

        public void SabitLokasyon(string Tablo1Adı, string DosyaYolu)
        {
            Sorgu1 = "Select * from " + Tablo1Adı + "";
            DL.databasebağlan(DosyaYolu);
            DL.GenelVeriÇek(Sorgu1, Komut, Kayıt, Tablo1, DosyaYolu);
        }

        public void SezgiselEnİyi(string Tablo2Adı, string DosyaYolu2,string Tspİsmi)
        {
            Sorgu2 = "Select * from " + Tablo2Adı + " where TSPİsmi='"+Tspİsmi+"'";
            DL.databasebağlan(DosyaYolu2);
            DL.GenelVeriÇek(Sorgu2, Komut, Kayıt, Tablo2, DosyaYolu2);
        }

        string Sorgu3;

        public void SezgiselVeriGüncelle(string DosyaYolu2, string Tspİsmi,string EnİyiYol,string Mesafe)
        {
            Sorgu3 = "Update TabuEnİyi Set EnİyiYol=@1,Mesafe=@2 where TSPİsmi='"+Tspİsmi+"'";
            DL.databasebağlan(DosyaYolu2);
            DL.GenelVeriGüncelle(Sorgu3, Komut, DosyaYolu2, EnİyiYol, Mesafe);
        }

        public void SezgiselVeriEkle(string DosyaYolu2, string Tspİsmi, string EnİyiYol, string Mesafe)
        {
            Sorgu4 = "Insert into TabuEnİyi(TSPİsmi,EnİyiYol,Mesafe) Values(@1,@2,@3)";
            DL.databasebağlan(DosyaYolu2);
            DL.GenelVeriEkle(Sorgu4, Komut, DosyaYolu2, Tspİsmi, EnİyiYol, Mesafe);
        }

        public void TavlamaDenet(string DosyaYolu, string BS, string SA, string C1, string A1, string İS, string B1, string C2, string A2, string BKÇKO, string SKÇKO, string D1, double Mesafe)
        {
            string Sorgu5 = "Insert into TavlamaDeney(BaşlangıçSıcaklığı,SıcaklıkAzalışı,C1,A1,İterasyonSayısı,B1,C2,A2,BKÇKO,SKÇKO,Duruş,Mesafe) values(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12)";
            DL.databasebağlan(DosyaYolu);
            DL.GenelVeriEkle2(Sorgu5, Komut, DosyaYolu, BS, SA, C1, A1, İS, B1, C2, A2, BKÇKO, SKÇKO, D1, Mesafe);
        
        }


        public void TabuDeney(string DosyaYolu, string DK, string A1, string İS, string B1, string C1, string A2, string TBB, string TDBB, double Mesafe)
        {
            string Sorgu5 = "Insert into TabuDeney(DurdurmaKriteri,A1,İterasyonSayısı,B1,C1,A2,TBB,TDBB,Mesafe) values(@1,@2,@3,@4,@5,@6,@7,@8,@9)";
            DL.databasebağlan(DosyaYolu);
            DL.GenelVeriEkle3(Sorgu5, Komut, DosyaYolu, DK,A1,İS,B1,C1,A2,TBB,TDBB, Mesafe);

        }

        public void GenetikDeney(string DosyaYolu, string PB, string ÇD, string MD, string ÇY, string DK, double Mesafe)
        {
            string Sorgu5 = "Insert into GenetikDeney(PB,ÇD,MD,ÇY,DK,Mesafe) values(@1,@2,@3,@4,@5,@6)";
            DL.databasebağlan(DosyaYolu);
            DL.GenelVeriEkle4(Sorgu5, Komut, DosyaYolu, PB,ÇD,MD,ÇY,DK, Mesafe);

        }



        #endregion

        #region Lokasyon Yerleştir

        public void LokasyonYerleştir(Graphics ÇizimYap, PictureBox PictureBox1, DataTable Tablo1, Brush Dolgu1, Brush Dolgu2, Font Yazı1)
        {
            for (int i = 0; i < Tablo1.Rows.Count; i++)
            {
                ÇizimYap = PictureBox1.CreateGraphics();
                ÇizimYap.FillEllipse(Dolgu1, Convert.ToInt64(Tablo1.Rows[i]["X"].ToString()), Convert.ToInt64(Tablo1.Rows[i]["Y"].ToString()), 20, 20);
                ÇizimYap.DrawString(Tablo1.Rows[i]["Lokasyon"].ToString(), Yazı1, Dolgu2, Convert.ToInt64(Tablo1.Rows[i]["X"].ToString()), Convert.ToInt64(Tablo1.Rows[i]["Y"].ToString()));
            }

           
        }

        #endregion

        #region Rastgele Değer Üret

        public static string Sıralama = "";

        public void RastgeleDeğerÜret(int[] RotaSeçim, int[] EnİyiRotaSeçim, DataTable Tablo1, int X1)
        { 
            Sıralama = "";
            for (int i4 = 0; i4 <TavlamaBenzetimi.TabloT.Rows.Count - 1; i4++)
            {
                RotaSeçim[i4] = 0;
            }

            RotaSeçim[0] = 0;
            EnİyiRotaSeçim[0] = 0;
            Sıralama += TavlamaBenzetimi.TabloT.Rows[0]["Lokasyon"].ToString();

            for (int i3 = 1; i3 < TavlamaBenzetimi.TabloT.Rows.Count - 1; i3++)
            {
                do
                {
                    X1 = r.Next(1, TavlamaBenzetimi.TabloT.Rows.Count - 1);
                } while (Array.IndexOf(RotaSeçim, X1) != -1);

                RotaSeçim[i3] = X1;
                EnİyiRotaSeçim[i3] = X1;
                Sıralama += " - " + TavlamaBenzetimi.TabloT.Rows[X1]["Lokasyon"].ToString();

            }

            RotaSeçim[TavlamaBenzetimi.TabloT.Rows.Count - 1] = TavlamaBenzetimi.TabloT.Rows.Count - 1;
            Sıralama += " - " + TavlamaBenzetimi.TabloT.Rows[TavlamaBenzetimi.TabloT.Rows.Count - 1]["Lokasyon"].ToString();
        }

        #endregion

        #region Rastgele Değer Üret

        

        public void RastgeleDeğerÜrett(int[] RotaSeçim, int[] EnİyiRotaSeçim, DataTable Tablo1, int X1)
        {
            Sıralama = "";
            for (int i4 = 0; i4 < TabuArama.TabloT.Rows.Count - 1; i4++)
            {
                RotaSeçim[i4] = 0;
            }

            RotaSeçim[0] = 0;
            EnİyiRotaSeçim[0] = 0;
            Sıralama += TabuArama.TabloT.Rows[0]["Lokasyon"].ToString();

            for (int i3 = 1; i3 < TabuArama.TabloT.Rows.Count - 1; i3++)
            {
                do
                {
                    X1 = r.Next(1, TabuArama.TabloT.Rows.Count - 1);
                } while (Array.IndexOf(RotaSeçim, X1) != -1);

                RotaSeçim[i3] = X1;
                EnİyiRotaSeçim[i3] = X1;
                Sıralama += " - " + TabuArama.TabloT.Rows[X1]["Lokasyon"].ToString();

            }

            RotaSeçim[TabuArama.TabloT.Rows.Count - 1] = TabuArama.TabloT.Rows.Count - 1;
            Sıralama += " - " + TabuArama.TabloT.Rows[TabuArama.TabloT.Rows.Count - 1]["Lokasyon"].ToString();
        }

        #endregion

        public void RastgeleDeğerÜrettt(int[] RotaSeçim, int[] EnİyiRotaSeçim, DataTable Tablo1, int X1)
        {
            Sıralama = "";
            for (int i4 = 0; i4 < GenetikAlgoritmalar.TabloT.Rows.Count - 1; i4++)
            {
                RotaSeçim[i4] = 0;
            }

            RotaSeçim[0] = 0;
            EnİyiRotaSeçim[0] = 0;
            Sıralama += GenetikAlgoritmalar.TabloT.Rows[0]["Lokasyon"].ToString();

            for (int i3 = 1; i3 < GenetikAlgoritmalar.TabloT.Rows.Count - 1; i3++)
            {
                do
                {
                    X1 = r.Next(1, GenetikAlgoritmalar.TabloT.Rows.Count - 1);
                } while (Array.IndexOf(RotaSeçim, X1) != -1);

                RotaSeçim[i3] = X1;
                EnİyiRotaSeçim[i3] = X1;
                Sıralama += " - " + GenetikAlgoritmalar.TabloT.Rows[X1]["Lokasyon"].ToString();

            }

            RotaSeçim[GenetikAlgoritmalar.TabloT.Rows.Count - 1] = GenetikAlgoritmalar.TabloT.Rows.Count - 1;
            Sıralama += " - " + GenetikAlgoritmalar.TabloT.Rows[GenetikAlgoritmalar.TabloT.Rows.Count - 1]["Lokasyon"].ToString();
        }

        #region Mesafe Hesapla

        public static double Toplam1 = 0;
        public static double Mesafe1 = 0;

        public void MesafeHesapla(DataTable Tablo1, int[] RotaSeçim)
        {
            Toplam1 = 0;

            for (int i4 = 0; i4 < Tablo1.Rows.Count - 1; i4++)
            {

                Mesafe1 = Math.Sqrt(Math.Pow((double.Parse(Tablo1.Rows[RotaSeçim[i4]]["X"].ToString()) - double.Parse(Tablo1.Rows[RotaSeçim[i4 + 1]]["X"].ToString())), 2) + Math.Pow((double.Parse(Tablo1.Rows[RotaSeçim[i4]]["Y"].ToString()) - double.Parse(Tablo1.Rows[RotaSeçim[i4 + 1]]["Y"].ToString())), 2));

                Toplam1 += Mesafe1;
            }


        }

        #endregion

        public static string Sıralama2 = "";

        public void RotaSırala(int[] Dizi1, DataTable Tablot)
        {
            Sıralama2 = "Depo";

            for (int p1 = 1; p1 < Tablot.Rows.Count - 1; p1++)
            {
                Sıralama2 += " - " + Tablot.Rows[Dizi1[p1]]["Lokasyon"].ToString();
            }

            Sıralama2 += " - " + Tablot.Rows[Tablot.Rows.Count - 1]["Lokasyon"].ToString();
        }
    }
}
