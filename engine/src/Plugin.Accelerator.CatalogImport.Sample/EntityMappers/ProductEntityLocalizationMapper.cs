using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityMappers
{
    public class ProductEntityLocalizationMapper : ProductEntityMapper
    {
        public ProductEntityLocalizationMapper(Product product, CommercePipelineExecutionContext context)
            : base(product, null, context)
        {
        }

        protected override void MapLocalizeValues(SellableItem sellableItem)
        {
            sellableItem.Name = this.Product.Name;
            sellableItem.DisplayName = this.Product.DisplayName;
            sellableItem.Description = this.Product.Description;
        }
    }
}