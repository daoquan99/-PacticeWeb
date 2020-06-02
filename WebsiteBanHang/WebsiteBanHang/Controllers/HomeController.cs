using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var listDienThoaiMoi = db.SanPhams.Where(x => x.MaLoaiSP == 3 && x.Moi==1 && x.DaXoa==false);
            ViewBag.DienThoaiMoi = listDienThoaiMoi;
            ViewBag.Ten = "Điện thoại mới nhất";

            var listLapTopMoi = db.SanPhams.Where(x => x.MaLoaiSP == 1 && x.Moi == 1 && x.DaXoa == false);
            ViewBag.LapTopMoi = listLapTopMoi;

            var listIPadMoi = db.SanPhams.Where(x => x.MaLoaiSP == 2 && x.Moi == 1 && x.DaXoa == false);
            ViewBag.IPadMoi = listIPadMoi;
            return View();
        }
        public ActionResult DemoLogin()
        {
            return View();
        }

        public ActionResult MenuPartial()
        {
            var listSanPham = db.SanPhams;
            return View(listSanPham);
        }
        public ActionResult DanhMuc()
        {
            var listSanPham = db.SanPhams;
            return View(listSanPham);
        }


        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Thêm Thành công";

                    //db.ThanhViens.Add(tv);
                    //db.SaveChanges();
                }
                else
                    ViewBag.ThongBao = "Thêm Thất bại";
                return View();
            }
            ViewBag.ThongBao = "Sai mã captcha";    

            
            return View();
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thich?");
            lstCauHoi.Add("Môn thể thao mà bạn yêu thich?");
            lstCauHoi.Add("Công việc mà bạn yêu thich?");
            lstCauHoi.Add("Bài hát mà bạn yêu thich?");
            return lstCauHoi;
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string TenTK = f["txtTenDangNhap"].ToString();
            string MatKhau = f["txtMatKhau"].ToString();

            ThanhVien tv1 = db.ThanhViens.SingleOrDefault(x => x.TaiKhoan == TenTK && x.MatKhau==MatKhau);
            if(tv1!=null)
            {
                Session["TaiKhoan"] = tv1;
                return Content("<script>window.location.reload();</script>");
            }
            return Content("<script>alert('Sai tài khoản hoặc mật khẩu')</script>");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
        
    }
}