using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Framework.UI.MVC.ModelBinders
{
    public class DecimalBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Decimal) ||
                context.Metadata.ModelType == typeof(Decimal?))
            {
                return new DecimalBinder();
            }

            return null;
        }
    }
}
