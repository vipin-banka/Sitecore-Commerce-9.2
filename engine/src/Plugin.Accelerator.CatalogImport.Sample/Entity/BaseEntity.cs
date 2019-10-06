using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class BaseEntity : IEntity
    {
        public BaseEntity()
        {
            this.Parents = new List<string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IList<string> Parents { get; set; }
    }
}