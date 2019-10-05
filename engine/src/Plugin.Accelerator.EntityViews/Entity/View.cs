using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Entity
{
    public abstract class View<TEntity> 
        where TEntity : CommerceEntity
    {
        public abstract string ViewName { get; }

        public abstract string ViewDisplayName { get; }

        public virtual Task<bool> ShouldViewApply(CommercePipelineExecutionContext context, EntityView entityView,
            TEntity entity)
        {
            return Task.FromResult(string.IsNullOrWhiteSpace(entityView.Action));
        }

        public abstract Task ModifyView(CommercePipelineExecutionContext context, EntityView entityView, TEntity entity);
    }
}