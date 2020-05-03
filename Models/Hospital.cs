using CustomGenerics.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoED1.Models
{
    public class Hospital
    {
        public List<string> Departments { get; set; }
        public PriorityQueue <PatientModel> InfectedPatients { get; set; }
        public PriorityQueue <PatientModel> SuspiciousPatients { get; set; }

        public void GetDepartments(string Hospital)
        {
            switch (Hospital)
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