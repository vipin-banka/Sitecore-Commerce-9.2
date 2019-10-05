using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityComponentMappers
{
    public class HardwareComponentMapper : ProductComponentMapper<HardwareComponent>
    {
        public HardwareComponentMapper(Product product, CommerceEntity commerceEntity, CommercePipelineExecutionContext context)
            : base(product, commerceEntity, context)
        { }

        protected override void Map(HardwareComponent component)
        {
            component.Accessories = this.Product.Accessories;
            component.Dimensions = this.Product.Dimensions;
        }

        protected override void MapLocalizeValues(HardwareComponent component)
        {
            component.Accessories = this.Product.Accessories;
            component.Dimensions = this.Product.Dimensions;
        }
    }
}