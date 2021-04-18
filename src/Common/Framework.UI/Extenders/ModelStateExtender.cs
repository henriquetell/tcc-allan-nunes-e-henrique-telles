using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.UI.Extenders
{
    public static class ModelStateExtender
    {
        public static List<string> GetErrosList(this ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(v => v.Errors).Select(m => m.ErrorMessage);

            return erros.ToList();
        }

        public static string GetErros(this ModelStateDictionary modelState, string separador = null)
        {
            separador = separador ?? Environment.NewLine;

            var erros = GetErrosList(modelState);

            return string.Join(separador, erros.ToArray());
        }


        public static bool IsValid<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression)
        {
            var key = GetExpressionText(expression);
            if (modelState.ContainsKey(key) &&
                modelState[key].ValidationState == ModelValidationState.Invalid)
            {
                return false;
            }

            var result = modelState.FindKeysWithPrefix(key);

            return result.Any(r => r.Value.ValidationState == ModelValidationState.Invalid)
                ? false
                : true;
        }

        private static string GetExpressionText(LambdaExpression expression)
        {
            var unaryExpression = expression.Body as UnaryExpression;

            if (IsConversionToObject(unaryExpression))
            {
                return ExpressionHelper.GetExpressionText(Expression.Lambda(
                    unaryExpression.Operand,
                    expression.Parameters[0]));
            }

            return ExpressionHelper.GetExpressionText(expression);
        }

        private static bool IsConversionToObject(UnaryExpression expression)
        {
            return expression?.NodeType == ExpressionType.Convert &&
                expression.Operand?.NodeType == ExpressionType.MemberAccess &&
                expression.Type == typeof(object);
        }

        public static void Remover<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression)
        {
            var key = GetExpressionText(expression);

            foreach (var item in modelState.FindKeysWithPrefix(key))
                modelState.Remove(item.Key);
        }

        public static void RemoverIf<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, bool condition)
        {
            if (!condition) return;
            var key = GetExpressionText(expression);
            foreach (var item in modelState.FindKeysWithPrefix(key))
                modelState.Remove(item.Key);
        }
    }
}