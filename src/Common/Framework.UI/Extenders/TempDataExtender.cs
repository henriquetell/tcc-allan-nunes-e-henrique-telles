using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Framework.UI.Extenders
{
    public static class TempDataExtender
    {

        public static T Get<T>(this ITempDataDictionary tempData) where T : class
        {
            var key = typeof(T).FullName;
            return Get<T>(tempData, key);
        }
        private static T Get<T>(ITempDataDictionary tempData, string key) where T : class
        {
            if (!tempData.ContainsKey(key))
                return null;

            var buffer = tempData[key] as byte[];
            if (buffer == null)
                return null;

            using (var memoryStream = new MemoryStream(buffer))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(memoryStream) as T;
            }
        }

        public static void Set<T>(this ITempDataDictionary tempData, T obj) where T : class
        {
            var key = typeof(T).FullName;
            Set(tempData, obj, key);
        }
        private static void Set<T>(ITempDataDictionary tempData, T obj, string key) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);

                tempData[key] = memoryStream.ToArray();
            }
        }

        public static void SaveModelStateErros(this ITempDataDictionary tempData, ModelStateDictionary modelState)
        {
            var modelStateValues = modelState.Where(i => i.Value.ValidationState == ModelValidationState.Invalid)
                    .ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage ?? e.Exception.Message).ToArray());

            tempData["#modelStateErros#"] = JsonConvert.SerializeObject(modelStateValues);
        }

        public static void LoadModelStateErros(this ITempDataDictionary tempData, ModelStateDictionary modelState)
        {
            var tempDataValue = tempData["#modelStateErros#"] as string;
            if (string.IsNullOrWhiteSpace(tempDataValue))
                return;

            var modelStateValues = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(tempDataValue);
            if (modelStateValues != null)
            {
                foreach (var item in modelStateValues)
                {
                    foreach (var erro in item.Value)
                    {
                        modelState.AddModelError(item.Key, erro);
                    }
                }
            }
        }

        public static void SaveModelStateValue(this ITempDataDictionary tempData, ModelStateDictionary modelState, string property = null)
        {
            var modelStateValues = modelState
                .Where(i => string.IsNullOrWhiteSpace(property) || i.Key == property)
                .ToDictionary(i => i.Key, i => i.Value.AttemptedValue);

            tempData[$"#modelStateValues|{property ?? string.Empty}#"] = JsonConvert.SerializeObject(modelStateValues);
        }

        public static void LoadModelStateValue(this ITempDataDictionary tempData, ModelStateDictionary modelState, string property = null)
        {
            var tempDataValue = tempData[$"#modelStateValues|{property ?? string.Empty}#"] as string;
            if (string.IsNullOrWhiteSpace(tempDataValue))
                return;

            var modelStateValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(tempDataValue);
            if (modelStateValues != null)
            {
                foreach (var item in modelStateValues.Where(i => string.IsNullOrWhiteSpace(property) || i.Key == property))
                {
                    if (!string.IsNullOrWhiteSpace(property) && !string.IsNullOrWhiteSpace(item.Value))
                        modelState.SetModelValue(property, new ValueProviderResult(item.Value));
                }
            }
        }
    }
}
