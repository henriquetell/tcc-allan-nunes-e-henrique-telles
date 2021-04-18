namespace ApplicationCore.Entities
{
    public class ConteudoAnexoEntity : EntityBase, IEntityDateLog
    {
        public int IdConteudo { get; set; }
        public string Anexo { get; set; }
        public string NomeOriginal { get; set; }
        public ConteudoEntity Conteudo { get; set; }
    }
}
