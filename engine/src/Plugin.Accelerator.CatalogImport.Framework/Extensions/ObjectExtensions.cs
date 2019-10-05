using Newtonsoft.Json;

namespace Plugin.Accelerator.CatalogImport.Framework.Extensions
{
    public static class ObjectExtensions
    {
        public static T Clone<T>(this T value) where T : class
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(serializedValue) as T;
        }
    }
}