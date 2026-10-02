using System.ComponentModel.DataAnnotations;

namespace MSUFootballBooking.Models
{
    public class BookingViewModel
    {
        public string StudentEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกสนาม")]
        public string FieldName { get; set; } = string.Empty; // สนามฟ้า, สนามแดง, สนามเล็ก

        [Required(ErrorMessage = "กรุณาเลือกวันที่")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "กรุณาเลือกรอบเวลา")]
        public string TimeSlot { get; set; } = string.Empty; // เช่น "17:00 - 18:00"

        public string BookingCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
