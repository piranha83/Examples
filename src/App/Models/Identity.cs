using System.Text.Json.Serialization;
using System.Xml.Serialization;
using App.Repositories;

namespace App.Models
{
    public class Identity
    {  
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }  
}