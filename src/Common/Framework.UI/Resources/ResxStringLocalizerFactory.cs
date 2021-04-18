using Framework.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace Framework.UI.Resources
{
    public class ResxStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IHostingEnvironment _applicationEnvironment;
        private readonly IResourceNamesCache _resourceNamesCache = new ResourceNamesCache();

        private readonly MemoryCache _resourceInstance;

        public ResxStringLocalizerFactory(IHostingEnvironment applicationEnvironment,
            IOptions<MemoryCacheOptions> optionsAccessor)
        {
            _applicationEnvironment = applicationEnvironment;

            _resourceInstance = new MemoryCache(optionsAccessor.Value);
        }

        public IStringLocalizer Create(Type resourceType)
        {
            var cacheItem = _resourceInstance.Get(resourceType.FullName) as IStringLocalizer;
            if (cacheItem != null)
                return cacheItem;

            var resourceManager = new ResourceManager(resourceType);

            cacheItem = new ResxStringLocalizer(resourceManager, null,
                _resourceNamesCache,
                _applicationEnvironment.ContentRootPath);

            _resourceInstance.Set(resourceType.FullName, cacheItem);

            return cacheItem;
        }

       
        public IStringLocalizer Create(string baseName, string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                location = _applicationEnvironment.ApplicationName;
            }
            var assembly = Assembly.Load(new AssemblyName(location));

            return new ResxStringLocalizer(
                new ResourceManager(baseName, assembly),
                null,
                _resourceNamesCache,
                _applicationEnvironment.ContentRootPath);
        }
    }
}
