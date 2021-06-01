using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Security.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly ISecurityService _securityService;

        public UserController(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _securityService.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {
            if (ModelState.IsValid)
            {
                var success = await _securityService.CreateUser(model);
                if (success)
                    return RedirectToAction("Index");
                ModelState.AddModelError("", $"Could not insert user {model.Name} into database");
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var success = await _securityService.DeleteUser(id);
            if (success)
                return RedirectToAction("Index");
            ModelState.AddModelError("", $"Could not delete user with id {id} from database");
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _securityService.GetUser(id);
            if (user != null)
                return View(user);
            ModelState.AddModelError("", $"Could not edit user with id {id}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (ModelState.IsValid)
            {
                var success = await _securityService.EditUser(model);
                if (success)
                    return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"Could not edit user {model.Name} with id {model.UserId}");
            return RedirectToAction("Index");
        }
    }
}