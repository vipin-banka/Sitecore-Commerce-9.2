using Plugin.Accelerator.EntityRelationships.Entity;
using Plugin.Accelerator.EntityRelationships.Extensions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.RemoveRelationshipsFromReferenceEntityBlock)]
    public class RemoveRelationshipsFromReferenceEntityBlock : PipelineBlock<RelationshipArgument, RelationshipArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly IFindEntityVersionsPipeline _findEntityVersionsPipeline;

        public RemoveRelationshipsFromReferenceEntityBlock(IPersistEntityPipeline persistEntityPipeline,
            IDoesEntityExistPipeline doesEntityExistPipeline,
            IFindEntityPipeline findEntityPipeline,
            IFindEntityVersionsPipeline findEntityVersionsPipeline)
        {
            this._persistEntityPipeline = persistEntityPipeline;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
            this._findEntityPipeline = findEntityPipeline;
            this._findEntityVersionsPipeline = findEntityVersionsPipeline;
        }

        public override async Task<RelationshipArgument> Run(RelationshipArgument arg, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(arg.SourceName) || string.IsNullOrEmpty(arg.TargetName))
            {
                return arg;
            }

            var sourceEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(CommerceEntity), arg.SourceName, arg.SourceEntityVersion.HasValue ? arg.SourceEntityVersion.Value : context.GetRequestedEntityVersionValue(arg.SourceName), false), context).ConfigureAwait(false);

            if (sourceEntity != null)
            {
                string[] entityIds = arg.TargetName.Split('|');
                string[] strArray = entityIds;

                for (int index = 0; index < strArray.Length; ++index)
                {
                    var referenceEntityId = strArray[index];
                    var findEntityArgument = new FindEntityArgument(typeof(RelationshipsEntity), referenceEntityId, context.GetRequestedEntityVersionValue(referenceEntityId), false);
                    var referenceEntities = await context.GetEntityiesToManageRelationships(this._findEntityVersionsPipeline, findEntityArgument).ConfigureAwait(false);
                    if (referenceEntities == null || !referenceEntities.Any())
                    {
                        continue;
                    }

                    var associationEntities = await context.GetRelationshipsEntityiesForCommerceEntities(this._findEntityPipeline, referenceEntities).ConfigureAwait(false);
                    if (associationEntities == null || !associationEntities.Any())
                    {
                        continue;
                    }

                    foreach (var associationEntity in associationEntities)
                    {
                        var entityReference = associationEntity.Associations.FirstOrDefault(x => x.EntityTarget.Equals(arg.SourceName, StringComparison.OrdinalIgnoreCase) 
                        && x.EntityVersion == sourceEntity.EntityVersion
                        && x.RelationshipType.Equals(arg.RelationshipType, StringComparison.OrdinalIgnoreCase));
                        if (entityReference != null)
                        {
                            associationEntity.Associations.Remove(entityReference);

                            await this._persistEntityPipeline.Run(new PersistEntityArgument(associationEntity), context);
                        }
                    }
                }
            }

            return arg;
        }
    }
}