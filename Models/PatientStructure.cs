using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class PatientStructure : IComparable
    {
        public string CUI { get; set; }
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [Display(Name = "Edad")]
        public int Age { get; set; }
        public int Priority { get; set; }
        public string Hospital { get; set; }
        public string Status { get; set; }
        public bool IsInfected { get; set; }
        [Display(Name = "Fecha de ingreso")]
        public DateTime ArrivalDate { get; set; }

        public int CompareTo(object obj)
        {
            return this.CUI.CompareTo(((PatientStructure)obj).CUI);
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

        public static Comparison<PatientStructure> CompareByName = delegate (PatientStructure patient1, PatientStructure patient2)
        {
            return patient1.Name.CompareTo(patient2.Name);
        };

        public static Comparison<PatientStructure> CompareByLastName = delegate (PatientStructure patient1, PatientStructure patient2)
        {
            return patient1.LastName.CompareTo(patient2.LastName);
        };

        public static Comparison<PatientStructure> CompareByCUI = delegate (PatientStructure patient1, PatientStructure patient2)
        {
            return patient1.CUI.CompareTo(patient2.CUI);
        };

        public static Comparison<PatientStructure> CompareByPriority = delegate (PatientStructure patient1, PatientStructure patient2)
        {
            return patient1.Priority.CompareTo(patient2.Priority);
        }
    }
}