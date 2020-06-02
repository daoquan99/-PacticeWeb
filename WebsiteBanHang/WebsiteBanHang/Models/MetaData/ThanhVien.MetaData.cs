using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetaData))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetaData
        {
            public int MaThanhVien { get; set; }

            [DisplayName("Tài khoản")]
            [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} tài khoản phải có từ {1} đến {2} ký tự")]
            [Required(ErrorMessage = "{0} không được rỗng!")]
            public string TaiKhoan { get; set; }
            
            [DisplayName("Mật khẩu")]
            [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} tài khoản phải có ít nhất {2} ký tự")]
            [Required(ErrorMessage = "{0} không được rỗng!")]
            public string MatKhau { get; set; }

            [DisplayName("Mật khẩu")]
            [StringLength(200, MinimumLength = 6, ErrorMessage = "{0} tài khoản phải có ít nhất {2} ký tự")]
            [Required(ErrorMessage = "{0} không được rỗng!")]
            public string HoTen { get; set; }

            [DisplayName("Địa chỉ")]
            [Required(ErrorMessage = "{0} không được rỗng!")]
            public string DiaChi { get; set; }
            
            [Required(ErrorMessage ="{0} không được để trống")]
            [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                            ErrorMessage = "{0} không hợp lệ")]
            public string Email { get; set; }
            
            [DisplayName("Số điện thoại")]
            [StringLength(10, ErrorMessage ="{0} không được quá 10 số!")]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Số điện thoại không hợp lệ")]
            public string SoDienThoai { get; set; }
            
            public string CauHoi { get; set; }
            public string CauTraLoi { get; set; }
        }

    }
}