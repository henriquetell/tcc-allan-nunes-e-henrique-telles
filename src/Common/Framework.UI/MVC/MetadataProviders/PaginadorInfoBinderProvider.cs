using Framework.Data;
using Framework.UI.MVC.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Framework.UI.MVC.MetadataProviders
{
    public class PaginadorInfoBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(PaginadorInfo))
                return new PaginadorInfoModelBinder();

            return null;
        }
    }
}