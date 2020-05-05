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
            AddHospital("Capital");
            AddHospital("Quetzaltenango");
            AddHospital("Peten");
            AddHospital("Escuintla");
            AddHospital("Oriente");
        }

        private void AddHospital(string hospital)
        {
            var newHospital = new Hospital()
            {
                HospitalName = hospital,
                BedsInUse = 0,
                InfectedQueue = new CustomGenerics.Structures.PriorityQueue<PatientModel>(),
                SuspiciousQueue = new CustomGenerics.Structures.PriorityQueue<PatientModel>()
            };
            newHospital.GetDepartments();
            Storage.Instance.Hospitals.Add(newHospital);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var option = collection["Option"];
            switch (option)
            {
                case "NewCase":
                    return RedirectToAction("NewCase");
                case "HospitalsList":
                    return RedirectToAction("HospitalsList");
                case "PatientsList":
                    return RedirectToAction("PatientsList");
                case "Statistics":
                    return RedirectToAction("Statistics");
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
            foreach (var hospital in Storage.Instance.Hospitals)
            {
                if (hospital.HospitalName == patient.Hospital)
                {
                    hospital.SuspiciousQueue.AddPatient(patient.CUI, patient.ArrivalDate, patient.Priority);
                }
            }
        }

        private string GetHospital(string department)
        {
            foreach (var hospital in Storage.Instance.Hospitals)
            {
                if (hospital.Departments.Contains(department))
                {
                    return hospital.HospitalName;
                }
            }
            return null;
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
            return View(Storage.Instance.CountryStatistics);
        }

        public ActionResult HospitalsList()
        {
            return View(Storage.Instance.Hospitals);
        } 

        public ActionResult Hospital(string name)
        {
            var showHospital = Storage.Instance.Hospitals.First();
            foreach (var hospital in Storage.Instance.Hospitals)
            {
                if (hospital.HospitalName == name)
                {
                    showHospital = hospital;
                }
            }
            return View(showHospital);
        }
    }
}
