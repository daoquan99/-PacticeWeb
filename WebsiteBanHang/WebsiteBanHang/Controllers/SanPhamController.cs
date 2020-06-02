using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;

namespace WebsiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: SanPham
        public ActionResult SanPham1()
        {
            var lstSanPhamLPMoi = db.SanPhams.Where(x => x.MaLoaiSP == 2);
            ViewBag.lstSP = lstSanPhamLPMoi;

            var lstSanPhamDT = db.SanPhams.Where(x => x.MaLoaiSP == 1);
            ViewBag.lstSPDT = lstSanPhamDT;

            var lstSanPhamIpad = db.SanPhams.Where(x => x.MaLoaiSP == 3);
            ViewBag.lstSPIpad = lstSanPhamIpad;

            return View();
        }
        public ActionResult SanPham2()
        {
            var lstSanPhamLPMoi = db.SanPhams.Where(x => x.MaLoaiSP == 2 && x.Moi == 1);
            ViewBag.lstSP = lstSanPhamLPMoi;
            return View();
        }
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            var lstSanPhamLPMoi = db.SanPhams.Where(x => x.MaLoaiSP == 2 && x.Moi == 1);
            return PartialView(lstSanPhamLPMoi);
        }

        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        //xây dựng trang xem chi tiết
        public ActionResult ChiTietSanPham(int? id,string tensp)
        {

            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa==false);
            
            if(sp==null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX,int? page)
        {
            //if(Session["TaiKhoan"]==null || Session["TaiKhoan"].ToString()=="")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if(MaLoaiSP==null || MaNSX==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dsSP = db.SanPhams.Where(x => x.MaLoaiSP == MaLoaiSP && x.MaNSX == MaNSX);
            if(dsSP.Count()==0)
            {
                return HttpNotFound();
            }
            if(Request.HttpMethod!="GET")
            {
                page = 1;
            }    
            //thực hiện chức năng phân trang
            //tạo biến số sản phẩm trên trang
            int pageSize = 6;
            //tạo biến số trang hiện tại
            int pageNumBer = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            return View(dsSP.OrderBy(x=>x.MaSP).ToPagedList(pageNumBer,pageSize));
        }
    }
}