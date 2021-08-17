using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using CompanyControl_API.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;

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

        [HttpPost]
        public JsonResult Post(Department department)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            int lastDepartmentId = dbClient.GetDatabase("testDB").GetCollection<Department>("Department").AsQueryable().Count();
            department.DepartmentId = lastDepartmentId + 1;

            dbClient.GetDatabase("testDB").GetCollection<Department>("Department").InsertOne(department);

            return new JsonResult("Departamento criado");

        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            var filter = Builders<Department>.Filter.Eq("DepartmentId", department.DepartmentId);

            var update = Builders<Department>.Update.Set("DepartmentName", department.DepartmentName);

            dbClient.GetDatabase("testDB").GetCollection<Department>("Department").UpdateOne(filter, update);

            return new JsonResult("Departamento atualizado");

        }
    }
}