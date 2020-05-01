using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Hospital
    {
        public PriorityQueue <PatientModel> InfectedPatients { get; set; }
        public PriorityQueue <PatientModel> SuspiciousPatients { get; set; }
    }
}