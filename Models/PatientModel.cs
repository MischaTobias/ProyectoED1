using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientModel : IComparable
    {
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "Departamento")]
        public string Department { get; set; }
        [Display(Name = "Municipalidad")]
        public string Municipality { get; set; }
        [Display(Name = "Síntomas")]
        public string Symptoms { get; set; } 
        [Display(Name = "Descripción del contagio")]
        public string InfectionDescription { get; set; }
        public bool IsInfected { get; set; }
        public int CUI { get; set; }
        [Display(Name = "Edad")]
        public int Age { get; set; }
        public int Priority { get; set; }

        public int CompareTo(object obj)
        {
            return this.Age.CompareTo(((PatientModel)obj).Age);
        }

        public void PriorityAssignment()
        {
            if (Age > 60)
            {
                if (IsInfected)
                {
                    Priority = 1;
                }
                else
                {
                    Priority = 4;
                }
            }
            else if (17 < Age && Age <= 60)
            {
                if (IsInfected)
                {
                    Priority = 3;
                }
                else
                {
                    Priority = 7;
                }
            }
            else if (1 < Age && Age <= 17)
            {
                if (IsInfected)
                {
                    Priority = 5;
                }
                else
                {
                    Priority = 8;
                }
            }
            else
            {
                if (IsInfected)
                {
                    Priority = 2;
                }
                else
                {
                    Priority = 6;
                }
            }
        }

        public static Comparison<PatientModel> CompareByName = delegate (PatientModel patient1, PatientModel patient2)
        {
            return patient1.Name.CompareTo(patient2.Name);
        };

        public static Comparison<PatientModel> CompareByLastName = delegate (PatientModel patient1, PatientModel patient2)
        {
            return patient1.LastName.CompareTo(patient2.LastName);
        };

        public static Comparison<PatientModel> CompareByCUI = delegate (PatientModel patient1, PatientModel patient2)
        {
            return patient1.CUI.CompareTo(patient2.CUI);
        };
    }
}