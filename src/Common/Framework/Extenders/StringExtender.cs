using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.Extenders
{
    public static class StringExtender
    {
        #region ToInt32

        public static int ToInt32(this string numero)
        {
            try
            {
                return Convert.ToInt32(numero);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter " + numero + " para Int32", ex);
            }
        }

        public static int ToInt32(this string numero, int defaultNumber)
        {
            try
            {
                int.TryParse(numero, out defaultNumber);
                return defaultNumber;
            }
            catch (Exception)
            {
                return defaultNumber;
            }
        }

        public static bool IsInt32(this string numero)
        {
            try
            {
                if (string.IsNullOrEmpty(numero))
                    return true;
                int outNumber;
                return int.TryParse(numero, out outNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter " + numero + " para Int32", ex);
            }
        }

        #endregion

        #region ToInt64

        public static long ToLong(this string numero)
        {
            try
            {
                return Convert.ToInt64(numero);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter " + numero + " para ToInt64", ex);
            }
        }


        public static long ToLong(this string numero, long defaultNumber)
        {
            try
            {
                long.TryParse(numero, out defaultNumber);
                return defaultNumber;
            }
            catch (Exception)
            {
                return defaultNumber;
            }
        }

        #endregion

        #region ToDecimal

        public static decimal ToDecimal(this string numero)
        {
            try
            {
                return string.IsNullOrEmpty(numero)
                    ? decimal.Zero
                    : Convert.ToDecimal(numero);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter " + numero + " para Decimal", ex);
            }
        }

        public static decimal ToDecimal(this string numero, decimal defaultValue)
        {
            try
            {
                return string.IsNullOrEmpty(numero)
                    ? decimal.Zero
                    : Convert.ToDecimal(numero);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region To Float

        public static float ToFloat(this string numero,
            float? defaultValue = null,
            CultureInfo culture = null)
        {
            try
            {
                if (string.IsNullOrEmpty(numero))
                    return defaultValue ?? 0;

                return culture == null
                    ? float.Parse(numero)
                    : float.Parse(numero, culture);
            }
            catch (Exception ex)
            {
                if (defaultValue.HasValue)
                    return defaultValue.Value;

                throw new Exception("Não foi possível converter " + numero + " para Float", ex);
            }
        }

        public static bool IsFloat(this string numero)
        {
            try
            {
                if (string.IsNullOrEmpty(numero))
                    return true;

                float outNumber;
                return float.TryParse(numero, out outNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível verificar " + numero + " para Float", ex);
            }
        }

        #endregion

        #region To Bool

        public static bool ToBool(this string value)
        {
            return ToBool(value, false);
        }

        public static bool ToBool(this string value, bool defaultValue)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return defaultValue;
                return Convert.ToBoolean(value.ToLower());
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        #endregion

        #region To Guid

        public static Guid ToGuid(this string guid)
        {
            try
            {
                return new Guid(guid);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível converter " + guid + " para Guid", ex);
            }
        }

        public static Guid ToGuid(this string guid, Guid guidDefault)
        {
            try
            {
                return new Guid(guid);
            }
            catch (Exception)
            {
                return guidDefault;
            }
        }

        #endregion

        #region ToDateTime

        public static DateTime ToDateTime(this string datetime, DateTime defaultValue)
        {
            try
            {
                return DateTime.Parse(datetime, new CultureInfo("pt-BR", false));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this string datetime)
        {
            try
            {
                return DateTime.Parse(datetime, new CultureInfo("pt-BR", false));
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ToDateTimeOrNull(this string datetime)
        {
            try
            {
                return DateTime.Parse(datetime, new CultureInfo("pt-BR", false));
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Verificação
        public static bool IsNullOrEmpty(this string value)
        {
            try
            {
                return string.IsNullOrEmpty(value);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível avaliar a string", ex);
            }
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            try
            {
                return string.IsNullOrWhiteSpace(value);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível avaliar a string", ex);
            }
        }

        #endregion

        #region Remover
        public static string RemoveNaoNumericos(this string text)
        {
            if (text == null)
                return null;

            var reg = new System.Text.RegularExpressions.Regex(@"[^0-9]");
            var ret = reg.Replace(text, string.Empty);
            return ret;
        }

        public static string RemoveAcentuacao(this string text)
        {
            if (text == null) return null;

            var decomposed = text.Normalize(System.Text.NormalizationForm.FormD);
            var filtered = decomposed.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            return new string(filtered.ToArray());
        }

        #endregion

        #region GetFirsts

        public static string GetFirsts(this string text, int totalCaracter)
        {
            if (text == null)
                return text;

            if (text.Length <= totalCaracter)
                return text;

            return text.Substring(0, totalCaracter);
        }

        #endregion

        #region GetLasts

        public static string GetLasts(this string text, int totalCaracter)
        {
            if (text == null)
                return text;

            if (text.Length <= totalCaracter)
                return text;

            return text.Substring(text.Length - totalCaracter);
        }

        #endregion

        #region IsXmlWhiteSpace

        public static bool IsXmlWhiteSpace(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var pattern = "<[^<>^]+?>";
            var result1 = Regex.Replace(text, pattern, string.Empty);
            pattern = @"[\s\t\n]*";
            var result_final = Regex.Replace(result1, pattern, string.Empty);
            return string.IsNullOrEmpty(result_final);
        }

        #endregion

        #region ToUrlFriendly

        public static string ToUrlFriendly(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.RemoveAcentuacao().Trim();
            text = Regex.Replace(text, @"[^A-Za-z0-9_\.~]+", "-");

            return text;
        }
        #endregion

        #region NullIfEmpty

        public static string NullIfEmpty(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return text;
        }

        #endregion

        #region Format
        public static string FormatText(this string text, string value)
        {
            return string.Format(text, value);
        }
        public static string FormatText(this string text, int value)
        {
            return string.Format(text, value);
        }
        #endregion

    }
}