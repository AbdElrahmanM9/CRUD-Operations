using CRUD.Models;
using CRUD.Repository;
using CRUD.ViewModel;
using Rotativa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private IRepository<Employee> _employeeRepository;
        private IRepository<Governorate> _GovernorateRepository;
        private IRepository<Center> _CentersRepository;
        private IRepository<Village> _VillageRepository;
        public EmployeeController()
        {
            _employeeRepository = new GenericRepository<Employee>(new Entities());
            _GovernorateRepository = new GenericRepository<Governorate>(new Entities());
            _CentersRepository = new GenericRepository<Center>(new Entities());
            _VillageRepository = new GenericRepository<Village>(new Entities());

        }
        public EmployeeController(IRepository<Employee> employeeRepository, IRepository<Governorate> governorateRepository, IRepository<Center> centerRepository, IRepository<Village> villageRepository)
        {
            _employeeRepository = employeeRepository;
            _GovernorateRepository = governorateRepository;
            _CentersRepository = centerRepository;
            _VillageRepository = villageRepository;
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = _employeeRepository.GetAll().Where(x => x.IsDeleted != true);

            return View(model);
        }

        private SelectList GetGovernorateList()
        {
            var governorates = _GovernorateRepository.GetAll()
                                       .Select(g => new Tuple<int, string>(g.Id, g.GovernorateName))
                                       .ToList();

            return new SelectList(governorates, "Item1", "Item2");
        }
        public ActionResult GetCenters(int governorateId)
        {
            var centers = _CentersRepository.GetAll().Where(c => c.GovernorateId == governorateId).ToList();
            return Json(centers, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVillages(int centerId)
        {
            var villages = _VillageRepository.GetAll().Where(v => v.CenterId == centerId).ToList();
            return Json(villages, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,DataEntry")]
        public ActionResult AddEmployee()
        {
            ViewBag.GovernorateList = GetGovernorateList();
            return View();
        }
        public ActionResult AddNewEmployee(EmployeesModel employee)
        {
            if (ModelState.IsValid)
            {
                Employee employeeEntity = new Employee();
                employeeEntity.EmployeeName = employee.EmployeeName;
                employeeEntity.Salary = employee.Salary;
                employeeEntity.NationalID = employee.NationalID;
                employeeEntity.GovernorateName = employee.Governorate;
                employeeEntity.CenterName = employee.Center;
                employeeEntity.VillageName = employee.VillageName;
                employeeEntity.IsDeleted = false;

                _employeeRepository.Insert(employeeEntity);
                _employeeRepository.Save();
                return RedirectToAction("Index", "Employee");
            }
            return RedirectToAction("Index", "Employee");
        }
        [Authorize(Roles = "Admin,Modify")]
        public ActionResult EditEmp(int Id)
        {
            Employee model = _employeeRepository.GetById(Id);
            ViewBag.GovernorateList = GetGovernorateList();

            return PartialView(model);
        }

        public ActionResult EditEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Update(employee);
                _employeeRepository.Save();
                return RedirectToAction("Index", "Employee");
            }
            else
            {
                return View(employee);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Modify")]
        public ActionResult Delete(int Id)
        {
            Employee employee = _employeeRepository.GetById(Id);
            employee.IsDeleted = true;
            _employeeRepository.UpdateToDelete(employee);
            _employeeRepository.Save();
            return RedirectToAction("Index", "Employee");
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Modify")]
        public ActionResult GetEmpDetails(int Id)
        {
            var model = _employeeRepository.GetById(Id);
            return PartialView(model);
        }
    }
}