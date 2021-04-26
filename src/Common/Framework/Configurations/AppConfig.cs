namespace Framework.Configurations
{
    public class AppConfig
    {
        public string Projeto { get; set; }
        public AdminConfig Admin { get; set; } = new AdminConfig();
    }
}
