using Online_Sınav_Sistemi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Online_Sınav_Sistemi.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        OnlineSınavEntities db = new OnlineSınavEntities();
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Profile(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var uye = db.Uye.Where(m => m.UyeID == id).SingleOrDefault();
            return View(uye);
            
        }

        public ActionResult YeniResim(Uye model, HttpPostedFileBase file)
        {
            var uyeid = Session["UyeID"];
            int uyeID = Convert.ToInt32(uyeid);
            if (!ModelState.IsValid)
            {
                return View();
            }
            Uye uye = db.Uye.Find(uyeID);
            file = Request.Files["ImageUpload"];
            if (file != null && file.FileName != null && file.FileName != "")
            {
                FileInfo fi = new FileInfo(file.FileName);
                if (fi.Extension != ".jpeg" && fi.Extension != ".jpg")
                {
                    TempData["Errormsg"] = "Image File Extension is Not valid";
                    return View(uye);
                }
                else
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/profilePhotos"), pic);
                    file.SaveAs(path);
                    byte[] img = null;
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    img = br.ReadBytes((int)fs.Length);
                    uye.Fotograf = img;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile/" + @Session["UyeID"]);
            }
            return View();
            //İşlemimiz başarıyla biterse, başarılı olduğuna dair bir sayfaya yönlendiriyoruz.


        }

        
    }
}