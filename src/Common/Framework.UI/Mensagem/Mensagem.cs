using Framework.Extenders;
using Framework.UI.Extenders;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace Framework.UI.Mensagem
{
    [Serializable]
    public class Mensagem
    {
        public Mensagem() { }

        public Mensagem(ETipoMensagem tipo, string titulo, string descricao)
        {
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
        }

        public ETipoMensagem Tipo { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }


        public static Mensagem[] Recuperar(ITempDataDictionary tempData)
        {
            var mensagens = tempData.Get<Mensagem[]>();
            return mensagens ?? new Mensagem[0];
        }

        internal void Salvar(ITempDataDictionary tempData)
        {
            var mensagens = new List<Mensagem>(Recuperar(tempData));
            mensagens.Add(this);

            tempData.Set(mensagens.ToArray());
        }
    }
}