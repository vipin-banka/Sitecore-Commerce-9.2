using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Entity
{
    public abstract class ComponentActionView<TEntity, TComponent> : ActionView<TEntity>
        where TEntity : CommerceEntity
        where TComponent : Component
    {
        private readonly PersistEntityCommand _persistEntityCommand;

        protected ComponentActionView(PersistEntityCommand persistEntityCommand)
        {
            _persistEntityCommand = persistEntityCommand;
        }

        public abstract Task DoAction(CommercePipelineExecutionContext context,
            EntityView entityView,
            TEntity entity,
            TComponent component);

        public virtual Task EntityPersisted(CommercePipelineExecutionContext context,
            EntityView entityView,
            TEntity entity,
            TComponent component)
        {
            return Task.CompletedTask;
        }

        public virtual void SetComponent(CommercePipelineExecutionContext context,
            EntityView entityView,
            TEntity entity,
            TComponent component)
        {
            entity.SetComponent(component);
        }

        public virtual TComponent GetComponent(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
        {
            return entity?.GetComponent<TComponent>();
        }

        public override async Task DoAction(CommercePipelineExecutionContext context, EntityView entityView, TEntity entity)
        {
            var component = this.GetComponent(context, entityView, entity);

            await this.DoAction(context, entityView, entity, component);

            this.SetComponent(context, entityView, entity, component);

            entity = await this._persistEntityCommand.Process(context.CommerceContext, entity) as TEntity;

            await this.EntityPersisted(context, entityView, entity, component);
        }

        public override async Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
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
                IsRequired = false,
                RawValue = getter(component),
                IsReadOnly = false
            });
        }
    }
}