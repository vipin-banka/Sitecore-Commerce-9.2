using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Handlers;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Mappers
{
    public abstract class BaseEntityComponentMapper<TSourceEntity, TCommerceEntity, TComponent> : BaseComponentMapper<TComponent>
    where TSourceEntity : IEntity
    where TCommerceEntity : CommerceEntity
    where TComponent : Component, new()
    {
        public TSourceEntity SourceEntity { get; }

        protected BaseEntityComponentMapper(TSourceEntity sourceEntity, TCommerceEntity commerceEntity, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
        :base(new CommerceEntityComponentHandler(commerceEntity), commerceCommander, context)
        {
            this.SourceEntity = sourceEntity;
        }

        protected BaseEntityComponentMapper(TSourceEntity sourceEntity, IComponentHandler componentHandler, CommerceCommander commerceCommander, CommercePipelineExecutionContext context)
            : base(componentHandler, commerceCommander, context)
        {
            this.SourceEntity = sourceEntity;
        }
    }
}