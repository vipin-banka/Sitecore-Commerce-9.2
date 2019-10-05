using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.Components = new List<string>();
            this.Parents = new List<string>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public IList<string> Components { get; set; }

        public IList<string> Parents { get; set; }
    }
}