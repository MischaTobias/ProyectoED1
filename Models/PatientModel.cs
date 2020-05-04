using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientModel : PatientStructure
    {
        [Display(Name = "Departamento")]
        public string Department { get; set; }
        [Display(Name = "Municipalidad")]
        public string Municipality { get; set; }
        [Display(Name = "Síntomas")]
        public string Symptoms { get; set; } 
        [Display(Name = "Descripción del contagio")]
        public string InfectionDescription { get; set; }
        public int InfectionChance { get; set; }

        public void InfectionTest()
        {
            Random Rnd = new Random();
            if (Rnd.Next(100) >= InfectionChance)
            {
                IsInfected = true;
                Status = "Infected";
            }
            else
            {
                Status = "NotInfected";
            }
        }
    }
}