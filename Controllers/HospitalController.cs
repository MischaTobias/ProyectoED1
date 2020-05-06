using ProyectoED1.Helpers;
using ProyectoED1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult PatientsList(int? page, string search, string criteria)
        {
            var patientsList = new List<PatientStructure>();
            int pageSize = 10;
            int pageNumber = page ?? 1;
            if (criteria == null && search != null)
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
            if (search != null)
            {
                switch (criteria)
                {
                    case "name":
                        patient.Name = search;
                        //list = Storage.Instance.PatientsByName.Search(patient, Storage.Instance.PatientsByName.Root, PatientStructure.CompareByName);
                        break;
                    case "lastname":
                        patient.LastName = search;
                        //list = Storage.Instance.PatientsByLastName.Search(patient, Storage.Instance.PatientsByLastName.Root, PatientStructure.CompareByLastName);
                            break;
                    case "cui":
                        patient.CUI = search;
                        //list = Storage.Instance.PatientsByCUI.Search(patient, Storage.Instance.PatientsByCUI.Root, PatientStructure.CompareByCUI);
                        break;
                }
            }
            else
            {
                foreach (var node in Storage.Instance.PatientsByName.GetList())
                {
                    list.Add(node.Patient);
                }
            }
            return list;
        }

        public ActionResult Statistics()
        {
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
            else
            {
                var patientCUI = hosp.SuspiciousQueue.GetFirst().Patient.CUI;
                //Storage.Instance.PatientsHash.Search(patientCUI).Value.InfectionTest();
            }
            return RedirectToAction("Hospital");
        }

        public ActionResult GetRecovered(Bed bed)
        {
            bed.Availability = true;
            //Cambiar el estado del paciente
            //Storage.Instance.PatientsByName();
            //Storage.Instance.PatientsByLastName();
            //Storage.Instance.PatientsByCUI();
            //Storage.Instance.PatientsHash();
            //Storage.Instance.BedHash();
            //Storage.Instance.Hospitals();
            return RedirectToAction("PatientsList");
        }
    }
}
