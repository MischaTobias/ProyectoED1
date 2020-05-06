
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
        [Display(Name = "Hospital")]
        public string HospitalName { get; set; }
        public List<string> Departments { get; set; }
        public PriorityQueue<PatientModel> InfectedQueue { get; set; }
        public PriorityQueue<PatientModel> SuspiciousQueue { get; set; }
        public string[] Bedcodearray = new string[10];
        [Display(Name = "Camas usadas")]
        public int BedsInUse { get; set; }
        public int Hospitalcode { get; set; }   

        public List<Bed> BedList { get; set; }
        public List<PatientModel> InfectedList { get; set; }
        public List<PatientModel> SuspiciousList { get; set; }

        public Hospital()
        {
            BedList = new List<Bed>();
            InfectedList = new List<PatientModel>();
            SuspiciousList = new List<PatientModel>();
        }

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
        public bool BedFull()
        {
            bool IsFull = false;
            foreach (var item in Bedcodearray)
            {
                if(Storage.Instance.BedHash.Search(item).Value.Availability == true)
                {
                    IsFull = true;
                }       
            }
            return IsFull;
        }
        public bool InfectedQueueFull()
        {
            return InfectedQueue.IsFull();
        }
        public bool SuspiciousQueueFull()
        {
            return SuspiciousQueue.IsFull();
        }       
    }
}