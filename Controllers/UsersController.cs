﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SewingCompany.ViewModels.User;
using X.PagedList;

namespace SewingCompany.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {

        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name_desc" ? "name_asc" : "name_desc";
            ViewBag.EmailSortParm = sortOrder == "email_desc" ? "email_asc" : "email_desc";
            ViewBag.RoleSortParm = sortOrder == "role_desc" ? "role_asc" : "role_desc";

            var users = _userManager.Users.OrderBy(user => user.Id);

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(us => us.UserName);
                    break;
                case "email_asc":
                    users = users.OrderBy(us => us.UserName);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(us => us.Email);
                    break;
                case "role_asc":
                    users = users.OrderBy(us => us.Email);
                    break;
                case "role_desc":
                    users = users.OrderByDescending(us => us.Email);
                    break;
                default:
                    users = users.OrderBy(us => us.UserName);
                    break;
            }


            List<UserViewModel> userViewModel = new List<UserViewModel>();

            string urole = "";
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count() > 0)
                {
                    urole = userRoles[0] ?? "";
                }

                userViewModel.Add(
                    new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        RoleName = urole

                    });

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(userViewModel.ToPagedList(pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel user = new CreateUserViewModel();

            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                var role = model.UserRole;
                if (role.Count() > 0)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            string userRole = "";
            if (userRoles.Count() > 0)
            {
                userRole = userRoles[0] ?? "";
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                UserRole = userRole
            };
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name", model.UserRole);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var oldRoles = await _userManager.GetRolesAsync(user);

                    if (oldRoles.Count() > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                    }
                    var newRole = model.UserRole;
                    if (newRole.Count() > 0)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}