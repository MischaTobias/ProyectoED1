using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Bed : IComparable
    {
        [Display(Name = "Disponibilidad")]
        public string Availability { get; set; }
        public PatientStructure Patient { get; set; }

        public int CompareTo(object obj)
        {
            return this.Patient.CompareTo(((Bed)obj).Patient);
        }

        public void EmptyBed()
        {
            Patient = null;
            Availability = "Disponible";
        }
    }
}