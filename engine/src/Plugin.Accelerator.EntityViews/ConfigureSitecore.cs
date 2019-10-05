using Microsoft.Extensions.DependencyInjection;
using Plugin.Accelerator.EntityViews.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using System.Reflection;

namespace Plugin.Accelerator.EntityViews
{
    public class ConfigureSitecore : IConfigureSitecore
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);

            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IRunningPluginsPipeline>(c => {
                    c.Add<RegisteredPluginBlock>().After<RunningPluginsBlock>();
                })                
            );

            services.RegisterAllCommands(assembly);
        }
    }
}