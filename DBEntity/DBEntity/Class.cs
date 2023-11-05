using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntity
{
    internal class Class
    {
        public int ClassId { get; set; }
        public string Description { get; set; }
        public ICollection<AssignedClass> Classes { get; set; }

    }
}
