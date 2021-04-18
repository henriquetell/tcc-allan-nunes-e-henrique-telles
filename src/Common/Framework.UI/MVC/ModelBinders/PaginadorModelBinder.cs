using Framework.Extenders;
using Framework.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Framework.UI.MVC.ModelBinders
{
    public class PaginadorModelBinder : IModelBinder, IModelBinderProvider
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType == typeof(Paginador))
            {
                var key = bindingContext.ModelName;

                var actualValue = new Paginador(
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

            return value.ToInt32();
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context) => this;
    }
}