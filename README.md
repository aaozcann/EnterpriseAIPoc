# Enterprise .NET AI POC

Bu proje, şirketlerin "Sadece bir API'ye JSON gönderip cevap alma" algısını kırıp; yapay zekayı kurumun **kendi C# kodlarıyla, kendi kurallarıyla ve kendi kalıcı veritabanıyla** (Redis) konuşturmayı hedefleyen bir "Kurumsal AI Asistanı" (Proof of Concept) çalışmasıdır.

## 🌟 Neler Yapıyoruz?

- **Orkestra Şefi (Semantic Kernel & Function Calling):** AI sadece dış dünyadan konuşan bir özetleyici değil. Yazdığımız C# metotlarını (örn. Ahmet'in yıllık iznini bulan ERP sorgusu) AI'ın birer Tool/Araç olarak görüp ihtiyaç anında **otonom olarak kendi kendine çağırmasını (AutoInvoke)** sağladık.
- **Kurumsal Hafıza (Kernel Memory & RAG):** RAG (Retrieval-Augmented Generation) sistemini kurduk. Kurumun "2024 İzin Yönetmeliği" gibi şirkete özel sırlarını veya kurallarını **kalıcı olarak Redis Stack Vektör veritabanında** indeksledik (In-memory değil, kalıcı!).
- **Gerçek Zamanlı Karar Zinciri (Chain of Thought):** Asistana *"Ahmet'in elindeki mevcut kalan izni seneye devredebilir mi?"* diye sorduğunuzda; AI önce C# tarafına gidip Ahmet'in iznini sorguluyor, saniyeler içinde karar verip 2. aşamada Redis'teki şirket tüzüğünü tarıyor ve bu iki farklı veriyi bir cümle içinde profesyonelce yoğurarak asıl cevabı veriyor.
- **Güçlü Tip Sistemi (Type Safety & DI):** Magic stringler yok! Projede Dependency Injection (DI) altyapısı ve Options Pattern tam anlamıyla `.NET 10` native mimarisi üzerine kurgulanmıştır. İş mantığı ve modüller birbirinden izole edilmiştir.

## 🛠️ Temel Teknolojiler & Stack

- **Platform:** C# / `.NET 10`
- **Orkestrasyon:** `Microsoft.SemanticKernel`
- **Kurumsal RAG/Bellek:** `Microsoft.KernelMemory.Core`
- **Vektör Veritabanı:** `Redis Stack` (Docker üzerinde `redis/redis-stack` imajı ile çalışır)
- **Model:** OpenAI `gpt-4o` 

---

## 🚀 Projeyi Ayağa Kaldırma (Kurulum)

Projeyi kendi bilgisayarınızda denemek için aşağıdaki adımları izleyebilirsiniz.

### 1. Redis Stack Kurulumu (Vektör Araması İçin Zorunludur)
Kernel Memory "Standart" Redis ile çalışmaz çünkü Vektör aramasını (`FT.CREATE`, `HNSW` vs.) desteklemesi gerekir. Bunun için Docker üzerinden "Redis Stack" imajını ayağa kaldırmalısınız. Terminalde şu komutu çalıştırın:
```sh
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
```
*(Not: `http://localhost:8001` adresine giderek RedisInsight arayüzünden veritabanındaki verilerinizi görsel olarak yönetebilirsiniz).*

### 2. OpenAI API Anahtarını Eklemek
Projedeki `appsettings.json` dosyasını açın ve `YOUR_OPENAI_API_KEY_HERE` yazan alana kendi openai (`sk-proj...`) anahtarınızı yapıştırın.

*(Önemli: GitHub'a atarken burayı açık bırakmamaya özen gösterin, production için User Secrets ya da Key Vault tercih edin).*

### 3. Uygulamayı Başlatma
Proje terminalden `dotnet run` ile veya doğrudan Visual Studio üzerinden **F5** kullanılarak başlatılır.
- **İlk açılışta:** RAG maliyet tasarrufunu (Embedding) ayarladığımız için, önce şirket mevzuatını vektörize edip Redis'e yazar.
- **Sonraki açılışlarda:** C# kodundaki `IsDocumentReadyAsync` filtresi sayesinde saniyesinde hafızadan okur, tekrar tekrar OpenAI masrafı çıkarmaz.

---

## 💡 Örnek Kullanım ve Test Sorusu

Uygulama çalıştıktan sonra konsoldaki asistana şu cümleyi sorun:

> **"Ahmet'in halihazırdaki kalan izni bu sene kullanılmazsa seneye devredilebilir mi?"**

**Sistemin Çalışma Özütü (Chain of Thought):**
1. Asistan soruyu anlar. Kendi emrine verilen servislerden **`LeaveManagementPlugin`** i (C# kodu) AutoInvoke ile çalıştırır. (Geriye 14 döner).
2. Sonrasında bu 14 günlük kurumsal değeri ne yapacağını bilmediği için, ikinci emrindeki **`CorporateMemoryPlugin`** i tetikler, Redis'e *"izin devir kurallarını"* sorar. RAG devreye girer.
3. Asistan bu iki veriyi birleştirir: *"Ahmet'in sistemde bulduğum 14 günlük izni bulunmaktadır. Ancak şirket mevzuatına göre..."* şeklinde sentezleyerek insansı bir çıktı üretir.

---
*Bu çalışma, C# ile "Native AI" mimarisi oluşturmanın ne kadar sürdürülebilir, güvenilir ve kurumsal standartlara uygun olabileceğini göstermek adına LinkedIn (POC) serisi için hazırlanmıştır.*