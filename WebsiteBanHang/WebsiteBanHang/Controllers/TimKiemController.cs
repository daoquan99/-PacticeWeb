using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;

namespace WebsiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa,int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //thực hiện chức năng phân trang
            //tạo biến số sản phẩm trên trang
            int pageSize = 6;
            //tạo biến số trang hiện tại
            int pageNumBer = (page ?? 1);
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(x => x.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(x=>x.TenSP).ToPagedList(pageNumBer,pageSize));
        }
        //[HttpPost]
        //public ActionResult KQTimKiem(string sTuKhoa, int? page,FormCollection f)
        //{
        //    if (Request.HttpMethod != "GET")
        //    {
        //        page = 1;
        //    }
        //    //thực hiện chức năng phân trang
        //    //tạo biến số sản phẩm trên trang
        //    int pageSize = 6;
        //    //tạo biến số trang hiện tại
        //    int pageNumBer = (page ?? 1);
        //    //Tìm kiếm theo tên sản phẩm
        //    var lstSP = db.SanPhams.Where(x => x.TenSP.Contains(sTuKhoa));
        //    ViewBag.TuKhoa = sTuKhoa;
        //    return View(lstSP.OrderBy(x => x.TenSP).ToPagedList(pageNumBer, pageSize));
        //}

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            //gọi về hàm get tìm kiếm
            return RedirectToAction("KQTimKiem",new { @sTuKhoa=sTuKhoa});
        }
        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {

            var lstSP = db.SanPhams.Where(x => x.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstSP.OrderBy(x=>x.DonGia));
        }
    }
}