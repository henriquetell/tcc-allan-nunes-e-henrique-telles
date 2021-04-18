using ApplicationCore.Enuns;

namespace ApplicationCore.DataValue
{
    public class ProdutoDataValue
    {
        public string Titulo { get; set; }
        public string Codigo { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoLonga { get; set; }
        public decimal Preco { get; set; }
        public int Id { get; set; }
        public string Imagem { get; set; }
        public ECategoriaProduto CategoriaProduto { get; set; }
    }
}
