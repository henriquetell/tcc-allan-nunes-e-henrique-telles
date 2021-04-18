using System;

namespace Framework.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ExportarAttribute : Attribute
    {
        public string Titulo { get; set; }
        public int Ordem { get; set; }

        public ExportarAttribute(string titulo, int ordem = 0)
        {
            Titulo = titulo;
            Ordem = ordem;
        }
    }
}
