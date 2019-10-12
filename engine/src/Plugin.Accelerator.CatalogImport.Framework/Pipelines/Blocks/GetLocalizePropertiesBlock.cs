using Plugin.Accelerator.CatalogImport.Framework.Abstractions;
using Plugin.Accelerator.CatalogImport.Framework.Extensions;
using Plugin.Accelerator.CatalogImport.Framework.Model;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetLocalizePropertiesBlock)]
    public class GetLocalizePropertiesBlock : PipelineBlock<ImportLocalizeContentArgument, ImportLocalizeContentArgument, CommercePipelineExecutionContext>
    {
        private readonly CommerceCommander _commerceCommander;

        public GetLocalizePropertiesBlock(CommerceCommander commerceCommander)
        {
            this._commerceCommander = commerceCommander;
        }

        public override async Task<ImportLocalizeContentArgument> Run(ImportLocalizeContentArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg.ImportEntityArgument.ImportHandler.HasLanguages())
            {
                IList<LocalizablePropertyValues> entityLocalizableProperties = null;

                IList<LocalizableComponentPropertiesValues> componentsPropertiesList = new List<LocalizableComponentPropertiesValues>();

                foreach (var language in arg.ImportEntityArgument.ImportHandler.GetLanguages())
                {
                    entityLocalizableProperties = await PerformEntityLocalization(arg, context, language, entityLocalizableProperties).ConfigureAwait(false);

                    if (arg.CommerceEntity.EntityComponents == null || !arg.CommerceEntity.EntityComponents.Any())
                        continue;

                    await PerformEntityComponentsLocalization(arg, context, language, componentsPropertiesList).ConfigureAwait(false);

                    if (!arg.ImportEntityArgument.ImportHandler.HasVariants(language) )
                    {
                        continue;
                    }

                    await PerformEntityVariantsLocalization(arg, context, language, componentsPropertiesList).ConfigureAwait(false);
                }

                arg.Properties = entityLocalizableProperties;
                arg.ComponentsProperties = componentsPropertiesList;
            }

            return await Task.FromResult(arg);
        }

        private async Task PerformEntityVariantsLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language,
            IList<LocalizableComponentPropertiesValues> componentsPropertiesList)
        {
            foreach (var variant in arg.ImportEntityArgument.ImportHandler.GetVariants(language))
            {
                var itemVariationComponent = arg.CommerceEntity.GetVariation(variant.Id);

                if (itemVariationComponent == null)
                    continue;

                var itemVariantComponentLocalizationMapper = await this._commerceCommander
                    .Pipeline<IResolveComponentLocalizationMapperPipeline>()
                    .Run(new ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument, arg.CommerceEntity,
                            itemVariationComponent, language, variant), context).ConfigureAwait(false);
                itemVariantComponentLocalizationMapper.Execute(componentsPropertiesList, itemVariationComponent,
                    language);

                await PerformEntityVariantLocalization(arg, context, language, componentsPropertiesList, itemVariationComponent, variant).ConfigureAwait(false);
            }
        }

        private async Task PerformEntityVariantLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizableComponentPropertiesValues> componentsPropertiesList,
            ItemVariationComponent itemVariationComponent, IEntity variant)
        {
            if (itemVariationComponent.ChildComponents != null
                && itemVariationComponent.ChildComponents.Any())
            {
                foreach (var component in itemVariationComponent.ChildComponents)
                {
                    var mapper = await this._commerceCommander.Pipeline<IResolveComponentLocalizationMapperPipeline>()
                        .Run(new
                            ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument, arg.CommerceEntity,
                                component, language, variant), context).ConfigureAwait(false);

                    mapper.Execute(componentsPropertiesList, component, language);
                }
            }
        }

        private async Task PerformEntityComponentsLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizableComponentPropertiesValues> componentsPropertiesList)
        {
            foreach (var commerceEntityComponent in arg.CommerceEntity.EntityComponents)
            {
                var entityComponentLocalizationMapper = await this._commerceCommander
                    .Pipeline<IResolveComponentLocalizationMapperPipeline>()
                    .Run(
                        new ResolveComponentLocalizationMapperArgument(arg.ImportEntityArgument,
                            arg.CommerceEntity, commerceEntityComponent, language), context)
                    .ConfigureAwait(false);

                entityComponentLocalizationMapper.Execute(componentsPropertiesList, commerceEntityComponent,
                    language);
            }
        }

        private async Task<IList<LocalizablePropertyValues>> PerformEntityLocalization(ImportLocalizeContentArgument arg,
            CommercePipelineExecutionContext context, ILanguageEntity language, IList<LocalizablePropertyValues> entityLocalizableProperties)
        {
            if (arg.ImportEntityArgument.ImportHandler is IEntityLocalizationMapper mapper)
            {
                entityLocalizableProperties = mapper.Map(language, entityLocalizableProperties);
            }
            else
            {
                var entityLocalizationMapper = await this._commerceCommander
                    .Pipeline<IResolveEntityLocalizationMapperPipeline>()
                    .Run(new ResolveEntityLocalizationMapperArgument(arg.ImportEntityArgument, language),
                        context).ConfigureAwait(false);

                if (entityLocalizationMapper != null)
                {
                    entityLocalizableProperties = entityLocalizationMapper.Map(language, entityLocalizableProperties);
                }
            }

            return entityLocalizableProperties;
        }
    }
}