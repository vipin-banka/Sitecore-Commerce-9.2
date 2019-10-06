using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.VariantComponentMappers
{
    public class VariantComponentMapper : BaseVariantComponentMapper<SourceProduct, SourceProductVariant, CommerceEntity, VariantComponent>
    {
        public VariantComponentMapper(SourceProduct product, SourceProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, context)
        { }

        protected override void Map(VariantComponent component)
        {
            component.Color = this.SourceVariant.Color;
            component.Length = this.SourceVariant.Length;
        }

        protected override void MapLocalizeValues(VariantComponent component)
        {
            component.Color = this.SourceVariant.Color;
            component.Length = this.SourceVariant.Length;
        }
    }
}