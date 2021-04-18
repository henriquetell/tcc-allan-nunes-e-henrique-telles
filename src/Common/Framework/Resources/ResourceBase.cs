using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Framework.Resources
{
    public abstract class ResourceBase : IStringLocalizer
    {
        private Dictionary<string, string> Valores { get; set; } = new Dictionary<string, string>();

        public LocalizedString this[string name]
        {
            get
            {
                var nomeResource = ObterNome(name);

                if (Valores.ContainsKey(nomeResource))
                    return new LocalizedString(name, Valores[nomeResource]);

                return new LocalizedString(name, "", true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var ls = this[name];
                return AlterarDescricao(ls, ls.Value, arguments);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures)
        {
            foreach (var item in Valores)
            {
                yield return new LocalizedString(item.Key, item.Value);
            }
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }

        public void Init()
        {
            var values = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType == typeof(LocalizedString))
                .Where(p => p.Name != "Item")
                .Select(p => (LocalizedString)p.GetValue(this));

            foreach (var item in values)
            {
                if (Valores.ContainsKey(item.Name))
                    throw new Exception($"A Chave {item.Name} já foi adicionada ao ressource.");

                Valores.Add(item.Name, item.Value);
            }
        }

        protected LocalizedString Resource(string nome, string valor) =>
            new LocalizedString(ObterNome(nome), valor);

        private string ObterNome(string nome) =>
            $"{GetType().Name}.{nome}";

        protected LocalizedString AlterarDescricao(LocalizedString localizedString, string novaDescricao, params object[] arguments)
        {
            return new LocalizedString(localizedString.Name,
                string.Format(novaDescricao, arguments));
        }
    }
}
