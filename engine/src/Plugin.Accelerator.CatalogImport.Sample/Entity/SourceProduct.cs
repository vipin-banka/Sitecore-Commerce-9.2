using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Entity;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceProduct : IEntity
    {
        public SourceProduct()
        {
            this.Parents = new List<string>();
            this.Languages = new List<LanguageEntity<SourceProduct>>();
            this.Variants = new List<SourceProductVariant>();
        }

        [EntityId()]
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        [Parents()]
        public IList<string> Parents { get; set; }

        public string PartNumber { get; set; }

        public string Weight { get; set; }

        public string Accessories { get; set; }

        public string Dimensions { get; set; }

        [Languages()]
        public IList<LanguageEntity<SourceProduct>> Languages { get; set; }

        [Variants()]
        public IList<SourceProductVariant> Variants { get; set; }
    }
}