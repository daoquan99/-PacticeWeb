using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyPhieuNhap

        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSP = db.SanPhams;

            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap pn,IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSP = db.SanPhams;

            return View();
        }
    }
}