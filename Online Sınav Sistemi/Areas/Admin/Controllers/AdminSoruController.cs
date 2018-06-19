using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Sınav_Sistemi.Models;
using System.IO;

namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
{
    public class AdminSoruController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        
        public ActionResult Index(int? id)
        {

                int İD = Convert.ToInt32(id);
                Session["IndexID"] = İD;
                //var soru = db.Soru.Include(s => s.Konu).Include(s => s.Sınav);
                var res = db.Soru.Where(x => x.SınavID == İD).ToList();
                //return View(soru.ToList());
               
                return View(res);
            
        }

        // GET: /Soru/Details/5
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

        // GET: /Soru/Create
        public ActionResult Create(int? id)
        {
            int a = Convert.ToInt32(Session["dd"]);
            int yDersID = Convert.ToInt32(Session["YeniDersID"]);
            int sınavID = Convert.ToInt32(id);
            Session["SınavID"] = sınavID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //db.DersYetki.Where(x => x.UyeID == UyeİD).ToList();
            //ViewBag.KonuID = new SelectList(db.Konu.Where(x => x.DersID == a), "KonuID", "KonuAdi");
            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi");
            ViewBag.DogruSecenekID = new SelectList(db.Secenek, "DogruSecenekID", "SecenekAdı");
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi");
            return View();
        }

        // POST: /Soru/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoruID,SınavID,KonuID,Puan,SoruResmi,SoruAdı,DogruCevapID")] Soru soru)
        {
            HttpPostedFileBase file = Request.Files["ImageUpload"];
            if (file != null && file.FileName != null && file.FileName != "")
            {
                FileInfo fi = new FileInfo(file.FileName);
                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
                {
                    TempData["Errormsg"] = "Image File Extension is Not valid";
                    return View(soru);
                }
                else
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), pic);
                    file.SaveAs(path);
                    byte[] img = null;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    img = br.ReadBytes((int)fs.Length);   //Byte değeri img ye atadı.
                    soru.SoruResmi = img;  //img nin içinde bulunan binary değeri veritabanına kaydediyor.
                }
            }

            if (ModelState.IsValid)
            {
                int sınavID = Convert.ToInt32(Session["SınavID"]);
                soru.SınavID = sınavID;
                db.Soru.Add(soru);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["IndexID"] });
            }

            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
            ViewBag.DogruSecenekID = new SelectList(db.Secenek, "DogruSecenekID", "SecenekAdı",soru.DogruCevapID);
            return View(soru);
        }

        // GET: /Soru/Edit/5
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
            //db.DersYetki.Where(x => x.UyeID == UyeİD).ToList();
            ViewBag.KonuID = new SelectList(db.Konu.Where(x => x.DersID == id), "KonuID", "KonuAdi", soru.KonuID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
            return View(soru);
        }

        // POST: /Soru/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoruID,SınavID,KonuID,Puan,SoruResmi,SoruAdı,DogruCevapID")] Soru soru)
        {
            if (ModelState.IsValid)
            {
                db.Entry(soru).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["IndexID"] });
            }
            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
            return View(soru);
        }

        // GET: /Soru/Delete/5
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

        // POST: /Soru/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Soru soru = db.Soru.Find(id);
            db.Soru.Remove(soru);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = Session["IndexID"] });
        }

        [HttpPost]
        public void Sil(int id)
        {
            Soru soru = db.Soru.Find(id);

            db.Soru.Remove(soru);

            var d =
                    from details in db.Secenek
                    where details.SoruID == id
                    select details;

            foreach (var detail in d)
            {
                db.Secenek.Remove(detail);
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


//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using Online_Sınav_Sistemi.Models;
//using System.IO;

//namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
//{
//    public class AdminSoruController : Controller
//    {
//        private OnlineSınavEntities db = new OnlineSınavEntities();

//        // GET: /Soru/
//        public ActionResult Index()
//        {
//            var soru = db.Soru.Include(s => s.Konu).Include(s => s.Sınav);
//            return View(soru.ToList());
//        }

//        // GET: /Soru/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Soru soru = db.Soru.Find(id);
//            if (soru == null)
//            {
//                return HttpNotFound();
//            }
//            return View(soru);
//        }

//        // GET: /Soru/Create
//        public ActionResult Create()
//        {
//            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi");
//            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi");
//            ViewBag.DogruCevapID = new SelectList(db.Secenek, "SecenekID", "SecenekAdı");
//            return View();
//        }

//        // POST: /Soru/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "SoruID,SınavID,KonuID,Puan,SoruResmi,SoruAdı,DogruCevapID")] Soru soru)
//        {
//            HttpPostedFileBase file = Request.Files["ImageUpload"];
//            if (file != null && file.FileName != null && file.FileName != "")
//            {
//                FileInfo fi = new FileInfo(file.FileName);
//                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
//                {
//                    TempData["Errormsg"] = "Image File Extension is Not valid";
//                    return View(soru);
//                }
//                else
//                {
//                    string pic = System.IO.Path.GetFileName(file.FileName);
//                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), pic);
//                    file.SaveAs(path);
//                    byte[] img = null;
//                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
//                    BinaryReader br = new BinaryReader(fs);
//                    img = br.ReadBytes((int)fs.Length);
//                    soru.SoruResmi = img;
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                db.Soru.Add(soru);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
//            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
//            ViewBag.DogruCevapID = new SelectList(db.Secenek, "SecenekID", "SecenekAdı",soru.DogruCevapID);
//            return View(soru);
//        }

//        // GET: /Soru/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Soru soru = db.Soru.Find(id);
//            if (soru == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
//            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
//            return View(soru);
//        }

//        // POST: /Soru/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include="SoruID,SınavID,KonuID,Puan,SoruResmi,SoruAdı,DogruCevapID")] Soru soru)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(soru).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.KonuID = new SelectList(db.Konu, "KonuID", "KonuAdi", soru.KonuID);
//            ViewBag.SınavID = new SelectList(db.Sınav, "SınavID", "SınavAdi", soru.SınavID);
//            return View(soru);
//        }

//        // GET: /Soru/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Soru soru = db.Soru.Find(id);
//            if (soru == null)
//            {
//                return HttpNotFound();
//            }
//            return View(soru);
//        }

//        // POST: /Soru/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Soru soru = db.Soru.Find(id);
//            db.Soru.Remove(soru);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//    }
//}