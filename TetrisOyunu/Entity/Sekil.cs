namespace TetrisOyunu
{
    class Sekil
    {
        public int Genislik; // Her şekil genişliği vardır. Bu değişkenle işimiz kolaylaştıracak
        public int Yukseklik; // Her şekil yüsekliği için
        public int[,] Noktalar; // Şekil dizisi (Her şekil matris olarak tanımlanacak)

        private int[,] yardimciNoktalar; // Şekil geri dönmesi için kullanılacak
        public void sekilYonuDegistir() // Şekil yönünü değiştirmesi için kullanılacak
        {
            // noktalar değerlerini yardımcı noktalara geri döndür
            // basitçe (geriDon metodu) kullanılabilmesi için
            yardimciNoktalar = Noktalar;
            Noktalar = new int[Genislik, Yukseklik];

            for (int i = 0; i < Genislik; i++)
            {
                for (int j = 0; j < Yukseklik; j++)
                {
                    Noktalar[i, j] = yardimciNoktalar[Yukseklik - 1 - j, i]; // şekli saat yönünde döndür
                }
            }

            // Şekilin genişliği yüksekliğine ata ve yüksekliği genişliğine ata (Swap)
            int gecici = Genislik;
            Genislik = Yukseklik;
            Yukseklik = gecici;
        }

        // oyuncu şekli yönünü değiştirdiğinde 
        // diğer şekillere dokunacak ise bu durumda geri dönme yapılması gerekiyor
        public void geriDon()
        {
            Noktalar = yardimciNoktalar;

            // Şekilin genişliği yüksekliğine ata ve yüksekliği genişliğine ata (Swap)
            int gecici = Genislik;
            Genislik = Yukseklik;
            Yukseklik = gecici;
        }
    }
}
