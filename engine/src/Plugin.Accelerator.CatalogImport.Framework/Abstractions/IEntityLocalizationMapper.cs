using Plugin.Accelerator.CatalogImport.Framework.Model;
using System.Collections.Generic;

namespace Plugin.Accelerator.CatalogImport.Framework.Abstractions
{
    public interface IEntityLocalizationMapper
    {
        IList<LocalizablePropertyValues> Map(ILanguageEntity languageEntity, IList<LocalizablePropertyValues> entityLocalizableProperties);
    }
}