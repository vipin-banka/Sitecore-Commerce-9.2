using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.EntityRelationships.Models
{
    public class RelatedVersionedEntityReference : EntityReference
    {
        public RelatedVersionedEntityReference(string entityId, int? entityVersion, string relationshipType = "", string entityName = "")
            : base(entityId, entityName)
        {
            this.EntityVersion = entityVersion;
            this.RelationshipType = relationshipType;
        }

        public int? EntityVersion { get; set; }

        public string RelationshipType { get; set; }
    }
}