using Plugin.Accelerator.EntityViews.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.PopulateActionBlock)]
    public class PopulateActionBlock<TActionView, TEntity> 
        : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
        where TActionView : ActionView<TEntity>
        where TEntity : CommerceEntity
    {
        private readonly TActionView _actionView;

        public CommerceCommander Commander { get; set; }

        public ViewCommander ViewCommander { get; }

        public PopulateActionBlock(CommerceCommander commander, ViewCommander viewCommander, TActionView actionView)
            : base(null)
        {
            _actionView = actionView;
            this.Commander = commander;
            ViewCommander = viewCommander;
        }

        public override Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            var request = this.ViewCommander.CurrentEntityViewArgument(context.CommerceContext);
            if (request == null) return Task.FromResult(arg);

            var entity = request.Entity as TEntity;
            if (entity == null) return Task.FromResult(arg);

            if (!_actionView.ShouldAddAction(context, arg, entity))
                return Task.FromResult(arg);

            var actionPolicy = arg.GetPolicy<ActionsPolicy>();
            actionPolicy.AddAction(
                new EntityActionView
                {
                    Name = _actionView.ActionName,
                    DisplayName = _actionView.ActionDisplayName,
                    Description = string.Empty,
                    IsEnabled = _actionView.IsEnabled,
                    EntityView = _actionView.GetEntityView(context, arg),
                    Icon = _actionView.ActionIcon,
                    RequiresConfirmation = _actionView.RequiresConfirmation
                });

            return Task.FromResult(arg);
        }
    }
}