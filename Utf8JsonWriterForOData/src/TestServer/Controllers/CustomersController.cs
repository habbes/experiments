using ExperimentsLib;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestServer.Controllers
{
    //[Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ILogger<CustomersController> logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            this.logger = logger;
        }
        // GET: api/<CustomersController>
        [HttpGet("customers/{writer}")]
        public IEnumerable<Customer> Get([FromQuery] int? count)
        {
            var data = CustomerDataSet.GetCustomers(count ?? 100);
            logger.LogInformation($"Retrieve {count} Customer entities from datasets.");
            return data;
        }
    }
}
