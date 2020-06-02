using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();   
        // GET: KhachHang
        public ActionResult KhachHang()
        {
            var dsKH = db.KhachHangs;
            return View(dsKH);
        }
    }
}