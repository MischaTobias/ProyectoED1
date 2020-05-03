using ProyectoED1.Helpers;
using ProyectoED1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoED1.Controllers
{
    public class HospitalController : Controller
    {
        public bool FirstTime = true;

        public ActionResult Index()
        {
            if (FirstTime)
            {
                LoadHospitalsByDepartment();
                FirstTime = false;
            }
            return View();
        }

        private void LoadHospitalsByDepartment()
        {
            Storage.Instance.HCapital.Add("Guatemala");
            Storage.Instance.HCapital.Add("Sacatepéquez");
            Storage.Instance.HCapital.Add("Chimaltenango");
            Storage.Instance.HQuetzaltenango.Add("Quetzaltenango");
            Storage.Instance.HQuetzaltenango.Add("Totonicapán");
            Storage.Instance.HQuetzaltenango.Add("San Marcos");
            Storage.Instance.HQuetzaltenango.Add("Huehuetenango");
            Storage.Instance.HPeten.Add("Petén");
            Storage.Instance.HPeten.Add("Alta Verapaz");
            Storage.Instance.HPeten.Add("Baja verapaz");
            Storage.Instance.HPeten.Add("Sololá");
            Storage.Instance.HPeten.Add("Quiché");
            Storage.Instance.HEscuintla.Add("Escuintla");
            Storage.Instance.HEscuintla.Add("Santa Rosa");
            Storage.Instance.HEscuintla.Add("Jutiapa");
            Storage.Instance.HEscuintla.Add("Suchitepéquez");
            Storage.Instance.HEscuintla.Add("Retalhuleu");
            Storage.Instance.HOriente.Add("Izabal");
            Storage.Instance.HOriente.Add("Zacapa");
            Storage.Instance.HOriente.Add("Chiquimula");
            Storage.Instance.HOriente.Add("Jalapa");
            Storage.Instance.HOriente.Add("El Progreso");
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var option = collection["Option"];
            switch (option)
            {
                case "NewCase":
                    return RedirectToAction("NewCase");
                case "PatientsList":
                    break;
                case "Statistics":
                    break;
            }
            return View();
        }

        public ActionResult NewCase()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewCase(FormCollection collection)
        {
            try
            {
                var newPatient = new PatientModel()
                {
                    Name = collection["Name"],
                    LastName = collection["LastName"],
                    Department = collection["Department"],
                    Hospital = GetHospital(collection["Department"]),
                    Municipality = collection["Municipality"],
                    Symptoms = collection["Symptoms"],
                    CUI = int.Parse(collection["CUI"]),
                    Age = int.Parse(collection["Age"]),
                    InfectionDescription = collection["InfectionDescription"],
                    Status = "Sospechoso"
                };
                newPatient.PriorityAssignment();
                var structurePatient = new PatientStructure()
                {
                    Name = newPatient.Name,
                    LastName = newPatient.LastName,
                    CUI = newPatient.CUI,
                    Status = newPatient.Status,
                    Age = newPatient.Age,
                    ArrivalDate = newPatient.ArrivalDate,
                    Hospital = newPatient.Hospital,
                    IsInfected = newPatient.IsInfected,
                    Priority = newPatient.Priority
                };
                Storage.Instance.PatientsHash.Insert(newPatient, newPatient.CUI);
                //Generar paciente para AVL's
                //Storage.Instance.PatientsByName.add(patient);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("InfectionDescription", "Por favor asegúrese de haber llenado todos los campos correctamente.");
                return View("NewCase");
            }
        }

        private string GetHospital(string department)
        {
            if (Storage.Instance.HCapital.Contains(department))
            {
                return "Capital";
            }
            else if (Storage.Instance.HQuetzaltenango.Contains(department))
            {
                return "Quetzaltenango";
            }
            else if (Storage.Instance.HPeten.Contains(department))
            {
                return "Peten";
            }
            else if (Storage.Instance.HEscuintla.Contains(department))
            {
                return "Escuintla";
            }
            else if (Storage.Instance.HOriente.Contains(department))
            {
                return "Oriente";
            }
            else
            {
                return null;
            }
        }

        public ActionResult PatientsList()
        {
            var patientsList = new List<PatientModel>();
            foreach (var patient in Storage.Instance.PatientsHash.GetAsNodes())
            {
                patientsList.Add(patient.Value);
            }
            return View(patientsList);
        }
    }
}
