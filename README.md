# FUTSniper

FUTSniper, **.NET 9 Windows Forms** ve **Selenium WebDriver** kullanılarak geliştirilmiş bir masaüstü otomasyon uygulamasıdır.  
Uygulama, Microsoft Edge üzerinden EA FC Ultimate Team Web App üzerinde belirli arama/satın alma döngülerini otomatikleştirmek için tasarlanmıştır.

## Özellikler

- Edge tarayıcısına remote debugging ile bağlanma
- Fiyat alanlarını otomatik güncelleme
- Arama başlatma ve sonuçları kontrol etme
- Uygun sonuç bulunduğunda satın alma onayı
- Döngü, bekleme ve hız limiti ayarları
- Log ekranı ve temel sayaçlar (alış/döngü/hata)

## Teknolojiler

- .NET 9 (`net9.0-windows`)
- Windows Forms
- DevExpress WinForms
- Selenium WebDriver
- Microsoft Edge WebDriver

## Gereksinimler

- Windows işletim sistemi
- .NET 9 SDK
- Microsoft Edge yüklü olmalı
- EdgeDriver (Selenium ile uyumlu sürüm)

## Kurulum

```bash
git clone https://github.com/mavifd/FUTSniper.git
cd FUTSniper
dotnet restore
```

## Çalıştırma

```bash
dotnet run --project /home/runner/work/FUTSniper/FUTSniper/FifaSnipe.csproj
```

## Kullanım Akışı (Özet)

1. Uygulamayı açın.
2. **Prepare Edge** ile Edge’i remote debugging modunda başlatın.
3. Web App oturumunu açın ve ilgili sayfaya gelin.
4. Fiyat/döngü ayarlarını girin.
5. **Başlat** ile işlemi başlatın, gerekirse **Durdur** ile sonlandırın.

## Notlar

- Proje bir Windows hedefli uygulamadır; Linux/macOS ortamında doğrudan derleme/çalıştırma desteklenmez.
- Otomasyon kullanımı ilgili platform kurallarına uygun şekilde yapılmalıdır.
