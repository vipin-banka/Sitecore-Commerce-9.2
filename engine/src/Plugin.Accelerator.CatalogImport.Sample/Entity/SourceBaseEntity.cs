using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceBaseEntity : IEntity
    {
        public SourceBaseEntity()
        {
            this.Parents = new List<string>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }
    }
}