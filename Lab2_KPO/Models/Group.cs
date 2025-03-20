using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_KPO.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
