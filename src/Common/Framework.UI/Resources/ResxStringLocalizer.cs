using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Framework.UI.Resources
{
    public class ResxStringLocalizer : IStringLocalizer, IHtmlLocalizer, IViewLocalizer
    {
        private readonly ResourceManager _resourceManager;
        private readonly ResourceManager _customResourceManager;

        private readonly IResourceNamesCache _resourceNamesCache;
        private string _applicationBasePath;

        

        public ResxStringLocalizer(ResourceManager resourceManager, ResourceManager customResourceManager,
            IResourceNamesCache resourceNamesCache,
            string applicationBasePath)
        {
            _resourceManager = resourceManager;
            _customResourceManager = customResourceManager;

            _resourceNamesCache = resourceNamesCache;
            _applicationBasePath = applicationBasePath;
        }

        LocalizedHtmlString IHtmlLocalizer.this[string name]
        {
            get
            {
                var value = GetStringSafely(name);
                return new LocalizedHtmlString(name, value ?? name, value == null);
            }
        }

        LocalizedHtmlString IHtmlLocalizer.this[string name, params object[] arguments]
        {
            get
            {
                var format = GetStringSafely(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedHtmlString(name, value, format == null);
            }
        }

        public virtual LocalizedString this[string name]
        {
            get
            {
                var value = GetStringSafely(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetStringSafely(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }


     
        public virtual LocalizedHtmlString Html(string name)
        {
            var value = GetStringSafely(name);
            return new LocalizedHtmlString(name, value ?? name, isResourceNotFound: value == null);
        }

        public virtual LocalizedHtmlString Html(string name, params object[] arguments)
        {
            var format = GetStringSafely(name);
            var value = string.Format(format ?? name, arguments);
            return new LocalizedHtmlString(name, value, isResourceNotFound: format == null);
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new ResxStringLocalizer(_resourceManager, _customResourceManager,
                    _resourceNamesCache, _applicationBasePath);
        }

        public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures) =>
            GetAllStrings(includeAncestorCultures, CultureInfo.CurrentUICulture);

        protected IEnumerable<LocalizedString> GetAllStrings(bool includeAncestorCultures, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected string GetStringSafely(string name)
        {
            var resourceValue = string.Empty;

            try
            {
                if (_customResourceManager == null)
                    resourceValue = _resourceManager.GetString(name);
                else
                {
                    resourceValue = _customResourceManager.GetString(name) ?? _resourceManager.GetString(name);
                }
            }
            catch (MissingManifestResourceException)
            {
                return name;
            }

            return resourceValue;
        }

        IHtmlLocalizer IHtmlLocalizer.WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public LocalizedString GetString(string name)
        {
            throw new NotImplementedException();
        }

        public LocalizedString GetString(string name, params object[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}