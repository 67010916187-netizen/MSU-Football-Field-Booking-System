using System.ComponentModel.DataAnnotations;

namespace MSUFootballBooking.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "กรุณากรอกอีเมลนิสิต")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        [Display(Name = "อีเมลนิสิต มมส.")]
        public string StudentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        [DataType(DataType.Password)]
        [Display(Name = "รหัสผ่าน")]
        public string Password { get; set; } = string.Empty;
    }
}