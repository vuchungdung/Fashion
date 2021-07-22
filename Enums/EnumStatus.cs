using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fashion.Enums
{
    public enum EnumStatus
    {
        [Display(Name ="Đăng ngay")]
        Actived,
        [Display(Name = "Chờ")]
        Blocked
    }
}