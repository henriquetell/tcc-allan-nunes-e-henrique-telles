using System.ComponentModel;

namespace ApplicationCore.Enuns
{
    public enum ETipoSku : byte
    {
        [Description(nameof(Voltagem))]
        Voltagem = 1,

        [Description(nameof(Cor))]
        Cor = 2,

        [Description(nameof(Tamanho))]
        Tamanho = 3,

        [Description(nameof(Data))]
        Data = 4
    }
}
