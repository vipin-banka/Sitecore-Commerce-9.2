using Plugin.Accelerator.EntityViews.Entity;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityViews.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.DoActionBlock)]
    public class DoActionBlock<TActionView, TEntity>
        : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
        where TActionView : ActionView<TEntity>
        where TEntity : CommerceEntity
    {
        private readonly PersistEntityCommand _persistEntityCommand;
        private readonly TActionView _actionView;

        public CommerceCommander Commander { get; set; }

        public DoActionBlock(CommerceCommander commander, TActionView actionView, PersistEntityCommand persistEntityCommand)
            : base(null)
        {
            _persistEntityCommand = persistEntityCommand;
            _actionView = actionView;
            this.Commander = commander;
        }

        public override async Task<EntityView> Run(EntityView arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{this.Name}: The argument can not be null");

            // Only proceed if the right action was invoked
            if (string.IsNullOrEmpty(arg.Action) || !arg.Action.Equals(_actionView.ActionName, StringComparison.OrdinalIgnoreCase))
                return arg;

            var entity = context.CommerceContext.GetObject<TEntity>(x => x.Id.Equals(arg.EntityId) && x.EntityVersion == arg.EntityVersion);
            if (entity == null) return arg;

            await this._actionView.DoAction(context, arg, entity);

            return arg;
        }
    }
}