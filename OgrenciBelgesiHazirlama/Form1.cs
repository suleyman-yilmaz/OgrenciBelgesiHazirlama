using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace OgrenciBelgesiHazirlama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
 
            //Encoding asciiEncoding = Encoding.GetEncoding("Windows-1254"); bu kod 2005 sürümünde kullanılır !!
            pictureBox1.ImageLocation = "goruntuler/logo.png";
        }
        public static void Kopyala(string kaynakDizin, string hedefDizin)
        {
            System.IO.DirectoryInfo diKaynak = new System.IO.DirectoryInfo(kaynakDizin);
            System.IO.DirectoryInfo diHedef = new System.IO.DirectoryInfo(hedefDizin);
            TumunuKopyala(diKaynak, diHedef);
        }
        public static void TumunuKopyala(System.IO.DirectoryInfo kaynak, System.IO.DirectoryInfo hedef)
        {
            System.IO.Directory.CreateDirectory(hedef.FullName);
            // Tum dosyalari yeni dizine kopyala.
            foreach (System.IO.FileInfo fi in kaynak.GetFiles())
            {
                fi.CopyTo(System.IO.Path.Combine(hedef.FullName, fi.Name), true);
            }
            // Ozyinelemeli bicimde her bir alt dizini kopyala.
            foreach (System.IO.DirectoryInfo diKaynakAltDizin in kaynak.GetDirectories())
            {
                System.IO.DirectoryInfo sonrakiHedefAltDizin = hedef.CreateSubdirectory(diKaynakAltDizin.Name);
                TumunuKopyala(diKaynakAltDizin, sonrakiHedefAltDizin);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Ogrenci numarasini long'a dönüştürmemizdeki amac, sayisal olmayan bir
                // deger girildiginde "catch" blogundaki uyarinin verilmesini saglamaktir.
                Convert.ToInt64(textBox1.Text);
                string okunanOgrenci;
                string[] ogrenciBilgileri;
                bool ogrenciBulundu = false;
                System.IO.StreamReader sr = new System.IO.StreamReader("ogrenciler.txt"); //, Encoding.GetEncoding("Windows-1254")); bu kod yazım hatası meydana getiriyor ve 2005 sürümünde kullanılıyor.
                okunanOgrenci = sr.ReadLine();
                while (okunanOgrenci != null)
                {
                    ogrenciBilgileri = okunanOgrenci.Split('#');
                    if (textBox1.Text == ogrenciBilgileri[0])
                    { // ogrenci bulundu
                        textBox2.Text = ogrenciBilgileri[1];
                        textBox3.Text = ogrenciBilgileri[2];
                        textBox4.Text = ogrenciBilgileri[3];
                        textBox5.Text = ogrenciBilgileri[4];
                        textBox6.Text = ogrenciBilgileri[5];
                        textBox7.Text = ogrenciBilgileri[6];
                        ogrenciBulundu = true;
                        break;
                    }
                    okunanOgrenci = sr.ReadLine();
                }
                sr.Close();
                if (ogrenciBulundu == false)
                {
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    MessageBox.Show("BU NUMARA HERHANGİ BİR ÖĞRENCİYE AİT DEĞİLDİR.");
                    textBox1.Text = "";
                }
            }
            catch
            {
                MessageBox.Show("ÖĞRENCİ NUMARASI SAYISAL BİR DEĞER OLMALIDIR.");
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0 ||textBox4.Text.Length == 0 || textBox5.Text.Length == 0 || textBox6.Text.Length == 0 ||textBox7.Text.Length == 0 || textBox8.Text.Length == 0 || textBox9.Text.Length == 0 ||textBox10.Text.Length == 0)
            {
                MessageBox.Show("YUKARIDA YER ALAN METİN KUTULARININ TAMAMINI DOLDURUNUZ.");
            }
            else
            {
                if (System.IO.Directory.Exists("gecici"))
                {
                    System.IO.Directory.Delete("gecici", true);
                }
                Kopyala("Ogrenci Belgesi", "gecici");
                string sablon;
                System.IO.StreamReader sr = new System.IO.StreamReader("gecici/ogrencibelgesi.html",Encoding.GetEncoding("Windows-1254"));
                sablon = sr.ReadToEnd();
                sr.Close();
                sablon = sablon.Replace("$^1^$", textBox2.Text);
                sablon = sablon.Replace("$^2^$", textBox3.Text);
                sablon = sablon.Replace("$^3^$", textBox1.Text);
                sablon = sablon.Replace("$^4^$", textBox4.Text);
                sablon = sablon.Replace("$^5^$", textBox5.Text);
                sablon = sablon.Replace("$^6^$", textBox6.Text);
                sablon = sablon.Replace("$^7^$", textBox7.Text);
                sablon = sablon.Replace("$^8^$", dateTimePicker1.Value.Day.ToString());
                sablon = sablon.Replace("$^9^$", dateTimePicker1.Value.Month.ToString());
                sablon = sablon.Replace("$^10^$", dateTimePicker1.Value.Year.ToString());
                sablon = sablon.Replace("$^11^$", textBox8.Text);
                sablon = sablon.Replace("$^12^$", textBox9.Text);
                sablon = sablon.Replace("$^13^$", textBox10.Text);
                System.IO.File.Delete("gecici/ogrencibelgesi.html");
                System.IO.StreamWriter sw = new System.IO.StreamWriter("gecici/ogrencibelgesi.html", false,Encoding.GetEncoding("Windows-1254"));
                sw.Write(sablon);
                sw.Close();
                System.IO.File.Copy("gecici/ogrencibelgesi.html", "gecici/ogrencibelgesi.doc");
                System.Diagnostics.ProcessStartInfo sti = new System.Diagnostics.ProcessStartInfo();
                sti.FileName = "WINWORD.EXE";
                sti.Arguments = "gecici/ogrencibelgesi.doc";
                System.Diagnostics.Process.Start(sti);
            }
        }
    }
}
