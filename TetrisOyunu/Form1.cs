using System;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisOyunu
{
    public partial class Form1 : Form
    {
        Sekil mevcutSekil; // Sekil sınfından bir nesneyi tanımla mevcutSekil olarak adlandır 
        Sekil sonrakiSekil; // Sekil sınfından bir nesneyi tanımla sonrakiSekil olarak adlandır  
        Timer oyunTimeri = new Timer(); // Şekiller hareket ettiği için Timer sınıfından bir nesneyi tanımla 
        int puan = 0; // Puan hesaplamak için bir değişken

        public Form1()
        {
            InitializeComponent();

            tuvalYukle(); // Oyun ekranı başlatmak için bir metod

            this.KeyPreview = true; // Klavye yuşları kullanmak için
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Enabled = true;

            mevcutSekil = ortayaHizalanmisRastgeleSekilOlustur(); // Yeni şeklil getir
            sonrakiSekil = sonrakiSekilGetir(); // Gelecek şekil getir

            oyunTimeri.Tick += Timer_Tick;
            oyunTimeri.Interval = 500; // Timer hızı
            // Timerleri başlat
            oyunTimeri.Enabled = true; 
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            // Timerleri durdur
            oyunTimeri.Enabled = false;
            timer1.Enabled = false;
            oyunYenidenBaslat(); //Oyunu yeniden başlatmak için bir metod kullan            
        }

        private void oyunYenidenBaslat()
        {
            puan = 0; // Puanı sıfırla
            lblPuan.Text = "";
            label2.Text = "";

            tuvalYukle(); //Oyun ekranını yeniden yükle
            mevcutSekil = ortayaHizalanmisRastgeleSekilOlustur(); // Yeni şekili getir 
            sonrakiSekil = sonrakiSekilGetir(); // Gelecek şekili getir

            oyunTimeri.Enabled = true; // Oyun timerini başlat
            timer1.Enabled = true; // timeri başlat 
        }

        // Oyun ekranı yüklemek için gereken değişkenler
        Bitmap tuvalBitmap;
        Graphics tuvalGrafikleri;
        int tuvalGenislik = 15;
        int tuvalYukseklik = 20;
        int[,] tuvalNoktaDizisi;
        int noktaBoyutu = 20;
        private void tuvalYukle()
        {
            // pictureBox1'in boyutlarıyla Bitmap oluştur
            // pictureBox1.Width=300, pictureBox1.Height=400 olarak önceden Formda belirledim
            tuvalBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height); 

            tuvalGrafikleri = Graphics.FromImage(tuvalBitmap);
            // Bitmap'i çizip boyala
            tuvalGrafikleri.FillRectangle(Brushes.Black, 0, 0, tuvalBitmap.Width, tuvalBitmap.Height);

            // Bitmap'i pictureBox1'a yükle
            pictureBox1.Image = tuvalBitmap;

            // Tuval nokta dizisini belirle. varsayılan olarak tüm öğeleri sıfırdır
            tuvalNoktaDizisi = new int[tuvalGenislik, tuvalYukseklik];
        }

        int mevcutX; // Mevcut şekil X'i 
        int mevcutY; // Mevcut şekil Y'yi
        private Sekil ortayaHizalanmisRastgeleSekilOlustur()
        {
            Sekil sekil = SekillerIsleyici.rastgeleSekil(); // SekillerIsleyici sınıfınden rastgele şekil getir

            // X ve y değerlerini sanki oyun ekranının ortasındaymış gibi hesaplayın
            mevcutX = 7;
            mevcutY = -sekil.Yukseklik;

            return sekil; 
        }

        Bitmap sonrakiSekilBitmap; // Gelecek şekil için
        Graphics sonrakiSekilGrafikleri;
        private Sekil sonrakiSekilGetir()
        {
            Sekil sekil = ortayaHizalanmisRastgeleSekilOlustur(); //rastgele şekil getir 

            // Yan pictureBox'deki sonraki şekli gösteren kodlar
            sonrakiSekilBitmap = new Bitmap(6 * noktaBoyutu, 6 * noktaBoyutu);
            sonrakiSekilGrafikleri = Graphics.FromImage(sonrakiSekilBitmap);

            // Arka rengi açık gri
            sonrakiSekilGrafikleri.FillRectangle(Brushes.LightGray, 0, 0, sonrakiSekilBitmap.Width, sonrakiSekilBitmap.Height);

            // Yan pictureBox'deki şekil için ideal konumu bul
            int startX = (6 - sekil.Genislik) / 2;
            int startY = (6 - sekil.Yukseklik) / 2;

            for (int i = 0; i < sekil.Yukseklik; i++)
            {
                int j = 0;
                while (j < sekil.Genislik)
                {
                    if (sekil.Noktalar[i, j] == 1)
                    {
                        // Şekil rengi mavi olsun
                        sonrakiSekilGrafikleri.FillRectangle(Brushes.DarkViolet,
                       (startX + j) * noktaBoyutu, (startY + i) * noktaBoyutu, noktaBoyutu, noktaBoyutu);
                    }
                    else // sekil.Noktalar[i, j] == 0 durumu
                    { 
                        // Arka rengi gibi olsun 
                        sonrakiSekilGrafikleri.FillRectangle(Brushes.LightGray,
                        (startX + j) * noktaBoyutu, (startY + i) * noktaBoyutu, noktaBoyutu, noktaBoyutu);
                    }
                    j++;
                }
            }

            // Gelecek şekil pictureBox2'de göster 
            pictureBox2.Size = sonrakiSekilBitmap.Size; 
            pictureBox2.Image = sonrakiSekilBitmap;

            return sekil;
        }

        Bitmap calisanBitmap;
        Graphics calismaGrafikleri;
        private void Timer_Tick(object sender, EventArgs e)
        {
            bool tasimaBasarili = mumkunseSekliTasi(asagiHaraket: 1);

            // şekil alta ulaştıysa veya başka şekillere dokunduysa
            if (!tasimaBasarili)
            {
                // çalışmaBitmap'i tuvalBitmap'e kopyala
                tuvalBitmap = new Bitmap(calisanBitmap);

                tuvalNoktaDizisiniGuncelle();

                // Sonraki şekil getir
                mevcutSekil = sonrakiSekil;
                sonrakiSekil = sonrakiSekilGetir();

                doldurulmuşSatirlariTemizle(); // Tetris durumu
            }
        }

        private void tuvalNoktaDizisiniGuncelle()
        {
            int i = 0;
            while (i < mevcutSekil.Genislik)
            {
                for (int j = 0; j < mevcutSekil.Yukseklik; j++)
                {
                    if (mevcutSekil.Noktalar[j, i] == 1)
                    {
                        if (oyunuKaybettiMi()) // Oyunu kaybedip kaybetmediğini kontrol et
                            return;

                        tuvalNoktaDizisi[mevcutX + i, mevcutY + j] = 1; 
                    }
                }
                i++;
            }
        }

        private bool oyunuKaybettiMi()
        {
            if (mevcutY < 0) // Şekil tepeye ulaştığında
            {
                oyunTimeri.Enabled = false; // Oyun timerini durdur
                timer1.Enabled = false; // timeri durdur 
                MessageBox.Show("Oyun Kaybettiniz.\nSon Puanınız = " + lblPuan.Text); // Oyun kaybettiğini bildir
                Application.Restart(); // Formu yeniden başlat
                return true;
            }
            return false;
        }

        public void doldurulmuşSatirlariTemizle()
        {
            label2.Text = "";
            // Her satırı kontrol et
            for (int i = 0; i < tuvalYukseklik; i++)
            {
                int j = tuvalGenislik - 1;
                while (j >= 0)
                {
                    if (tuvalNoktaDizisi[j, i] == 0)
                        break;
                    j--;
                }

                if (j == -1)
                {
                    // Puanı ve labelleri güncelle
                    puan += 100;
                    lblPuan.Text = puan.ToString();
                    label2.Text = "Tebrikler! Tetris Yapabildiniz.";

                    // Tuval nokta dizisini kontrole göre güncelle
                    j = 0;
                    while (j < tuvalGenislik)
                    {
                        int k = i;
                        while (k > 0)
                        {
                            tuvalNoktaDizisi[j, k] = tuvalNoktaDizisi[j, k - 1];
                            k--;
                        }
                        tuvalNoktaDizisi[j, 0] = 0;
                        j++;
                    }
                }
            }

            // Güncellenen dizi değerlerine göre pictureBox1 çiz
            for (int i = 0; i < tuvalGenislik; i++)
            {
                for (int j = 0; j < tuvalYukseklik; j++)
                {
                    tuvalGrafikleri = Graphics.FromImage(tuvalBitmap);

                    if (tuvalNoktaDizisi[i, j] == 1)
                    {
                        Rectangle s1 = new Rectangle(i * noktaBoyutu, j * noktaBoyutu, noktaBoyutu, noktaBoyutu);
                        tuvalGrafikleri.FillRectangle(Brushes.DarkViolet, s1);
                    }
                    else
                    {
                        Rectangle s2 = new Rectangle(i * noktaBoyutu, j * noktaBoyutu, noktaBoyutu, noktaBoyutu);
                        tuvalGrafikleri.FillRectangle(Brushes.Black, s2);
                    }
                }
            }

            // pictureBox1'i güncelle
            pictureBox1.Image = tuvalBitmap;
        }

        // Şekil, alta ulaşırsa veya diğer şekillere dokunursa 
        private bool mumkunseSekliTasi(int asagiHaraket = 0, int tarafiHaraket = 0)
        {
            int yeniX = mevcutX + tarafiHaraket;
            int yeniY = mevcutY + asagiHaraket;

            // Alta veya yan çubuğa ulaşıp ulaşmadığını kontrol et
            if (yeniX < 0 || yeniX + mevcutSekil.Genislik > tuvalGenislik
                || yeniY + mevcutSekil.Yukseklik > tuvalYukseklik)
                return false;

            // Başka şekillere dokunup dokunmadığını kontrol et 
            for (int i = 0; i < mevcutSekil.Genislik; i++)
            {
                for (int j = 0; j < mevcutSekil.Yukseklik; j++)
                {
                    if (yeniY + j > 0 && tuvalNoktaDizisi[yeniX + i, yeniY + j] == 1 && mevcutSekil.Noktalar[j, i] == 1)
                        return false;
                }
            }

            // Şeklin X'i ve Y'yi güncelle
            mevcutX = yeniX;
            mevcutY = yeniY;

            sekilCiz(); //Şekli ekranda çiz

            return true;
        }

        private void sekilCiz() //Şekil ekranda çizmek için bir metod
        {
            // tuvalBitmap'i calısmaBitmap'e kopyala
            calisanBitmap = new Bitmap(tuvalBitmap);
            calismaGrafikleri = Graphics.FromImage(calisanBitmap);

            for (int i = 0; i < mevcutSekil.Genislik; i++)
            {
                for (int j = 0; j < mevcutSekil.Yukseklik; j++)
                {
                    if (mevcutSekil.Noktalar[j, i] == 1)
                    {
                        // Şeklin rengi mavi olsun
                        Rectangle rec = new Rectangle
                            ((mevcutX + i) * noktaBoyutu, (mevcutY + j) * noktaBoyutu, noktaBoyutu, noktaBoyutu);
                        calismaGrafikleri.FillRectangle(Brushes.DarkViolet, rec);
                    }
                }
            }
            // pictureBox1'i güncelle
            pictureBox1.Image = calisanBitmap;
        }

        // Bu metodu taşların hareketini kontrol etmek için kullanılacak
        // Formda 2 button kullanıldığı için Form_keyDown kullanılamadı   
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Oyun başlamadığında 
            if (!oyunTimeri.Enabled)
                return true;

            int dikeyHareket = 0;
            int yatayHareket = 0;

            // Basılan tuşa göre dikey ve yatay hareket değerlerini hesapla
            // Şekli sola taşı
            if (keyData == Keys.Left)
                dikeyHareket--;

            // Şekli sağa taşı
            else if (keyData == Keys.Right)
                dikeyHareket++;

            // Şekli daha hızlı aşağı taşı
            else if (keyData == Keys.Down)
                yatayHareket++;

            // Şekli saat yönünde döndür
            else if (keyData == Keys.Up)
                mevcutSekil.sekilYonuDegistir();

            // Yönler tuşlarından hariç bir tuşa basıldığında
            else
                return true;


            bool tasimaBasarili = mumkunseSekliTasi(yatayHareket, dikeyHareket);

            // Oyuncu şekli döndürmeye çalışıyorsa, 
            // ancak bu hareket mümkün değilse şekli geri dön
            if (!tasimaBasarili && keyData == Keys.Up)
                mevcutSekil.geriDon();

            return base.ProcessCmdKey(ref msg, keyData); // Metoda yeniden çağrılır
        }

        private void timer1_Tick(object sender, EventArgs e) // Süreye göre puanı artırmak için 
        {
            puan++; // puan birer artır
            lblPuan.Text = puan.ToString(); // puan lblPuan'a yazdır
        }
    }
}