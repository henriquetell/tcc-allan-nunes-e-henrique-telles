using Framework.ValueObjects;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Framework.UI.MVC.ModelBinders
{
    public class StringValueObjectBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType.GetInterface(typeof(IStringValueObject).FullName) != null)
            {
                var binderType = (typeof(StringValueObjectBinder<>)).MakeGenericType(context.Metadata.ModelType);
                return Activator.CreateInstance(binderType) as IModelBinder;
            }

            return null;
        }
    }
}
