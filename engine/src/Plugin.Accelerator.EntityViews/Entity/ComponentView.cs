using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Entity
{
    public abstract class ComponentView<TEntity, TComponent> : View<TEntity>
        where TEntity : CommerceEntity
        where TComponent : Component
    {
        public virtual TComponent GetComponent(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
        {
            return entity?.GetComponent<TComponent>();
        }

        public override async Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView, TEntity entity)
        {
            var component = this.GetComponent(context, entityView, entity);
            await this.ModifyView(context, entityView, entity, component);
        }

        public abstract Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity,
            TComponent component);

        protected void AddProperty(EntityView entityView, TComponent component, Expression<Func<TComponent, object>> property,
            bool isReadOnly = true, bool isRequired = false)
        {
            var propertyInfo = ExpressionUtils.GetProperty(property);
            var getter = ExpressionUtils.CreateGetter(property);
            entityView.Properties.Add(new ViewProperty
            {
                Name = propertyInfo.Name,
                IsRequired = isRequired,
                RawValue = getter(component),
                IsReadOnly = isReadOnly
            });
        }
    }
}