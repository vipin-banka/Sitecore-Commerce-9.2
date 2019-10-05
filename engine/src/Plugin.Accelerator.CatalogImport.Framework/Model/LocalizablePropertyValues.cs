using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Model
{
    public class LocalizablePropertyValues
    {
        public LocalizablePropertyValues()
        {
            this.Parameters = new List<Parameter>();
        }

        public string PropertyName { get; set; }

        public IList<Parameter> Parameters { get; set; }
    }
}