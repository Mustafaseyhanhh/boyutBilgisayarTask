using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EczaneV3.Entites.Models.ViewModels
{
    public class EditPasswordViewModel
    {
        [Display(Name = "Eski Şifre")]
        public string OldPassword { get; set; }
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }
    }
}
