namespace Framework.Configurations
{
    public class AppConfig
    {
        public string Projeto { get; set; }
        public string DestinatarioFaleConosco { get; set; }
        public AdminConfig Admin { get; set; } = new AdminConfig();
        public ShopConfig Shop { get; set; } = new ShopConfig();
    }
}
