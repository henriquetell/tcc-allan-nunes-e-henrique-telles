using System.ComponentModel;

namespace Framework.Security.Authorization
{
    public enum AuthPermissaoTipoAcao
    {
        [Description(nameof(Leitura))]
        Leitura = 1,

        [Description(nameof(Escrever))]
        Escrever = 2,

        [Description(nameof(Excluir))]
        Excluir = 3
    }
}