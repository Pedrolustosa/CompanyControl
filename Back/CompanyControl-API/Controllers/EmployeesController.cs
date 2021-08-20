using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using CompanyControl_API.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace CompanyControl_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
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

            int LastEmployeeId = dbClient.GetDatabase("testDB").GetCollection<Department>("Employee").AsQueryable().Count();
            employee.EmployeeId = LastEmployeeId + 1;

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").InsertOne(employee);

            return new JsonResult("Added Successfully");

        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", employee.EmployeeId);

            var update = Builders<Employee>.Update.Set("EmployeeName", employee.EmployeeName)
                                                  .Set("Department", employee.Department)
                                                  .Set("DateOfJoining", employee.DateOfJoining)
                                                  .Set("PhotoFileName", employee.PhotoFileName);

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Departamento atualizado");

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeesAppCon"));

            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Departamento extinto");

        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}