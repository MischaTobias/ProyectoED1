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
        /// <summary>
        /// Variable that indicates if it's the first time they log in to index.
        /// </summary>
        public static bool FirstTime = true;

        /// <summary>
        /// Procedure that excecutes LoadHospitalsByDepartment() if it's the first time, and loads view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (FirstTime)
            {
                LoadHospitalsByDepartment();
                FirstTime = false;
            }
            return View();
        }

        /// <summary>
        /// Procedure that Excecutes AddHospital() for each of the five hospitals in Guatemala.
        /// </summary>
        private void LoadHospitalsByDepartment()
        {
            AddHospital("Capital");
            AddHospital("Quetzaltenango");
            AddHospital("Petén");
            AddHospital("Escuintla");
            AddHospital("Oriente");
        }

        /// <summary>
        /// Procedure that creates a new hospital with the name that's received as a parameter and
        /// initializes its queues and calls GetDepartments().
        /// Finally, it enqueues the new hospital into a hospital list in Storage.
        /// </summary>
        /// <param name="hospital"></param> string that indicates hospital's name
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

        /// <summary>
        /// ActionResult that redirects to a different action depending on the option the user chose
        /// through the buttons.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns NewCase View
        /// </summary>
        /// <returns></returns>
        public ActionResult NewCase()
        {
            return View();
        }

        /// <summary>
        /// Creates a new patient, validating that all data is correct and that the CUI isn't the same.
        /// After that, it adds the patient to the patients hash, and to the AVL's according to its criteria.
        /// Finally, it sends the patient to SendToHospital(), actualizes the country statistics and 
        /// returns the user to the Index menu.
        /// If the user input has errors, it shows an advice of how to fix the error.
        /// </summary>
        /// <param name="collection"></param> collection obtained from the create view.
        /// <returns></returns>
        [HttpPost]
        public ActionResult NewCase(FormCollection collection)
        {
            try
            {
                if (int.Parse(collection["Age"]) < 0)
                {
                    ModelState.AddModelError("Age", "Por favor ingrese una edad válida");
                }
                else if (collection["Department"] == "Seleccionar Departamento")
                {
                    ModelState.AddModelError("Department", "Por favor seleccione un departamento");
                }
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

        /// <summary>
        /// Returns boolean according to a string parameter.
        /// </summary>
        /// <param name="data"></param>string gotten from view checkboxes.
        /// <returns></returns>
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

        /// <summary>
        /// Gets from the hospital's list, the one to which the patient has been assigned and enqueues the patient into the
        /// hospital's suspicious list.
        /// </summary>
        /// <param name="patient"></param> PatientStructure object that is going to be enqueued.
        private void SendToHospital(PatientStructure patient)
        {
            var hospital = Storage.Instance.Hospitals.First(x => x.HospitalName == patient.Hospital);
            hospital.SuspiciousQueue.AddPatient(patient.CUI, patient.ArrivalDate, patient, patient.Priority);
        }

        /// <summary>
        /// Returns an hospital according to the list of departments each hospital in the hospital's list has.
        /// If the department isn't contained in any, it returns a null value.
        /// </summary>
        /// <param name="department"></param> string that represents the department in which the patient lives.
        /// <returns></returns>
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

        /// <summary>
        /// ActionResult that shows the general patients list in a pagedlist.
        /// Also, if a correct parameter is received, it calls GetPatients().
        /// If the user's input has an error, it shows an advice in the view.
        /// </summary>
        /// <param name="page"></param> Number of page in pagedlist.
        /// <param name="search"></param> Searched value in the patients information.
        /// <param name="criteria"></param> Criteria chosen to search the value.
        /// <returns></returns>
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

        /// <summary>
        /// If search has a value, it calls the AVL.Search() to obtain the values that contain the searched value.
        /// If it doesn't have a value, it returns the entire patient list obtained from the AVL ordered by patient's CUI.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Calls Statistics.GetPercentage() and shows the country statistics in a view.
        /// If countrystatistics is null, it shows a null value.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Shows the hospital's list.
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalsList()
        {
            return View(Storage.Instance.Hospitals);
        } 

        /// <summary>
        /// If advice has a value, it shows an advice on the view.
        /// It shows the hospitals data in the view. This is data such as bedslist, suspiciousqueue and infectedqueue.
        /// Also, in order to show the hospital queues, generates a list copy for each queue.
        /// </summary>
        /// <param name="name"></param> string that contains the hospital's name.
        /// <param name="advice"></param> string that gives advice of an error.
        /// <returns></returns>
        public ActionResult Hospital(string name, string advice)
        {
            var showHospital = Storage.Instance.Hospitals.Find(x => x.HospitalName == name);
            var newqueue = new PriorityQueue<PatientStructure>();
            showHospital.InfectedList = new List<PatientStructure>();
            showHospital.SuspiciousList = new List<PatientStructure>();
            var queueClone = showHospital.InfectedQueue;
            var node = queueClone.GetFirst();
            while (node != null)
            {
                showHospital.InfectedList.Add(node.Patient);
                newqueue.AddPatient(node.Patient.CUI, node.Patient.ArrivalDate, node.Patient, node.Patient.Priority);
                node = queueClone.GetFirst();
            }
            showHospital.InfectedQueue = newqueue;
            newqueue = new PriorityQueue<PatientStructure>();
            queueClone = showHospital.SuspiciousQueue;
            node = queueClone.GetFirst();
            while (node != null)
            {
                showHospital.SuspiciousList.Add(node.Patient);
                newqueue.AddPatient(node.Patient.CUI, node.Patient.ArrivalDate, node.Patient, node.Patient.Priority);
                node = queueClone.GetFirst();
            }
            showHospital.SuspiciousQueue = newqueue;

            if (advice != "")
            {
                TempData["Error"] = advice;
            }
            return View(showHospital);
        }

        /// <summary>
        /// If the Infectedqueue is full or if it has a patient with a higher priority than the suspicious queue,
        /// it shows an advice to set a bed as empty.
        /// If it doesn't, gets the patient at the top of the suspiciousqueue and looks for it in the PatientsHash.
        /// After that, it calls InfectionTest() and actualizes the country's statistics.
        /// Finally, redirects to Hospital()
        /// </summary>
        /// <param name="hospital"></param> string that contains the current hospital's name
        /// <returns></returns>
        public ActionResult Test(string hospital)
        {
            var hosp = Storage.Instance.Hospitals.Find(x => x.HospitalName == hospital);
            if (hosp.InfectedQueueFull())
            {
                return RedirectToAction("Hospital", new { name = hosp.HospitalName, advice = "La cola de infectados está llena, por favor libere una cama antes de continuar." });
            }
            else if (hosp.InfectedQueue.Root != null)
            {
                if (hosp.InfectedQueue.Root.Patient.Priority < hosp.SuspiciousQueue.Root.Patient.Priority)
                {
                    if (hosp.BedFull())
                    {
                        return RedirectToAction("Hospital", new { name = hosp.HospitalName, advice = "Hay un paciente que necesita ser atendido antes de que realice más pruebas de COVID-19, por favor libere una cama." });
                    }
                    else
                    {
                        var patient = hosp.InfectedQueue.GetFirst().Patient;
                        Storage.Instance.BedHash.Insert(new Bed() { Patient = patient, Availability = "No Disponible" }, patient.CUI, GetMultiplier(patient.Hospital));
                    }
                }
                else
                {
                    var patient = hosp.SuspiciousQueue.GetFirst().Patient;
                    var infected = Storage.Instance.PatientsHash.Search(patient.CUI).Value.InfectionTest();
                    if (infected)
                    {
                        patient.Status = "Contagiado";
                        patient.IsInfected = true;
                        Storage.Instance.PatientsHash.Search(patient.CUI).Value.PriorityAssignment();
                        patient.PriorityAssignment();
                        Storage.Instance.PatientsByCUI.ChangeValue(patient, Storage.Instance.PatientsByCUI.Root, PatientStructure.CompareByCUI, PatientStructure.CompareByCUI);
                        Storage.Instance.PatientsByName.ChangeValue(patient, Storage.Instance.PatientsByName.Root, PatientStructure.CompareByName, PatientStructure.CompareByCUI);
                        Storage.Instance.PatientsByLastName.ChangeValue(patient, Storage.Instance.PatientsByLastName.Root, PatientStructure.CompareByLastName, PatientStructure.CompareByCUI);
                        Storage.Instance.CountryStatistics.Suspicious--;
                        Storage.Instance.CountryStatistics.Infected++;
                        if (hosp.BedFull())
                        {
                            Storage.Instance.Hospitals.Find(x => x.HospitalName == patient.Hospital).InfectedQueue.AddPatient(patient.CUI, patient.ArrivalDate, patient, patient.Priority);
                        }
                        else
                        {
                            Storage.Instance.BedHash.Insert(new Bed() { Patient = patient, Availability = "No Disponible" }, patient.CUI, GetMultiplier(patient.Hospital));
                        }
                        Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList = new List<Bed>();
                        for (int i = 0; i < 10; i++)
                        {
                            var node = Storage.Instance.BedHash.GetT(i, GetMultiplier(hospital));
                            if (node != null)
                            {
                                Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList.Add(node.Value);
                            }
                        }
                        Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedsInUse = Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList.Count();
                        return RedirectToAction("Hospital", new { name = hosp.HospitalName });
                    }
                }
            }
            else
            {
                if (hosp.SuspiciousQueue.Root == null)
                {
                    return RedirectToAction("Hospital");
                }
                var patient = hosp.SuspiciousQueue.GetFirst().Patient;
                var infected = Storage.Instance.PatientsHash.Search(patient.CUI).Value.InfectionTest();
                if (infected)
                {
                    patient.Status = "Contagiado";
                    patient.IsInfected = true;
                    Storage.Instance.PatientsHash.Search(patient.CUI).Value.PriorityAssignment();
                    patient.PriorityAssignment();
                    Storage.Instance.PatientsByCUI.ChangeValue(patient, Storage.Instance.PatientsByCUI.Root, PatientStructure.CompareByCUI, PatientStructure.CompareByCUI);
                    Storage.Instance.PatientsByName.ChangeValue(patient, Storage.Instance.PatientsByName.Root, PatientStructure.CompareByName, PatientStructure.CompareByCUI);
                    Storage.Instance.PatientsByLastName.ChangeValue(patient, Storage.Instance.PatientsByLastName.Root, PatientStructure.CompareByLastName, PatientStructure.CompareByCUI);
                    Storage.Instance.CountryStatistics.Suspicious--;
                    Storage.Instance.CountryStatistics.Infected++;
                    if (hosp.BedFull())
                    {
                        Storage.Instance.Hospitals.Find(x => x.HospitalName == patient.Hospital).InfectedQueue.AddPatient(patient.CUI, patient.ArrivalDate, patient, patient.Priority); 
                    }
                    else
                    {
                        Storage.Instance.BedHash.Insert(new Bed() { Patient = patient, Availability = "No Disponible" }, patient.CUI, GetMultiplier(patient.Hospital));
                    }
                    Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList = new List<Bed>();
                    for (int i = 0; i < 10; i++)
                    {
                        var node = Storage.Instance.BedHash.GetT(i, GetMultiplier(hospital));
                        if (node != null)
                        {
                            Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList.Add(node.Value);
                        }
                    }
                    Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedsInUse = Storage.Instance.Hospitals.First(x => x.HospitalName == hosp.HospitalName).BedList.Count();
                    return RedirectToAction("Hospital", new { name = hosp.HospitalName });
                }
                else
                {
                    return RedirectToAction("Hospital", new { name = hosp.HospitalName, advice = "La prueba del paciente ha salido negativa, se ha descartado su caso" });
                }
            }
            return RedirectToAction("Hospital");
        }

        /// <summary>
        /// Deletes the selected bed from the bed hash and changes the patient's status to recovered.
        /// It changes the Patient's info in the hash and the AVL's.
        /// Finally, redirects to PatientsList().
        /// </summary>
        /// <param name="bed"></param> bed that has been selected to set empty.
        /// <returns></returns>
        public ActionResult GetRecovered(PatientStructure Patient)
        {
            Patient.IsInfected = false;
            Patient.Status = "Recuperado";
            Storage.Instance.BedHash.Delete(Patient.CUI, GetMultiplier(Patient.Hospital));
            Storage.Instance.PatientsHash.Search(Patient.CUI).Value.Status = "Recuperado";
            Storage.Instance.PatientsHash.Search(Patient.CUI).Value.IsInfected = false;
            Storage.Instance.PatientsByCUI.ChangeValue(Patient, Storage.Instance.PatientsByCUI.Root, PatientStructure.CompareByCUI, PatientStructure.CompareByCUI);
            Storage.Instance.PatientsByName.ChangeValue(Patient, Storage.Instance.PatientsByName.Root, PatientStructure.CompareByName, PatientStructure.CompareByCUI);
            Storage.Instance.PatientsByLastName.ChangeValue(Patient, Storage.Instance.PatientsByLastName.Root, PatientStructure.CompareByLastName, PatientStructure.CompareByCUI);
            Storage.Instance.CountryStatistics.Infected--;
            return RedirectToAction("Hospital", new { name = Patient.Hospital });
        }

        /// <summary>
        /// returns a multiplier according to the patient's hospital.
        /// </summary>
        /// <param name="patient"></param> patient that will be inserted into bed's hash.
        /// <returns></returns>
        private int GetMultiplier(string hospital)
        {
            switch (hospital)
            {
                case "Capital":
                    return 1;
                case "Quetzaltenango":
                    return 2;
                case "Petén":
                    return 3;
                case "Escuintla":
                    return 4;
                case "Oriente":
                    return 5;
            }
            return -1;
        }
    }
}
