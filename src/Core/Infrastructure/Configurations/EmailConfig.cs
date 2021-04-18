namespace Infrastructure.Configurations
{
    public class EmailConfig
    {
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public bool Ssl { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string EnderecoRemetente { get; set; }
        public string NomeRemetente { get; set; }

        public string DebugEmail { get; set; }

    }
}
