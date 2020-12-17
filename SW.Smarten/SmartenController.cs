using Marten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.Smarten
{
    [ApiController]
    [Route("api/{EntityUrl}")]
    public class SmartenController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;
        private readonly SmartenOptions smartenOptions;

        [FromRoute]
        public string EntityUrl { get; set; }

        public SmartenController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            smartenOptions = serviceProvider.GetService<SmartenOptions>();
        }

        [HttpGet]
        async public Task<IActionResult> Search()
        {
            var result = await ResolveEntityAdapter(EntityUrl).Search(""); 
            return Ok(result);
        }

        [HttpGet("{key}")]
        async public Task<IActionResult> GetById([FromRoute] string key)
        {
            var result = await ResolveEntityAdapter(EntityUrl).GetById(key); 
            return Ok(result);
        }

        [HttpPost]
        async public Task<IActionResult> BulkSave([FromBody] object bulkData)
        {
            await ResolveEntityAdapter(EntityUrl).BulkSave(bulkData.ToString()); 
            return Ok();
        }

        private IEntityAdapter ResolveEntityAdapter(string entityUrl)
        {
            var entityAdapterType = smartenOptions.EntityDictionary[entityUrl];
            return (IEntityAdapter)serviceProvider.GetService(entityAdapterType);
        }
    }
}
