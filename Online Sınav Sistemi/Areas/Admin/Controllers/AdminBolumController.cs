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
    public class AdminBolumController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Bolum/
        public ActionResult Index()
        {
            return View(db.Bolum.ToList());
        }

        // GET: /Bolum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolum bolum = db.Bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            //return View(bolum);
            return PartialView(bolum);
        }

        // GET: /Bolum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Bolum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="BolumID,BolumAdi")] Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                db.Bolum.Add(bolum);
                db.SaveChanges();
                return RedirectToAction("Index");
                // return Redirect("~/Bolum/Index");
                //return PartialView("Index");
            }

            return View(bolum);
           

        }

        // GET: /Bolum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolum bolum = db.Bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            
            return View(bolum);
            //return PartialView(bolum);
        }

        // POST: /Bolum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="BolumID,BolumAdi")] Bolum bolum)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(bolum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bolum);
        }
        
        // GET: /Bolum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolum bolum = db.Bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return PartialView(bolum);
           // return View(bolum);
        }

        // POST: /Bolum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bolum bolum = db.Bolum.Find(id);
            db.Bolum.Remove(bolum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public void Sil(int id)
        {
            Bolum bolum = db.Bolum.Find(id);

            db.Bolum.Remove(bolum);

            var f =
                    from details in db.Donem
                    where details.BolumID == id
                    select details;

            foreach (var detail in f)
            {
                db.Donem.Remove(detail);
            }



            var d =
                    from details in db.Ders
                    where details.Donem.BolumID == id
                    select details;

            foreach (var detail in d)
            {
                db.Ders.Remove(detail);
            }



            var a =
                    from details in db.Konu
                    where details.Ders.Donem.BolumID == id
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
