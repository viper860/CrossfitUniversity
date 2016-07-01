using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrossfitUniversity.DAL;
using CrossfitUniversity.Models;
using CrossfitUniversity.ViewModels;

namespace CrossfitUniversity.Controllers
{
    public class AffiliateController : Controller
    {
        private CrossfitContext db = new CrossfitContext();

        // GET: Affiliate
        public ActionResult Index(int? id)
        {
            //return View(db.Affiliates.ToList());

            var viewModel = new AffiliateIndexData();
            viewModel.Affiliates = db.Affiliates
                .Include(a => a.Athletes)
                //.Include(i => i.Courses.Select(c => c.Department))
                .OrderBy(a => a.Name).Take(50);

            if (id != null)
            {
                ViewBag.CfAffiliateId = id.Value;
                viewModel.Athletes = viewModel.Affiliates.Where(
                    a => a.CfAffiliateId == id.Value).FirstOrDefault().Athletes;
            }

            return View(viewModel);
        }

        // GET: Affiliate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affiliate affiliate = db.Affiliates.Find(id);
            if (affiliate == null)
            {
                return HttpNotFound();
            }
            return View(affiliate);
        }

        // GET: Affiliate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Affiliate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AffiliateId,CfAffiliateId,Name,Address,Phone,Url,Latitude,Longitude,Cfkids")] Affiliate affiliate)
        {
            if (ModelState.IsValid)
            {
                db.Affiliates.Add(affiliate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(affiliate);
        }

        // GET: Affiliate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affiliate affiliate = db.Affiliates.Find(id);
            if (affiliate == null)
            {
                return HttpNotFound();
            }
            return View(affiliate);
        }

        // POST: Affiliate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AffiliateId,CfAffiliateId,Name,Address,Phone,Url,Latitude,Longitude,Cfkids")] Affiliate affiliate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(affiliate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(affiliate);
        }

        // GET: Affiliate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Affiliate affiliate = db.Affiliates.Find(id);
            if (affiliate == null)
            {
                return HttpNotFound();
            }
            return View(affiliate);
        }

        // POST: Affiliate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Affiliate affiliate = db.Affiliates.Find(id);
            db.Affiliates.Remove(affiliate);
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
