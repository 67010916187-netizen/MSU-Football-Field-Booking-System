using System.ComponentModel.DataAnnotations;

namespace MSUFootballBooking.Models
{
    // Model สำหรับหน้า ล็อกอิน
    public class StudentLoginModel
    {
        [Required(ErrorMessage = "กรุณากรอกอีเมลนิสิต")]
        [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@msu\.ac\.th$", ErrorMessage = "กรุณาใช้อีเมลนิสิต มมส. (@msu.ac.th) เท่านั้น")]
        public string StudentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    // Model สำหรับหน้า ฟอร์มการจอง
    public class FieldBookingViewModel
    {
        public string StudentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกสนามฟุตบอล")]
        public string FieldName { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกวันที่ต้องการจอง")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "กรุณาเลือกเวลาที่ต้องการจอง")]
        public string TimeSlot { get; set; } = string.Empty;
    }

    // Model สำหรับหน้า ผลการจองสำเร็จ
    public class BookingSummaryModel
    {
        public string StudentEmail { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}