using System.ComponentModel;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace EnterpriseAIPoc.Plugins;

public class CorporateMemoryPlugin
{
    private readonly IKernelMemory _kernelMemory;

    public CorporateMemoryPlugin(IKernelMemory kernelMemory)
    {
        _kernelMemory = kernelMemory;
    }

    [KernelFunction("SearchCorporateRules")]
    [Description("Şirket mevzuatı, kuralları, yönetmelikleri veya politikaları hakkında bilgi almak gerektiğinde 'Kurumsal Hafıza'da (Vektör Veritabanı / Redis) arama yapmak için bu aracı kullanın.")]
    public async Task<string> SearchCorporateRules(
        [Description("Aranacak yasa, kural veya mevzuatla ilgili arama sorgusu/cümlesi. Örn: 'kullanılmayan izinlerin devri', 'yemek limiti'")] string query)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"\n[Agent Düşünce Zinciri (AutoInvoke)] RAG Sistemi Devrede! Redis'te şu kural aranıyor: '{query}'");
        Console.ResetColor();

        // 0.4 esnek arama eşiği (minRelevance) ile Redis DB'ye soruyoruz
        var searchResult = await _kernelMemory.SearchAsync(query, minRelevance: 0.4);

        if (!searchResult.Results.Any())
        {
            return "Kurumsal hafızada bu konuyla ilgili herhangi bir resmi mevzuat veya kayıt bulunamadı.";
        }

        // Bulunan en alakalı mevzuat bölümünü geri dönüyoruz ki AI muhakemesine katsın
        var bestMatch = searchResult.Results.First().Partitions.First().Text;
        return bestMatch;
    }
}