using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Bed : IComparable
    {
        /// <summary>
        /// Variable declaration
        /// </summary>
        [Display(Name = "Disponibilidad")] //Changes the item's name that's displayed in view
        public string Availability { get; set; }
        public PatientStructure Patient { get; set; }

        /// <summary>
        /// IComparable implementation, compares the bed patient.
        /// </summary>
        /// <param name="obj"></param> object received to compare
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return this.Patient.CompareTo(((Bed)obj).Patient);
        }

        /// <summary>
        /// Changes the bed's availability to "Disponible" and makes the patient's value null.
        /// </summary>
        public void EmptyBed()
        {
            Patient = null;
            Availability = "Disponible";
        }
    }
}