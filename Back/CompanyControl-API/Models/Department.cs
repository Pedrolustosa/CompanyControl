using MongoDB.Bson;

namespace CompanyControl_API.Models
{
    public class Department
    {
        public ObjectId Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}