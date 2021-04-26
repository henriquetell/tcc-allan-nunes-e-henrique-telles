using System.ComponentModel;

namespace ApplicationCore.Enuns
{
    public enum EStatus : byte
    {
        [Description(nameof(Ativo))]
        Ativo = 1,

        [Description(nameof(Inativo))]
        Inativo = 2
    }
}
