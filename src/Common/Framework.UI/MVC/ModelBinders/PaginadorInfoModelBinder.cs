using Framework.Data;
using Framework.Extenders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Framework.UI.MVC.ModelBinders
{
    public class PaginadorInfoModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(PaginadorInfo))
            {
                var key = bindingContext.ModelName;

                var actualValue = new PaginadorInfo(
                    GetValue(bindingContext, "numeroPagina"),
                    GetValue(bindingContext, "tamanhoPagina"));

                bindingContext.Result = ModelBindingResult.Success(actualValue);
            }

            return Task.FromResult(0);
        }

        private static int? GetValue(ModelBindingContext bindingContext, string key)
        {
            var value = bindingContext.ValueProvider.GetValue(key).FirstValue;

            if (value == null)
                return null;

            return Convert.ToInt32(value);
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context) => this;
    }
}
