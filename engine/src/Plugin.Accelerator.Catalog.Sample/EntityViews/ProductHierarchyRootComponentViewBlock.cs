using Plugin.Accelerator.Catalog.Sample.Components;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.Accelerator.Catalog.Sample.EntityViews
{
    [PipelineDisplayName(Constants.ProductHierarchyRootComponentViewBlock)]
    public class ProductHierarchyRootComponentViewBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        public override async Task<EntityView> Run(
          EntityView arg,
          CommercePipelineExecutionContext context)
        {
            KnownCatalogViewsPolicy policy = context.GetPolicy<KnownCatalogViewsPolicy>();
            EntityViewArgument entityViewArgument = context.CommerceContext.GetObject<EntityViewArgument>();
            if (string.IsNullOrEmpty(entityViewArgument?.ViewName) || !entityViewArgument.ViewName.Equals(policy.Master, StringComparison.OrdinalIgnoreCase))
                return await Task.FromResult(arg);

            if (!entityViewArgument.Entity.HasComponent<ProductHierarchyRootComponent>())
            {
                return arg;
            }

            var component = entityViewArgument.Entity.GetComponent<ProductHierarchyRootComponent>();
            var childView = new Sitecore.Commerce.EntityViews.EntityView
            {
                Name = "ProductHierarchyRootComponentDetails",
                DisplayName = "Product Hierarchy Root Component Details",
                EntityId = entityViewArgument.EntityId,
                EntityVersion = entityViewArgument.Entity.EntityVersion
            };

            arg.ChildViews.Add(childView);

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(ProductHierarchyRootComponent.PartNumber),
                DisplayName = "PartNumber",
                IsRequired = false,
                RawValue = component?.PartNumber,
                Value = component?.PartNumber,
                IsReadOnly = true
            });

            childView.Properties.Add(new ViewProperty
            {
                Name = nameof(ProductHierarchyRootComponent.Weight),
                DisplayName = "Weight",
                IsRequired = false,
                RawValue = component?.Weight,
                Value = component?.Weight,
                IsReadOnly = true
            });
            
            return arg;
        }
    }
}
