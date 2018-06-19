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
    public class SınavController : Controller
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
            
            if (sınav == null)
            {
                return HttpNotFound();
            }            
            return PartialView(sınav);
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
        public ActionResult Create([Bind(Include= "SınavID,SınavAdi,SınavSüresi,BaslangicTarihi,BitisTarihi")] Sınav sınav)
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
            return PartialView(sınav);
        }

        // POST: /Sınav/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "SınavID,SınavAdi,SınavSüresi,BaslangicTarihi,BitisTarihi")] Sınav sınav)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sınav).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(sınav);
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
            return PartialView(sınav);
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
