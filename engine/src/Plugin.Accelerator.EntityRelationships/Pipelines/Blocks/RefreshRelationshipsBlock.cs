namespace Plugin.Accelerator.EntityRelationships.Pipelines.Blocks
{
    using Plugin.Accelerator.EntityRelationships.Extensions;
    using Plugin.Accelerator.EntityRelationships.Models;
    using Plugin.Accelerator.EntityRelationships.Pipelines.Arguments;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [PipelineDisplayName(Constants.RefreshRelationshipsBlock)]
    public class RefreshRelationshipsBlock : PipelineBlock<RefreshRelationshipsArgument, RefreshRelationshipsArgument, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IFindEntityVersionsPipeline _findEntityVersionsPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;
        private readonly IDeleteRelationshipPipeline _deleteRelationshipPipeline;

        public RefreshRelationshipsBlock(IPersistEntityPipeline persistEntityPipeline,
            IFindEntityVersionsPipeline findEntityVersionsPipeline,
            IFindEntityPipeline findEntityPipeline,
            IDeleteRelationshipPipeline deleteRelationshipPipeline)
        {
            this._persistEntityPipeline = persistEntityPipeline;
            this._findEntityPipeline = findEntityPipeline;
            this._findEntityVersionsPipeline = findEntityVersionsPipeline;
            this._deleteRelationshipPipeline = deleteRelationshipPipeline;
        }

        public override async Task<RefreshRelationshipsArgument> Run(RefreshRelationshipsArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull(arg.EntityId + ": argument cannot be null");

            if (arg.EntityReferences == null || !arg.EntityReferences.Any())
            {
                return arg;
            }

            var findEntityArgument = new FindEntityArgument(typeof(CommerceEntity), arg.EntityId, null, false);
            var entities = await context.GetEntityiesToManageRelationships(this._findEntityVersionsPipeline, findEntityArgument).ConfigureAwait(false);

            if (entities == null || !entities.Any())
            {
                return arg;
            }

            var associatedEntities = await context.GetRelationshipsEntityiesForCommerceEntities(this._findEntityPipeline, entities).ConfigureAwait(false);

            if (associatedEntities == null || !associatedEntities.Any())
            {
                return arg;
            }


            foreach (var associationEntity in associatedEntities)
            {
                if (associationEntity.Associations == null || !associationEntity.Associations.Any())
                {
                    continue;
                }

                var removableEntities = new List<RelatedVersionedEntityReference>();
                foreach (var association in FilterBasedOnRelationType(associationEntity.Associations))
                {
                    if (!arg.EntityReferences.Any(x => x.EntityTarget.Equals(association.EntityTarget, System.StringComparison.OrdinalIgnoreCase)
                    && x.RelationshipType.Equals(association.RelationshipType, System.StringComparison.OrdinalIgnoreCase)))
                    {
                        removableEntities.Add(association);
                    }
                }

                if (removableEntities != null && removableEntities.Any())
                {
                    //associationEntity.Associations = associationEntity.Associations.Except(removableEntities).ToList();

                    //await this._persistEntityPipeline.Run(new PersistEntityArgument(associationEntity), context).ConfigureAwait(false);

                    foreach (var removableEntity in removableEntities)
                    {
                        var requestedEntityVersion = new RequestedEntityVersion(removableEntity.EntityTarget, removableEntity.EntityVersion);
                        var relationshipArgument = new RelationshipArgument(removableEntity.EntityTarget, arg.EntityId, removableEntity.RelationshipType);
                        context.CommerceContext.AddUniqueModelByType(requestedEntityVersion);
                        await this._deleteRelationshipPipeline.Run(relationshipArgument, context).ConfigureAwait(false);
                        context.CommerceContext.RemoveModel(requestedEntityVersion);
                    }
                }
            }

            return arg;
        }

        private IList<RelatedVersionedEntityReference> FilterBasedOnRelationType(IList<RelatedVersionedEntityReference> associations)
        {
            var result = new List<RelatedVersionedEntityReference>();

            if (associations == null || !associations.Any())
            {
                return result;
            }

            var relationshipTypes = new List<string> { "CatalogToSellableItem", "CategoryToSellableItem" };
            result = associations.Where(x => relationshipTypes.Contains(x.RelationshipType)).ToList(); ;
            ////foreach(var relationshipType in relationshipTypes)
            ////{
            ////    query = query.Where(x => x.RelationshipType.Equals(relationshipType, System.StringComparison.OrdinalIgnoreCase));
            ////}

            ////result = query.ToList();
            return result;
        }
    }
}