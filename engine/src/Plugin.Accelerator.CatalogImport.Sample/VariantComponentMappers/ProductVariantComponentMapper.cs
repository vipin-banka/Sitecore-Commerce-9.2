using Plugin.Accelerator.CatalogImport.Framework.Handlers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Plugin.Accelerator.CatalogImport.Sample.EntityComponentMappers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Globalization;

namespace Plugin.Accelerator.CatalogImport.Sample.VariantComponentMappers
{
    public abstract class ProductVariantComponentMapper<T> : ProductComponentMapper<T>
    where T : Component, new()
    {
        protected ProductVariant ProductVariant { get; }

        protected ProductVariantComponentMapper(Product product, ProductVariant productVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
        : base(product, commerceEntity, new ItemVariationComponentChildComponentHandler(commerceEntity, parentComponent), context)
        {
            this.ProductVariant = productVariant;
        }

        protected override string GetLocalizableComponentPath(T component)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}",
                typeof(ItemVariationsComponent).Name,
                typeof(ItemVariationComponent).Name,
                component.GetType().Name);
        }
    }
}