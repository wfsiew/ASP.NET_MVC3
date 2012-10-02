using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using mvcweb.Models;
using mvcweb.Repositories;
using mvcweb.App;

namespace mvcweb.Controllers
{
    public class MainController : Controller
    {
        private ICarRepository o;

        public MainController(ICarRepository p)
        {
            o = p;
        }

        [NHibernateSession]
        public ActionResult Cars()
        {
            Dictionary<string, object> dic = o.GetAll(1, 0);
            ViewData["data"] = dic;

            return View("carspage");
        }

        public ActionResult Add()
        {
            return View("addpage", new Car());
        }

        [HttpPost]
        [NeedsPersistence]
        public ActionResult Add(Car car)
        {
            if (ModelState.IsValid)
            {
                o.Add(car);
                return RedirectToAction("Added");
            }

            else
                return View("addpage", car);
        }

        public ActionResult Added()
        {
            return View("addedpage");
        }

        [NeedsPersistence]
        public ActionResult Delete(int id)
        {
            o.Delete(id);
            ViewData["id"] = id;
            return View("deletedpage");
        }

        [NHibernateSession]
        public ActionResult Edit(int id)
        {
            Car car = o.Get(id);
            return View("editpage", car);
        }

        [HttpPost]
        [NeedsPersistence]
        public ActionResult Edit(Car car)
        {
            o.Edit(car);
            ViewData["id"] = car.ID;
            return View("editedpage");
        }

        public static void Route(RouteCollection routes)
        {
            routes.MapRoute(
                "Cars",
                "main/cars",
                new { controller = "Main", action = "Cars" }
            );
            routes.MapRoute(
                "CarsAdd",
                "main/cars/add",
                new { controller = "Main", action = "Add" }
            );
            routes.MapRoute(
                "CarsDelete",
                "main/cars/delete/{id}",
                new { controller = "Main", action = "Delete" }
            );
            routes.MapRoute(
                "CarsEdit",
                "main/cars/edit/{id}",
                new { controller = "Main", action = "Edit" }
            );
        }
    }
}