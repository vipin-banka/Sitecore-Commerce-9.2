using Plugin.Accelerator.Catalog.Sample.Components;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.VariantComponentMappers
{
    public class VariantComponentMapper : ProductVariantComponentMapper<VariantComponent>
    {
        public VariantComponentMapper(Product product, ProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, context)
        { }

        protected override void Map(VariantComponent component)
        {
            component.Color = this.ProductVariant.Color;
            component.Length = this.ProductVariant.Length;
        }

        protected override void MapLocalizeValues(VariantComponent component)
        {
            component.Color = this.ProductVariant.Color;
            component.Length = this.ProductVariant.Length;
        }
    }
}