using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Sınav_Sistemi;
using Online_Sınav_Sistemi.Models;

namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
{
    public class AdminKonuController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Konu/
        public ActionResult Index()
        {
            var konu = db.Konu.Include(k => k.Ders);
            return View(konu.ToList());
        }

        // GET: /Konu/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            return PartialView(konu);
        }

        // GET: /Konu/Create
        public ActionResult Create()
        {
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi");
            return View();
        }

        // POST: /Konu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="KonuID,KonuAdi,DersID")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                db.Konu.Add(konu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", konu.DersID);
            return View(konu);
        }

        // GET: /Konu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", konu.DersID);
            return PartialView(konu);
        }

        // POST: /Konu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="KonuID,KonuAdi,DersID")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(konu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", konu.DersID);
            return PartialView(konu);
        }

        // GET: /Konu/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konu konu = db.Konu.Find(id);
            if (konu == null)
            {
                return HttpNotFound();
            }
            return PartialView(konu);
        }

        // POST: /Konu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Konu konu = db.Konu.Find(id);
            db.Konu.Remove(konu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void Sil(int id)
        {
            Konu konu = db.Konu.Find(id);

            db.Konu.Remove(konu);
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
