namespace Framework.Configurations
{
    public class FrameworkConfig
    {
        public CdnConfig Cdn { get; set; } = new CdnConfig();
        public SecurityConfig Security { get; set; } = new SecurityConfig();
    }
}
