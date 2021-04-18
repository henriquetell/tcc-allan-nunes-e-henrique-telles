using Framework.Extenders;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.ValueObjects
{
    public class NumeroCartaoCredito
    {
        private readonly string Value;

        public bool Valido => Validar();
        public string Numero => Value;

        public string NumeroMascarado => string.IsNullOrWhiteSpace(Value) ? null : $"{Value.Substring(0, 6)}******{Value.Substring(12, 4)}";


        public NumeroCartaoCredito(string numeroCartao)
        {
            Value = Regex.Replace(numeroCartao ?? string.Empty, "[^0-9]", string.Empty);
        }

        private bool Validar()
        {
            if (ValidarCartaoTeste())
                return true;

            int i, checkSum = 0;

            for (i = Value.Length - 1; i >= 0; i -= 2)
                checkSum += (Value[i] - '0');

            for (i = Value.Length - 2; i >= 0; i -= 2)
            {
                var val = ((Value[i] - '0') * 2);
                while (val > 0)
                {
                    checkSum += (val % 10);
                    val /= 10;
                }
            }

            return ((checkSum % 10) == 0);
        }

        private bool ValidarCartaoTeste()
        {
            var cartaoTestes = new[]
            {
                "9999999999999999",
                "0000000000000001", //Operação realizada com Sucesso
                "0000000000000002", //Não Autorizada
                "0000000000000003", //Cartão Expirado
                "0000000000000004", //Operação realizada com Sucesso
                "0000000000000005", //Cartão Bloqueado
                "0000000000000006", //Operação realizada com Sucesso
                "0000000000000007", //Cartão Cancelado
                "0000000000000008", //Problemas com o Cartão de Crédito
                "0000000000000009", //Operation Successful / Timed Out
            };

            return EnvironmentHelper.Desenvolvimento && cartaoTestes.Contains(Value);
        }

        public static implicit operator NumeroCartaoCredito(string numero) => new NumeroCartaoCredito(numero);

    }
}
