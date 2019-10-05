//using Plugin.Belden.Catalog.Associations.Commands;
//using Plugin.Belden.Catalog.Associations.Models;
using Plugin.Accelerator.CatalogImport.Sample.Pipelines.Arguments;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Framework.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Accelerator.CatalogImport.Sample.Pipelines.Blocks
{
    [PipelineDisplayName(Constants.AssociateParentsBlock)]
    public class AssociateParentsBlock : PipelineBlock<AssociateParentsArgument, AssociateParentsArgument, CommercePipelineExecutionContext>
    {
        private readonly AssociateSellableItemToParentCommand _associateSellableItemToParent;
        private readonly IDoesEntityExistPipeline _doesEntityExistPipeline;
        private readonly CommerceCommander _commerceCommander;

        public AssociateParentsBlock(CommerceCommander commerceCommander,
            AssociateSellableItemToParentCommand associateSellableItemToParent,
            IDoesEntityExistPipeline doesEntityExistPipeline)
        {
            this._commerceCommander = commerceCommander;
            this._associateSellableItemToParent = associateSellableItemToParent;
            this._doesEntityExistPipeline = doesEntityExistPipeline;
        }

        public override async Task<AssociateParentsArgument> Run(AssociateParentsArgument arg, CommercePipelineExecutionContext context)
        {
            //string sellableItemId = arg.EntityId;

            //var entityReferences = new List<VersionEntityReference>();
            //if (arg.Product.Parents != null)
            //{
            //    IDictionary<string, IList<string>> entityAssociations = await GetAssociatedEntities(arg.Product.Parents, context).ConfigureAwait(false);

            //    foreach (var catalog in entityAssociations)
            //    {
            //        if (catalog.Value == null || !catalog.Value.Any())
            //        {
            //            var r = await this._associateSellableItemToParent
            //            .Process(context.CommerceContext, catalog.Key, catalog.Key, sellableItemId).ConfigureAwait(false);
            //            var relationshipArgument = context.CommerceContext.GetObject<RelationshipArgument>();
            //            entityReferences.Add(new VersionEntityReference(catalog.Key, null, relationshipArgument.RelationshipType));
            //            context.CommerceContext.RemoveObject(relationshipArgument);
            //        }
            //        else
            //        {
            //            foreach (var parentId in catalog.Value)
            //            {
            //                var r = await this._associateSellableItemToParent
            //                .Process(context.CommerceContext, catalog.Key, parentId, sellableItemId).ConfigureAwait(false);
            //                var relationshipArgument = context.CommerceContext.GetObject<RelationshipArgument>();
            //                entityReferences.Add(new VersionEntityReference(parentId, null, relationshipArgument.RelationshipType));
            //                context.CommerceContext.RemoveObject(relationshipArgument);
            //            }
            //        }
            //    }

            //    if (entityReferences.Any())
            //    {
            //        await this._commerceCommander.Command<RefreshAssociationCommand>().Process(context.CommerceContext, arg.EntityId, entityReferences).ConfigureAwait(false);
            //    }
            //}

            return await Task.FromResult(arg);
        }

        private async Task<IDictionary<string, IList<string>>> GetAssociatedEntities(IList<string> parents, CommercePipelineExecutionContext context)
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