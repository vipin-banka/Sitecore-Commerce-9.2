using System.Collections.Generic;
using Plugin.Accelerator.CatalogImport.Framework.Model;

namespace Plugin.Accelerator.CatalogImport.Framework.Abstractions
{
    public interface IEntityLocalizationMapper
    {
        IList<LocalizablePropertyValues> Map(string language, IList<LocalizablePropertyValues> entityLocalizableProperties);
    }
}