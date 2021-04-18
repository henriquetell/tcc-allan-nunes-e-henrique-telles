using Framework.UI.MVC.MetadataProviders;
using Framework.UI.MVC.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace Framework.UI.Extenders
{
    public static class MvcOptionsExtender
    {
        public static void AddBinders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new PaginadorInfoBinderProvider());
            options.ModelBinderProviders.Insert(0, new DateTimeBinderProvider());
            options.ModelBinderProviders.Insert(0, new DecimalBinderProvider());
            options.ModelBinderProviders.Insert(0, new DoubleBinderProvider());
            options.ModelBinderProviders.Insert(0, new StringValueObjectBinderProvider());
        }
    }
}
