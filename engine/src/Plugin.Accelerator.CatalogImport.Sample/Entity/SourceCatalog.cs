using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Entity;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceCatalog : BaseEntity
    {
        public SourceCatalog()
        {
            this.Languages = new List<LanguageEntity<SourceCatalog>>();
        }

        public IList<LanguageEntity<SourceCatalog>> Languages { get; set; }
    }
}