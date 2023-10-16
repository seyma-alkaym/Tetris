using System;

namespace TetrisOyunu
{
    class SekillerIsleyici
    {
        private static Sekil[] sekillerDizisi; // Tüm şekiller ihtiva edecek bir dizi

        // static yapıcısı: El ile başlatmaya gerek yok
        static SekillerIsleyici()
        {
            // Şekiller oluşturup diziye ekle 
            sekillerDizisi = new Sekil[] // Sekil sınıfından bir dizi oluştur
            {
                // Her şeklin genişliğini, yüksekliğini ve matrisini tanımla
                new Sekil {Genislik = 3, Yukseklik = 2, Noktalar = new int[,] {{ 0, 1, 0 }, { 1, 1, 1 }}},

                new Sekil {Genislik = 3, Yukseklik = 2, Noktalar = new int[,] {{ 0, 0, 1 }, { 1, 1, 1 }}},

                new Sekil {Genislik = 3, Yukseklik = 2, Noktalar = new int[,] {{ 1, 0, 0 }, { 1, 1, 1 }}},

                new Sekil {Genislik = 2, Yukseklik = 2, Noktalar = new int[,] {{ 1, 1 }, { 1, 1 }}},

                new Sekil {Genislik = 1, Yukseklik = 4, Noktalar = new int[,] {{ 1 }, { 1 }, { 1 }, { 1 }}},
                                    
                new Sekil {Genislik = 3, Yukseklik = 2, Noktalar = new int[,] {{ 1, 1, 0 }, { 0, 1, 1 }}},

                new Sekil {Genislik = 3, Yukseklik = 2, Noktalar = new int[,] {{ 0, 1, 1 }, { 1, 1, 0 }}}
            };
        }

        // Diziden rastgele bir şekilde bir şekil döndür
        public static Sekil rastgeleSekil()
        {
            Random rassal = new Random();
            Sekil sekil = sekillerDizisi[rassal.Next(sekillerDizisi.Length)];
            return sekil;
        }
    }
}
