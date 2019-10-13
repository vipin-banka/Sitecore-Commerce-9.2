﻿namespace Plugin.Accelerator.CatalogImport.Framework
{
    using Microsoft.Extensions.DependencyInjection;
    using Plugin.Accelerator.CatalogImport.Framework.Pipelines;
    using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.Catalog;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;
    using System.Reflection;

    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IRunningPluginsPipeline>(c =>
                {
                    c.Add<Pipelines.Blocks.RegisteredPluginBlock>()
                        .After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure =>
                {
                    configure.Add<ConfigureServiceApiBlock>();
                })
                .AddPipeline<IImportEntityPipeline, ImportEntityPipeline>(configure =>
                {
                    configure
                        .Add<PrepImportEntityBlock>()
                        .Add<ResolveImportHandlerInstanceBlock>()
                        .Add<GetSourceEntityBlock>()
                        .Add<ValidateSourceEntityBlock>()
                        .Add<ImportEntityBlock>();
                })
                .AddPipeline<IResolveEntityImportHandlerPipeline, ResolveEntityImportHandlerPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityImportHandlerBlock>();
                })
                .AddPipeline<ISetComponentsPipeline, SetComponentsPipeline>(configure =>
                {
                    configure
                        .Add<UpdateEntityBlock>()
                        .Add<SetEntityComponentsBlock>()
                        .Add<ImportEntityVariantsBlock>();
                })
                .AddPipeline<IResolveEntityMapperPipeline, ResolveEntityMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityMapperBlock>();
                })
                .AddPipeline<IResolveComponentMapperPipeline, ResolveComponentMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveComponentMapperBlock>();
                })
                .AddPipeline<IResolveEntityLocalizationMapperPipeline, ResolveEntityLocalizationMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveEntityLocalizationMapperBlock>();
                })
                .AddPipeline<IResolveComponentLocalizationMapperPipeline, ResolveComponentLocalizationMapperPipeline>(configure =>
                {
                    configure
                        .Add<ResolveComponentLocalizationMapperBlock>();
                })
                .ConfigurePipeline<ICreateCatalogPipeline>(configure =>
                {
                    configure
                        .Add<SetComponentsOnCatalogEntityBlock>()
                        .After<CreateCatalogBlock>();
                })
                .ConfigurePipeline<ICreateCategoryPipeline>(configure =>
                {
                    configure
                        .Add<SetComponentsOnCatalogEntityBlock>()
                        .After<CreateCategoryBlock>();
                })
                .ConfigurePipeline<ICreateSellableItemPipeline>(configure =>
                {
                    configure
                        .Add<SetComponentsOnCatalogEntityBlock>()
                        .After<CreateSellableItemBlock>();
                })
                .AddPipeline<IAssociateParentsPipeline, AssociateParentsPipeline>(configure =>
                {
                    configure
                        .Add<Pipelines.Blocks.AssociateCategoryToParentBlock>()
                        .Add<Pipelines.Blocks.AssociateSellableItemToParentBlock>();
                })
                .AddPipeline<IImportLocalizeContentPipeline, ImportLocalizeContentPipeline>(configure =>
                {
                    configure
                        .Add<GetLocalizePropertiesBlock>()
                        .Add<GetLocalizationEntityBlock>()
                        .Add<SetLocalizePropertiesBlock>();
                })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}