using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using CompanyControl_API.Models;
using Microsoft.Extensions.Configuration;


namespace CompanyControl_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            var dbList = dbClient.GetDatabase("testDB").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dbList);
        }
    }
}