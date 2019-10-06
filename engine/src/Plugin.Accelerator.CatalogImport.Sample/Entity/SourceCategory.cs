using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Entity;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceCategory : BaseEntity
    {
        public SourceCategory()
        {
            this.Languages = new List<LanguageEntity<SourceCategory>>();
        }

        public IList<LanguageEntity<SourceCategory>> Languages { get; set; }
    }
}