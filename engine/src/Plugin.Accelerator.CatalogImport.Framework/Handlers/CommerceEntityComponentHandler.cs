using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Sitecore.Commerce.Core;
using System;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Handlers
{
    public class CommerceEntityComponentHandler : IComponentHandler
    {
        public CommerceEntity ParenEntity { get; }

        public CommerceEntityComponentHandler(CommerceEntity parenEntity)
        {
            this.ParenEntity = parenEntity;
        }

        public virtual Component GetComponent(Type type)
        {
            return this.ParenEntity.EntityComponents.FirstOrDefault(c => c.GetType() == type);
        }

        public virtual Component GetComponent(Type type, string id)
        {
            return this.ParenEntity.EntityComponents.FirstOrDefault(c => 
                (!string.IsNullOrEmpty(id) && c.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                || c.GetType() == type);
        }

        public virtual void SetComponent(Component component)
        {
            this.ParenEntity.SetComponent(component);
        }

        public virtual Component AddComponent(Component component)
        {
            var existingComponent = this.GetComponent(component.GetType(), component.Id);
            if (existingComponent == null)
            {
                this.ParenEntity.SetComponent(component);
                return component;
            }

            return existingComponent;
        }

        public virtual Component RemoveComponent<T>() where T : Component
        {
            var component = this.GetComponent(typeof(T));
            if (component != null)
            {
                this.ParenEntity.RemoveComponent(typeof(T));
            }

            return component;
        }

        public virtual Type GetEntityType()
        {
            return this.ParenEntity.GetType();
        }
    }
}