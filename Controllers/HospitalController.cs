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
            Storage.Instance.HCapital.GetDepartments("Capital");
            Storage.Instance.HQuetzaltenango.GetDepartments("Quetzaltenango");
            Storage.Instance.HPeten.GetDepartments("Peten");
            Storage.Instance.HEscuintla.GetDepartments("Escuintla");
            Storage.Instance.HOriente.GetDepartments("Oriente");
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
                foreach (var patient in Storage.Instance.PatientsHash.GetAsNodes())
                {
                    if (patient.Value.CUI == int.Parse(collection["CUI"]))
                    {
                        ModelState.AddModelError("CUI", "Un paciente con el mismo dpi ya ha sido ingresado en el sistema. Ingrese otro paciente.");
                        return View("NewCase");
                    }
                }
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
                    IsInfected = false,
                    ArrivalDate = DateTime.Parse(collection["ArrivalDate"]),
                    Status = "Sospechoso"
                };
                newPatient.PriorityAssignment();
                var structurePatient = new PatientStructure()
                {
                    Name = newPatient.Name,
                    LastName = newPatient.LastName,
                    Hospital = newPatient.Hospital,
                    CUI = newPatient.CUI,
                    Age = newPatient.Age,
                    IsInfected = newPatient.IsInfected,
                    ArrivalDate = newPatient.ArrivalDate,
                    Priority = newPatient.Priority,
                    Status = newPatient.Status
                };
                Storage.Instance.PatientsHash.Insert(newPatient, newPatient.CUI);
                Storage.Instance.PatientsByName.AddPatient(structurePatient, PatientStructure.CompareByName);
                Storage.Instance.PatientsByLastName.AddPatient(structurePatient, PatientStructure.CompareByLastName);
                Storage.Instance.PatientsByCUI.AddPatient(structurePatient, PatientStructure.CompareByCUI);
                if (Storage.Instance.CountryStatistics.Suspicious == null)
                {
                    Storage.Instance.CountryStatistics.Suspicious = 0;
                }
                Storage.Instance.CountryStatistics.Suspicious++;
                SendToHospital(structurePatient);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("InfectionDescription", "Por favor asegúrese de haber llenado todos los campos correctamente.");
                return View("NewCase");
            }
        }

        private void SendToHospital(PatientStructure patient)
        {
            switch (patient.Hospital)
            {
                case "Capital":
                    Storage.Instance.HCapital.SuspiciousPatients.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                    break;
                case "Quetzaltenango":
                    Storage.Instance.HQuetzaltenango.SuspiciousPatients.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                    break;
                case "Peten":
                    Storage.Instance.HPeten.SuspiciousPatients.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                    break;
                case "Escuintla":
                    Storage.Instance.HEscuintla.SuspiciousPatients.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                    break;
                case "Oriente":
                    Storage.Instance.HOriente.SuspiciousPatients.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                    break;
            }
        }

        private string GetHospital(string department)
        {
            if (Storage.Instance.HCapital.Departments.Contains(department))
            {
                return "Capital";
            }
            else if (Storage.Instance.HQuetzaltenango.Departments.Contains(department))
            {
                return "Quetzaltenango";
            }
            else if (Storage.Instance.HPeten.Departments.Contains(department))
            {
                return "Peten";
            }
            else if (Storage.Instance.HEscuintla.Departments.Contains(department))
            {
                return "Escuintla";
            }
            else if (Storage.Instance.HOriente.Departments.Contains(department))
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

        public ActionResult Statitics()
        {
            return View();
        }
    }
}
