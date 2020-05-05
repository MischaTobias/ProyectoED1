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
        public bool Availability { get; set; }
        public int Bedcode { get; set; }
        public PatientStructure Patient { get; set; }
        public string Hospital { get; set; }

        public int CompareTo(object obj)
        {
            return this.Bedcode.CompareTo(((Bed)obj).Bedcode);
        }

        public void EmptyBed()
        {
            Patient = null;
            Availability = true;
        }
    }
}