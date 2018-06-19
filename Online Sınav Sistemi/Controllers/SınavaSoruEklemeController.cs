using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Online_Sınav_Sistemi.Models;

namespace Online_Sınav_Sistemi.Controllers
{
    public class SınavaSoruEklemeController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /SınavaSoruEkleme/
        public ActionResult Index()
        {
            var sınavasoruekleme = db.SınavaSoruEkleme.Include(s => s.Sınav).Include(s => s.Soru);
            return View(sınavasoruekleme.ToList());
        }

        // GET: /SınavaSoruEkleme/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SınavaSoruEkleme sınavasoruekleme = db.SınavaSoruEkleme.Find(id);
            if (sınavasoruekleme == null)
            {
                return HttpNotFound();
            }
            return View(sınavasoruekleme);
        }

        // GET: /SınavaSoruEkleme/Create
        public ActionResult Create()
        {
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi");
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "Soru1");
            return View();
        }

        // POST: /SınavaSoruEkleme/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SınavaSoruEklemeID,SınavID,SoruID")] SınavaSoruEkleme sınavasoruekleme)
        {
            if (ModelState.IsValid)
            {
                db.SınavaSoruEkleme.Add(sınavasoruekleme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", sınavasoruekleme.SınavID);
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "Soru1", sınavasoruekleme.SoruID);
            return View(sınavasoruekleme);
        }

        // GET: /SınavaSoruEkleme/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SınavaSoruEkleme sınavasoruekleme = db.SınavaSoruEkleme.Find(id);
            if (sınavasoruekleme == null)
            {
                return HttpNotFound();
            }
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", sınavasoruekleme.SınavID);
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "Soru1", sınavasoruekleme.SoruID);
            return View(sınavasoruekleme);
        }

        // POST: /SınavaSoruEkleme/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SınavaSoruEklemeID,SınavID,SoruID")] SınavaSoruEkleme sınavasoruekleme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sınavasoruekleme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", sınavasoruekleme.SınavID);
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "Soru1", sınavasoruekleme.SoruID);
            return View(sınavasoruekleme);
        }

        // GET: /SınavaSoruEkleme/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SınavaSoruEkleme sınavasoruekleme = db.SınavaSoruEkleme.Find(id);
            if (sınavasoruekleme == null)
            {
                return HttpNotFound();
            }
            return View(sınavasoruekleme);
        }

        // POST: /SınavaSoruEkleme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SınavaSoruEkleme sınavasoruekleme = db.SınavaSoruEkleme.Find(id);
            db.SınavaSoruEkleme.Remove(sınavasoruekleme);
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