namespace QuickBaseApiTest;

public class BaseTest
{
    private HttpClient httpClient { get; set; }
    public string BaseUrl { get; } = "https://api.quickbase.com/v1";
    public string Token { get; } = "b98z9y_uyp_0_chme9gnbhtt3vrc9bavtvr7yz7k";
    public string RealmHostname { get; } = "team.quickbase.com";
    public string TableId { get; } = "bupigvc9u";
}