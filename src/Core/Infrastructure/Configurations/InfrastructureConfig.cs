namespace Infrastructure.Configurations
{
    public class InfrastructureConfig
    {
        public string ConnectionString { get; set; }
        public StorageConfig Storage { get; set; } = new StorageConfig();
        public EmailConfig Email { get; set; } = new EmailConfig();
        public PagSeguroConfig PagSeguro { get; set; } = new PagSeguroConfig();
    }
}
