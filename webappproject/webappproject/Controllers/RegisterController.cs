﻿using Microsoft.AspNetCore.Mvc;
using webappproject.Models;
using webappproject.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webappproject.Controllers
{
    public class RegisterController : Controller
    {
        UserService _userService = new UserService();
        RolService _rolService = new RolService();
        BanService _banService = new BanService();

        public IActionResult Index()
        {
            return View();
        }


        public bool IsValidation(User model)
        {
            var validation = true;

            if (string.IsNullOrEmpty(model.Name?.Trim()))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(model.Surname?.Trim()))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(model.Gender?.Trim()))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(model.Email?.Trim()))
            {
                validation = false;
            }
            if (string.IsNullOrEmpty(model.Password?.Trim()))
            {
                validation = false;
            }

            return validation;

        }



        [HttpPost]
        public IActionResult Index(User model)
        {
            if (!IsValidation(model))
            {
                TempData["error"] += "Please fill the empty places. ";
                return View(model);
            }
            if (_userService.CheckEmail(model.Email))
            {
                TempData["error"] += "This e-mail is already being used. ";
                return View(model);
            }
            if (_banService.IsBanned(model.Email))
            {
                TempData["error"] += "This email has been banned from the web site. ";
                return View(model);
            }

            model.FirstLogIn = true;
            model.RolId = _rolService.Get(x => x.Name == "User").First().Id;

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new System.String(stringChars);

            model.ProfileUrl = finalString;

            _userService.Add(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
