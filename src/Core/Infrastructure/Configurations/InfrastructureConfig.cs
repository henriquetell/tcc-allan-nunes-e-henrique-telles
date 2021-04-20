namespace Infrastructure.Configurations
{
    public class InfrastructureConfig
    {
        public StorageConfig Storage { get; set; } = new StorageConfig();
        public EmailConfig Email { get; set; } = new EmailConfig();
        public PagSeguroConfig PagSeguro { get; set; } = new PagSeguroConfig();
    }
}
