namespace Plugin.Accelerator.CatalogImport.Framework.Policy
{
    public class MapperType
    {
        public MapperType()
        {
            this.Key = string.Empty;
            this.Type = string.Empty;
        }

        public string Key { get; set; }

        public string Type { get; set; }

        public string FullTypeName { get; set; }

        public string LocalizationFullTypeName { get; set; }
    }
}