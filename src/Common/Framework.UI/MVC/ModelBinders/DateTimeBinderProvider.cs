using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Framework.UI.MVC.ModelBinders
{
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTime) ||
                context.Metadata.ModelType == typeof(DateTime?))
            {
                return new DateTimeBinder();
            }

            return null;
        }
    }
}
