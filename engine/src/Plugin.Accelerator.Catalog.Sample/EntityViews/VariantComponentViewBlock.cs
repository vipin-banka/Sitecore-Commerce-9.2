using Plugin.Accelerator.Catalog.Sample.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.Accelerator.Catalog.Sample.EntityViews
{
    [PipelineDisplayName(Constants.VariantComponentViewBlock)]
    public class VariantComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override async Task<EntityView> Run(
          EntityView arg,
          CommercePipelineExecutionContext context)
        {
            KnownCatalogViewsPolicy policy = context.GetPolicy<KnownCatalogViewsPolicy>();
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
            if (string.IsNullOrEmpty(entityViewArgument?.ViewName) || !entityViewArgument.ViewName.Equals(policy.Variant, StringComparison.OrdinalIgnoreCase))
                return await Task.FromResult(arg);

            if (!entityViewArgument.Entity.HasComponent<ItemVariationsComponent>())
            {
                return arg;
            }

            var variation = (entityViewArgument.Entity as SellableItem).GetVariation(entityViewArgument.ItemId);

            if (variation == null || !variation.HasComponent<VariantComponent>())
            {
                return arg;
            }

            var component = variation.GetComponent<VariantComponent>();
            var childView = new Sitecore.Commerce.EntityViews.EntityView
            {
                Name = "VariantComponentDetails",
                DisplayName = "Variant Component Details",
                EntityId = entityViewArgument.EntityId,
                EntityVersion = entityViewArgument.Entity.EntityVersion,
                ItemId = arg.ItemId
            };

            arg.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantComponent.Color),
                DisplayName = "Color",
                IsRequired = false,
                RawValue = component?.Color,
                Value = component?.Color,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(VariantComponent.Length),
                DisplayName = "Length",
                IsRequired = false,
                RawValue = component?.Length,
                Value = component?.Length,
                IsReadOnly = true
            });
            
            return arg;
        }
    }
}
