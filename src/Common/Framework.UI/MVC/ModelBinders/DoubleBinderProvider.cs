using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Framework.UI.MVC.ModelBinders
{
    public class DoubleBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Double) ||
                context.Metadata.ModelType == typeof(Double?))
            {
                return new DoubleBinder();
            }

            return null;
        }
    }
}
