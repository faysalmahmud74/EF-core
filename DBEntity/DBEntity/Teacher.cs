using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntity
{
    internal class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public string Nationality { get; set; }
        public DateTime JoinDate { get; set; }
        public ICollection<AssignedClass> Classes { get; set; }
    }
}
