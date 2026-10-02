using Microsoft.AspNetCore.Mvc;
using MSUFootballBooking.Models;

namespace MSUFootballBooking.Controllers
{
    public class BookingController : Controller
    {
        // 1. หน้า Login (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. การประมวลผล Login (POST)
        [HttpPost]
        public IActionResult Login(StudentLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // บันทึก Session เมื่อเข้าสู่ระบบสำเร็จ
                HttpContext.Session.SetString("StudentEmail", model.StudentEmail);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // 3. ออกจากระบบ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // 4. หน้าฟอร์มเลือกสนาม วัน เวลา (GET)
        [HttpGet]
        public IActionResult Index()
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");

            // ตรวจสอบว่าถ้ายังไม่ได้ล็อกอิน ให้กลับไปหน้า Login
            if (string.IsNullOrEmpty(studentEmail))
            {
                return RedirectToAction("Login");
            }

            var model = new FieldBookingViewModel
            {
                StudentEmail = studentEmail,
                BookingDate = DateTime.Today
            };

            ViewBag.TimeSlots = GetTimeSlots();
            return View(model);
        }

        // 5. บันทึกข้อมูลการจอง (POST)
        [HttpPost]
        public IActionResult Index(FieldBookingViewModel model)
        {
            var studentEmail = HttpContext.Session.GetString("StudentEmail");
            if (string.IsNullOrEmpty(studentEmail))
            {
                return RedirectToAction("Login");
            }

            model.StudentEmail = studentEmail;

            if (ModelState.IsValid)
            {
                var summary = new BookingSummaryModel
                {
                    StudentEmail = model.StudentEmail,
                    FieldName = model.FieldName,
                    BookingDate = model.BookingDate,
                    TimeSlot = model.TimeSlot
                };

                return View("Success", summary);
            }

            ViewBag.TimeSlots = GetTimeSlots();
            return View(model);
        }

        // ฟังก์ชันสร้างรายการรอบเวลา 06.00 - 22.00 น. (รอบละ 1 ชม.)
        private List<string> GetTimeSlots()
        {
            var timeSlots = new List<string>();
            for (int hour = 6; hour < 22; hour++)
            {
                string start = hour.ToString("D2") + ".00";
                string end = (hour + 1).ToString("D2") + ".00";
                timeSlots.Add($"{start} - {end} น.");
            }
            return timeSlots;
        }
    }
}
