using Plugin.Accelerator.Catalog.Sample.EntityViews;
using Sitecore.Commerce.EntityViews;
using RegisteredPluginBlock = Plugin.Accelerator.Catalog.Sample.Pipelines.Blocks.RegisteredPluginBlock;

namespace Plugin.Accelerator.Catalog.Sample
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
                    c.Add<RegisteredPluginBlock>().After<RunningPluginsBlock>();
                })
                .ConfigurePipeline<IGetEntityViewPipeline>(c =>
                {
                    c
                    .Add<HardwareComponentViewBlock>()
                    .Add<ProductHierarchyRootComponentViewBlock>()
                    .Add<VariantComponentViewBlock>();
                })
            );

            services.RegisterAllCommands(assembly);
        }
    }
}