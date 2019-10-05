namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class LanguageEntity<T>
    {
        public string Language { get; set; }

        public T Entity { get; set; }
    }
}