using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using System;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Extensions
{
    public static class CommerceEntityExtensions
    {
        public static ItemVariationComponent GetVariation(
            this CommerceEntity instance,
            string variationId)
        {
            if (instance == null || !instance.HasComponent<ItemVariationsComponent>() || string.IsNullOrEmpty(variationId))
                return null;
            return instance.GetComponent<ItemVariationsComponent>().ChildComponents.OfType<ItemVariationComponent>().FirstOrDefault(x => x.Id.Equals(variationId, StringComparison.OrdinalIgnoreCase));
        }
    }
}