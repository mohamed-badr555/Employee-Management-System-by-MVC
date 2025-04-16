using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Demo.PL.Controllers
{


    //Inhertiance : DepartmentController is a Controller
    //Composition : DepartmentController ihass a Controller
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork
            //IDepartmentRepository departmentsRepo
            )//ask clr for creating an object from implmenting IDepartmentRepository
        {
            this.unitOfWork = unitOfWork;
            //_departmentRepository = departmentsRepo;   
        }

        public IActionResult Index()
        {
            TempData.Keep();
            var department = unitOfWork.DepartmentRepository.GetAll();
            return View(department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                    unitOfWork.DepartmentRepository.Add(department);
                var count = unitOfWork.Complete();
                if (count > 0)
                    TempData["Message"] = "Department is Created Successfully";
                else
                    TempData["Message"] = "Department is not Created ";

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? id , string ViewName= "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var department = unitOfWork.DepartmentRepository.Get(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(ViewName, department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (!id.HasValue)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepository.Get(id.Value);
            //if (department is null)
            //{
            //    return NotFound();
            //}
            //return View(department);
            //instead repeat code
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id , Department department)
        {
            //if anyone change id from console
            if (id != department.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    unitOfWork.DepartmentRepository.Update(department);
                    unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
               
            }
            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
                unitOfWork.DepartmentRepository.Delete(department);
                unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
        }


    }
}
