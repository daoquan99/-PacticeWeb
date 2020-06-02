using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class DemoAjaxController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: DemoAjax
        public ActionResult DemoAjax()
        {
            return View();
        }
        public ActionResult LoadAjaxActionLink()
        {
            System.Threading.Thread.Sleep(2000);
            return Content("Hello Ajax");
        }
        //xử lý ajax beginform
        public ActionResult LoadAjaxBeginForm(FormCollection f)
        {
            System.Threading.Thread.Sleep(5000);
            string KQ = f["txt1"].ToString();
            return Content(KQ);
        }

        //xử lý ajax Jquery
        public ActionResult LoadAjaxJquery(int a,int b)
        {
            System.Threading.Thread.Sleep(2000);
            return Content((a + b).ToString());
        }

        //sử dụng load ajax trả về là 1 partial view 

        //[ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            var lstSanPhamLPMoi = db.SanPhams;
            //return PartialView(lstSanPhamLPMoi);
            return PartialView(lstSanPhamLPMoi);
        }
    }
}