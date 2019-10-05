using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Handlers;
using Plugin.Accelerator.CatalogImport.Framework.Mappers;
using Plugin.Accelerator.CatalogImport.Sample.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Sample.EntityComponentMappers
{
    public abstract class ProductComponentMapper<T> : BaseComponentMapper<T>
    where T : Component, new()
    {
        protected Product Product { get; }

        protected ProductComponentMapper(Product product, CommerceEntity commerceEntity, CommercePipelineExecutionContext context)
        : base(new CommerceEntityComponentHandler(commerceEntity), context)
        {
            this.Product = product;
        }

        protected ProductComponentMapper(Product product, CommerceEntity commerceEntity, IComponentHandler componentHandler, CommercePipelineExecutionContext context)
            : base(componentHandler, context)
        {
            this.Product = product;
        }
    }
}