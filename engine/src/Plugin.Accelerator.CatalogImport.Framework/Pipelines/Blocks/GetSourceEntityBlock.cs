using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.GetSourceEntityBlock)]
    public class GetSourceEntityBlock : PipelineBlock<ImportEntityArgument, ImportEntityArgument, CommercePipelineExecutionContext>
    {
        private CommerceCommander _commerceCommander;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;

        public GetSourceEntityBlock(
            CommerceCommander commerceCommander,
            IDoesEntityExistPipeline doesEntityExistPipeline)
        {
            this._commerceCommander = commerceCommander;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
        }

        public override async Task<ImportEntityArgument> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            arg.SourceEntity = arg.ImportHandler.GetSourceEntity();
            if (arg.SourceEntity == null)
            {
                context.Abort(await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "SourceEntityMissing", null, $"Source entity missing for entityType={arg.SourceEntityDetail.EntityType}."), context);
            }

            var parentList = arg.ImportHandler.GetParentList();
            
            if (parentList != null && parentList.Any())
            {
                arg.ImportHandler.ParentEntityIds = await GetParentEntities(parentList, context)
                    .ConfigureAwait(false);
            }

            return arg;
        }

        private async Task<IDictionary<string, IList<string>>> GetParentEntities(IList<string> parents, CommercePipelineExecutionContext context)
        {
            var result = new Dictionary<string, IList<string>>();

            var separators = new List<string> { "/" }.ToArray();
            foreach (var parentItem in parents)
            {
                var names = parentItem.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                var catalogId = await GetCatalogId(names[0], context);
                if (!string.IsNullOrEmpty(catalogId))
                {
                    if (!result.ContainsKey(catalogId))
                    {
                        result.Add(catalogId, new List<string>());
                    }

                    var categoryId = await GetCategoryId(catalogId, names[names.Length - 1], context);
                    if (!string.IsNullOrEmpty(categoryId))
                    {
                        if (!result[catalogId].Contains(categoryId))
                        {
                            result[catalogId].Add(categoryId);
                        }
                    }
                }
            }

            return result;
        }

        private async Task<string> GetCatalogId(string catalogName, CommercePipelineExecutionContext context)
        {
            var result = string.Empty;
            var catalogId = catalogName.ToEntityId<Sitecore.Commerce.Plugin.Catalog.Catalog>();
            result = await this.ValidateEntityId(typeof(Sitecore.Commerce.Plugin.Catalog.Catalog),
                catalogId, context);
            return await Task.FromResult(result);
        }

        private async Task<string> GetCategoryId(string catalogId, string categoryName, CommercePipelineExecutionContext context)
        {
            var result = string.Empty;
            var categoryId = categoryName.ToCategoryId(catalogId.SimplifyEntityName());
            result = await this.ValidateEntityId(typeof(Sitecore.Commerce.Plugin.Catalog.Category),
                categoryId, context);
            return await Task.FromResult(result);
        }

        private async Task<string> ValidateEntityId(Type entityType, string entityId, CommercePipelineExecutionContext context)
        {
            var result = string.Empty;
            var entityExists = await this._doesEntityExistPipeline.Run(new FindEntityArgument(entityType,
                entityId,
                null,
                false), context)
                .ConfigureAwait(false);

            if (entityExists)
            {
                result = entityId;
            }

            return await Task.FromResult(result);
        }
    }
}