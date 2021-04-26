namespace Infrastructure.Configurations
{
    public class InfrastructureConfig
    {
        public string DefaultConnection { get; set; }
        public StorageConfig Storage { get; set; } = new StorageConfig();
        public EmailConfig Email { get; set; } = new EmailConfig();
    }
}
