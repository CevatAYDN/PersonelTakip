using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entity.ViewModel
{
   public class ProfilViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name="Ad")]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        [StringLength(25)]

        public string Surname { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(25)]

        public string UserName { get; set; }
        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Display(Name = "Eski Şifre")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Şifreniz En az 5 Karatker Olmalı")]

        public string OldPassword { get; set; }
        [Display(Name = "Yeni Şifre")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz En az 5 Karatker Olmalı")]

        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name="Şifre Tekrar")]
        [Compare("NewPassword",ErrorMessage ="Şifreler uyuşmuyor")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Şifreniz en az 5 karakter olmalıdır")]
        public string NewPasswordConfirm { get; set; }
    }
}
