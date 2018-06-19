using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Online_Sınav_Sistemi.Models;

namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
{
    public class AdminSinavController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Sınav/
        public ActionResult Index()
        {
            return View(db.Sınav.ToList());
        }

        // GET: /Sınav/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sınav sınav = db.Sınav.Find(id);
            SınavaSoruEkleme sınavsoru = db.SınavaSoruEkleme.Find(id);
            if (sınav == null)
            {
                return HttpNotFound();
            }
            return View(sınav);
        }

        // GET: /Sınav/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Sınav/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SınavID,SınavAdi,SınavSüresi")] Sınav sınav)
        {
            if (ModelState.IsValid)
            {
                db.Sınav.Add(sınav);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sınav);
        }

        // GET: /Sınav/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sınav sınav = db.Sınav.Find(id);
            if (sınav == null)
            {
                return HttpNotFound();
            }
            return View(sınav);
        }

        // POST: /Sınav/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SınavID,SınavAdi,SınavSüresi")] Sınav sınav)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sınav).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sınav);
        }

        // GET: /Sınav/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sınav sınav = db.Sınav.Find(id);
            if (sınav == null)
            {
                return HttpNotFound();
            }
            return View(sınav);
        }

        // POST: /Sınav/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sınav sınav = db.Sınav.Find(id);
            db.Sınav.Remove(sınav);
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