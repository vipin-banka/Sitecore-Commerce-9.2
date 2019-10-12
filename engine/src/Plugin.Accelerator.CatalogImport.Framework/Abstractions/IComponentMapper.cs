using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Abstractions
{
    public interface IComponentMapper
    {
        ComponentAction GetComponentAction();

        Component Map();

        Component Remove();
    }
}