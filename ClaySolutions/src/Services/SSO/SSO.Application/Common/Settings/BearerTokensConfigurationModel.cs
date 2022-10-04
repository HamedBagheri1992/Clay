namespace SSO.Application.Common.Settings
{
    public class BearerTokensConfigurationModel
    {
        public const string NAME = "BearerTokens";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public bool AllowMultipleLoginsFromTheSameUser { get; set; }
        public bool AllowSignoutAllUserActiveClients { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }
}
