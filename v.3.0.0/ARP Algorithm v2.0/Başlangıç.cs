using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace ARP_Algorithm_v2._0
{
    public partial class Başlangıç : Form
    {
        #region Tanımlamlar

        Controllers BL = new Controllers();
        #endregion

        #region Değişkenler

        public static string TabloAdı1;

        string DosyaYolu;

        public static string VeriKaynağı;
        public static string DosyaUzantısı;

        StreamReader Oku;

        string line;

        string[] Lokasyon;

        string T1, T2;

        #endregion

        #region Form İşlemleri

        public Başlangıç()
        {
            InitializeComponent();
        }

        public static DataTable Tablo1 = new DataTable();

        private void Başlangıç_Load(object sender, EventArgs e)
        {
            Tablo1.Columns.Add("Lokasyon", typeof(string));
            Tablo1.Columns.Add("X", typeof(string));
            Tablo1.Columns.Add("Y", typeof(string));
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

        private void PicAktar_Click(object sender, EventArgs e)
        {
            GroupYöntem.Enabled = true;
            VeriKaynağı = ComboVeriKaynağı.Text;
        }

        private void PicTavlama_Click(object sender, EventArgs e)
        {
            if (ComboVeriKaynağı.Text=="Sabit Lokasyon")
            {
                TabloAdı1 = "SabitLokasyon";
                BL.SabitLokasyon(TabloAdı1, "ARP1.accdb");
            }

            TavlamaBenzetimi TBForm = new TavlamaBenzetimi();
            BL.FormAç(this, TBForm);
        }

        #endregion

        #region MenüStrip Olayları

        private void MenüÇıkış_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Form Taşıma

        private void Başlangıç_MouseDown(object sender, MouseEventArgs e)
        {
            BL.MouseDown(this, e);
        }

        private void Başlangıç_MouseMove(object sender, MouseEventArgs e)
        {
            BL.MouseMove(this, e);
        }

        private void Başlangıç_MouseUp(object sender, MouseEventArgs e)
        {
            BL.MouseUp(this);
        }

        #endregion

        private void ComboVeriKaynağı_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboVeriKaynağı.Text=="Sabit Lokasyon")
            {
                PicAktar.BackColor = Color.DeepSkyBlue;
                PicAktar.Enabled = true;
                PanelBilgi.Enabled = false;
                ComboDosyaUzantısı.Enabled = false;
                TextDosyaYolu.Enabled = false;
                PicDosyaBul.Enabled = false;
                
            }

            else if (ComboVeriKaynağı.Text=="Dış Veri")
            {
                
                PanelBilgi.Enabled = true;
                PicAktar.Enabled = true;
                ComboDosyaUzantısı.Enabled = true;
                TextDosyaYolu.Enabled = true;
                PicDosyaBul.Enabled = true;
               
            }

            else if (ComboVeriKaynağı.Text == "Rastgele Lokasyon")
            {
                PicAktar.BackColor = Color.DeepSkyBlue;
                PanelBilgi.Enabled = true;
                PicAktar.Enabled = true;
               
                ComboDosyaUzantısı.Enabled = false;
                TextDosyaYolu.Enabled = false;
                PicDosyaBul.Enabled = false;
                
            }
        }


        private void PicDosyaBul_Click(object sender, EventArgs e)
        {
            DosyaUzantısı = ComboDosyaUzantısı.Text;
            OpenFileDialog Dosya = new OpenFileDialog();
            Tablo1.Clear();
            #region Text Dosyası Veri Al

            if (ComboDosyaUzantısı.Text == "txt")
            {
                Dosya.Filter = "Text Dosyası |*.txt";
                Dosya.Title = "Text Dosyası Seç";
                Dosya.ShowDialog();
                DosyaYolu = Dosya.FileName;
                TextDosyaYolu.Text = DosyaYolu;
                Oku = File.OpenText(DosyaYolu);

                while ((line=Oku.ReadLine())!=null)
                {
                    line = line.TrimEnd();
                    Lokasyon=line.Split(' ');
                    if (Lokasyon.Length!=0)
                    {
                        T1=(Lokasyon[1].ToString());
                        T2=(Lokasyon[2].ToString());
                        Tablo1.Rows.Add(Lokasyon[0],T1,T2 );
                    }
                  
                }

                Oku.Close();

                
            }
            #endregion

            #region Access Dosyasından Veri Al

            else if (ComboDosyaUzantısı.Text == "accdb")
            {
                Dosya.Filter = "Access Veri Tabanı |*.accdb";
                Dosya.Title = "Access Veri Tabanı Dosyası Seç";
                Dosya.ShowDialog();
                DosyaYolu = Dosya.FileName;
                TextDosyaYolu.Text = DosyaYolu;

                TabloAdı1 = "SabitLokasyon";
                BL.SabitLokasyon(TabloAdı1, DosyaYolu);
            }

            #endregion

            PicAktar.BackColor = Color.DeepSkyBlue;
        }

        private void PicTabu_Click(object sender, EventArgs e)
        {
            if (ComboVeriKaynağı.Text == "Sabit Lokasyon")
            {
                TabloAdı1 = "SabitLokasyon";
                BL.SabitLokasyon(TabloAdı1, "ARP1.accdb");
            }

            TabuArama TAForm = new TabuArama();
            BL.FormAç(this, TAForm);
        }

        private void PicGenetik_Click(object sender, EventArgs e)
        {
            if (ComboVeriKaynağı.Text == "Sabit Lokasyon")
            {
                TabloAdı1 = "SabitLokasyon";
                BL.SabitLokasyon(TabloAdı1, "ARP.accdb");
            }
            GenetikAlgoritmalar GAForm = new GenetikAlgoritmalar();
            BL.FormAç(this, GAForm);
        }
    }
}
