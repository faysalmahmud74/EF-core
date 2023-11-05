using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntity
{
    internal class AssignedClass
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }

        public Teacher Teacher { get; set; }
        public Student Student { get; set; }
        public Class Class { get; set; }

    }
}

