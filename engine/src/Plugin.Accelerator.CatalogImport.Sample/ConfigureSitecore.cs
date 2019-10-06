namespace Plugin.Accelerator.CatalogImport.Sample
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
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
            );

            services.RegisterAllCommands(assembly);
        }
    }
}