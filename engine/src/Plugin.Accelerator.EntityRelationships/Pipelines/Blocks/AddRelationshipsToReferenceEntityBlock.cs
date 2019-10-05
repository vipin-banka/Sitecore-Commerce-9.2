using Plugin.Accelerator.EntityRelationships.Extensions;
using Plugin.Accelerator.EntityRelationships.Models;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AddRelationshipsToReferenceEntityBlock)]
    public class AddRelationshipsToReferenceEntityBlock : PipelineBlock<RelationshipArgument, RelationshipArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly IFindEntityVersionsPipeline _findEntityVersionsPipeline;

        public AddRelationshipsToReferenceEntityBlock(IPersistEntityPipeline persistEntityPipeline,
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

            var parentEntity = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(CommerceEntity), arg.SourceName, context.GetRequestedEntityVersionValue(arg.SourceName), false), context).ConfigureAwait(false);

            if (parentEntity == null)
            {
                return arg;
            }

            context.CommerceContext.AddUniqueObjectByType(arg);
            List<string> targetList = new List<string>();
            if (arg.TargetName.Contains("|"))
            {
                string targetName = arg.TargetName;
                char[] chArray = new char[1] { '|' };
                targetList.AddRange((IEnumerable<string>)targetName.Split(chArray));
            }
            else
                targetList.Add(arg.TargetName);

            foreach(var referenceEntityId in targetList)
            {
                var findArgument = new FindEntityArgument(typeof(CommerceEntity), referenceEntityId, context.GetRequestedEntityVersionValue(referenceEntityId), false);
                var referenceEntities = await context.GetEntityiesToManageRelationships(this._findEntityVersionsPipeline, findArgument).ConfigureAwait(false);

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
                    && x.EntityVersion == parentEntity.EntityVersion
                    && x.RelationshipType.Equals(arg.RelationshipType, StringComparison.OrdinalIgnoreCase));

                    if (entityReference == null)
                    {
                        entityReference = new RelatedVersionedEntityReference(arg.SourceName, parentEntity.EntityVersion, arg.RelationshipType, parentEntity.Name);
                        associationEntity.Associations.Add(entityReference);

                        await this._persistEntityPipeline.Run(new PersistEntityArgument(associationEntity), context).ConfigureAwait(false);
                    }
                }
            }           

            return arg;
        }
    }
}