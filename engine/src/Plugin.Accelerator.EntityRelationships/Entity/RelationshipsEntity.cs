using Plugin.Accelerator.EntityRelationships.Models;
using Sitecore.Commerce.Core;
using System.Collections.Generic;

namespace Plugin.Accelerator.EntityRelationships.Entity
{
    public class RelationshipsEntity : CommerceEntity
    {
        public RelationshipsEntity()
        {
            this.Associations = new List<RelatedVersionedEntityReference>();
        }

        public IList<RelatedVersionedEntityReference> Associations { get; set; }
    }
}