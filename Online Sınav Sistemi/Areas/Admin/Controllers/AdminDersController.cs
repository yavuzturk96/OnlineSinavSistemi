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
    public class AdminDersController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Ders/
        public ActionResult Index()
        {
            var ders = db.Ders.Include(d => d.Donem);
            return View(ders.ToList());
        }

        // GET: /Ders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ders ders = db.Ders.Find(id);
            if (ders == null)
            {
                return HttpNotFound();
            }
            return PartialView(ders);
        }

        // GET: /Ders/Create
        public ActionResult Create()
        {
            ViewBag.DonemID = new SelectList(db.Donem, "DonemID", "DonemAdi");
            return View();
        }

        // POST: /Ders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DersID,DersAdi,DonemID")] Ders ders)
        {
            if (ModelState.IsValid)
            {
                db.Ders.Add(ders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DonemID = new SelectList(db.Donem, "DonemID", "DonemAdi", ders.DonemID);
            return View(ders);
        }

        // GET: /Ders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ders ders = db.Ders.Find(id);
            if (ders == null)
            {
                return HttpNotFound();
            }
            ViewBag.DonemID = new SelectList(db.Donem, "DonemID", "DonemAdi", ders.DonemID);
            return PartialView(ders);
        }

        // POST: /Ders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DersID,DersAdi,DonemID")] Ders ders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DonemID = new SelectList(db.Donem, "DonemID", "DonemAdi", ders.DonemID);
            return PartialView(ders);
        }

        // GET: /Ders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ders ders = db.Ders.Find(id);
            if (ders == null)
            {
                return HttpNotFound();
            }
            return PartialView(ders);
        }

        // POST: /Ders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ders ders = db.Ders.Find(id);
            db.Ders.Remove(ders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void Sil(int id)
        {
            Ders ders = db.Ders.Find(id);

            db.Ders.Remove(ders);

            var d =
                    from details in db.Konu
                    where details.DersID == id
                    select details;

            foreach (var detail in d)
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
