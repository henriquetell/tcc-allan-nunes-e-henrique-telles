using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Framework.ValueObjects
{
    [JsonConverter(typeof(StringValueJsonConverter<Email>))]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class Email : IStringValueObject
    {

        private readonly Lazy<Match> _lazyMatch;

        public string Usuario => _lazyMatch.Value.Success ? _lazyMatch.Value.Groups[nameof(Usuario)].Value : null;
        public string Dominio => _lazyMatch.Value.Success ? _lazyMatch.Value.Groups[nameof(Dominio)].Value : null;
        public bool Valido => _lazyMatch.Value.Success;

        public string Value { get; private set; }


        public Email(string originalInput)
            : this()
        {
            Value = originalInput?.Trim();
        }

        protected Email()
        {
            _lazyMatch = new Lazy<Match>(LazyMatchFactory);
        }

        private Match LazyMatchFactory()
        {
            /// <see cref="http://www.regular-expressions.info/email.html"/>
            //const string PatternRfc5322 = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01 -\x08\x0b\x0c\x0e -\x1f\x21\x23 -\x5b\x5d -\x7f] |\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])\z";

            /// <see cref="https://www.w3.org/TR/2012/WD-html-markup-20120320/input.email.html#input.email.attrs.value.single"/>
            const string PatternW3c = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";


            if (!string.IsNullOrWhiteSpace(Value) && Regex.IsMatch(Value, PatternW3c))
                return Regex.Match(Value, @"(?'Usuario'^[^@]+)@(?'Dominio'.*$)");

            return Match.Empty;
        }


        public static implicit operator Email(string email) => new Email(email);

        public static implicit operator string(Email email) => string.IsNullOrWhiteSpace(email.Value) || !email.Valido
            ? null
            : email.Value;

        public override string ToString() => this;

        private string DebuggerDisplay => $"{Value}{(!Valido ? " (Inválido)" : "")}";

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }
}