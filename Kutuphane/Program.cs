using System;
using System.Collections.Generic;

class Program
{
    
    class Kitap
    {
        public string KitapAdi { get; set; }
        public string YazarAdi { get; set; }
        public int YayinYili { get; set; }
        public int StokAdedi { get; set; }
    }

   
    class Kiralama
    {
        public string KullaniciAdi { get; set; }
        public string KitapAdi { get; set; }
        public DateTime IadeTarihi { get; set; }
    }

    static List<Kitap> kitaplar = new List<Kitap>();
    static List<Kiralama> kiralamaKayitlari = new List<Kiralama>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nKütüphane Sistemi");
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitap Kirala");
            Console.WriteLine("3. Kitap İade");
            Console.WriteLine("4. Kitap Arama");
            Console.WriteLine("5. Raporlama");
            Console.WriteLine("0. Çıkış");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine();

            switch (secim)
            {
                case "1":
                    KitapEkle();
                    break;
                case "2":
                    KitapKirala();
                    break;
                case "3":
                    KitapIade();
                    break;
                case "4":
                    KitapArama();
                    break;
                case "5":
                    Raporlama();
                    break;
                case "0":
                    Console.WriteLine("Programdan çıkılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                    break;
            }
        }
    }

    static void KitapEkle()
    {
        Console.Write("Kitap Adı: ");
        string kitapAdi = Console.ReadLine();

        Console.Write("Yazar Adı: ");
        string yazarAdi = Console.ReadLine();

        Console.Write("Yayın Yılı: ");
        int yayinYili = int.Parse(Console.ReadLine());

        Console.Write("Adet (Stok): ");
        int adet = int.Parse(Console.ReadLine());

        var mevcutKitap = kitaplar.Find(k => k.KitapAdi == kitapAdi && k.YazarAdi == yazarAdi);

        if (mevcutKitap != null)
        {
            mevcutKitap.StokAdedi += adet;
            Console.WriteLine("Kitap zaten kayıtlı. Stok artırıldı.");
        }
        else
        {
            kitaplar.Add(new Kitap
            {
                KitapAdi = kitapAdi,
                YazarAdi = yazarAdi,
                YayinYili = yayinYili,
                StokAdedi = adet
            });
            Console.WriteLine("Yeni kitap eklendi.");
        }
    }

    static void KitapKirala()
    {
        Console.WriteLine("Mevcut Kitaplar:");
        for (int i = 0; i < kitaplar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {kitaplar[i].KitapAdi} - {kitaplar[i].YazarAdi} (Stok: {kitaplar[i].StokAdedi})");
        }

        Console.Write("Kiralamak istediğiniz kitabın numarasını seçin: ");
        int kitapNo = int.Parse(Console.ReadLine()) - 1;

        if (kitapNo < 0 || kitapNo >= kitaplar.Count || kitaplar[kitapNo].StokAdedi == 0)
        {
            Console.WriteLine("Geçersiz seçim veya stokta kitap yok.");
            return;
        }

        Console.Write("Kaç günlüğüne kiralamak istiyorsunuz? ");
        int gunSayisi = int.Parse(Console.ReadLine());

        Console.Write("Bütçeniz (TL): ");
        int butce = int.Parse(Console.ReadLine());

        int toplamUcret = gunSayisi * 5;
        if (butce < toplamUcret)
        {
            Console.WriteLine("Bütçeniz yeterli değil.");
            return;
        }

        Console.Write("Adınız: ");
        string kullaniciAdi = Console.ReadLine();

        kitaplar[kitapNo].StokAdedi--;
        kiralamaKayitlari.Add(new Kiralama
        {
            KullaniciAdi = kullaniciAdi,
            KitapAdi = kitaplar[kitapNo].KitapAdi,
            IadeTarihi = DateTime.Now.AddDays(gunSayisi)
        });

        Console.WriteLine($"Kitap kiralandı. İade tarihi: {DateTime.Now.AddDays(gunSayisi):dd.MM.yyyy}");
    }

    static void KitapIade()
    {
        Console.Write("İade edeceğiniz kitabın adı: ");
        string kitapAdi = Console.ReadLine();

        var kiralananKitap = kiralamaKayitlari.Find(k => k.KitapAdi == kitapAdi);
        if (kiralananKitap == null)
        {
            Console.WriteLine("Bu kitap kiralanmamış.");
            return;
        }

        var kitap = kitaplar.Find(k => k.KitapAdi == kitapAdi);
        kitap.StokAdedi++;
        kiralamaKayitlari.Remove(kiralananKitap);

        Console.WriteLine("Kitap iade edildi.");
    }

    static void KitapArama()
    {
        Console.WriteLine("1. Kitap adına göre arama");
        Console.WriteLine("2. Yazar adına göre arama");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        Console.Write("Arama terimi: ");
        string terim = Console.ReadLine();

        List<Kitap> sonuc = null;
        if (secim == "1")
        {
            sonuc = kitaplar.FindAll(k => k.KitapAdi.Contains(terim, StringComparison.OrdinalIgnoreCase));
        }
        else if (secim == "2")
        {
            sonuc = kitaplar.FindAll(k => k.YazarAdi.Contains(terim, StringComparison.OrdinalIgnoreCase));
        }

        if (sonuc == null || sonuc.Count == 0)
        {
            Console.WriteLine("Eşleşen kitap bulunamadı.");
        }
        else
        {
            foreach (var kitap in sonuc)
            {
                Console.WriteLine($"{kitap.KitapAdi} - {kitap.YazarAdi} ({kitap.YayinYili}) [Stok: {kitap.StokAdedi}]");
            }
        }
    }

    static void Raporlama()
    {
        Console.WriteLine("1. Tüm kitapları listele");
        Console.WriteLine("2. Belirli bir yazara ait kitapları listele");
        Console.WriteLine("3. Belirli bir yayın yılına ait kitapları listele");
        Console.WriteLine("4. Kirada olan kitapları listele");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        if (secim == "1")
        {
            foreach (var kitap in kitaplar)
            {
                Console.WriteLine($"{kitap.KitapAdi} - {kitap.YazarAdi} ({kitap.YayinYili}) [Stok: {kitap.StokAdedi}]");
            }
        }
        else if (secim == "2")
        {
            Console.Write("Yazar adı: ");
            string yazarAdi = Console.ReadLine();

            var yazarKitaplari = kitaplar.FindAll(k => k.YazarAdi.Contains(yazarAdi, StringComparison.OrdinalIgnoreCase));
            foreach (var kitap in yazarKitaplari)
            {
                Console.WriteLine($"{kitap.KitapAdi} - {kitap.YazarAdi} ({kitap.YayinYili}) [Stok: {kitap.StokAdedi}]");
            }
        }
        else if (secim == "3")
        {
            Console.Write("Yayın yılı: ");
            int yayinYili = int.Parse(Console.ReadLine());

            var yilinKitaplari = kitaplar.FindAll(k => k.YayinYili == yayinYili);
            foreach (var kitap in yilinKitaplari)
            {
                Console.WriteLine($"{kitap.KitapAdi} - {kitap.YazarAdi} ({kitap.YayinYili}) [Stok: {kitap.StokAdedi}]");
            }
        }
        else if (secim == "4")
        {
            foreach (var kiralama in kiralamaKayitlari)
            {
                Console.WriteLine($"{kiralama.KitapAdi} - {kiralama.KullaniciAdi} (İade Tarihi: {kiralama.IadeTarihi:dd.MM.yyyy})");
            }
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
        }
    }
}
