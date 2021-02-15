using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using App.Repositories;

namespace App.Models
{
    public class Department: IEntity<int>
    {
        public int Id { get; set; }  
        public string Name { get; set; }  
        public virtual List<Employee> Employees { get; set; } = new List<Employee>();
    }
}