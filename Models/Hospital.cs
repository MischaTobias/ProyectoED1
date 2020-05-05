using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Hospital
    {
        [Display(Name = "Hospital")]
        public string HospitalName { get; set; }
        public List<string> Departments { get; set; }
        public PriorityQueue<PatientModel> InfectedQueue { get; set; }
        public PriorityQueue<PatientModel> SuspiciousQueue { get; set; }
        public int[] Bedcodearray = new int[10];
        public int BedsInUse { get; set; }

        public List<Bed> BedList { get; set; }
        public List<PatientModel> InfectedList { get; set; }
        public List<PatientModel> SuspiciousList { get; set; }

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
                case "Peten":
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
    }
}