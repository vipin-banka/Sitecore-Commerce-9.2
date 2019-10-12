﻿using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System.Globalization;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public class BaseItemVariationComponentMapper<TSourceEntity, TSourceVariant, TCommerceEntity> : BaseVariantComponentMapper<TSourceEntity, TSourceVariant, TCommerceEntity, ItemVariationComponent>
        where TSourceEntity : IEntity
        where TSourceVariant : IEntity
        where TCommerceEntity : CommerceEntity
    {
        public BaseItemVariationComponentMapper(TSourceEntity sourceEntity, TSourceVariant sourceVariant, CommerceEntity commerceEntity, Component parentComponent, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            :base(sourceEntity, sourceVariant, commerceEntity, parentComponent, commerceCommander, context)
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