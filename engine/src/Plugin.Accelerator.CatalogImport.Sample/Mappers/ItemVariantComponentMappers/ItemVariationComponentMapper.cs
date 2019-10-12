using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;

namespace Plugin.Accelerator.CatalogImport.Sample.Mappers.ItemVariantComponentMappers
{
    public class ItemVariationComponentMapper : BaseItemVariationComponentMapper<SourceProduct, SourceProductVariant, CommerceEntity>
    {
        public ItemVariationComponentMapper(SourceProduct product, SourceProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, commerceCommander, context)
        { }

        protected override string ComponentId => this.SourceVariant.Id;

        protected override void Map(ItemVariationComponent component)
        {
            component.Id = this.SourceVariant.Id;
            component.DisplayName = this.SourceVariant.DisplayName;
            component.Description = this.SourceVariant.Description;
        }

        protected override void MapLocalizeValues(ItemVariationComponent component)
        {
            component.DisplayName = this.SourceVariant.DisplayName;
            component.Description = this.SourceVariant.Description;
        }
    }
}