using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.Extenders
{
    public static class EnvironmentHelper
    {
        public static bool Desenvolvimento => VerificarTipoAmbiente("Development", nameof(Desenvolvimento));
        public static bool Homologacao => VerificarTipoAmbiente("Homologação");
        public static bool Producao => VerificarTipoAmbiente("Production", "Produção");

        private static string[] EnvVarirableNames = new[] { "ASPNETCORE_ENVIRONMENT", "WEBJOB_ENVIRONMENT" };

        private static bool VerificarTipoAmbiente(params string[] nomes) =>
            EnvVarirableNames.Select(e => Environment.GetEnvironmentVariable(e))
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => NormalizadarNome(e))
                .Any(e => nomes.Select(n => NormalizadarNome(n)).Any(n => n == e));


        private static string NormalizadarNome(string nome) =>
            Regex.Replace(nome.RemoveAcentuacao(), "\\s", "");
    }
}
