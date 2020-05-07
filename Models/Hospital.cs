
using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProyectoED1.Helpers;

namespace ProyectoED1.Models
{
    public class Hospital
    {
        /// <summary>
        /// Variable declaration
        /// </summary>
        [Display(Name = "Hospital")]
        public string HospitalName { get; set; }
        public List<string> Departments { get; set; }
        public PriorityQueue<PatientStructure> InfectedQueue { get; set; }
        public PriorityQueue<PatientStructure> SuspiciousQueue { get; set; }
        public string[] Bedcodearray = new string[10];
        [Display(Name = "Camas usadas")]
        public int BedsInUse { get; set; } 
        public List<Bed> BedList { get; set; }

        /// <summary>
        /// Constructor that initiates the bedlist
        /// </summary>
        public Hospital()
        {
            BedList = new List<Bed>();
        }

        /// <summary>
        /// Fills the Department's list according to the hospital's name
        /// </summary>
        public void GetDepartments()
        {
            Departments = new List<string>();
            switch (HospitalName)
            {
                case "Capital":
                    Departments.Add("Guatemala");
                    Departments.Add("Sacatepéquez");
                    Departments.Add("Chimaltenango");
                    break;
                case "Quetzaltenango":
                    Departments.Add("Quetzaltenango");
                    Departments.Add("Totonicapán");
                    Departments.Add("San Marcos");
                    Departments.Add("Huehuetenango");
                    break;
                case "Petén":
                    Departments.Add("Petén");
                    Departments.Add("Alta Verapaz");
                    Departments.Add("Baja verapaz");
                    Departments.Add("Sololá");
                    Departments.Add("Quiché");
                    break;
                case "Escuintla":
                    Departments.Add("Escuintla");
                    Departments.Add("Santa Rosa");
                    Departments.Add("Jutiapa");
                    Departments.Add("Suchitepéquez");
                    Departments.Add("Retalhuleu");
                    break;
                case "Oriente":
                    Departments.Add("Izabal");
                    Departments.Add("Zacapa");
                    Departments.Add("Chiquimula");
                    Departments.Add("Jalapa");
                    Departments.Add("El Progreso");
                    break;
            }
        }

        /// <summary>
        /// If the bedshash is full, returns true, if not, returns false.
        /// </summary>
        /// <returns></returns>
        public bool BedFull()
        {
            if (BedsInUse == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns PriorityQueue.IsFull();
        /// </summary>
        /// <returns></returns>
        public bool InfectedQueueFull()
        {
            return InfectedQueue.IsFull();
        }

        /// <summary>
        /// Returns PriorityQueue.IsFull();
        /// </summary>
        /// <returns></returns>
        public bool SuspiciousQueueFull()
        {
            return SuspiciousQueue.IsFull();
        }       
    }
}