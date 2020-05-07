using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientModel : PatientStructure
    {
        /// <summary>
        /// Variable Declaration
        /// </summary>
        [Display(Name = "Departamento")]
        public string Department { get; set; }
        [Display(Name = "Municipalidad")]
        public string Municipality { get; set; }
        [Display(Name = "Síntomas")]
        public string Symptoms { get; set; } 
        [Display(Name = "Descripción del contagio")]
        public string InfectionDescription { get; set; }
        public int InfectionChance { get; set; }

        /// <summary>
        /// Calculates the infection chance the patient has according to the percentages given.
        /// </summary>
        /// <param name="TraveltoEuropa"></param> represents if the patient has traveled to Europe
        /// <param name="Knowninfected"></param> represents if the patient knows someone that's infected
        /// <param name="Familiarinfected"></param> represents if the patient has a sibling that's infected
        /// <param name="ReunionWithSuspicious"></param> represents if the patient has had a meeting with a suspicios person.
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

        /// <summary>
        /// Generates a random number, and then returns a value and changes the patient's status 
        /// according to the random result. 
        /// </summary>
        /// <returns></returns>
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