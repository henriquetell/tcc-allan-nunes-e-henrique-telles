using System;
using System.ComponentModel;

namespace Framework.UI.Mensagem
{
    [Serializable]
    public enum ETipoMensagem
    {
        [Description("info")]
        Informacao,

        [Description("success")]
        Sucesso,

        [Description("warning")]
        Perigo,

        [Description("error")]
        Erro
    }
}