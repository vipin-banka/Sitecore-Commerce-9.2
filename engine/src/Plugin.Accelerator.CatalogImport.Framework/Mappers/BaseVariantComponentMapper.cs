using Plugin.Accelerator.CatalogImport.Framework.Handlers;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Globalization;
using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public abstract class BaseVariantComponentMapper<TSourceEntity, TSourceVariant, TCommerceEntity, TComponent> : BaseEntityComponentMapper<TSourceEntity, TCommerceEntity, TComponent>
        where TSourceEntity : IEntity
        where TSourceVariant : IEntity
        where TCommerceEntity : CommerceEntity
        where TComponent : Component, new()
    {
        protected TSourceVariant SourceVariant { get; }

        protected BaseVariantComponentMapper(TSourceEntity sourceEntity, TSourceVariant sourceVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
        : base(sourceEntity, new ItemVariationComponentChildComponentHandler(commerceEntity, parentComponent), context)
        {
            this.SourceVariant = sourceVariant;
        }

        protected override string GetLocalizableComponentPath(TComponent component)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}",
                typeof(ItemVariationsComponent).Name,
                typeof(ItemVariationComponent).Name,
                component.GetType().Name);
        }
    }
}