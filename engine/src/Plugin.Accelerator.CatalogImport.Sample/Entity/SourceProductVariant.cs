﻿using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Metadata;

namespace Plugin.Accelerator.CatalogImport.Sample.Entity
{
    public class SourceProductVariant : IEntity
    {
        [EntityId()]
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string Length { get; set; }
    }
}