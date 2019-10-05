using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityComponentMappers
{
    public class ProductHierarchyRootComponentMapper : ProductComponentMapper<ProductHierarchyRootComponent>
    {
        public ProductHierarchyRootComponentMapper(Product product, CommerceEntity commerceEntity, CommercePipelineExecutionContext context)
            : base(product, commerceEntity, context)
        { }

        protected override void Map(ProductHierarchyRootComponent component)
        {
            component.PartNumber = this.Product.PartNumber;
            component.Weight = this.Product.Weight;
        }
    }
}