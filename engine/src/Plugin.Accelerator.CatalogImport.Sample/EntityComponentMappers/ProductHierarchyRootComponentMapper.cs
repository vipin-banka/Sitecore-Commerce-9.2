using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityComponentMappers
{
    public class ProductHierarchyRootComponentMapper : BaseEntityComponentMapper<SourceProduct, CommerceEntity, ProductHierarchyRootComponent>
    {
        public ProductHierarchyRootComponentMapper(SourceProduct product, CommerceEntity commerceEntity, CommercePipelineExecutionContext context)
            : base(product, commerceEntity, context)
        { }

        protected override void Map(ProductHierarchyRootComponent component)
        {
            component.PartNumber = this.SourceEntity.PartNumber;
            component.Weight = this.SourceEntity.Weight;
        }
    }
}