namespace BudgetJar.Api.Services
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiryHours { get; set; } = 12;
    }
}
