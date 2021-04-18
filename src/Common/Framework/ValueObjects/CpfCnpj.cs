using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Framework.ValueObjects
{
    [JsonConverter(typeof(StringValueJsonConverter<CpfCnpj>))]
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class CpfCnpj : IStringValueObject
    {
        public bool Cpf => ValidarCpf();
        public bool Cnpj => ValidarCnpj();

        public string Numero => (Cpf || Cnpj) ? Value : null;

        public string NumeroComMascara => Mascara();

        public bool Valido => Cpf || Cnpj;

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value == null ? null : Regex.Replace(value, @"[^0-9\+]", string.Empty);
            }
        }

        protected CpfCnpj() { }

        public CpfCnpj(string originalInput)
        {
            Value = originalInput;
        }

        public static implicit operator CpfCnpj(string cpfCnpj) => new CpfCnpj(cpfCnpj);
        public static implicit operator String(CpfCnpj cpfCnpj) => cpfCnpj == null || string.IsNullOrWhiteSpace(cpfCnpj.Numero) || !cpfCnpj.Valido
            ? null
            : cpfCnpj.Numero;


        private string Mascara()
        {
            if (Cpf) return Convert.ToUInt64(Numero).ToString(@"000\.000\.000\-00");
            if (Cnpj) return Convert.ToUInt64(Numero).ToString(@"00\.000\.000/0000-00");

            return string.Empty;
        }

        #region Validação de CPF
        private bool ValidarCpf()
        {
            var cpf = Value;
            if (cpf == null || cpf.Length != 11) return false;

            string auxiliar;
            for (var numero = 0; numero <= 9; numero++)
            {
                auxiliar = "".PadRight(11, char.Parse(Convert.ToString(numero)));

                if (cpf.Trim() == auxiliar)
                    return false;
            }

            auxiliar = cpf.Substring(0, cpf.Length - 2);
            var peso = 10;
            var soma = 0;

            foreach (char numero in auxiliar.ToCharArray())
            {
                soma += Convert.ToInt16(numero.ToString()) * peso;
                peso--;
            }

            var resto = soma % 11;

            var digito = resto < 2 ? "0" : Convert.ToString(11 - resto);

            auxiliar += digito;
            peso = 11;
            soma = 0;

            foreach (char numero in auxiliar.ToCharArray())
            {
                soma += Convert.ToInt16(numero.ToString()) * peso;
                peso--;
            }

            resto = soma % 11;

            digito += (resto < 2) ? "0" : Convert.ToString(11 - resto);

            return cpf.EndsWith(digito);
        }

        #endregion

        #region Validação de CNPJ

        private bool ValidarCnpj()
        {
            var cnpj = Value;
            if (cnpj == null || cnpj.Length != 14) return false;

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;
            for (var i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            var resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            var digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (var i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            digito = digito + resto;
            return cnpj.EndsWith(digito);
        }

        #endregion

        private string DebuggerDisplay => $"{(Numero ?? Value)}{(!Valido ? " (Inválido)" : "")}";
        public override string ToString() => this;
        public override int GetHashCode() => Value?.GetHashCode() ?? 0;


    }
}
