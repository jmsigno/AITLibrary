using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AITLibrary.Models;

namespace AITLibrary.Controllers
{
    public class AccountController : Controller
    {
        UserManagementService.UserManagementServiceSoapClient userMgr = new UserManagementService.UserManagementServiceSoapClient();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult ValidateUser(LoginDTO user)
        {
            if (user.Username != null && user.Password != null )
            {
                UserManagementService.WUserDTO isValidUser = userMgr.validateUser(user.Username, user.Password);

                if(isValidUser.UserName != null)
                {
                    ViewBag.Message = "Login Successfull";
                    Session["validUserID"] = isValidUser.UID.ToString(); ;
                    Session["validUsername"] = isValidUser.UserName.ToString();
                    Session["UserLevel"] = isValidUser.UserLevel;
                    Session["UserEmail"] = isValidUser.UserEmail.ToString();

                    if (isValidUser.UserLevel > 1)
                    {
                        return RedirectToAction("../Admin/AdminPanel");
                    }
                    return RedirectToAction("../Media/Library");
                }
            }
            ViewBag.Invalid = "Invalid username or password.";
            return View("Login");
        }
    }
}