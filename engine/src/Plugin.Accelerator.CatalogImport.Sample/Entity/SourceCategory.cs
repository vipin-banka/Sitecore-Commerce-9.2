using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Entity;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceCategory : SourceBaseEntity
    {
        public SourceCategory()
        {
            this.Languages = new List<LanguageEntity<SourceCategory>>();
        }

        [Languages()]
        public IList<LanguageEntity<SourceCategory>> Languages { get; set; }
    }
}