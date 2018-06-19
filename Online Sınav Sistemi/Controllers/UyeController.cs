using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Online_Sınav_Sistemi.Models;
using System.IO;

namespace Online_Sınav_Sistemi.Controllers
{
    public class UyeController : Controller
    {

        private OnlineSınavEntities db = new OnlineSınavEntities();

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
                return RedirectToAction("Profile/"+@Session["UyeID"]);
            }
            return View();
            //İşlemimiz başarıyla biterse, başarılı olduğuna dair bir sayfaya yönlendiriyoruz.
           
            
        }
        public ActionResult Profile(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var uye = db.Uye.Where(m => m.UyeID == id).SingleOrDefault();

            return View(uye);
            
        }
        // GET: Uye
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uye.Where(u => u.Numara == uye.Numara).SingleOrDefault();
            if (login.Numara == uye.Numara && login.Sifre == uye.Sifre)
            {
                Session["UyeID"] = login.UyeID;
                Session["Numara"] = login.Numara;
                Session["YetkiID"] = login.YetkiID;
                Session["YetkiAdi"] = login.Ad;
                Session["YetkiSoyadı"] = login.Soyad;
                Session["YetkiEmail"] = login.Email;
                Session["Resim"] = login.Fotograf;
                if (login.YetkiID == 1) { return Redirect("~/Admin/AdminHome/Profile/"+@Session["UyeID"]); }
                else if (login.YetkiID == 2) { return Redirect("~/Admin/AdminHome/Profile/" + @Session["UyeID"]); /*RedirectToAction("Index", "Uye");*/ }               
                else return Redirect("~/Uye/Profile/"+@Session["UyeID"]); /*RedirectToAction("Index", "Home");*/

            }
            else
            {
                ViewBag.Uyarı = "Numara ya da Şifrenizi kontrol ediniz!";
                return View();
            }

        }

        
        [HttpPost]
        public ActionResult YeniUyelik(Uye model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            
 
            Uye uye = new Uye();
            foreach (var item in db.Uye)
            {
                if (item.Email == model.Email)
                {
                    ViewBag.Uyarı = "Böyle bir E-Posta mevcut lütfen farklı bir E-Posta giriniz.";
                    return View();
                }
                else uye.Email = model.Email;
                uye.Sifre = model.Sifre;
                uye.Ad = model.Ad;
                uye.Soyad = model.Soyad;
                if (item.Numara == model.Numara)
                {
                    ViewBag.Uyarı = "Böyle bir Numara mevcut lütfen farklı bir Numara giriniz.";
                    return View();
                }
                else uye.Numara = model.Numara;
            }

            uye.YetkiID = 3;
                db.Uye.Add(uye);
                db.SaveChanges();

                //İşlemimiz başarıyla biterse, başarılı olduğuna dair bir sayfaya yönlendiriyoruz.
                return Redirect("~/Uye/Login");
            
        }

        public ActionResult Logout()
        {
            Session["UyeID"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "Uye");
        }
        
    }
}