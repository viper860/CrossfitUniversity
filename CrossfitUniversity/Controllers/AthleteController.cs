using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using PagedList;
using System.Web.Mvc;
using CrossfitUniversity.DAL;
using CrossfitUniversity.Models;

namespace CrossfitUniversity.Controllers
{
    public class AthleteController : Controller
    {
        private CrossfitContext db = new CrossfitContext();

        // GET: Athlete
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //return View(db.Athletes.ToList());
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RegionSortParm = sortOrder == "Region" ? "region_desc" : "Region";
            //int PageSize = 10;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var athletes = from a in db.Athletes
                           select a;
            if (!String.IsNullOrEmpty(searchString))
            {
                athletes = athletes.Where(a => a.Name.Contains(searchString)
                                       || a.Region.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    athletes = athletes.OrderByDescending(a => a.Name);
                    break;
                case "Region":
                    athletes = athletes.OrderBy(a => a.Region);
                    break;
                case "region_desc":
                    athletes = athletes.OrderByDescending(a => a.Region);
                    break;
                default:
                    athletes = athletes.OrderBy(a => a.Name);
                    break;
            }
            //return View(athletes.ToList());
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(athletes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Athlete/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }

        // GET: Athlete/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Athlete/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AthleteId,CfId,Name,Region,Team,Gender,Age,Height,Weight,Fran,Helen,Grace,Filthy50,Fgonebad,Run400,Run5k,Candj,Snatch,Deadlift,Backsq,Pullups,Eat,Train,Background,Experience,Schedule,Howlong,RetrievedDatetime")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                db.Athletes.Add(athlete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(athlete);
        }

        // GET: Athlete/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }

        // POST: Athlete/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AthleteId,CfId,Name,Region,Team,Gender,Age,Height,Weight,Fran,Helen,Grace,Filthy50,Fgonebad,Run400,Run5k,Candj,Snatch,Deadlift,Backsq,Pullups,Eat,Train,Background,Experience,Schedule,Howlong,RetrievedDatetime")] Athlete athlete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(athlete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(athlete);
        }

        // GET: Athlete/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Athlete athlete = db.Athletes.Find(id);
            if (athlete == null)
            {
                return HttpNotFound();
            }
            return View(athlete);
        }

        // POST: Athlete/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Athlete athlete = db.Athletes.Find(id);
            db.Athletes.Remove(athlete);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
