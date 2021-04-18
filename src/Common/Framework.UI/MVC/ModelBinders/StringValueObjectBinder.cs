using Framework.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Framework.UI.MVC.ModelBinders
{
    public class StringValueObjectBinder<TStringValueObjectValue> : IModelBinder
        where TStringValueObjectValue: class, IStringValueObject
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var actualValue = Activator.CreateInstance(typeof(TStringValueObjectValue), valueProviderResult.FirstValue);
            bindingContext.Result = ModelBindingResult.Success(actualValue);
            return Task.CompletedTask;
        }
    }
}
