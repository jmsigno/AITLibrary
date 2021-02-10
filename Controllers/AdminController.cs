using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AITLibrary.Models;

namespace AITLibrary.Controllers
{
    public class AdminController : Controller
    {
        UserManagementService.UserManagementServiceSoapClient ManageUser = new UserManagementService.UserManagementServiceSoapClient();
        MediaManagementService.MediaManagementServiceSoapClient ManageMedia = new MediaManagementService.MediaManagementServiceSoapClient();

        public ActionResult AdminPanel()
        {
            ListUsers();
            return View();
        }

        public ActionResult MediaManager()
        {
            ListAllMedia();
            return View();
        }

        public ActionResult Reports(int? value)
        {
            List<MediaManagementService.WBorrowDTO> borrowReport = new List<MediaManagementService.WBorrowDTO>(ManageMedia.BorrowLists());
            List<MediaManagementService.WReservationsDTO> reserveReport = new List<MediaManagementService.WReservationsDTO>(ManageMedia.ReserveList());

            List<SelectListItem> items = new List<SelectListItem>();
            SelectListItem r = new SelectListItem() { Text = "--select--", Value = "0", Selected = true };
            SelectListItem r1 = new SelectListItem() { Text = "Borrow List", Value = "1" };
            SelectListItem r2 = new SelectListItem() { Text = "Reserve List", Value = "2" };

            items.Add(r);
            items.Add(r1);
            items.Add(r2);
            ViewBag.Reports = items;

            if (value != null)
            {
                items.Where(i => i.Value == value.ToString()).First().Selected = true;
                if (value == 1)
                {
                    ViewBag.BorrowList = borrowReport;
                    ViewBag.BorrowListCount = borrowReport.Count;

                }
                else if (value == 2)
                {
                    ViewBag.ReserveList = reserveReport;
                    ViewBag.ReserveListCount = reserveReport.Count;
                }
            }
            return View();
        }

        public void ListUsers()
        {
            List<UserManagementService.WUserDTO> allUsers = new List<UserManagementService.WUserDTO>(ManageUser.RetrieveAllUsers());
            ViewBag.UserList = allUsers;
            ViewBag.UserCount = allUsers.Count;
        }

        public void ListAllMedia()
        {
            List<MediaManagementService.WMediaDTO> allMedia = new List<MediaManagementService.WMediaDTO>(ManageMedia.GetAllMedia()); 
            ViewBag.Item = allMedia;
            ViewBag.ItemCount = allMedia.Count;
        }
        public ActionResult AddUser(UserDTO user)
        {
            UserManagementService.WUserDTO newUser = new UserManagementService.WUserDTO()
            {
                UserName = user.UserName,
                Password = user.Password,
                UserLevel = user.UserLevel,
                UserEmail = user.UserEmail
            };

            if (ManageUser.AddUser(newUser) == 1)
            {
                ViewBag.AddUserResult = "User " + user.UserName + " added successfully";
            }
            ListUsers();
            return View("AdminPanel");
        }

        public ActionResult UpdateUser(UserDTO user)
        {
            UserManagementService.WUserDTO newUserData = new UserManagementService.WUserDTO()
            {
                UID = user.UID,
                UserName = user.UserName,
                Password = user.Password,
                UserLevel = user.UserLevel,
                UserEmail = user.UserEmail
            };

            int x = ManageUser.UpdateUser(newUserData);
            if (x == 1)
            {
                ViewBag.UpdateUserResult = "User " + user.UserName + " updated!";
            }
            else if (x == 0)
            {
                ViewBag.UpdateUserResult = "User " + user.UserName + " not found.";
            }
            else
            {
                ViewBag.UpdateUserResult = "An error occured, please try again.";
            }
            ListUsers();
            return View("AdminPanel");
        }

        public ActionResult DeleteUser(string userID)
        {
            if (ManageUser.DeleteUser(Convert.ToInt32(userID)) > 0)
            {
                ViewBag.DeleteUserResult = "User deleted successfully";
            }
            else
            {
                ViewBag.DeleteUserResult = "An error occured, please try again.";
            }
            ListUsers();
            return View("AdminPanel");
        }

        public ActionResult DeleteMedia(int mediaID)
        {
            ViewBag.DeleteResult = null;

            if (ManageMedia.DeleteMedia(mediaID) == true)
            {
                ViewBag.DeleteResult = "Media " + mediaID + " successfully deleted.";
            }
            else
            {
                ViewBag.DeleteResult = "Media " + mediaID + " not found.";
            }
            ListAllMedia();
            return View("MediaManager");
        }

        public ActionResult AddMedia(MediaDTO media)
        {
            ViewBag.AddResult = null;

            MediaManagementService.WMediaDTO newMedia = new MediaManagementService.WMediaDTO()
            {
                mediaTitle = media.mediaTitle,
                genre = media.genre,
                director = media.director,
                language = media.language,
                publishYear = media.publishYear,
                budget = media.budget
            };

            if (ManageMedia.InsertMedia(newMedia) == true)
            {
                ViewBag.AddResult = "" + media.mediaTitle + " successfully added.";
            }
            else
            {
                ViewBag.AddResult = "" + media.mediaTitle + " already exist.";
            }
            ListAllMedia();
            return View("MediaManager");
        }

        public ActionResult UpdateMedia(MediaDTO media)
        {
            MediaManagementService.WMediaDTO newMedia = new MediaManagementService.WMediaDTO()
            {
                mediaID = media.mediaID,
                mediaTitle = media.mediaTitle,
                genre = media.genre,
                director = media.director,
                language = media.language,
                publishYear = media.publishYear,
                budget = media.budget
            };
            if (media.budget < 1000)
            {
                if (ManageMedia.UpdateMedia(newMedia) == true)
                {
                    ViewBag.UpdateResult = "" + media.mediaTitle + " successfully updated.";
                }
                else
                {
                    ViewBag.UpdateResult = "Media " + media.mediaID + " doesn't exist.";
                }
            }
            else
            {
                ViewBag.UpdateResult = "Budget Limit is 999.99 due to database definition.";
            }
            ListAllMedia();
            return View("MediaManager");
        }
    }
}