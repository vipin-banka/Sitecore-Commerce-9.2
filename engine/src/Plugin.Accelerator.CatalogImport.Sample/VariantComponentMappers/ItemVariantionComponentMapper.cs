using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Globalization;

namespace Plugin.Accelerator.CatalogImport.Sample.VariantComponentMappers
{
    public class ItemVariantionComponentMapper : ProductVariantComponentMapper<ItemVariationComponent>
    {
        public ItemVariantionComponentMapper(Product product, ProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
            :base(product, productVariant, commerceEntity, parentComponent, context)
        { }

        protected override bool AllowMultipleComponents => true;

        protected override string ComponentId => this.ProductVariant.Id;

        protected override void Map(ItemVariationComponent component)
        {
            component.Id = this.ProductVariant.Id;
            component.DisplayName = this.ProductVariant.DisplayName;
            component.Description = this.ProductVariant.Description;
        }

        protected override void MapLocalizeValues(ItemVariationComponent component)
        {
            component.DisplayName = this.ProductVariant.DisplayName;
            component.Description = this.ProductVariant.Description;
        }

        protected override string GetLocalizableComponentPath(ItemVariationComponent component)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                typeof(ItemVariationsComponent).Name,
                typeof(ItemVariationComponent).Name);
        }
    }
}