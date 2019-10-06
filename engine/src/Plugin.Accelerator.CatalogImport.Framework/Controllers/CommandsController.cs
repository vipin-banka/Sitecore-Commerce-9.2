using Newtonsoft.Json;
using Plugin.Accelerator.CatalogImport.Framework.Model;

namespace Plugin.Accelerator.CatalogImport.Framework.Controllers
{
    using Commands;
    using Microsoft.AspNetCore.Mvc;
    using Sitecore.Commerce.Core;
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.OData;

    public class CommandsController : CommerceController
    {
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [HttpPut]
        [Route("ImportEntity()")]
        public async Task<IActionResult> ImportEntity([FromBody] ODataActionParameters value)
        {
            if (!this.ModelState.IsValid || value == null)
                return new BadRequestObjectResult(this.ModelState);

            if (!value.ContainsKey("metadata") || value["metadata"] == null)
                return new BadRequestObjectResult(value);

            if (!value.ContainsKey("entity") || value["entity"] == null)
                return new BadRequestObjectResult(value);

            var metadata = value["metadata"].ToString();
            var entity = value["entity"].ToString();
            var sourceEntity = JsonConvert.DeserializeObject<SourceEntityDetail>(metadata);
            sourceEntity.SerializedEntity = entity;

            var command = this.Command<ImportEntityCommand>();
            var commerceEntity = await command.Process(this.CurrentContext, sourceEntity);
            return new ObjectResult(command);
        }
    }
}