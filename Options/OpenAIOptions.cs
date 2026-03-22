namespace EnterpriseAIPoc.Options;

public class OpenAIOptions
{
    public const string SectionName = "OpenAI";

    public string ModelId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
