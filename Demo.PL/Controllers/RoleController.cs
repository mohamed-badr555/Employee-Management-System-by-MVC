using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManage;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManage ,IMapper mapper)
        {
            this.roleManage = roleManage;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var users = await roleManage.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name



                }).ToListAsync();
                return View(users);
            }
            else
            {
                var role = await roleManage.FindByNameAsync(name);
                if (role is not null)
                {

                var mappedRole = new RoleViewModel()
                {
                    Id = role?.Id,
                    RoleName = role?.Name

                };
                return View(new List<RoleViewModel> { mappedRole });
                }
                else
                {
                    return View(Enumerable.Empty<RoleViewModel>());
                }
            }
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rolemapped = mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                    var result =await  roleManage.CreateAsync(rolemapped);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }

            [HttpGet]
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            var role = await roleManage.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }
            var mappedrole = mapper.Map<IdentityRole, RoleViewModel>(role);

            return View(ViewName, mappedrole);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await roleManage.FindByIdAsync(id);
                    user.Name = roleVM.RoleName;
              

                    var result = await roleManage.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(roleVM);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
    

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            //if (id != userVM.Id)
            //    return BadRequest();
            try
            {
                //var mappeduseer = mapper.Map<UserViewModel, ApplicationUser>(userVM);
                var user = await roleManage.FindByIdAsync(id);
                await roleManage.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
