using Plugin.Accelerator.EntityRelationships.Entity;
using Plugin.Accelerator.EntityRelationships.Extensions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.EntityVersions;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.CloneRelationshipsEntityBlock)]
    public class CloneRelationshipsEntityBlock : PipelineBlock<AddEntityVersionArgument, AddEntityVersionArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public CloneRelationshipsEntityBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<AddEntityVersionArgument> Run(AddEntityVersionArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull(this.Name + ": The argument cannot be null");

            string associationEntityId = arg.Entity.Id.ToRelationshipsEntityId();
            if (!string.IsNullOrEmpty(associationEntityId))
            {
                var associationEntity = await this._commerceCommander.GetEntity<RelationshipsEntity>(context.CommerceContext, associationEntityId).ConfigureAwait(false);
                if (associationEntity != null)
                {
                    var associationEntity2 = associationEntity.Clone<RelationshipsEntity>(new DateTimeOffset?(), false);
                    associationEntity2.Version = 0;
                    associationEntity2.EntityVersion = arg.NewVersion;
                    int num = await this._commerceCommander.PersistEntity(context.CommerceContext, associationEntity2).ConfigureAwait(false) ? 1 : 0;
                }
            }

            return arg;
        }
    }
}