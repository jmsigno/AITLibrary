using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AITLibrary.Controllers
{
    public class UserDashboardController : Controller
    {
        //MediaManagementService.MediaManagerSoapClient MediaManager = new MediaManagementService.MediaManagerSoapClient();
        // GET: UserDashboard
        //iMediaManager ManageMedia = new MediaManager();

        MediaManagementService.MediaManagementServiceSoapClient MediaManager = new MediaManagementService.MediaManagementServiceSoapClient();

        public ActionResult MyMedia()
        {
            MyBorrowList();
            MyReserveList();
            return View();
        }

        public void MyBorrowList()
        {
            List<MediaManagementService.WBorrowDTO> borrowList = new List<MediaManagementService.WBorrowDTO>(MediaManager.MyBorrowList(Convert.ToInt32(Session["validUserID"])));
            ViewBag.BorrowList = borrowList;
            ViewBag.BorrowCount = borrowList.Count;
        }

        public void MyReserveList()
        {
           List<MediaManagementService.WReservationsDTO> reserveList = new List<MediaManagementService.WReservationsDTO>(MediaManager.MyReservations(Convert.ToInt32(Session["validUserID"])));
            ViewBag.ReserveList = reserveList;
            ViewBag.ReserveCount = reserveList.Count;
        }


        public ActionResult UserAction(string command, string mediaID)
        {
            if (command == "BORROW" && mediaID != null)
            {
                Borrow(Convert.ToInt32(mediaID));
                MyBorrowList();
                MyReserveList();
            }
            else if (command == "RETURN" && mediaID != null)
            {
                ReturnMedia(Convert.ToInt32(mediaID));
                MyBorrowList();
                MyReserveList();
            }
            else if (command == "CANCEL" && mediaID != null)
            {
                CancelReservation(Convert.ToInt32(mediaID));
                MyBorrowList();
                MyReserveList();
            }
            return View("MyMedia");
        }

        public ActionResult Borrow(int mediaID)
        {
            MediaManagementService.WMediaDTO mediaDTO = MediaManager.RetrieveMedia(mediaID);
            int userID = Convert.ToInt32(Session["validUserID"]);
            int x = MediaManager.BorrowMedia(userID, mediaID);

            if (x > 0)
            {
                ViewBag.BorrowResult = "You Borrowed" + mediaDTO.mediaTitle;
            }
            else if (x < 0)
            {
                ViewBag.BorrowResult = "A database error occured, please try again.";
            }
            else
            {
                ViewBag.BorrowResult = "Cannot borrow " + mediaDTO.mediaTitle + " because it is currently reserved/borrowed, please reserve to borrow.";
            }
            return View();
        }

        public ActionResult ReturnMedia(int mediaID)
        {
            if (MediaManager.ReturnMedia(Convert.ToInt32(Session["validUserID"]), mediaID) == true)
            {
                ViewBag.ReturnResult = "Media Successfully Returned.";
            }
            return View();
        }

        public ActionResult CancelReservation(int mediaID)
        {
            MediaManager.CancelReservation(Convert.ToInt32(Session["validUserID"]), mediaID);

            return View();
        }
    }
}