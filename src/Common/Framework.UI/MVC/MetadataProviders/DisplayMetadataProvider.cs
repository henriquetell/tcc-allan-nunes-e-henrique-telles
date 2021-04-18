using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;

namespace Framework.UI.MVC.MetadataProviders
{
    public class DisplayMetadataProvider : IDisplayMetadataProvider
    {
        private readonly Type _resourceType;

        public DisplayMetadataProvider(Type resourceType)
        {
            _resourceType = resourceType;
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            var propertyAttributes = context.Attributes;
            var modelMetadata = context.DisplayMetadata;
            var propertyName = context.Key.Name;
            var containerName = context.Key.ContainerType?.Name;

            if (string.IsNullOrWhiteSpace(propertyName) ||
                string.IsNullOrWhiteSpace(containerName) ||
                !IsTransformRequired(propertyName, modelMetadata, propertyAttributes))
                return;

            var displayName = new ResourceManager(_resourceType).GetString($"{containerName}_{propertyName}");
            if (string.IsNullOrWhiteSpace(displayName))
                displayName = new ResourceManager(_resourceType).GetString(propertyName);
            if (!string.IsNullOrWhiteSpace(displayName))
                modelMetadata.DisplayName = () => displayName;
        }

        private static bool IsTransformRequired(string propertyName, DisplayMetadata modelMetadata, IReadOnlyList<object> propertyAttributes)
        {
            if (!string.IsNullOrEmpty(modelMetadata.SimpleDisplayProperty))
                return false;

            if (propertyAttributes.OfType<DisplayNameAttribute>().Any())
                return false;

            if (propertyAttributes.OfType<DisplayAttribute>().Any())
                return false;

            return !string.IsNullOrEmpty(propertyName);
        }
    }
}