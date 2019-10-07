using Plugin.Accelerator.Catalog.Sample.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.Accelerator.Catalog.Sample.EntityViews
{
    [PipelineDisplayName(Constants.HardwareComponentViewBlock)]
    public class HardwareComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override async Task<EntityView> Run(
          EntityView arg,
          CommercePipelineExecutionContext context)
        {
            KnownCatalogViewsPolicy policy = context.GetPolicy<KnownCatalogViewsPolicy>();
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
            if (string.IsNullOrEmpty(entityViewArgument?.ViewName) || !entityViewArgument.ViewName.Equals(policy.Master, StringComparison.OrdinalIgnoreCase))
                return await Task.FromResult(arg);

            if (!entityViewArgument.Entity.HasComponent<HardwareComponent>())
            {
                return arg;
            }

            var component = entityViewArgument.Entity.GetComponent<HardwareComponent>();
            var childView = new Sitecore.Commerce.EntityViews.EntityView
            {
                Name = "HardwareComponentDetails",
                DisplayName = "Hardware Component Details",
                EntityId = entityViewArgument.EntityId,
                EntityVersion = entityViewArgument.Entity.EntityVersion
            };

            arg.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(HardwareComponent.Accessories),
                DisplayName = "Accessories",
                IsRequired = false,
                RawValue = component?.Accessories,
                Value = component?.Accessories,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(HardwareComponent.Dimensions),
                DisplayName = "Dimensions",
                IsRequired = false,
                RawValue = component?.Dimensions,
                Value = component?.Dimensions,
                IsReadOnly = true
            });
            
            return arg;
        }
    }
}
