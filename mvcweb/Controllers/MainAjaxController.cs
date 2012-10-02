using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Mime;
using mvcweb.Models;
using mvcweb.Repositories;
using mvcweb.App;

namespace mvcweb.Controllers
{
    public class MainAjaxController : Controller
    {
        private ICarRepository o;

        public MainAjaxController(ICarRepository p)
        {
            o = p;
        }

        [NHibernateSession]
        public ActionResult CarsPage()
        {
            Dictionary<string, object> dic = o.GetAll(1, 0);
            ViewData["data"] = dic;

            return View("carsajaxpage");
        }

        [NHibernateSession]
        public ActionResult Cars(int find = 0, string keyword = "", int pgnum = 1, int pgsize = 0)
        {
            Dictionary<string, object> dic = null;

            if (find == 0 && string.IsNullOrEmpty(keyword))
                dic = o.GetAll(pgnum, pgsize);

            else
                dic = o.GetFilterBy(find, keyword, pgnum, pgsize);

            ViewData["data"] = dic;

            return View("carlist");
        }

        public ActionResult Add()
        {
            ViewData["formTitle"] = "Create New Car";
            return View("saveform", new Car());
        }

        [HttpPost]
        [NeedsPersistence]
        public JsonResult Add(Car car)
        {
            if (ModelState.IsValid)
            {
                o.Add(car);
                return Json(new Dictionary<string, object> { { "success", 1 } }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                Dictionary<string, object> dic = Car.GetErrors(ModelState);
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [NHibernateSession]
        public JsonResult Delete(int id, int find = 0, string keyword = "", int pgnum = 1, int pgsize = 0)
        {
            o.Delete(id);
            string itemscount = o.GetItemMessage(find, keyword, pgnum, pgsize);
            return Json(new Dictionary<string, object> { { "success", 1 }, { "itemscount", itemscount } }, JsonRequestBehavior.AllowGet);
        }

        [NHibernateSession]
        public ActionResult Edit(int id)
        {
            Car car = o.Get(id);
            ViewData["formTitle"] = "Edit Car";
            return View("saveform", car);
        }

        [HttpPost]
        [NeedsPersistence]
        public JsonResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                o.Edit(car);
                return Json(new Dictionary<string, object> { { "success", 1 } }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                Dictionary<string, object> dic = Car.GetErrors(ModelState);
                return Json(dic, JsonRequestBehavior.AllowGet);
            }
        }

        public static void Route(RouteCollection routes)
        {
            routes.MapRoute(
                "AjaxCarsPage",
                "ajax/cars",
                new { controller = "MainAjax", action = "CarsPage" }
            );
            routes.MapRoute(
                "AjaxCars",
                "ajax/cars/list",
                new { controller = "MainAjax", action = "Cars" }
            );
            routes.MapRoute(
                "AjaxCarsAdd",
                "ajax/cars/add",
                new { controller = "MainAjax", action = "Add" }
            );
            routes.MapRoute(
                "AjaxDelete",
                "ajax/cars/delete",
                new { controller = "MainAjax", action = "Delete" }
            );
            routes.MapRoute(
                "AjaxEdit",
                "ajax/cars/edit/{id}",
                new { controller = "MainAjax", action = "Edit" }
            );
        }
    }
}