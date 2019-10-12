namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class ComponentMetadataPolicy : Sitecore.Commerce.Core.Policy
    {
        public ComponentMetadataPolicy()
        {
            this.MapperKey = string.Empty;
        }

        public string MapperKey { get; set; }
    }
}