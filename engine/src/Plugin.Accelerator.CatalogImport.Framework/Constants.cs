namespace Plugin.Accelerator.CatalogImport.Framework
{
    public static class Constants
    {
        public const string ImportEntityPipeline = "CatalogImport.Framework.pipeline.ImportEntity";

        public const string AssociateParentsPipeline = "CatalogImport.Framework.pipeline.AssociateParents";

        public const string ImportLocalizeContentPipeline = "CatalogImport.Framework.pipeline.ImportLocalizeContent";

        public const string SetComponentsPipeline = "CatalogImport.Framework.pipeline.SetComponents";

        public const string RegisteredPluginBlock = "CatalogImport.Framework.block.RegisteredPlugin";

        public const string ConfigureServiceApiBlock = "CatalogImport.Framework.block.ConfigureServiceApi";

        public const string PrepImportEntityBlock = "CatalogImport.Framework.block.PrepImportEntity";

        public const string ResolveImportHandlerInstanceBlock = "CatalogImport.Framework.block.ResolveImportHandlerInstance";

        public const string GetSourceEntityBlock = "CatalogImport.Framework.block.GetSourceEntity";

        public const string ValidateSourceEntityBlock = "CatalogImport.Framework.block.ValidateSourceEntity";

        public const string ImportEntityBlock = "CatalogImport.Framework.block.ImportEntity";

        public const string UpdateEntityBlock = "CatalogImport.Framework.block.UpdateEntity";

        public const string SetEntityComponentsBlock = "CatalogImport.Framework.block.SetEntityComponents";

        public const string ImportEntityVariantsBlock = "CatalogImport.Framework.block.ImportEntityVariants";

        public const string SetItemVariantsComponentsBlock = "CatalogImport.Framework.block.SetItemVariantsComponents";

        public const string SetComponentsOnCatalogEntityBlock = "CatalogImport.Framework.block.SetComponentsOnCatalogEntity";

        public const string AssociateCategoryToParentBlock = "CatalogImport.Framework.block.AssociateCategoryToParent";

        public const string AssociateSellableItemToParentBlock = "CatalogImport.Framework.block.AssociateSellableItemToParent";

        public const string GetLocalizePropertiesBlock = "CatalogImport.Framework.block.GetLocalizeProperties";

        public const string SetLocalizePropertiesBlock = "CatalogImport.Framework.block.SetLocalizeProperties";

        public const string GetLocalizationEntityBlock = "CatalogImport.Framework.block.GetLocalizationEntity";
    }
}