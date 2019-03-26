using System;
using System.Collections.Generic;
using System.Text;

namespace Journal_RGR.Models
{
    public class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public int Type { get; set; }
        public int Semester { get; set; }
    }
}
