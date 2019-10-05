using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityMappers
{
    public class ProductEntityMapper : BaseEntityMapper<SellableItem>
    {
        public Product Product { get; }

        public ProductEntityMapper(Product product, SellableItem sellableItem, CommercePipelineExecutionContext context)
            : base(sellableItem, context)
        {
            this.Product = product;
        }

        public override void Map()
        {
            this.CommerceEntity.Name = this.Product.Name;
            this.CommerceEntity.DisplayName = this.Product.DisplayName;
            this.CommerceEntity.Description = this.Product.Description;
        }
    }
}