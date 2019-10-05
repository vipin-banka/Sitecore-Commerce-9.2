namespace Plugin.Accelerator.CatalogImport.Sample.Controllers
{
    using Commands;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
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
        [Route("ImportProduct()")]
        public async Task<IActionResult> ImportProduct([FromBody] ODataActionParameters value)
        {
            if (!this.ModelState.IsValid || value == null)
                return new BadRequestObjectResult(this.ModelState);

            if (!value.ContainsKey("product") || value["product"] == null)
                return new BadRequestObjectResult(value);

            var product = JsonConvert.DeserializeObject<Entity.Product>(value["product"].ToString());

            if (product == null)
                return new BadRequestObjectResult(value);

            if (product == null || string.IsNullOrEmpty(product.Id))
                product.Id = Guid.NewGuid().ToString("N");

            var command = this.Command<ImportProductCommand>();
            var sellableItem = await command.Process(this.CurrentContext, product);
            return new ObjectResult(command);
        }
    }
}