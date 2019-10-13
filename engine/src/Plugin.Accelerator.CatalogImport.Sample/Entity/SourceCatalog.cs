using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Entity;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceCatalog : IEntity
    {
        public SourceCatalog()
        {
            this.Languages = new List<LanguageEntity<SourceCatalog>>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceCatalog>> Languages { get; set; }
    }
}