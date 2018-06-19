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
    public class AdminDonemController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Donem/
        public ActionResult Index()
        {
            var donem = db.Donem.Include(d => d.Bolum);
            return View(donem.ToList());
        }

        // GET: /Donem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donem donem = db.Donem.Find(id);
            if (donem == null)
            {
                return HttpNotFound();
            }
            return PartialView(donem);
        }

        // GET: /Donem/Create
        public ActionResult Create()
        {
            ViewBag.BolumID = new SelectList(db.Bolum, "BolumID", "BolumAdi");
            return View();
        }

        // POST: /Donem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DonemID,DonemAdi,BolumID")] Donem donem)
        {
            if (ModelState.IsValid)
            {
                db.Donem.Add(donem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BolumID = new SelectList(db.Bolum, "BolumID", "BolumAdi", donem.BolumID);
            return View(donem);
        }

        // GET: /Donem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donem donem = db.Donem.Find(id);
            if (donem == null)
            {
                return HttpNotFound();
            }
            ViewBag.BolumID = new SelectList(db.Bolum, "BolumID", "BolumAdi", donem.BolumID);
            return PartialView(donem);
        }

        // POST: /Donem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DonemID,DonemAdi,BolumID")] Donem donem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BolumID = new SelectList(db.Bolum, "BolumID", "BolumAdi", donem.BolumID);
            return PartialView(donem);
        }

        // GET: /Donem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donem donem = db.Donem.Find(id);
            if (donem == null)
            {
                return HttpNotFound();
            }
            return PartialView(donem);
        }

        // POST: /Donem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donem donem = db.Donem.Find(id);
            db.Donem.Remove(donem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void Sil(int id)
        {
            Donem donem = db.Donem.Find(id);

            db.Donem.Remove(donem);

            var d =
                    from details in db.Ders
                    where details.DonemID == id
                    select details;

            foreach (var detail in d)
            {
                db.Ders.Remove(detail);
            }

            var a =
                    from details in db.Konu
                    where details.Ders.DonemID == id
                    select details;

            foreach (var detail in a)
            {
                db.Konu.Remove(detail);
            }

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
