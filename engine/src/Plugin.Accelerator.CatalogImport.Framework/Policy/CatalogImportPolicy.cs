namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class CatalogImportPolicy : Sitecore.Commerce.Core.Policy
    {
        public CatalogImportPolicy()
        {
            this.DeleteOrphanVariant = true;
        }

        public bool DeleteOrphanVariant { get; set; }

        public Mappings Mappings { get; set; }
    }
}