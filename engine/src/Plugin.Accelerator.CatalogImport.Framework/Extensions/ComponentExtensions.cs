using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework.Extensions
{
    public static class ComponentExtensions
    {
        public static ComponentMetadataPolicy GetComponentMetadataPolicy(this Component component)
        {
            if (component.HasPolicy<ComponentMetadataPolicy>())
            {
                return component.GetPolicy<ComponentMetadataPolicy>();
            }

            return new ComponentMetadataPolicy();
        }

        public static void SetComponentMetadataPolicy(this Component component, string componentName)
        {
            component?.SetComponentMetadataPolicy(new ComponentMetadataPolicy() { MapperKey = componentName });
        }

        public static void SetComponentMetadataPolicy(this Component component, ComponentMetadataPolicy componentMetadataPolicy)
        {
            component?.SetPolicy(componentMetadataPolicy);
        }
    }
}