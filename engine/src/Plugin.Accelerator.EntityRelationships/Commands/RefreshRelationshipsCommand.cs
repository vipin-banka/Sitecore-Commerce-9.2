using Plugin.Accelerator.EntityRelationships.Models;
using Plugin.Accelerator.EntityRelationships.Pipelines;
using Plugin.Accelerator.EntityRelationships.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Commands
{
    public class RefreshRelationshipsCommand : CommerceCommand
    {
        private readonly IRefreshRelationshipsPipeline _refreshAssociationPipeline;
        public RefreshRelationshipsCommand(IServiceProvider serviceProvider,
            IRefreshRelationshipsPipeline refreshAssociationPipeline)
            : base(serviceProvider)
        {
            this._refreshAssociationPipeline = refreshAssociationPipeline;
        }

        public virtual async Task Process(
            CommerceContext commerceContext,
            string entityId, IList<RelatedVersionedEntityReference> finalEntityReferences)
        {
            using (CommandActivity.Start(commerceContext, this))
            {
               await this.PerformTransaction(commerceContext, async () => await this._refreshAssociationPipeline.Run(new RefreshRelationshipsArgument(entityId, finalEntityReferences), commerceContext.PipelineContextOptions));
            }
        }
    }
}