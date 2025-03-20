using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_KPO.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }
}
