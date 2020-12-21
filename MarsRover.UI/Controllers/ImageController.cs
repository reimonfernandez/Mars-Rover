using MarsRover.Model;
using MarsRover.Service.Contracts;
using MarsRover.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Configuration;
using System.Net.Http;

namespace MarsRover.UI.Controllers
{
    public class ImageController : Controller
    {
        readonly IMarsRoverService _marsRoverService;

        public ImageController(IMarsRoverService marsRoverService)
        {
            _marsRoverService = marsRoverService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessTextFile(HttpPostedFileBase file)
        {
            try
            {
                bool success = true;

                if (file == null)
                {
                    TempData["errorMsg"] = "File is not selected.";
                    success = false;
                }
                else if (file.ContentLength < 1)
                {
                    TempData["errorMsg"] = "File is empty.";
                    success = false;
                }
                else if (!string.Equals(".txt", Path.GetExtension(file.FileName)))
                {
                    TempData["errorMsg"] = "File type should be .txt";
                    success = false;
                }

                if (!success)
                {
                    return RedirectToAction("Index");
                }

                List<ImageViewViewModel> model = new List<ImageViewViewModel>();
                List<string> linesOfText = _marsRoverService.ExtractTextLinesFromFile(file);
                List<DateTime> validDates = _marsRoverService.GetAllValidDates(linesOfText);

                foreach (DateTime date in validDates)
                {
                    var getPhotosByDateResponse = _marsRoverService.GetPhotosByDate(date.ToString("yyyy-M-d"));

                    var photoList = getPhotosByDateResponse.Content.ReadAsAsync<MarsRoverPhotoList>().Result;

                    if (photoList != null && photoList.Photos != null && photoList.Photos.Any())
                    {
                        foreach (var photo in photoList.Photos)
                        {
                            model.Add(new ImageViewViewModel { Id = photo.Id, ImageSRC = photo.ImageSRC, Date = date.ToString("yyyy-MM-dd") });
                        }
                    }
                }

                TempData["photos"] = model;

                return RedirectToAction("ViewImages");
            }
            catch
            {
                TempData["errorMsg"] = "Sorry, something went wrong.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult ViewImages(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            var model = (IEnumerable<ImageViewViewModel>)TempData.Peek("photos");

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentSortOrder = sortingOrder;
            ViewBag.SortingID = sortingOrder == "ID_Desc" ? "ID" : "ID_Desc";
            ViewBag.SortingDate = sortingOrder == "Date_Desc" ? "Date" : "Date_Desc";

            if (searchData != null)
            {
                pageNo = 1;
            }
            else
            {
                searchData = filterValue;
            }

            ViewBag.FilterValue = searchData;

            if (model.Any())
            {
                if (!String.IsNullOrEmpty(searchData))
                {
                    model = model.Where(x => x.Id.ToString().Contains(searchData)
                        || x.Date.Contains(searchData));
                }

                switch (sortingOrder)
                {
                    case "ID":
                        model = model.OrderBy(x => x.Id);
                        break;
                    case "ID_Desc":
                        model = model.OrderByDescending(x => x.Id);
                        break;
                    case "Date":
                        model = model.OrderBy(x => DateTime.Parse(x.Date));
                        break;
                    case "Date_Desc":
                        model = model.OrderByDescending(x => DateTime.Parse(x.Date));
                        break;
                    default:
                        model = model.OrderBy(x => DateTime.Parse(x.Date));
                        break;
                }
            }

            int itemsPerPage = 0;
            int.TryParse(ConfigurationManager.AppSettings["PagedListMaxItemsPerPage"], out itemsPerPage);

            int numOfPages = (pageNo ?? 1);
            return View(model.ToPagedList(numOfPages, itemsPerPage > 0 ? itemsPerPage : 10));
        }
        
    }
}