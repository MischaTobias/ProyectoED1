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

        public void SetInfectionChance(bool TraveltoEuropa, bool Knowninfected, bool Familiarinfected, bool ReunionWithSuspicious)
        {
            int InfectionChance = 5;
            if (TraveltoEuropa)
            {
                InfectionChance += 10;
            }
            if (Knowninfected)
            {
                InfectionChance += 15;
            }
            if (Familiarinfected)
            {
                InfectionChance += 30;
            }
            if (ReunionWithSuspicious)
            {
                InfectionChance += 5;
            }
        }

        public bool InfectionTest()
        {
            Random Rnd = new Random();
            if (Rnd.Next(100) <= InfectionChance)
            {
                IsInfected = true;
                Status = "Infected";
                return true;
            }
            else
            {
                Status = "NotInfected";
                Hospital = null;
                return false;
            }
        }
    }
}