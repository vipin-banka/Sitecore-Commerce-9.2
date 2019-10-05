using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Model
{
    public class LocalizableComponentPropertiesValues
    {
        public LocalizableComponentPropertiesValues()
        {
            this.PropertyValues = new List<LocalizablePropertyValues>();
        }

        public string Path { get; set; }

        public string Id { get; set; }

        public IList<LocalizablePropertyValues> PropertyValues { get; set; }
    }
}