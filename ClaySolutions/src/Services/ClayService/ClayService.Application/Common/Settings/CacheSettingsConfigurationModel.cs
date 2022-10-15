namespace ClayService.Application.Common.Settings
{
    public class CacheSettingsConfigurationModel
    {
        public const string NAME = "CacheSettings";
        public string ConnectionString { get; set; }
        public bool InitData { get; set; }
    }
}
