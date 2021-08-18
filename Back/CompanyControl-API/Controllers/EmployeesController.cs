using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using CompanyControl_API.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CompanyControl_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            var dbList = dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            int LastEmployeeId = dbClient.GetDatabase("testdb").GetCollection<Department>("Employee").AsQueryable().Count();
            employee.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").InsertOne(employee);

            return new JsonResult("Novo Empregado cadastrado");

        }
    }
}