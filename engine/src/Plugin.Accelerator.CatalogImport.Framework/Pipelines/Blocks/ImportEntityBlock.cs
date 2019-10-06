using System;
using System.Collections.Concurrent;
using Plugin.Accelerator.CatalogImport.Framework.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Accelerator.CatalogImport.Framework.Policy;
using CommerceEntity = Sitecore.Commerce.Core.CommerceEntity;

namespace Plugin.Accelerator.CatalogImport.Framework.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.ImportEntityBlock)]
    public class ImportEntityBlock : PipelineBlock<ImportEntityArgument, CommerceEntity, CommercePipelineExecutionContext>
    {
        private readonly FindEntityCommand _findEntityCommand;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly ISetComponentsPipeline _setComponentsPipeline;
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IServiceProvider _serviceProvider;

        public ImportEntityBlock(
            IServiceProvider serviceProvider,
            FindEntityCommand findEntityCommand,
            IDoesEntityExistPipeline doesEntityExistPipeline,
            ISetComponentsPipeline setComponentsPipeline,
            IPersistEntityPipeline persistEntityPipeline)
        {
            this._serviceProvider = serviceProvider;
            this._findEntityCommand = findEntityCommand;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
            this._setComponentsPipeline = setComponentsPipeline;
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<CommerceEntity> Run(ImportEntityArgument arg, CommercePipelineExecutionContext context)
        {
            context.CommerceContext.AddUniqueObject(arg);

            var catalogImportPolicy = context.CommerceContext.GetPolicy<CatalogImportPolicy>();

            if (catalogImportPolicy == null)
                return null;

            var importHandler = catalogImportPolicy.Mappings.ImportHandler(arg);

            if (importHandler == null)
                return null;

            arg.ImportHandler = importHandler;
            arg.SourceEntity = importHandler.GetSourceEntity();
            if (arg.SourceEntity == null)
                return null;

            var parentList = importHandler.GetParentList();
            
            if (parentList != null && parentList.Any())
            {
                arg.Parents = await GetParentEntities(importHandler.GetParentList(), context).ConfigureAwait(false);
            }

            string entityId = importHandler.EntityId;
            var entity = await this._findEntityCommand.Process(context.CommerceContext, typeof(CommerceEntity), entityId)
                .ConfigureAwait(false);

            arg.CommerceEntity = entity;

            if (entity == null)
            {
                arg.IsNew = true;
                // run create 
                entity = await importHandler.Create(this._serviceProvider, arg.Parents, context).ConfigureAwait(false);

                arg.CommerceEntity = entity;
            }
            else
            {
                arg.IsNew = false;
                // run update
                await this._setComponentsPipeline.Run(entity, context)
                    .ConfigureAwait(false);

                await this._persistEntityPipeline.Run(new PersistEntityArgument(entity), context)
                    .ConfigureAwait(false);
            }

            return await Task.FromResult(entity);
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