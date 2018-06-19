using Online_Sınav_Sistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Online_Sınav_Sistemi.Controllers
{
    public class HomeController : Controller
    {
        private OnlineSınavEntities db = new OnlineSınavEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Mesajlar()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Sınavlar()
        {
            var uyeid = Session["UyeID"];
            int UyeİD = Convert.ToInt32(uyeid);
            //var dersYetki = db.DersYetki.Where(a => a.UyeID).Include(d => d.Uye);
            var res = db.DersYetki.Where(x => x.UyeID == UyeİD).ToList();
            return View(res);
            //return View(db.Sınav.ToList());

        }



        public ActionResult Sınav(int? id)
        {
            var uyeid = Session["UyeID"];
            int UyeİD = Convert.ToInt32(uyeid);
            if (id == null)
                return HttpNotFound();

            ExamViewModel model = new ExamViewModel() { Id = id.Value };

            var sınav = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
            var tik = db.DersYetki.Where(m => m.SınavID == id && m.UyeID == UyeİD).SingleOrDefault();
            var exam = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
            Session["DersYetkiID"] = tik.DersYetkiID;
            //var dersyetki = db.DersYetki.Where(m => m.SınavID == id).SingleOrDefault();
            int v = Convert.ToInt32(tik.DersYetkiID);


            if (exam != null)
            {
                //dmodel.tTarih = dersyetki.TikTarih;
                model.Duration = exam.SınavSüresi.Value;
                model.Title = exam.SınavAdi;
                model.BasTarihi = exam.BaslangicTarihi;
                model.BitTarihi = exam.BitisTarihi;
                model.TikTarihi = tik.TikTarih;
                model.Questions = (from q in db.Soru.Where(a => a.SınavID == id)
                                   select new QuestionViewModel()
                                   {
                                       Id = q.SoruID,
                                       ExamId = q.SınavID.Value,
                                       Title = q.SoruAdı,
                                       Point = q.Puan.Value,
                                       Image = q.SoruResmi,
                                       RightChoiceId = q.DogruCevapID.Value,
                                       Choices = db.Secenek.Where(a => a.SoruID == q.SoruID).ToList(),
                                       //dersYetkis=db.DersYetki.Where(b=>b.SınavID==q.SınavID && b.UyeID== UyeİD).ToList()
                                   }).ToList();
                //model.Questions = db.Soru.Where(a => a.SınavID == model.Id).ToList();

            }
            else
                return HttpNotFound();

            if (sınav == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [System.Web.Services.WebMethod]
        public JsonResult secenekDegistir(int checked_option_radio, int SoruID)
        {
            var uyeid = Session["UyeID"];
            int uyeID = Convert.ToInt32(uyeid);
            System.Data.SqlClient.SqlConnection baglanti;
            baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select * from Cevap where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
            baglanti.Open();
            cmd.Connection = baglanti;
            int cevapVarmı = Convert.ToInt32(cmd.ExecuteScalar());
            baglanti.Close();
            baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
            cmd = new System.Data.SqlClient.SqlCommand("Select * from Soru where SoruID=" + SoruID + " and DogruCevapID=" + checked_option_radio + "", baglanti);
            baglanti.Open();
            cmd.Connection = baglanti;
            int dogruCevapMı = Convert.ToInt32(cmd.ExecuteScalar());
            baglanti.Close();
            baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
            cmd = new System.Data.SqlClient.SqlCommand("Select Puan from Soru where SoruID=" + SoruID +"", baglanti);
            baglanti.Open();
            cmd.Connection = baglanti;
            int Puan = Convert.ToInt32(cmd.ExecuteScalar());
            baglanti.Close();
            if (cevapVarmı == 0)
            {
                //if (checked_option_radio == null)
                //{
                //    return Json(true, JsonRequestBehavior.AllowGet);

                //}
                if (dogruCevapMı == 0)
                {
                    db.Cevap.Add(new Cevap { UyeID = Convert.ToInt32(uyeid), SoruID = SoruID, SecilenCevapID = checked_option_radio, Puan = Puan, DogruCevapMı = 0 });
                }
                else
                {
                    db.Cevap.Add(new Cevap { UyeID = Convert.ToInt32(uyeid), SoruID = SoruID, SecilenCevapID = checked_option_radio, Puan = Puan, DogruCevapMı = 1 });
                }
                db.SaveChanges();

            }
            else
            {
                if (dogruCevapMı == 0)
                {
                    baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
                    cmd = new System.Data.SqlClient.SqlCommand("Update Cevap set SecilenCevapID=" + checked_option_radio + ", DogruCevapMı=" + dogruCevapMı + "  Where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
                    baglanti.Open();
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
                }
                else
                {
                    baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
                    cmd = new System.Data.SqlClient.SqlCommand("Update Cevap set SecilenCevapID=" + checked_option_radio + ", DogruCevapMı=" + 1 + " Where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
                    baglanti.Open();
                    cmd.ExecuteNonQuery();
                    baglanti.Close();
                }

            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cevaplar()
        {
            return View(db.Sınav.ToList());
        }
        public ActionResult CevapDetay(int? id)
        {
            if (id == null)
                return HttpNotFound();
            ExamViewModel model = new ExamViewModel() { Id = id.Value };
            var sınav = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
            var exam = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
            if (exam != null)
            {
                model.Id = exam.SınavID;
                model.Duration = exam.SınavSüresi.Value;
                model.Title = exam.SınavAdi;
                model.Questions = (from q in db.Soru.Where(a => a.SınavID == id)
                                   select new QuestionViewModel()
                                   {
                                       Id = q.SoruID,
                                       ExamId = q.SınavID.Value,
                                       Title = q.SoruAdı,
                                       Point = q.Puan.Value,
                                       RightChoiceId = q.DogruCevapID.Value,
                                       Choices = db.Secenek.Where(a => a.SoruID == q.SoruID).ToList(),
                                       Answers = db.Cevap.Where(a => a.SoruID == q.SoruID).ToList()
                                   }).ToList();

                //model.Questions = db.Soru.Where(a => a.SınavID == model.Id).ToList();
            }
            else
                return HttpNotFound();
            return View(model);
        }
        public ActionResult CevapDetayOgrenci(int? id)
        {
            var cevaplar = db.Cevap.Where(m => m.UyeID == id).ToList();
            return View(cevaplar);
        }

        [System.Web.Services.WebMethod]
        public JsonResult TikTarihKaydet(int dersYetkiID)
        {



            var uyeid = Session["UyeID"];
            int uyeID = Convert.ToInt32(uyeid);
            System.Data.SqlClient.SqlConnection baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
            baglanti.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select TikTarih from DersYetki where DersYetkiID=" + dersYetkiID + "", baglanti);
            cmd.Connection = baglanti;
            System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
            System.Collections.ArrayList TıklanmaTarihi = new System.Collections.ArrayList();

            while (dr.Read())
            {
                TıklanmaTarihi.Add(dr["TikTarih"]);
            }
            dr.Close();
            baglanti.Close();
            string tık = Convert.ToString(TıklanmaTarihi[0]);
            if (tık == "")
            {
                DateTime zaman = Convert.ToDateTime(DateTime.Now);
                //DateTime z = DateTime.Now;

                baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
                cmd = new System.Data.SqlClient.SqlCommand("Update DersYetki set TikTarih=@tarih Where DersYetkiID=" + dersYetkiID + "", baglanti);
                cmd.Parameters.AddWithValue("@tarih", zaman);
                baglanti.Open();
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }



            return Json(false, JsonRequestBehavior.AllowGet);


        }


        [System.Web.Services.WebMethod]
        public JsonResult TikDurumKaydet()
        {

            var DersYetkiID = Session["DersYetkiID"];
            int dersYetkiID = Convert.ToInt32(DersYetkiID);
            var uyeid = Session["UyeID"];
            int uyeID = Convert.ToInt32(uyeid);
            System.Data.SqlClient.SqlConnection baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
            baglanti.Open();
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select Durum from DersYetki where DersYetkiID=" + dersYetkiID + "", baglanti);
            cmd.Connection = baglanti;
            System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
            System.Collections.ArrayList Durum = new System.Collections.ArrayList();


            while (dr.Read())
            {
                Durum.Add(dr["Durum"]);
            }
            dr.Close();
            baglanti.Close();
            string tık = Convert.ToString(Durum[0]);
            if (tık == "")
            {
                DateTime zaman = Convert.ToDateTime(DateTime.Now);
                //DateTime z = DateTime.Now;
                string durum = "Tamamlandı";
                baglanti = new System.Data.SqlClient.SqlConnection(@"server=DESKTOP-TOA0HSB\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
                cmd = new System.Data.SqlClient.SqlCommand("Update DersYetki set Durum=@tarih Where DersYetkiID=" + dersYetkiID + "", baglanti);
                cmd.Parameters.AddWithValue("@tarih", durum);
                baglanti.Open();
                cmd.ExecuteNonQuery();
                baglanti.Close();
            }



            return Json(false, JsonRequestBehavior.AllowGet);


        }


    }
}


//using Online_Sınav_Sistemi.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;


//namespace Online_Sınav_Sistemi.Controllers
//{
//    public class HomeController : Controller
//    {
//        private OnlineSınavEntities db = new OnlineSınavEntities();

//        public ActionResult Index()
//        {
//            return View(db.Soru.ToList());
//        }

//        public ActionResult Mesajlar()
//        {
//            ViewBag.Message = "Your application description page.";

//            return View();
//        }

//        public ActionResult Sınavlar()
//        {
//            var uyeid = Session["UyeID"];
//            int UyeİD = Convert.ToInt32(uyeid);
//            //var dersYetki = db.DersYetki.Where(a => a.UyeID).Include(d => d.Uye);
//            var res = db.DersYetki.Where(x => x.UyeID == UyeİD).ToList();
//            return View(res);
//            //return View(db.Sınav.ToList());

//        }



//        public ActionResult Sınav(int? id)
//        {
//            if (id == null)
//                return HttpNotFound();

//            ExamViewModel model = new ExamViewModel() { Id = id.Value };
//            var sınav = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
//            var exam = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
//            if (exam != null)
//            {
//                model.Duration = exam.SınavSüresi.Value;
//                model.Title = exam.SınavAdi;
//                model.Questions = (from q in db.Soru.Where(a => a.SınavID == id)
//                                   select new QuestionViewModel()
//                                   {
//                                       Id = q.SoruID,
//                                       ExamId = q.SınavID.Value,
//                                       Title = q.SoruAdı,
//                                       Point = q.Puan.Value,
//                                       Image =q.SoruResmi,
//                                       RightChoiceId = q.DogruCevapID.Value,
//                                       Choices = db.Secenek.Where(a => a.SoruID == q.SoruID).ToList()
//                                   }).ToList();
//                //model.Questions = db.Soru.Where(a => a.SınavID == model.Id).ToList();
//            }
//            else
//                return HttpNotFound();

//            if (sınav == null)
//            {
//                return HttpNotFound();
//            }
//            return View(model);
//        }

//        [System.Web.Services.WebMethod]
//        public JsonResult secenekDegistir(int checked_option_radio, int SoruID)
//        {
//            var uyeid = Session["UyeID"];
//            int uyeID = Convert.ToInt32(uyeid);
//            System.Data.SqlClient.SqlConnection baglanti;
//            baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select * from Cevap where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
//            baglanti.Open();
//            cmd.Connection = baglanti;
//            int cevapVarmı = Convert.ToInt32(cmd.ExecuteScalar());
//            baglanti.Close();
//            baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//            cmd = new System.Data.SqlClient.SqlCommand("Select * from Soru where SoruID=" + SoruID + " and DogruCevapID=" + checked_option_radio + "", baglanti);
//            baglanti.Open();
//            cmd.Connection = baglanti;
//            int dogruCevapMı = Convert.ToInt32(cmd.ExecuteScalar());
//            baglanti.Close();
//            if (cevapVarmı == 0)
//            {
//                //if (checked_option_radio == null)
//                //{
//                //    return Json(true, JsonRequestBehavior.AllowGet);

//                //}
//                if (dogruCevapMı == 0)
//                {
//                    db.Cevap.Add(new Cevap { UyeID = Convert.ToInt32(uyeid), SoruID = SoruID, SecilenCevapID = checked_option_radio, Puan = 10, DogruCevapMı = 0 });
//                }
//                else
//                {
//                    db.Cevap.Add(new Cevap { UyeID = Convert.ToInt32(uyeid), SoruID = SoruID, SecilenCevapID = checked_option_radio, Puan = 10, DogruCevapMı = 1 });
//                }
//                db.SaveChanges();

//            }
//            else
//            {
//                if (dogruCevapMı == 0)
//                {
//                    baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//                    cmd = new System.Data.SqlClient.SqlCommand("Update Cevap set SecilenCevapID=" + checked_option_radio + ", DogruCevapMı=" + dogruCevapMı + "  Where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
//                    baglanti.Open();
//                    cmd.ExecuteNonQuery();
//                    baglanti.Close();
//                }
//                else
//                {
//                    baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//                    cmd = new System.Data.SqlClient.SqlCommand("Update Cevap set SecilenCevapID=" + checked_option_radio + ", DogruCevapMı=" + 1 + " Where SoruID=" + SoruID + " and UyeID=" + uyeID + "", baglanti);
//                    baglanti.Open();
//                    cmd.ExecuteNonQuery();
//                    baglanti.Close();
//                }

//            }

//            return Json(false, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult Cevaplar()
//        {
//            return View(db.Sınav.ToList());
//        }
//        public ActionResult CevapDetay(int? id)
//        {
//            if (id == null)
//                return HttpNotFound();
//            ExamViewModel model = new ExamViewModel() { Id = id.Value };
//            var sınav = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
//            var exam = db.Sınav.Where(m => m.SınavID == id).SingleOrDefault();
//            if (exam != null)
//            {
//                model.Id = exam.SınavID;
//                model.Duration = exam.SınavSüresi.Value;
//                model.Title = exam.SınavAdi;
//                model.Questions = (from q in db.Soru.Where(a => a.SınavID == id)
//                                   select new QuestionViewModel()
//                                   {
//                                       Id = q.SoruID,
//                                       ExamId = q.SınavID.Value,
//                                       Title = q.SoruAdı,
//                                       Point = q.Puan.Value,
//                                       RightChoiceId = q.DogruCevapID.Value,
//                                       Choices = db.Secenek.Where(a => a.SoruID == q.SoruID).ToList(),
//                                       Answers = db.Cevap.Where(a => a.SoruID == q.SoruID).ToList()
//                                   }).ToList();

//                //model.Questions = db.Soru.Where(a => a.SınavID == model.Id).ToList();
//            }
//            else
//                return HttpNotFound();
//            return View(model);
//        }
//        public ActionResult CevapDetayOgrenci(int? id)
//        {
//            var cevaplar = db.Cevap.Where(m => m.UyeID == id).ToList();
//            return View(cevaplar);
//        }

//        [System.Web.Services.WebMethod]
//        public JsonResult TikTarihKaydet(int dersYetkiID)
//        {

//            var uyeid = Session["UyeID"];
//            int uyeID = Convert.ToInt32(uyeid);
//                System.Data.SqlClient.SqlConnection baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//                baglanti.Open();
//                //cmd = new System.Data.SqlClient.SqlCommand("Select * from soru where SoruID in(select SoruID from SınavaSoruEkleme where SınavID in (select SınavID from Sınav where SınavID=" + Model.SınavID + "))", baglanti);
//                //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select DISTINCT UyeID from Cevap", baglanti);
//                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("Select TikTarih from DersYetki where DersYetkiID=" + dersYetkiID + "", baglanti);
//                cmd.Connection = baglanti;
//                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
//                System.Collections.ArrayList TıklanmaTarihi = new System.Collections.ArrayList();

//                    while (dr.Read())
//                    {
//                        TıklanmaTarihi.Add(dr["TikTarih"]);
//                    }
//                dr.Close();
//                baglanti.Close();
//                string tık=Convert.ToString(TıklanmaTarihi[0]);
//                if (tık == "")
//                {
//                    DateTime zaman = Convert.ToDateTime(DateTime.Now);
//                    //DateTime z = DateTime.Now;

//                    baglanti = new System.Data.SqlClient.SqlConnection(@"server=.\SQLEXPRESS;database=OnlineSınav;Trusted_Connection=yes");
//                    cmd = new System.Data.SqlClient.SqlCommand("Update DersYetki set TikTarih=@tarih Where DersYetkiID=" + dersYetkiID + "", baglanti);
//                    cmd.Parameters.AddWithValue("@tarih", zaman);
//                    baglanti.Open();
//                    cmd.ExecuteNonQuery();
//                    baglanti.Close();
//                }



//            return Json(false, JsonRequestBehavior.AllowGet);

//        }


//    }
//}