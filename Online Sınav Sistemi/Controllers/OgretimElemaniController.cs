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

namespace Online_Sınav_Sistemi.Controllers
{
    public class OgretimElemaniController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /OgretimElemani/
        public ActionResult Index()
        {
            var soru = db.Soru.Include(s => s.Konu);
            return View(soru.ToList());
        }

        // GET: /OgretimElemani/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Soru soru = db.Soru.Find(id);
            if (soru == null)
            {
                return HttpNotFound();
            }
            return View(soru);
        }

        // GET: /OgretimElemani/Create
        public ActionResult Create()
        {
            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi");
            return View();
        }

        // POST: /OgretimElemani/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SoruID,KonuID,SoruResmi,Soru1,Cevap,CevapA,CevapB,CevapC,CevapD,CevapE")] Soru soru)
        {
            if (ModelState.IsValid)
            {
                db.Soru.Add(soru);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
            return View(soru);
        }

        // GET: /OgretimElemani/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Soru soru = db.Soru.Find(id);
            if (soru == null)
            {
                return HttpNotFound();
            }
            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
            return View(soru);
        }

        // POST: /OgretimElemani/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SoruID,KonuID,SoruResmi,Soru1,Cevap,CevapA,CevapB,CevapC,CevapD,CevapE")] Soru soru)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soru).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
            return View(soru);
        }

        // GET: /OgretimElemani/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Soru soru = db.Soru.Find(id);
            if (soru == null)
            {
                return HttpNotFound();
            }
            return View(soru);
        }

        // POST: /OgretimElemani/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Soru soru = db.Soru.Find(id);
            db.Soru.Remove(soru);
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
