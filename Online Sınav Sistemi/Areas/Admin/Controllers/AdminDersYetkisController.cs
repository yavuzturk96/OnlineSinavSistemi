using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Sınav_Sistemi.Models;

namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
{
    public class AdminDersYetkisController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: Admin/AdminDersYetkis
        public ActionResult Index()
        {
            var dersYetki = db.DersYetki.Include(d => d.Uye).Include(d => d.Sınav);
            return View(dersYetki.ToList());
        }

        // GET: Admin/AdminDersYetkis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersYetki dersYetki = db.DersYetki.Find(id);
            if (dersYetki == null)
            {
                return HttpNotFound();
            }
            return View(dersYetki);
        }

        // GET: Admin/AdminDersYetkis/Create
        public ActionResult Create()
        {
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email");
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi");
            return View();
        }

        // POST: Admin/AdminDersYetkis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DersYetkiID,UyeID,SınavID")] DersYetki dersYetki)
        {
            if (ModelState.IsValid)
            {
                db.DersYetki.Add(dersYetki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", dersYetki.UyeID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", dersYetki.SınavID);
            return View(dersYetki);
        }

        // GET: Admin/AdminDersYetkis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersYetki dersYetki = db.DersYetki.Find(id);
            if (dersYetki == null)
            {
                return HttpNotFound();
            }
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", dersYetki.UyeID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", dersYetki.SınavID);
            return View(dersYetki);
        }

        // POST: Admin/AdminDersYetkis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DersYetkiID,UyeID,SınavID")] DersYetki dersYetki)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dersYetki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", dersYetki.UyeID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", dersYetki.SınavID);
            return View(dersYetki);
        }

        // GET: Admin/AdminDersYetkis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DersYetki dersYetki = db.DersYetki.Find(id);
            if (dersYetki == null)
            {
                return HttpNotFound();
            }
            return View(dersYetki);
        }

        // POST: Admin/AdminDersYetkis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DersYetki dersYetki = db.DersYetki.Find(id);
            db.DersYetki.Remove(dersYetki);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void Sil(int id)
        {
            DersYetki dersYetki = db.DersYetki.Find(id);

            db.DersYetki.Remove(dersYetki);

            //var d =
            //        from details in db.Secenek
            //        where details.SoruID == id
            //        select details;

            //foreach (var detail in d)
            //{
            //    db.Secenek.Remove(detail);
            //}

            db.SaveChanges();

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
