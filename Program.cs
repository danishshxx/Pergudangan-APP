using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Produk
{
    public int Id { get; set; }
    public string Nama { get; set; }
    public int Jumlah { get; set; }
    public decimal Harga { get; set; }
    public DateTime Timestamp { get; set; } // Waktu terakhir update
}

public class Gudang
{
    private string filePath = "produk.json";

    // Memuat data dari file JSON
    public List<Produk> LoadData()
    {
        if (!File.Exists(filePath))
        {
            return new List<Produk>();
        }

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Produk>>(json) ?? new List<Produk>();
    }

    // Menyimpan data ke file JSON
    public void SaveData(List<Produk> produkList)
    {
        string json = JsonConvert.SerializeObject(produkList, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // Tambah Produk
    public void TambahProduk(Produk produk)
    {
        var produkList = LoadData();
        produk.Timestamp = DateTime.Now; // Set timestamp saat barang ditambahkan
        produkList.Add(produk);
        SaveData(produkList);
        Console.WriteLine("Produk berhasil ditambahkan!");
    }

    // Tampilkan Semua Produk
    public void TampilkanSemuaProduk()
    {
        Console.WriteLine(
                    "=========================================================================================================="
        );
        var produkList = LoadData();
        if (produkList.Count == 0)
        {
            Console.WriteLine("Tidak ada produk yang tersedia.");
            return;
        }

        foreach (var produk in produkList)
        {
            Console.WriteLine($"ID: {produk.Id}  |Nama: {produk.Nama}  |Jumlah: {produk.Jumlah}  |Harga: {produk.Harga}  |Timestamp: {produk.Timestamp}|");
        }

        Console.WriteLine(
                    "=========================================================================================================="
        );
    }

    // Update Produk
    public void UpdateProduk(int id, string nama, int jumlah, decimal harga)
    {
        var produkList = LoadData();
        var produk = produkList.Find(p => p.Id == id);

        if (produk != null)
        {
            produk.Nama = nama;
            produk.Jumlah = jumlah;
            produk.Harga = harga;
            produk.Timestamp = DateTime.Now; // Perbarui timestamp
            SaveData(produkList);
            Console.WriteLine("Produk berhasil diupdate!");
        }
        else
        {
            Console.WriteLine("Produk tidak ditemukan.");
        }
    }

    // Hapus Produk
public void HapusProduk(int id)
{
    try
    {
        // Memuat data dari JSON
        var produkList = LoadData();

        // Mencari produk berdasarkan ID
        var produk = produkList.Find(p => p.Id == id);

        // Jika produk ditemukan, hapus
        if (produk != null)
        {
            produkList.Remove(produk);
            SaveData(produkList); // Menyimpan kembali data setelah dihapus
            Console.WriteLine("Produk berhasil dihapus!");
        }
        else
        {
            Console.WriteLine("Produk tidak ditemukan.");
        }
    }
    catch (Exception ex) // Menangkap semua jenis exception
    {
        Console.WriteLine($"Terjadi kesalahan saat menghapus produk: {ex.Message}");
    }
}


    // Cari Produk Berdasarkan Nama
    public void CariProduk(string keyword)
    {
        var produkList = LoadData();
        var result = produkList.FindAll(p => p.Nama.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        if (result.Count > 0)
        {
            foreach (var produk in result)
            {
                Console.WriteLine($"ID: {produk.Id}, Nama: {produk.Nama}, Jumlah: {produk.Jumlah}, Harga: {produk.Harga}, Timestamp: {produk.Timestamp}");
            }
        }
        else
        {
            Console.WriteLine("Tidak ada produk yang ditemukan.");
        }
    }

    // Filter Produk Berdasarkan Harga
    public void FilterProduk(decimal minPrice)
    {
        var produkList = LoadData();
        var result = produkList.FindAll(p => p.Harga >= minPrice);

        if (result.Count > 0)
        {
            foreach (var produk in result)
            {
                Console.WriteLine($"ID: {produk.Id}, Nama: {produk.Nama}, Jumlah: {produk.Jumlah}, Harga: {produk.Harga}, Timestamp: {produk.Timestamp}");
            }
        }
        else
        {
            Console.WriteLine("Tidak ada produk yang memenuhi kriteria harga.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Gudang gudang = new Gudang();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Aplikasi Pergudangan ===");
            Console.WriteLine("1. Tambah Produk");
            Console.WriteLine("2. Tampilkan Semua Produk");
            Console.WriteLine("3. Update Produk");
            Console.WriteLine("4. Hapus Produk");
            Console.WriteLine("5. Cari Produk");
            Console.WriteLine("6. Filter Produk Berdasarkan Harga");
            Console.WriteLine("0. Keluar");
            Console.Write("Pilih menu: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    TambahProduk(gudang);
                    break;
                case "2":
                    gudang.TampilkanSemuaProduk();
                    Console.WriteLine("Tekan ENTER untuk melanjutkan...");
                    Console.ReadLine();
                    break;
                case "3":
                    UpdateProduk(gudang);
                    break;
                case "4":
                    HapusProduk(gudang);
                    break;
                case "5":
                    CariProduk(gudang);
                    break;
                case "6":
                    FilterProduk(gudang);
                    break;
                case "0":
                    Console.WriteLine("Keluar dari program...");
                    return;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }
        }
    }

    static void TambahProduk(Gudang gudang)
    {
        Console.Clear();
        Console.WriteLine("=== Tambah Produk ===");
        Console.Write("Nama Produk: ");
        string nama = Console.ReadLine();
        Console.Write("Jumlah: ");
        int jumlah = int.Parse(Console.ReadLine());
        Console.Write("Harga: ");
        decimal harga = decimal.Parse(Console.ReadLine());

        int idBaru = gudang.LoadData().Count + 1;
        gudang.TambahProduk(new Produk { Id = idBaru, Nama = nama, Jumlah = jumlah, Harga = harga });
        Console.WriteLine("Tekan ENTER untuk melanjutkan...");
        Console.ReadLine();
    }

    static void UpdateProduk(Gudang gudang)
    {
        Console.Clear();
        Console.WriteLine("=== Update Produk ===");
        Console.Write("ID Produk yang akan diupdate: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("Nama Baru: ");
        string nama = Console.ReadLine();
        Console.Write("Jumlah Baru: ");
        int jumlah = int.Parse(Console.ReadLine());
        Console.Write("Harga Baru: ");
        decimal harga = decimal.Parse(Console.ReadLine());

        gudang.UpdateProduk(id, nama, jumlah, harga);
        Console.WriteLine("Tekan ENTER untuk melanjutkan...");
        Console.ReadLine();
    }

    static void HapusProduk(Gudang gudang)
    {
        Console.Clear();
        Console.WriteLine("=== Hapus Produk ===");
        Console.Write("ID Produk yang akan dihapus: ");
        int id = int.Parse(Console.ReadLine());

        gudang.HapusProduk(id);
        Console.WriteLine("Tekan ENTER untuk melanjutkan...");
        Console.ReadLine();
    }

    static void CariProduk(Gudang gudang)
    {
        Console.Clear();
        Console.WriteLine("=== Cari Produk ===");
        Console.Write("Masukkan kata kunci: ");
        string keyword = Console.ReadLine();

        gudang.CariProduk(keyword);
        Console.WriteLine("Tekan ENTER untuk melanjutkan...");
        Console.ReadLine();
    }

    static void FilterProduk(Gudang gudang)
    {
        Console.Clear();
        Console.WriteLine("=== Filter Produk Berdasarkan Harga ===");
        Console.Write("Masukkan harga minimal: ");
        decimal harga = decimal.Parse(Console.ReadLine());

        gudang.FilterProduk(harga);
        Console.WriteLine("Tekan ENTER untuk melanjutkan...");
        Console.ReadLine();
    }
}
