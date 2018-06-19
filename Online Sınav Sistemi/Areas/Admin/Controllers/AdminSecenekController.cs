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
//    public class AdminSecenekController : Controller
//    {
//        private OnlineSınavEntities db = new OnlineSınavEntities();

//        // GET: /Secenek/
//        public ActionResult Index()
//        {
//            var secenek = db.Secenek.Include(s => s.Soru);
//            return View(secenek.ToList());
//        }

//        // GET: /Secenek/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Secenek secenek = db.Secenek.Find(id);
//            if (secenek == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(secenek);
//        }

//        // GET: /Secenek/Create
//        public ActionResult Create()
//        {
//            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı");
//            return View();
//        }

//        // POST: /Secenek/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include="SecenekID,SoruID,SecenekAdı,SecenekResmi")] Secenek secenek)
//        {
//            HttpPostedFileBase file = Request.Files["ImageUpload"];
//            if (file != null && file.FileName != null && file.FileName != "")
//            {
//                FileInfo fi = new FileInfo(file.FileName);
//                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
//                {
//                    TempData["Errormsg"] = "Image File Extension is Not valid";
//                    return View(secenek);
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
//                    secenek.SecenekResmi = img;
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                db.Secenek.Add(secenek);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
//            return View(secenek);
//        }

//        // GET: /Secenek/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Secenek secenek = db.Secenek.Find(id);
//            if (secenek == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
//            return PartialView(secenek);
//        }

//        // POST: /Secenek/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include="SecenekID,SoruID,SecenekAdı,SecenekResmi")] Secenek secenek)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(secenek).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
//            return PartialView(secenek);
//        }

//        // GET: /Secenek/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Secenek secenek = db.Secenek.Find(id);
//            if (secenek == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(secenek);
//        }

//        // POST: /Secenek/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Secenek secenek = db.Secenek.Find(id);
//            db.Secenek.Remove(secenek);
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
    public class AdminSecenekController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        // GET: /Secenek/
        public ActionResult Index(int? id)
        {
            //var secenek = db.Secenek.Include(s => s.Soru);
            //return View(secenek.ToList());
            int İD = Convert.ToInt32(id);
            Session["SecenekIndexID"] = İD;
            var res = db.Secenek.Where(x => x.SoruID == İD).ToList();
            return View(res);
        }

        // GET: /Secenek/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secenek secenek = db.Secenek.Find(id);
            //if (secenek == null)
            //{
            //    return HttpNotFound();
            //}
            return PartialView(secenek);
        }

        // GET: /Secenek/Create
        public ActionResult Create(int? id)
        {
            
            int soruID = Convert.ToInt32(id);
            Session["SoruID"] = soruID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            //Secenek secenek = db.Secenek.Find(id);
            //if (secenek == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı");
            return View();
            //return View();
        }

        // POST: /Secenek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SecenekID,SoruID,SecenekAdı,SecenekResmi")] Secenek secenek)
        {
            HttpPostedFileBase file = Request.Files["ImageUpload"];
            if (file != null && file.FileName != null && file.FileName != "")
            {
                FileInfo fi = new FileInfo(file.FileName);
                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
                {
                    TempData["Errormsg"] = "Image File Extension is Not valid";
                    return View(secenek);
                }
                else
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), pic);
                    file.SaveAs(path);
                    byte[] img = null;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    img = br.ReadBytes((int)fs.Length);
                    secenek.SecenekResmi = img;
                }
            }
           
            if (ModelState.IsValid)
            {
                int soruID=Convert.ToInt32(Session["SoruID"]);
                secenek.SoruID =soruID;
                db.Secenek.Add(secenek);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["SecenekIndexID"] });
            }

            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
            return View(secenek);
        }

        // GET: /Secenek/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secenek secenek = db.Secenek.Find(id);
            if (secenek == null)
            {
                return HttpNotFound();
            }
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
            return PartialView(secenek);
        }

        // POST: /Secenek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SecenekID,SoruID,SecenekAdı,SecenekResmi")] Secenek secenek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(secenek).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = Session["SecenekIndexID"] });
            }
            ViewBag.SoruID = new SelectList(db.Soru, "SoruID", "SoruAdı", secenek.SoruID);
            return PartialView(secenek);
        }

        // GET: /Secenek/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secenek secenek = db.Secenek.Find(id);
            if (secenek == null)
            {
                return HttpNotFound();
            }
            return PartialView(secenek);
        }

        // POST: /Secenek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Secenek secenek = db.Secenek.Find(id);
            db.Secenek.Remove(secenek);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = Session["SecenekIndexID"] });
        }

        [HttpPost]
        public void Sil(int id)
        {
            Secenek secenek = db.Secenek.Find(id);

            db.Secenek.Remove(secenek);

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
        [System.Web.Services.WebMethod]
        public JsonResult secenekDegistir(int checked_option_radio, int SoruID)
        {
            System.Data.SqlClient.SqlConnection baglanti;
            baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Update Soru set DogruCevapID=" + checked_option_radio + " Where SoruID=" + SoruID + "", baglanti);
                    baglanti.Open();
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
         
            return Json(false, JsonRequestBehavior.AllowGet);
        }

    }
}
