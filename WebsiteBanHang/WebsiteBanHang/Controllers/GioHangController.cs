using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang==null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;

        }
        public ActionResult ThemGioHang(int MaSP,string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == MaSP);
            if(sp==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang checkSP = lstGioHang.SingleOrDefault(x => x.MaSP == MaSP);
            if(checkSP!=null)
            {
                if(sp.SoLuongTon <= checkSP.SoLuong)
                {
                    return View("ThongBao");
                }    
                checkSP.SoLuong++;
                checkSP.ThanhTien = checkSP.SoLuong * checkSP.DonGia;
                return Redirect(strURL);
            }
            
            ItemGioHang newItem = new ItemGioHang(MaSP);
            if (sp.SoLuongTon <= newItem.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(newItem);
            return Redirect(strURL);
        }

        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null) return 0;
            return lstGioHang.Sum(x => x.SoLuong);
        }
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null) return 0;
            return lstGioHang.Sum(x => x.ThanhTien);
        }
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            if(TinhTongTien()==0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;

                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        public ActionResult SuaGioHang(int MaSP)
        {
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == MaSP);
            if(sp==null)
            {
                //Đương dẫn không hơp lệ
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(x => x.MaSP == MaSP);
            if(spCheck==null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhapGioHang(ItemGioHang itemGH)
        {
            SanPham spCheck = db.SanPhams.SingleOrDefault(x => x.MaSP == itemGH.MaSP);
            if(spCheck.SoLuongTon< itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang itemGHUpdate = lstGioHang.Find(x=>x.MaSP==itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == MaSP);
            if (sp == null)
            {
                //Đương dẫn không hơp lệ
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(x => x.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang kh1 = new KhachHang();
            if(Session["TaiKhoan"]==null)
            {
                //Đối với khách hàng chưa có tài khoản 
                kh1 = kh;
                db.KhachHangs.Add(kh1);
                db.SaveChanges();
            }
            else
            {
                //đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                kh1.TenKH = tv.HoTen;
                kh1.DiaChi = tv.DiaChi;
                kh1.Email = tv.Email;
                kh1.SoDienThoai = tv.SoDienThoai;
                kh1.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(kh1);
                db.SaveChanges();
            }
            DonDatHang ddh = new DonDatHang();
            TaoDonDatHang(ddh, kh1);
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        void TaoDonDatHang(DonDatHang ddh,KhachHang kh1)
        {
            ddh.MaKH = kh1.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            List<ItemGioHang> lstGioHang = LayGioHang();
            foreach (var item in lstGioHang)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
        }

        //Thêm giỏ hàng ajax
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(x => x.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang checkSP = lstGioHang.SingleOrDefault(x => x.MaSP == MaSP);
            if (checkSP != null)
            {
                if (sp.SoLuongTon <= checkSP.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\");</script>");
                }
                checkSP.SoLuong++;
                checkSP.ThanhTien = checkSP.SoLuong * checkSP.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();

                return PartialView("GioHangPartial");
            }

            ItemGioHang newItem = new ItemGioHang(MaSP);
            if (sp.SoLuongTon <= newItem.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\");</script>");
            }
            lstGioHang.Add(newItem);
            ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();

                return PartialView("GioHangPartial");
        }

    }
}