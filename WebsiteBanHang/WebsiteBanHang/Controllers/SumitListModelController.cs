using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class SumitListModelController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: SumitListModel
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(IEnumerable<ChiTietPhieuNhap> Modelist)
        {
            return View();
        }
    }
}