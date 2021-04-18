namespace Infrastructure.Configurations
{
    public class StorageConfig
    {
        public string ConnectionString { get; set; }
        public string Url { get; set; }
        public string DiretorioImagensProduto { get; set; }
        public string DiretorioImagensProdutoAdicional { get; set; }
        public string DiretorioRelatorio { get; set; }

        public string DiretorioArquivoSeguroViagem { get; set; }
        public string DiretorioConteudoAnexo { get; set; }
        public string DiretorioConteudoAnexoDadosFormulario { get; set; }

        public string DiretorioConteudoAnexoSolicitacao { get; set; }
    }
}
