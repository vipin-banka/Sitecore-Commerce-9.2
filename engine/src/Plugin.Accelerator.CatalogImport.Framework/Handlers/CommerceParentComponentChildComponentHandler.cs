using Sitecore.Commerce.Core;
using System;
using System.Linq;

namespace Plugin.Accelerator.CatalogImport.Framework.Handlers
{
    public class CommerceParentComponentChildComponentHandler : CommerceEntityComponentHandler
    {
        public Component ParentComponent { get; }

        public CommerceParentComponentChildComponentHandler(CommerceEntity commerceEntity, Component parentComponent) : base(commerceEntity)
        {
            this.ParentComponent = parentComponent;
        }

        public override Component GetComponent(Type type)
        {
            return this.ParentComponent.ChildComponents.FirstOrDefault(c => c.GetType() == type);
        }

        public override Component GetComponent(Type type, string id)
        {
            return this.ParentComponent.ChildComponents.FirstOrDefault(c => c.GetType() == type && c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        public override void SetComponent(Component component)
        {
            this.ParentComponent.SetComponent(component);
        }

        public override Component AddComponent(Component component)
        {
            var existingComponent = this.GetComponent(component.GetType(), component.Id);
            if (existingComponent == null)
            {
                this.ParentComponent.ChildComponents.Add(component);
                return component;
            }

            return existingComponent;
        }

        public override Component RemoveComponent<T>()
        {
            var component = this.GetComponent(typeof(T));
            if (component != null)
            {
                this.ParentComponent.RemoveComponent(typeof(T));
            }

            return component;
        }
    }
}