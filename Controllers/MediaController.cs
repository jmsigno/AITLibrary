using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AITLibrary.Models;


namespace AITLibrary.Controllers
{
    public class MediaController : Controller
    {
        //MediaManagementService.MediaManagerSoapClient MediaManager = new MediaManagementService.MediaManagerSoapClient();
        //http://highoncoding.com/Articles/429_Returning_List_and_ArrayList_from_Web_Service_Method.aspx 

        //iMediaManager MediaManager = new MediaManager();

        MediaManagementService.MediaManagementServiceSoapClient MediaManager = new MediaManagementService.MediaManagementServiceSoapClient();

        public ActionResult Library()
        {
            return View();
        }

        public void MediaList()
        {
            List<MediaManagementService.WMediaDTO> allMedia = new List<MediaManagementService.WMediaDTO>(MediaManager.GetAllMedia());
            ViewBag.Item = allMedia;
            ViewBag.ItemCount = allMedia.Count;
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
        public ActionResult ViewAll()
        {
            MediaList();
            MyBorrowList();
            MyReserveList();
            return View("Library");
        }

        public ActionResult SearchByKeyword(string search)
        {
            List<MediaManagementService.WMediaDTO> searchResults = new List<MediaManagementService.WMediaDTO>(MediaManager.KeySearch(search));
            ViewBag.Item = searchResults;
            ViewBag.ItemCount = searchResults.Count;

            return View("Library");
        }
        public ActionResult SearchByID(string search)
        {
            List<MediaManagementService.WMediaDTO> searchResults = new List<MediaManagementService.WMediaDTO>(MediaManager.IdSearch(search));
            ViewBag.Item = searchResults;
            ViewBag.ItemCount = searchResults.Count;

            return View("Library");
        }
        public ActionResult SearchByTitle(string search)
        {
            List<MediaManagementService.WMediaDTO> searchResults = new List<MediaManagementService.WMediaDTO>(MediaManager.TitleSearch(search));
            ViewBag.Item = searchResults;
            ViewBag.ItemCount = searchResults.Count;

            return View("Library");
        }

        public ActionResult Action(string command, string mediaID)
        {
            if (command == "BORROW" && mediaID != null)
            {
                Borrow(Convert.ToInt32(mediaID));
            }
            else if (command == "RESERVE" && mediaID != null)
            {
                Reserve(Convert.ToInt32(mediaID));
            }
            else if (command == "RETURN" && mediaID != null)
            {
               ReturnMedia(Convert.ToInt32(mediaID));
            }
            else if (command == "CANCEL" && mediaID != null)
            {
                CancelReservation(Convert.ToInt32(mediaID));
            }
            return View("Library");
        }

        public ActionResult Borrow(int mediaID)
        {
            //MediaManagementService.MediaDTO mediaDTO = MediaManager.RetrieveMedia(mediaID);
            MediaManagementService.WMediaDTO mediaDTO = MediaManager.RetrieveMedia(mediaID);

            int userID = Convert.ToInt32(Session["validUserID"]);
            int x = MediaManager.BorrowMedia(userID, mediaID);

            if (x > 0)
            {
                ViewBag.BorrowResult = "You Borrowed" + mediaDTO.mediaTitle;
            }
            else if(x < 0)
            {
                ViewBag.BorrowResult = "A database error occured, please try again.";
            }
            else
            {
                ViewBag.BorrowResult = "Cannot borrow " + mediaDTO.mediaTitle + " because it is currently reserved/borrowed, please reserve to borrow.";
            }
            return View("Library");
        }

        public ActionResult ReturnMedia(int mediaID)
        {
            if(MediaManager.ReturnMedia(Convert.ToInt32(Session["validUserID"]), mediaID) == true)
            {
                ViewBag.ReturnResult = "Media Successfully Returned.";
            }
            return View();
        }

        public ActionResult Reserve(int mediaID)
        {
            //MediaManagementService.MediaDTO mediaDTO = MediaManager.RetrieveMedia(mediaID);
            MediaManagementService.WMediaDTO mediaDTO = MediaManager.RetrieveMedia(mediaID);

            int userID = Convert.ToInt32(Session["validUserID"]);
            int x = MediaManager.ReserveMedia(userID, mediaID);

            if ( x > 0 )
            {
                 ViewBag.ReserveResult = "You reserved " + mediaDTO.mediaTitle + ". Date reserved: " + DateTime.Today;
            }
            else if ( x < 0)
            {
                ViewBag.ReserveResult = "A database error occured, please try again.";
            }
            else
            {
                ViewBag.ReserveResult = "You already reserved " + mediaDTO.mediaTitle;
            }
            return View("Library");
        }

        public ActionResult CancelReservation(int mediaID)
        {
            MediaManager.CancelReservation(Convert.ToInt32(Session["validUserID"]), mediaID);

            return View("Library");
        }
    }
}