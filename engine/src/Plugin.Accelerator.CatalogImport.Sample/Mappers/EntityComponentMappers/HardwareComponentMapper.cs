using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.Mappers.EntityComponentMappers
{
    public class HardwareComponentMapper : BaseEntityComponentMapper<SourceProduct, CommerceEntity, HardwareComponent>
    {
        public HardwareComponentMapper(SourceProduct product, CommerceEntity commerceEntity, CommercePipelineExecutionContext context)
            : base(product, commerceEntity, context)
        { }

        protected override void Map(HardwareComponent component)
        {
            component.Accessories = this.SourceEntity.Accessories;
            component.Dimensions = this.SourceEntity.Dimensions;
        }

        protected override void MapLocalizeValues(HardwareComponent component)
        {
            component.Accessories = this.SourceEntity.Accessories;
            component.Dimensions = this.SourceEntity.Dimensions;
        }
    }
}