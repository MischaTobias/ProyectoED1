using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientStructure
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int CUI { get; set; }
        public string Hospital { get; set; }
        public string Status { get; set; }
        public bool IsInfected { get; set; }
    }
}