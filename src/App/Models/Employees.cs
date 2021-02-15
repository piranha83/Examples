using System.Text.Json.Serialization;
using System.Xml.Serialization;
using App.Repositories;

namespace App.Models
{
    public class Employee: IEntity<int> 
    {  
        public int Id { get; set; }
        public string FirstName { get; set; }  
        public string LastName { get; set; }  
        public decimal Salary { get; set; } 
        public string FullName => $"{FirstName} {LastName}";  

        [XmlIgnore, JsonIgnore]
        public virtual Department Department { get; set; }
        public int? DepartmentId { get; set; }  
    }  
}