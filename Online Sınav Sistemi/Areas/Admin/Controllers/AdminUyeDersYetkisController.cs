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
    public class AdminUyeDersYetkisController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: Admin/AdminUyeDersYetkis
        public ActionResult Index()
        {
            var uyeDersYetki = db.UyeDersYetki.Include(u => u.Ders).Include(u => u.Uye);
            return View(uyeDersYetki.ToList());
        }

        // GET: Admin/AdminUyeDersYetkis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UyeDersYetki uyeDersYetki = db.UyeDersYetki.Find(id);
            if (uyeDersYetki == null)
            {
                return HttpNotFound();
            }
            return View(uyeDersYetki);
        }

        // GET: Admin/AdminUyeDersYetkis/Create
        public ActionResult Create()
        {
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi");
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email");
            return View();
        }

        // POST: Admin/AdminUyeDersYetkis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UyeDersYetkiID,UyeID,DersID")] UyeDersYetki uyeDersYetki)
        {
            if (ModelState.IsValid)
            {
                db.UyeDersYetki.Add(uyeDersYetki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", uyeDersYetki.DersID);
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", uyeDersYetki.UyeID);
            return View(uyeDersYetki);
        }

        // GET: Admin/AdminUyeDersYetkis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UyeDersYetki uyeDersYetki = db.UyeDersYetki.Find(id);
            if (uyeDersYetki == null)
            {
                return HttpNotFound();
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", uyeDersYetki.DersID);
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", uyeDersYetki.UyeID);
            return View(uyeDersYetki);
        }

        // POST: Admin/AdminUyeDersYetkis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UyeDersYetkiID,UyeID,DersID")] UyeDersYetki uyeDersYetki)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uyeDersYetki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", uyeDersYetki.DersID);
            ViewBag.UyeID = new SelectList(db.Uye, "UyeID", "Email", uyeDersYetki.UyeID);
            return View(uyeDersYetki);
        }

        // GET: Admin/AdminUyeDersYetkis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UyeDersYetki uyeDersYetki = db.UyeDersYetki.Find(id);
            if (uyeDersYetki == null)
            {
                return HttpNotFound();
            }
            return View(uyeDersYetki);
        }

        // POST: Admin/AdminUyeDersYetkis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UyeDersYetki uyeDersYetki = db.UyeDersYetki.Find(id);
            db.UyeDersYetki.Remove(uyeDersYetki);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void Sil(int id)
        {
            UyeDersYetki uyeDersYetki = db.UyeDersYetki.Find(id);

            db.UyeDersYetki.Remove(uyeDersYetki);

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
