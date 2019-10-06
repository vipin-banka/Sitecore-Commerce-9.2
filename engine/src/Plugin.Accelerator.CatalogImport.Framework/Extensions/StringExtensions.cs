using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Extensions
{
    public static class StringExtensions
    {
        public static string ToLocalizationEntityId(this string commerceEntityId)
        {
            return string.Format("{0}{1}", 
                CommerceEntity.IdPrefix<LocalizationEntity>(), 
                commerceEntityId.Replace("Entity-", string.Empty));
        }
    }
}