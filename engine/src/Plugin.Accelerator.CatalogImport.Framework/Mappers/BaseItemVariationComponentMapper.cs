﻿using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Globalization;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public class BaseItemVariationComponentMapper<TSourceEntity, TSourceVariant, TCommerceEntity> : BaseVariantComponentMapper<TSourceEntity, TSourceVariant, TCommerceEntity, ItemVariationComponent>
        where TSourceEntity : class
        where TSourceVariant : class
        where TCommerceEntity : CommerceEntity
    {
        public BaseItemVariationComponentMapper(TSourceEntity sourceEntity, TSourceVariant sourceVariant, CommerceEntity commerceEntity, Component parentComponent, CommercePipelineExecutionContext context)
            :base(sourceEntity, sourceVariant, commerceEntity, parentComponent, context)
        { }

        protected override bool AllowMultipleComponents => true;

        protected override string GetLocalizableComponentPath(ItemVariationComponent component)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}",
                typeof(ItemVariationsComponent).Name,
                typeof(ItemVariationComponent).Name);
        }
    }
}