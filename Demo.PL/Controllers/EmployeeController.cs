using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Controllers
{
    //[Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitOfWork;
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork
            //,  IEmployeeRepository employeesRepo
            //,IDepartmentRepository departmentRepository
            )//ask clr for creating an object from implmenting IEmployeeRepository
        {
            _mapper = mapper;
            this.unitOfWork = unitOfWork;
            //_employeeRepository = employeesRepo;
            //_departmentRepository = departmentRepository;
        }

        public IActionResult Index(string searchInp)
        {
            var Employee = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(searchInp))
            {
             Employee = unitOfWork.EmployeeRepository.GetAll();
         

            }
            else
            {
                 Employee = unitOfWork.EmployeeRepository.SearchByName(searchInp.ToLower());
            }
            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employee);
            return View(mappedEmp);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel EmployeeVM)
        {
            EmployeeVM.ImageName = DocumentSettings.UploadFile(EmployeeVM.Image, "Images");

            var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
            if (ModelState.IsValid)
            {
                unitOfWork.EmployeeRepository.Add(mappedEmp);
                var count = unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction(nameof(Index));
                  
            }
            return View(mappedEmp);
        }

        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var Employee = unitOfWork.EmployeeRepository.Get(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            if (Employee is null)
            {
                return NotFound();
            }
            return View(ViewName, mappedEmp);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
           ///if (!id.HasValue)
           ///{
            ///    return BadRequest();
            ///}
            ///var Employee = _employeeRepository.Get(id.Value);
            ///if (Employee is null)
            ///{
            ///    return NotFound();
            ///}
            ///return View(Employee);
            ///instead repeat code
            ///ViewData["Departments"] = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            //if anyone change id from console
            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {

                    var oldFile = EmployeeVM.ImageName;
                  
                    EmployeeVM.ImageName= DocumentSettings.UpdateFile(EmployeeVM.Image, "Images", oldFile);
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                    unitOfWork.EmployeeRepository.Update(mappedEmp);
                  unitOfWork.Complete();
                
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(EmployeeVM);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                unitOfWork.EmployeeRepository.Delete(mappedEmp);
             var count=   unitOfWork.Complete();
                if (count > 0)
                    DocumentSettings.DeleteFile(EmployeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(EmployeeVM);
            }
        }

    }
}
