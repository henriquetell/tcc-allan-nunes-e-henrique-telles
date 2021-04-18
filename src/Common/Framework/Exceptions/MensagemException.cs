using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;

namespace Framework.Exceptions
{
    public class MensagemException : Exception
    {
        public readonly List<LocalizedString> Erros;

        public MensagemException()
            : base()
        {
            Erros = new List<LocalizedString>();
        }

        public MensagemException(params LocalizedString[] erros)
            : this(new List<LocalizedString>(erros))
        { }

        public MensagemException(IEnumerable<LocalizedString> erros)
        {
            Erros = new List<LocalizedString>();
            Erros.AddRange(erros);
        }

        public MensagemException(LocalizedString erro, Exception ex)
            : base(erro.Value, ex)
        {
            Erros = new List<LocalizedString>
            {
                erro
            };
        }
    }
}