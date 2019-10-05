using Plugin.Accelerator.EntityRelationships.Entity;
using Sitecore.Commerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.EntityRelationships.Extensions
{
    public static class CommercePipelineExecutionContextExtensions
    {
        public static RequestedEntityVersion GetRequestedEntityVersion(this CommercePipelineExecutionContext context, string entityId)
        {
            return context.CommerceContext.GetModel<RequestedEntityVersion>(x => x.EntityId.Equals(entityId, StringComparison.OrdinalIgnoreCase));
        }

        public static int? GetRequestedEntityVersionValue(this CommercePipelineExecutionContext context, string entityId)
        {
            var model = context.GetRequestedEntityVersion(entityId);
            return model != null ? model.EntityVersion : null;
        }

        public static async Task<IList<CommerceEntity>> GetEntityiesToManageRelationships(this CommercePipelineExecutionContext context, IFindEntityVersionsPipeline findEntityVersionsPipeline, FindEntityArgument findEntityArgument)
        {
            var applicableEntites = new List<CommerceEntity>();
            var entities = await findEntityVersionsPipeline.Run(findEntityArgument, context).ConfigureAwait(false);
            if (entities != null && entities.Any())
            {
                applicableEntites = entities.Where(x => !x.Published).ToList();
                if (applicableEntites == null || !applicableEntites.Any())
                {
                    applicableEntites = new List<CommerceEntity> { entities.OrderBy(x => x.EntityVersion).FirstOrDefault(x => x.Published) };
                }
            }

            return applicableEntites;
        }

        public static async Task<IList<RelationshipsEntity>> GetRelationshipsEntityiesForCommerceEntities(this CommercePipelineExecutionContext context, IFindEntityPipeline findEntityPipeline, IList<CommerceEntity> entities)
        {
            var result = new List<RelationshipsEntity>();
            if (entities == null || !entities.Any())
            {
                return result;
            }

            foreach (var entity in entities)
            {
                var associationEntity = await findEntityPipeline.Run(new FindEntityArgument(typeof(RelationshipsEntity), entity.Id.ToRelationshipsEntityId(), entity.EntityVersion, false), context).ConfigureAwait(false) as RelationshipsEntity;
                if (associationEntity != null)
                {
                    result.Add(associationEntity);
                }
            }

            return result;
        }
    }
}