using System;

namespace ApplicationCore.DataValue
{
    public class NpsDataValue
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Imagem { get; set; }
        public DateTime? DataLimite { get; set; }
        public string DescricaoLonga { get; set; }
        public bool Respondido { get; set; }
    }
}
