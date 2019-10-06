using Plugin.Accelerator.CatalogImport.Framework.Abstractions;

namespace Plugin.Accelerator.CatalogImport.Framework.Entity
{
    public class LanguageEntity<T> : ILanguageEntity<T>
    where T : IEntity
    {
        public string Language { get; set; }

        public IEntity GetEntity()
        {
            return Entity;
        }

        public T Entity { get; set; }
    }
}