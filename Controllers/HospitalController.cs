using ProyectoED1.Helpers;
using ProyectoED1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web;
using System.Web.Mvc;
using CustomGenerics.Structures;

namespace ProyectoED1.Controllers
{
    public class HospitalController : Controller
    {
        public static bool FirstTime = true;

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
            AddHospital("Petén");
            AddHospital("Escuintla");
            AddHospital("Oriente");
        }

        private void AddHospital(string hospital)
        {
            var newHospital = new Hospital()
            {
                HospitalName = hospital,
                BedsInUse = 0,
                InfectedQueue = new CustomGenerics.Structures.PriorityQueue<PatientStructure>(),
                SuspiciousQueue = new CustomGenerics.Structures.PriorityQueue<PatientStructure>()
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
                    if (patient.Value.CUI == collection["CUI"])
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
                    CUI = collection["CUI"],
                    Age = int.Parse(collection["Age"]),
                    InfectionDescription = collection["InfectionDescription"],
                    IsInfected = false,
                    ArrivalDate = DateTime.Parse(collection["ArrivalDate"]),
                    Status = "Sospechoso"
                };
                newPatient.SetInfectionChance(GetBool(collection["Europe"]), GetBool(collection["InfectedSibling"]), GetBool(collection["Socialmeeting"]), GetBool(collection["InfectedFriend"]));
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

        private bool GetBool(string data)
        {
            if (data == "true,false")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SendToHospital(PatientStructure patient)
        {
            var hospital = Storage.Instance.Hospitals.First(x => x.HospitalName == patient.Hospital);
            hospital.SuspiciousQueue.AddPatient(patient.CUI, patient.ArrivalDate, patient, patient.Priority);
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

        public ActionResult PatientsList(int? page, string search, string criteria)
        {
            var patientsList = GetPatients(null, null);
            int pageSize = 10;
            int pageNumber = page ?? 1;
            if (criteria == "Seleccionar Criterio" && search != "")
            {
                TempData["Error"] = "Por favor escoja un criterio de búsqueda.";
                return View(patientsList.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                patientsList = GetPatients(search, criteria);
            }
            return View(patientsList.ToPagedList(pageNumber, pageSize));
        }

        private List<PatientStructure> GetPatients(string search, string criteria)
        {
            var list = new List<PatientStructure>();
            var patient = new PatientStructure();
            if (search != null && search != "")
            {
                switch (criteria)
                {
                    case "Nombre":
                        patient.Name = search;
                        list = Storage.Instance.PatientsByName.Search(patient, Storage.Instance.PatientsByName.Root, PatientStructure.CompareByName);
                        break;
                    case "Apellido":
                        patient.LastName = search;
                        list = Storage.Instance.PatientsByLastName.Search(patient, Storage.Instance.PatientsByLastName.Root, PatientStructure.CompareByLastName);
                            break;
                    case "CUI/No. DPI":
                        patient.CUI = search;
                        list = Storage.Instance.PatientsByCUI.Search(patient, Storage.Instance.PatientsByCUI.Root, PatientStructure.CompareByCUI);
                        break;
                }
            }
            else
            {
                foreach (var node in Storage.Instance.PatientsByCUI.GetList())
                {
                    list.Add(node.Patient);
                }
            }
            return list;
        }

        public ActionResult Statistics()
        {
            Storage.Instance.CountryStatistics.GetPercentage();
            if (Storage.Instance.CountryStatistics != null)
            {
                return View(Storage.Instance.CountryStatistics);
            }
            else
            {
                return View(new Statistics());
            }
        }

        public ActionResult HospitalsList()
        {
            return View(Storage.Instance.Hospitals);
        } 

        public ActionResult Hospital(string name, string advice)
        {
            var showHospital = Storage.Instance.Hospitals.Find(x => x.HospitalName == name);
            if (advice != "")
            {
                TempData["Error"] = advice;
            }
            return View(showHospital);
        }

        public ActionResult Test(string hospital)
        {
            var hosp = Storage.Instance.Hospitals.Find(x => x.HospitalName == hospital);
            if (hosp.InfectedQueueFull())
            {
                return RedirectToAction("Hospital", new { name = hosp.HospitalName, testmade = "La cola de infectados está llena, por favor libere una cama antes de continuar." });
            }
            else if (hosp.InfectedQueue.Root != null)
            {
                if (hosp.InfectedQueue.Root.Patient.Priority < hosp.SuspiciousQueue.Root.Patient.Priority)
                {
                    return RedirectToAction("Hospital", new { name = hosp.HospitalName, testmade = "Hay un paciente que necesita ser atendido antes de que realice más pruebas de COVID-19." });
                }
            }
            else
            {
                var patient = hosp.SuspiciousQueue.GetFirst().Patient;
                var infected = Storage.Instance.PatientsHash.Search(patient.CUI).Value.InfectionTest();
                if (infected)
                {
                    Storage.Instance.CountryStatistics.Suspicious--;
                    Storage.Instance.CountryStatistics.Infected++;
                    Storage.Instance.BedHash.Insert(new Bed() { Patient = patient }, patient.CUI, GetMultiplier(patient));
                }
                else
                {

                }
            }
            return RedirectToAction("Hospital");
        }

        public ActionResult GetRecovered(Bed bed)
        {
            Storage.Instance.BedHash.Delete(bed.Patient.CUI, GetMultiplier(bed.Patient));
            bed.Availability = "Disponible";
            //Cambiar el estado del paciente
            //Storage.Instance.PatientsByName();
            //Storage.Instance.PatientsByLastName();
            //Storage.Instance.PatientsByCUI();
            //Storage.Instance.PatientsHash();
            //Storage.Instance.BedHash();
            //Storage.Instance.Hospitals();
            return RedirectToAction("PatientsList");
        }

        private int GetMultiplier(PatientStructure patient)
        {
            switch (patient.Hospital)
            {
                case "Capital":
                    return 0;
                case "Quetzaltenango":
                    return 1;
                case "Petén":
                    return 2;
                case "Escuintla":
                    return 3;
                case "Oriente":
                    return 4;
            }
            return -1;
        }
    }
}
