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
        public ActionResult Index()
        {
            return View();
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
                    Municipality = collection["Municipality"],
                    Symptoms = collection["Symptoms"],
                    CUI = int.Parse(collection["CUI"]),
                    Age = int.Parse(collection["Age"]),
                    InfectionDescription = collection["InfectionDescription"]
                };
                newPatient.PriorityAssignment();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("InfectionDescription", "Por favor asegúrese de haber llenado todos los campos correctamente.");
                return View("NewCase");
            }
        }
    }
}
