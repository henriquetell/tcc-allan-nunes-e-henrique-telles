using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using Framework.Extenders;
using System.Diagnostics;

namespace Framework.ValueObjects
{
    [JsonConverter(typeof(StringValueJsonConverter<Telefone>))]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Telefone : IStringValueObject
    {
        private readonly Lazy<Match> _lazyMatch;

        public string DDI => _lazyMatch.Value.Success ? _lazyMatch.Value.Groups[nameof(DDI)].Value ?? "" : null;
        public string DDD => _lazyMatch.Value.Success ? (_lazyMatch.Value.Groups[nameof(DDD)].Value ?? "").GetLasts(2) : null;
        public string Numero => _lazyMatch.Value.Success ? _lazyMatch.Value.Groups[nameof(Numero)].Value ?? "" : null;
        public bool Valido => _lazyMatch.Value.Success;

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value == null ? null : Regex.Replace(value, @"[^0-9\+]", string.Empty);
            }
        }

        public Telefone(string value)
            : this()
        {
            Value = value;
        }

        protected Telefone()
        {
            _lazyMatch = new Lazy<Match>(LazyMatchFactory);
        }

        private Match LazyMatchFactory()
        {
            return Regex.Match(Value ?? "", @"(?'DDI'\+[0-9]{2})?(?'DDD'0*[0-9]{2})(?'Numero'[0-9]{8,9})");
        }

        public static implicit operator Telefone(string telefone) => new Telefone(telefone);
        public static implicit operator string(Telefone telefone) => string.IsNullOrWhiteSpace(telefone.Value) || !telefone.Valido
            ? null
            : $"{telefone.DDI}{telefone.DDD}{telefone.Numero}";

        private string DebuggerDisplay => Valido
            ? $"{DDI}{DDD}{Numero}"
            : ((Value ?? "NULL") + " (Inválido)").Trim();

        public override string ToString() => this;

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }
}
