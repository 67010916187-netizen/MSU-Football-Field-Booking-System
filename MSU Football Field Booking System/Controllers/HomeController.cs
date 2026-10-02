using Microsoft.AspNetCore.Mvc;
using MSUFootballBooking.Models;

namespace MSUFootballBooking.Controllers
{
    public class HomeController : Controller
    {
        // 1. หน้าแรก (Login)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // ประมวลผลการ Login
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // ตรวจสอบว่าเป็นอีเมล มมส. หรือไม่ (@msu.ac.th หรือ @webmail.msu.ac.th)
            if (!model.StudentEmail.ToLower().EndsWith("@msu.ac.th") &&
                !model.StudentEmail.ToLower().EndsWith("@webmail.msu.ac.th"))
            {
                ModelState.AddModelError("StudentEmail", "ต้องใช้อีเมลนิสิต มมส. (@msu.ac.th) เท่านั้น");
                return View(model);
            }

            // บันทึก Session เก็บอีเมลที่ล็อกอินเข้าสู่ระบบ
            HttpContext.Session.SetString("UserEmail", model.StudentEmail);

            // ส่งไปยังหน้าเลือกสนาม
            return RedirectToAction("SelectField");
        }

        // 2. หน้าเลือกสนาม วัน และเวลา
        [HttpGet]
        public IActionResult SelectField()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index"); // ถ้ายังไม่ล็อกอิน ให้กลับไปหน้า Login
            }

            ViewBag.UserEmail = userEmail;
            return View(new BookingViewModel { StudentEmail = userEmail });
        }

        // ประมวลผลการจอง
        [HttpPost]
        public IActionResult SelectField(BookingViewModel model)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.UserEmail = userEmail;
                return View(model);
            }

            // สุ่มสร้างรหัสการจอง เช่น MSU-839210
            model.BookingCode = "MSU-" + new Random().Next(100000, 999999);
            model.StudentEmail = userEmail;
            model.CreatedAt = DateTime.Now;

            // ส่งข้อมูลไปยังหน้าใบสรุปการจอง
            return View("Summary", model);
        }

        // ออกจากระบบ
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}