using AutoMapper;
using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class userController: Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public userController(UserManager< ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager , IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                var users = await userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FirstName,
                    LName = U.LastName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = userManager.GetRolesAsync(U).Result
                
            }).ToListAsync();
                return View(users);
            }else
            {
                var user = await userManager.FindByEmailAsync(email);
                var mapperUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FirstName,
                    LName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel> { mapperUser });
            }
        }




        [HttpGet]
        public async  Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            var mappeduser = mapper.Map<ApplicationUser, UserViewModel>(user);

            return View(ViewName, mappeduser );
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id);
                    user.FirstName = userVM.FName;
                    user.LastName = userVM.LName;
                    user.PhoneNumber = userVM.PhoneNumber;

                    var result = await userManager.UpdateAsync(user);
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

            return View(userVM);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        //[HttpPost(Name = "Delete")]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> CotenfirmDele( string id)
        {
            //if (id != userVM.Id)
            //    return BadRequest();
            try
            {
                //var mappeduseer = mapper.Map<UserViewModel, ApplicationUser>(userVM);
                var user = await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error","Home");
            }
        }

    

}
}
