using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class Product : BaseEntity
    {
        public Product()
        {
            this.Languages = new List<LanguageEntity<Product>>();
            this.Variants = new List<ProductVariant>();
        }

        public string PartNumber { get; set; }

        public string Weight { get; set; }

        public string Accessories { get; set; }

        public string Dimensions { get; set; }

        public IList<LanguageEntity<Product>> Languages { get; set; }

        public IList<string> VariantComponents { get; set; }

        public IList<ProductVariant> Variants { get; set; }
    }
}