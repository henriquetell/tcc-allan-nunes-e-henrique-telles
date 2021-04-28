using ApplicationCore.Enuns;

namespace NpsFunctions.Models
{
    public class SalvarNpsCommand
    {
        public ENotaNps? Nota { get; set; }
        public string Comentario { get; set; }
    }
}
