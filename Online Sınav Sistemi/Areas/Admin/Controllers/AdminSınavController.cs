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
    public class AdminSınavController : Controller
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
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi");
            return View();
        }

        // POST: /Sınav/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include= "SınavID,SınavAdi,SınavSüresi,BaslangicTarihi,BitisTarihi,DersID")] Sınav sınav)
        {
            if (ModelState.IsValid)
            {
                Session["YDersID"] = sınav.DersID;
                db.Sınav.Add(sınav);
                db.SaveChanges();
                var x = sınav.DersID;
                int id = Convert.ToInt32(x);
                var ogrenciler = db.UyeDersYetki.Where(a => a.DersID == id);
                DersYetki ders = new DersYetki();
                ders.SınavID = sınav.SınavID;
                foreach (var og in ogrenciler)
                {
                    ders.UyeID = og.UyeID;
                    //ders.SınavID = sınav.SınavID;
                    db.DersYetki.Add(ders);
                }
                
                return RedirectToAction("Index");
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", sınav.DersID);
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
            TempData["İD"] = id;
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
        public ActionResult Edit([Bind(Include= "SınavID,SınavAdi,SınavSüresi,BaslangicTarihi,BitisTarihi,DersID")] Sınav sınav)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sınav).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DersID = new SelectList(db.Ders, "DersID", "DersAdi", sınav.DersID);
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

        [HttpPost]
        public void Sil(int id)
        {
            Sınav sınav = db.Sınav.Find(id);

            db.Sınav.Remove(sınav);

            var d =
                    from details in db.Soru
                    where details.SınavID == id
                    select details;

            foreach (var detail in d)
            {
                db.Soru.Remove(detail);
            }

            var s =
                    from details in db.Secenek
                    where details.Soru.SınavID == id
                    select details;

            foreach (var detail in s)
            {
                db.Secenek.Remove(detail);
            }

            var h =
                   from details in db.Cevap
                   where details.Soru.SınavID == id
                   select details;

            foreach (var detail in h)
            {
                db.Cevap.Remove(detail);
            }
            var y =
                 from details in db.DersYetki
                 where details.SınavID == id
                 select details;

            foreach (var detail in y)
            {
                db.DersYetki.Remove(detail);
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
