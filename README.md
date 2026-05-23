# RobocodeBots_ENAA

## Deskripsi Program
Program ini merupakan bot Robocode Tank Royale bernama **ENAA** yang dibuat untuk memenuhi Tugas Besar 1 Strategi Algoritma.  
Bot menggunakan pendekatan algoritma greedy dalam menentukan target, pergerakan, dan penembakan musuh secara efisien selama pertandingan berlangsung.

---

## Algoritma Greedy yang Digunakan
Bot menerapkan algoritma greedy dengan mengambil keputusan terbaik pada setiap kondisi saat ini tanpa mempertimbangkan seluruh kemungkinan jangka panjang. Implementasi greedy pada bot meliputi:

1. Mengunci musuh pertama yang terdeteksi sebagai target utama.
2. Menentukan besar firepower berdasarkan jarak musuh saat ini.
3. Memprediksi posisi musuh menggunakan data pergerakan terbaru.
4. Menghindari serangan lawan dengan movement dinamis dan perubahan arah otomatis.
5. Memprioritaskan serangan kepada target yang sedang terkunci dibanding mencari target lain.

Pendekatan ini membuat bot dapat bereaksi cepat dan menjaga efisiensi pengambilan keputusan selama battle berlangsung.

---

## Requirement Program
Sebelum menjalankan program, pastikan perangkat telah memiliki:

- Java Development Kit (JDK) 17 atau lebih baru
- robocode-tankroyale-gui.0.30.0
- dotnet-sdk-10.0.300-win-x64
- IDE/Text Editor seperti Visual Studio Code
- CMD

---

## Cara Compile dan Menjalankan Program

### 1. Clone Repository
```bash
git clone <link-repository>
```

### 2. Masuk ke Folder Project melalui CMD
```bash
cd C:\RobocodeBots\ENAA
```
### 3. Masukkan kode rahasia server
```bash
set SERVER_SECRET=LLZcXWW7aiUZcQ6uSoB1W5HIkFq6LDtjYD/aDj4fho
```

### 4. Compile Program
```bash
dotnet build
```

### 5. Jalankan Bot
Pastikan server Robocode Tank Royale sudah berjalan, kemudian jalankan bot melalui command:

```bash
dotnet run
```

---

## Struktur Repository
```plaintext
RobocodeBots_ENAA/
│
├── src/
│   └── source code bot
│
├── doc/
│   └── ENAA.pdf
│
├── README.md
│
```

---

## Author
Kelompok ENAA

- Adelia Eva Ananta

Institut Teknologi Sumatera (ITERA)
Teknik Informatika
2026