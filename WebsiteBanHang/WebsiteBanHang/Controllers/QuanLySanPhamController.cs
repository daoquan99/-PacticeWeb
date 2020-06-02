using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLySanPham
        public ActionResult Index()
        {
            var lstSanPham = db.SanPhams.Where(x => x.DaXoa == false);
            return View(lstSanPham);
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(x => x.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(x => x.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        SanPham ktraHinhAnhDauVao(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {
            int loi = 0;
            for(int i=0;i<HinhAnh.Count();i++)
            {
                if(HinhAnh[i] !=null)
                {
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.UpLoad1 = "Hình ảnh " + i + " không hợp lệ!";
                            loi++;
                            break;
                        }
                        else
                        {
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.Upload = "Hình ảnh " + i + " đã tồn tại!";
                                loi++;
                                break;
                            }
                        }
                    }
                }   
            }
            if (loi > 0) return null;
            ThemHinhAnhVaoFile(HinhAnh);
            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh1 = HinhAnh[1].FileName;
            sp.HinhAnh2 = HinhAnh[2].FileName;
            sp.HinhAnh3 = HinhAnh[3].FileName;
            sp.DaXoa = false;
            return sp;
        }
        int KtraLoiHinhAnh(HttpPostedFileBase[] HinhAnh)
        {
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.UpLoad1 = "Hình ảnh " + i + " không hợp lệ!";
                            loi++;
                            return loi;
                        }
                        else
                        {
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.Upload = "Hình ảnh " + i + " đã tồn tại!";
                                loi++;
                                return loi;
                            }
                        }
                    }
                }
            }
            return loi;
        }
        void ThemHinhAnhVaoFile(HttpPostedFileBase[] HinhAnh)
        {
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                var fileName = Path.GetFileName(HinhAnh[i].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                HinhAnh[i].SaveAs(path);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh/*, HttpPostedFileBase HinhAnh1, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3*/)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(x => x.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(x => x.MaNSX), "MaNSX", "TenNSX");

            sp = ktraHinhAnhDauVao(sp, HinhAnh);
            if(sp==null)
            {
                return View(sp);
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            
            //load dropdownlist
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(x => x.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(x => x.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(x => x.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(x => x.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            sp = ktraHinhAnhDauVao(sp, HinhAnh);
            if (sp == null) return View(sp);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //load dropdownlist
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(x => x.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(x => x.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if(sp==null)
            {
                return HttpNotFound();
            }
            sp.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}