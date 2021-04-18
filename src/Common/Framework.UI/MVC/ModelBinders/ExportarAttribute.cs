using System;

namespace Framework.UI.MVC.ModelBinders
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ExportarAttribute : Attribute
    {
        public string Titulo { get; set; }
        public int Campo { get; set; }
        public int Ordem { get; set; }

        public ExportarAttribute(string titulo, int ordem = 0, int campo = 0)
        {
            Titulo = titulo;
            Campo = campo;
            Ordem = ordem;
        }
    }
}
