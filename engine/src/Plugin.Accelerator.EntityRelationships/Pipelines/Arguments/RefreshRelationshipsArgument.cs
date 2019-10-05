using Plugin.Accelerator.EntityRelationships.Models;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.EntityRelationships.Pipelines.Arguments
{
    public class RefreshRelationshipsArgument : PipelineArgument
    {
        public RefreshRelationshipsArgument(string entityId, IList<RelatedVersionedEntityReference> entityReferences)
        {
            this.EntityId = entityId;
            this.EntityReferences = entityReferences;
        }

        public string EntityId { get; set; }

        public IList<RelatedVersionedEntityReference> EntityReferences { get; set; }
    }
}