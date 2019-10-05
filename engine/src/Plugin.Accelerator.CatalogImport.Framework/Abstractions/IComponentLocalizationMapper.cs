using Plugin.Accelerator.CatalogImport.Framework.Model;

namespace Plugin.Accelerator.CatalogImport.Framework.Abstractions
{
    public interface IComponentLocalizationMapper
    {
        LocalizableComponentPropertiesValues Map(string language, LocalizableComponentPropertiesValues localizableComponentPropertiesValues);
    }
}