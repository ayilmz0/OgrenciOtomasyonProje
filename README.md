# Öğrenci Otomasyon Sistemi

## Açıklama
Bu proje, bir üniversite öğrencilerinin akademik süreçlerini yönetmek için geliştirilmiş web tabanlı bir otomasyon sistemidir.
Öğrenciler, dersleri, notları ve öğretim üyeleriyle ilişkili bilgileri takip edebilir.

## Teknolojiler
- C# / ASP.NET Core MVC 
- SQL Server
- Entity Framework Core

## Özellikler
- Öğrenci kaydı, güncelleme ve silme
- Ders ve öğretim üyesi yönetimi
- Not giriş ve hesaplama sistemi
- Öğrenci ve ders ilişkilerini yönetme
- Dönem bazlı raporlama
- Transkript alma.

## Kurulum
1. SQL Server’da gerekli veritabanını oluşturun.
2. `appsettings.json` dosyasındaki bağlantı stringini güncelleyin.
3. Visual Studio’da projeyi açın ve çalıştırın.
4. Gerekirse `dotnet ef database update` ile migration’ları uygulayın.

## Kullanım
- Öğrenci bilgilerini görüntüleme ve düzenleme
- Ders ve öğretim üyesi ekleme/güncelleme
- Not girişlerini takip etme
- Dönem raporlarını görüntüleme
