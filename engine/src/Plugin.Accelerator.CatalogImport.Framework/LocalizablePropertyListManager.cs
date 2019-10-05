using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Sitecore.Commerce.Core;

namespace Plugin.Accelerator.CatalogImport.Framework
{
    public static class LocalizablePropertyListManager
    {
        private static readonly IDictionary<Type, IList<LocalizablePropertyValues>> EntityLocalizableProperties = new ConcurrentDictionary<Type, IList<LocalizablePropertyValues>>();

        private static readonly IDictionary<Type, IList<LocalizableComponentPropertiesValues>> EntityComponentLocalizableProperties = new ConcurrentDictionary<Type, IList<LocalizableComponentPropertiesValues>>();

        private static readonly object _lockEntityProperties = new object();

        private static readonly object _lockEntityComponentProperties = new object();

        public static IList<LocalizablePropertyValues> GetEntityProperties(Type type, CommercePipelineExecutionContext context)
        {
            if (EntityLocalizableProperties.ContainsKey(type))
            {
                return EntityLocalizableProperties[type];
            }

            lock (_lockEntityProperties)
            {
                if (EntityLocalizableProperties.ContainsKey(type))
                {
                    return EntityLocalizableProperties[type];
                }

                EntityLocalizableProperties.Add(type, context.GetEntityLocalizableProperties(type));
                return EntityLocalizableProperties[type];
            }
        }

        public static IList<LocalizableComponentPropertiesValues> GetEntityComponentProperties(Type type, CommercePipelineExecutionContext context)
        {
            if (EntityComponentLocalizableProperties.ContainsKey(type))
            {
                return EntityComponentLocalizableProperties[type];
            }

            lock (_lockEntityComponentProperties)
            {
                if (EntityComponentLocalizableProperties.ContainsKey(type))
                {
                    return EntityComponentLocalizableProperties[type];
                }

                EntityComponentLocalizableProperties.Add(type, context.GetEntityComponentsLocalizableProperties(type));
                return EntityComponentLocalizableProperties[type];
            }
        }
    }
}