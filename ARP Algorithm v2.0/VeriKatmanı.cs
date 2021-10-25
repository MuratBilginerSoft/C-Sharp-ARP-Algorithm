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
    public class VeriKatmanı
    {

        #region Tanımalamalar

        public static OleDbConnection Bağlantı; // Bağlantı Nesnesi


        #endregion

        #region Veri Tabanı İşlemleri

        public void databasebağlan(string DosyaYolu)
        {
            Bağlantı = new OleDbConnection("Provider=Microsoft.Ace.Oledb.12.0;Data Source='" + DosyaYolu + "'"); // Veri Tabanı Bağlatısı

            if (Bağlantı.State == ConnectionState.Closed)  // Bağlantı Açıkmı Kontrolü 
            {
                Bağlantı.Open(); // Bağlantı Açtık
            }

            else
            {
                Bağlantı.Close(); // Açık Bağlantı varsa kapattık.
                Bağlantı.Open();  // Bağlatıyı Açtık
            }
        }

        public void GenelVeriÇek(string sorgux, OleDbCommand komutx, OleDbDataAdapter kayıtx, DataTable tablox, string DosyaYolu)
        {
            databasebağlan(DosyaYolu); // Databasebağlan metodu çağrıldı.
            tablox.Clear(); // Tablo Verilerini Temizledik
            tablox.Dispose();
            komutx = new OleDbCommand(sorgux, Bağlantı);  // Sorgumuzun komutunu çalıştırdık.
            kayıtx = new OleDbDataAdapter(komutx); // Komutla çağrılan değerler kayda alındı.
            kayıtx.Fill(tablox); // Kayıda gelen değerler tabloya aktarıldı.
            kayıtx.Dispose(); // Kayıt Temizlendi.
            Bağlantı.Dispose(); // Bağlantı Temizlendi.
            Bağlantı.Close(); // Bağlantı Kapatıldı.
        }

        public void GenelVeriGüncelle(string sorgux,OleDbCommand Komutx,string DosyaYolu,string EnİyiYol,string Mesafe)
        {
            databasebağlan(DosyaYolu);
            Komutx = new OleDbCommand(sorgux,Bağlantı);
            Komutx.Parameters.AddWithValue("@1",EnİyiYol);
            Komutx.Parameters.AddWithValue("@2", Mesafe);
            Komutx.ExecuteNonQuery();
            Bağlantı.Dispose();
            Bağlantı.Close();
        
        }

        public void GenelVeriEkle(string sorgux, OleDbCommand Komutx, string DosyaYolu, string TSPİsim, string EnİyiYol, string Mesafe)
        {
            databasebağlan(DosyaYolu);
            Komutx = new OleDbCommand(sorgux, Bağlantı);
            Komutx.Parameters.AddWithValue("@1", TSPİsim);
            Komutx.Parameters.AddWithValue("@2", EnİyiYol);
            Komutx.Parameters.AddWithValue("@3", Mesafe);
            Komutx.ExecuteNonQuery();
            Bağlantı.Dispose();
            Bağlantı.Close();
        
        }


        public void GenelVeriEkle2(string sorgux, OleDbCommand Komutx, string DosyaYolu, string BS, string SA, string C1, string A1, string İS, string B1, string C2, string A2, string BKÇKO, string SKÇKO, string D1, double Mesafe)
        {
            databasebağlan(DosyaYolu);
            Komutx = new OleDbCommand(sorgux, Bağlantı);
            Komutx.Parameters.AddWithValue("@1", BS);
            Komutx.Parameters.AddWithValue("@2", SA);
            Komutx.Parameters.AddWithValue("@3", C1);
            Komutx.Parameters.AddWithValue("@4", A1);
            Komutx.Parameters.AddWithValue("@5", İS);
            Komutx.Parameters.AddWithValue("@6", B1);
            Komutx.Parameters.AddWithValue("@7", C2);
            Komutx.Parameters.AddWithValue("@8", A2);
            Komutx.Parameters.AddWithValue("@9", BKÇKO);
            Komutx.Parameters.AddWithValue("@10", SKÇKO);
            Komutx.Parameters.AddWithValue("@11", D1);
            Komutx.Parameters.AddWithValue("@12", Mesafe);
            Komutx.ExecuteNonQuery();
            Bağlantı.Dispose();
            Bağlantı.Close();

        }

        public void GenelVeriEkle3(string sorgux, OleDbCommand Komutx, string DosyaYolu, string DK, string A1, string İS, string B1, string C1, string A2, string TBB, string TDBB, double Mesafe)
        {
            databasebağlan(DosyaYolu);
            Komutx = new OleDbCommand(sorgux, Bağlantı);
            Komutx.Parameters.AddWithValue("@1", DK);
            Komutx.Parameters.AddWithValue("@2", A1);
            Komutx.Parameters.AddWithValue("@3", İS);
            Komutx.Parameters.AddWithValue("@4", B1);
            Komutx.Parameters.AddWithValue("@5", C1);
            Komutx.Parameters.AddWithValue("@6", A2);
            Komutx.Parameters.AddWithValue("@7", TBB);
            Komutx.Parameters.AddWithValue("@8", TDBB);
            Komutx.Parameters.AddWithValue("@9", Mesafe);
            Komutx.ExecuteNonQuery();
            Bağlantı.Dispose();
            Bağlantı.Close();

        }

        public void GenelVeriEkle4(string sorgux, OleDbCommand Komutx, string DosyaYolu, string PB, string ÇD, string MD, string ÇY, string DK, double Mesafe)
        {
            databasebağlan(DosyaYolu);
            Komutx = new OleDbCommand(sorgux, Bağlantı);
            Komutx.Parameters.AddWithValue("@1", PB);
            Komutx.Parameters.AddWithValue("@2", ÇD);
            Komutx.Parameters.AddWithValue("@3", MD);
            Komutx.Parameters.AddWithValue("@4", ÇY);
            Komutx.Parameters.AddWithValue("@5", DK);
            Komutx.Parameters.AddWithValue("@6", Mesafe);
            Komutx.ExecuteNonQuery();
            Bağlantı.Dispose();
            Bağlantı.Close();

        }

        #endregion

    }
}
