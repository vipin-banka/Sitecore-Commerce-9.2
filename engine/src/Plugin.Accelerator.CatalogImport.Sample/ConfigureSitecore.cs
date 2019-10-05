namespace Plugin.Accelerator.CatalogImport.Sample
{
    using Microsoft.Extensions.DependencyInjection;
    using Plugin.Accelerator.CatalogImport.Sample.Pipelines;
    using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks;
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
                .ConfigurePipeline<IRunningPluginsPipeline>(c => {
                    c.Add<Pipelines.Blocks.RegisteredPluginBlock>()
                        .After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure.Add<ConfigureServiceApiBlock>())
                .AddPipeline<IImportProductPipeline, ImportProductPipeline>(configure =>
                {
                    configure
                        .Add<ImportProductBlock>();
                })
                .AddPipeline<ISetComponentsPipeline, SetComponentsPipeline>(configure =>
                {
                    configure
                        .Add<SetComponentsBlocks>();
                })
                .AddPipeline<IAssociateParentsPipeline, AssociateParentsPipeline>(configure =>
                {
                    configure
                        .Add<AssociateParentsBlock>();
                })
                .AddPipeline<IImportLocalizeContentPipeline, ImportLocalizeContentPipeline>(configure =>
                {
                    configure                        
                        .Add<GetLocalizePropertiesBlock>()
                        .Add<GetLocalizationEntityBlock>()
                        .Add<SetLocalizePropertiesBlock>();
                })
                .ConfigurePipeline<ICreateSellableItemPipeline>(configure =>
                {
                    configure
                        .Add<SetComponentsOnListBlocks>()
                        .After<CreateSellableItemBlock>();
                })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}