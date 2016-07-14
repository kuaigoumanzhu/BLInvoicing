using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL.Framework;
using BL.Models;
using BL.MVC;

namespace BL.Web.Controllers
{
    public class LoginController : Controller
    {
        IAuthenticationService authenticationService = DIContainer.ResolvePerHttpRequest<IAuthenticationService>();
        [NoFilter]
        public ActionResult Login()
        {
            return View();
        }

        [NoFilter]
        [HttpPost]
        public ActionResult Login(string userName,string password)
        {
            UserInfo user = new UserInfo 
            {
                UserName=userName,
                TrueName=password
            };
            authenticationService.SignIn(userName, user, TimeSpan.FromMinutes(600));
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOut()
        {
            authenticationService.SignOut();
            return RedirectToAction("Login", "Login");
        }
    }
}
