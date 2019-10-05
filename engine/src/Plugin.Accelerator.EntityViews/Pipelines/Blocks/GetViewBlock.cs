using Plugin.Accelerator.EntityViews.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetViewBlock)]
    public class GetViewBlock<TView, TEntity>
        : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext> 
        where TView : View<TEntity>
        where TEntity : CommerceEntity
    {
        public readonly CommerceCommander CommerceCommander;
        public readonly ViewCommander ViewCommander;
        private readonly TView _view;

        public GetViewBlock(CommerceCommander commerceCommander, ViewCommander viewCommander, TView view)
            : base(null)
        {
            CommerceCommander = commerceCommander;
            ViewCommander = viewCommander;
            _view = view;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            var request = this.ViewCommander.CurrentEntityViewArgument(context.CommerceContext);
            if (request == null) return arg;

            var entity = request.Entity as TEntity;
            if (entity == null) return arg;

            if (!await _view.ShouldViewApply(context, arg, entity)) return arg;
            
            await _view.ModifyView(context, arg, entity);

            return arg;
        }
    }
}