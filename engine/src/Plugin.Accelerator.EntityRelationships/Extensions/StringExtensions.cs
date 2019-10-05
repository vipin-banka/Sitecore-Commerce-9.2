using Plugin.Accelerator.EntityRelationships.Entity;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.EntityRelationships.Extensions
{
    public static class StringExtensions
    {
        public static string ToRelationshipsEntityId(this string commerceEntityId)
        {
            return string.Format("{0}{1}", CommerceEntity.IdPrefix<RelationshipsEntity>(), commerceEntityId.Replace("Entity-", string.Empty));
        }
    }
}