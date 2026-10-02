using Microsoft.AspNetCore.Components.Routing;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Timers;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

< !DOCTYPE html >
< html lang = "th" >
< head >
    < meta charset = "UTF-8" >
    < meta name = "viewport" content = "width=device-width, initial-scale=1.0" >
    < title > ระบบจองสนามฟุตบอล มหาวิทยาลัยมหาสารคาม(MSU Football Reservation) </ title >
    < !--Tailwind CSS-- >
    < script src = "https://cdn.tailwindcss.com" ></ script >
    < !--Font Awesome Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
    <!-- Google Font (Kanit) -->
    <link href="https://fonts.googleapis.com/css2?family=Kanit:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
        body {
            font-family: 'Kanit', sans - serif;
background - color: #f3f4f6;
        }
        .bg - msu - navy {
    background - color: #1e3a8a; }
        .bg - msu - yellow {
        background - color: #f59e0b; }
    </ style >
</ head >
< body class= "min-h-screen flex flex-col justify-between" >

    < !--Toast Alert(ระบบแจ้งเตือน)-- >
    < div id = "toast" class= "fixed top-5 right-5 z-50 transform transition-all duration-300 translate-y-[-100px] opacity-0 flex items-center p-4 mb-4 text-gray-700 bg-white rounded-lg shadow-lg border-l-4" role = "alert" >
        < div id = "toast-icon" class= "inline-flex items-center justify-center flex-shrink-0 w-8 h-8 rounded-lg" ></ div >
        < div id = "toast-msg" class= "ml-3 text-sm font-normal pr-4" ></ div >
        < button onclick = "hideToast()" class= "ml-auto -mx-1.5 -my-1.5 bg-white text-gray-400 hover:text-gray-900 rounded-lg p-1.5 inline-flex items-center justify-center h-8 w-8" >
            < i class= "fa-solid fa-xmark" ></ i >
        </ button >
    </ div >

    < !--HEADER / NAVBAR-- >
    < header class= "bg-msu-navy text-white shadow-md" >
        < div class= "max-w-7xl mx-auto px-4 py-3 flex justify-between items-center" >
            < div class= "flex items-center space-x-3" >
                < div class= "bg-msu-yellow p-2 rounded-full text-slate-900 font-bold w-10 h-10 flex items-center justify-center text-xl shadow-inner" >
                    < i class= "fa-solid fa-futbol" ></ i >
                </ div >
                < div >
                    < h1 class= "text-xl font-bold text-amber-400 leading-tight" > MSU Field Booking</h1>
                    <p class= "text-xs text-gray-300" > ระบบจองสนามฟุตบอล มหาวิทยาลัยมหาสารคาม </ p >
                </ div >
            </ div >


            < div id = "user-info-bar" class= "hidden flex items-center space-x-4" >
                < div class= "text-right" >
                    < p id = "display-user-email" class= "text-sm font-semibold text-amber-300" ></ p >
                    < p class= "text-xs text-gray-300" > นิสิต / บุคลากร มมส.</ p >
                </ div >
                < button onclick = "logout()" class= "bg-red-600 hover:bg-red-700 text-white px-3 py-1.5 rounded-lg text-sm font-medium transition flex items-center space-x-1 shadow" >
                    < i class= "fa-solid fa-right-from-bracket" ></ i >
                    < span > ออกจากระบบ </ span >
                </ button >
            </ div >
        </ div >
    </ header >

    < !--SECTION 1: LOGIN PAGE(หน้าเข้าสู่ระบบ) -->
    < section id = "login-section" class= "flex-grow flex items-center justify-center p-4 my-8" >
        < div class= "bg-white rounded-2xl shadow-xl border border-gray-100 max-w-md w-full p-8 relative overflow-hidden" >
            < div class= "absolute top-0 left-0 w-full h-2 bg-amber-400" ></ div >


            < div class= "text-center mb-6" >
                < div class= "inline-block p-3 rounded-full bg-amber-100 text-amber-600 mb-2" >
                    < i class= "fa-solid fa-user-lock text-3xl" ></ i >
                </ div >
                < h2 class= "text-2xl font-bold text-gray-800" > เข้าสู่ระบบจองสนาม </ h2 >
                < p class= "text-sm text-gray-500 mt-1" > ใช้อีเมลนิสิต มมส.และรหัสผ่านของคุณ </ p >
            </ div >

            < form id = "login-form" onsubmit = "handleLogin(event)" class= "space-y-4" >
                < div >
                    < label class= "block text-sm font-medium text-gray-700 mb-1" > อีเมลนิสิต / บุคลากร มมส.</ label >
                    < div class= "relative" >
                        < span class= "absolute inset-y-0 left-0 flex items-center pl-3 text-gray-400" >
                            < i class= "fa-solid fa-envelope" ></ i >
                        </ span >
                        < input type = "email" id = "login-email" required placeholder = "66xxxxxxxx@msu.ac.th"
                            class= "w-full pl-10 pr-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-400 focus:border-amber-400 text-sm transition" >
                    </ div >
                    < p class= "text-xs text-gray-400 mt-1" > *ต้องลงท้ายด้วย @msu.ac.th หรือ @student.msu.ac.th</p>
                </div>

                <div>
                    <div class= "flex justify-between items-center mb-1" >
                        < label class= "block text-sm font-medium text-gray-700" > รหัสผ่าน(Password) </ label >
                        < button type = "button" onclick = "openForgotModal()" class= "text-xs text-blue-600 hover:underline" > ลืมรหัสผ่าน ?</ button >
                    </ div >
                    < div class= "relative" >
                        < span class= "absolute inset-y-0 left-0 flex items-center pl-3 text-gray-400" >
                            < i class= "fa-solid fa-key" ></ i >
                        </ span >
                        < input type = "password" id = "login-password" required placeholder = "กรอกรหัสผ่านของคุณ"
                            class= "w-full pl-10 pr-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-400 focus:border-amber-400 text-sm transition" >
                    </ div >
                </ div >

                < button type = "submit" class= "w-full bg-msu-navy hover:bg-blue-900 text-white font-medium py-2.5 rounded-lg transition shadow-md hover:shadow-lg flex items-center justify-center space-x-2" >
                    < i class= "fa-solid fa-right-to-bracket" ></ i >
                    < span > เข้าสู่ระบบ </ span >
                </ button >
            </ form >

            < div class= "mt-6 pt-4 border-t text-center text-xs text-gray-500" >
                < p >💡 การเข้าใช้งานครั้งแรก ระบบจะบันทึกรหัสผ่านที่คุณตั้งไว้เพื่อใช้อ้างอิงตรวจสอบในครั้งถัดไป</p>
            </div>
        </div>
    </section>

    <!-- SECTION 2: BOOKING SYSTEM(หน้าระบบจองสนาม) -->
    < section id = "booking-section" class= "hidden flex-grow max-w-7xl mx-auto w-full p-4 my-4 space-y-6" >


        < !--Welcome Banner-- >
        < div class= "bg-gradient-to-r from-blue-900 to-indigo-900 text-white rounded-xl p-6 shadow-md" >
            < h2 class= "text-2xl font-bold mb-1" > ยินดีต้อนรับสู่ระบบจองสนามฟุตบอล มมส.</ h2 >
            < p class= "text-blue-200 text-sm" > กรุณาเลือกสนาม วันที่ และช่วงเวลาที่คุณต้องการจอง(เปิดบริการ 06.00 - 22.00 น.)</ p >
        </ div >

        < div class= "grid grid-cols-1 lg:grid-cols-3 gap-6" >


            < !--ด้านซ้าย: ตัวเลือกสนาม และ วันที่ -->
            <div class= "lg:col-span-1 space-y-6" >


                < !--1.เลือกสนาม-- >
                < div class= "bg-white p-5 rounded-xl shadow-sm border border-gray-200" >
                    < h3 class= "font-bold text-gray-800 text-lg mb-3 flex items-center text-blue-900" >
                        < span class= "bg-amber-400 text-slate-900 rounded-full w-6 h-6 inline-flex items-center justify-center text-xs mr-2" > 1 </ span >
                        เลือกสนามฟุตบอล
                    </ h3 >
                    < div class= "space-y-3" >
                        < label class= "block relative cursor-pointer" >
                            < input type = "radio" name = "field-choice" value = "blue" checked onchange = "updateBookingGrid()" class= "peer sr-only" >
                            < div class= "p-3 border-2 border-gray-200 rounded-lg peer-checked:border-blue-600 peer-checked:bg-blue-50 transition flex items-center justify-between" >
                                < div class= "flex items-center space-x-3" >
                                    < div class= "w-4 h-4 rounded-full bg-blue-600" ></ div >
                                    < div >
                                        < p class= "font-semibold text-gray-800 text-sm" > สนามฟ้า </ p >
                                        < p class= "text-xs text-gray-500" > สนามใหญ่(หญ้าจริง 11 คน) </ p >
                                    </ div >
                                </ div >
                                < i class= "fa-solid fa-circle-check text-blue-600 opacity-0 peer-checked:opacity-100" ></ i >
                            </ div >
                        </ label >

                        < label class= "block relative cursor-pointer" >
                            < input type = "radio" name = "field-choice" value = "red" onchange = "updateBookingGrid()" class= "peer sr-only" >
                            < div class= "p-3 border-2 border-gray-200 rounded-lg peer-checked:border-red-600 peer-checked:bg-red-50 transition flex items-center justify-between" >
                                < div class= "flex items-center space-x-3" >
                                    < div class= "w-4 h-4 rounded-full bg-red-600" ></ div >
                                    < div >
                                        < p class= "font-semibold text-gray-800 text-sm" > สนามแดง </ p >
                                        < p class= "text-xs text-gray-500" > สนามลู่วิ่ง(สนามกลางแจ้ง) </ p >
                                    </ div >
                                </ div >
                                < i class= "fa-solid fa-circle-check text-red-600 opacity-0 peer-checked:opacity-100" ></ i >
                            </ div >
                        </ label >

                        < label class= "block relative cursor-pointer" >
                            < input type = "radio" name = "field-choice" value = "small" onchange = "updateBookingGrid()" class= "peer sr-only" >
                            < div class= "p-3 border-2 border-gray-200 rounded-lg peer-checked:border-emerald-600 peer-checked:bg-emerald-50 transition flex items-center justify-between" >
                                < div class= "flex items-center space-x-3" >
                                    < div class= "w-4 h-4 rounded-full bg-emerald-600" ></ div >
                                    < div >
                                        < p class= "font-semibold text-gray-800 text-sm" > สนามเล็ก </ p >
                                        < p class= "text-xs text-gray-500" > สนามหญ้าเทียม(7 คน) </ p >
                                    </ div >
                                </ div >
                                < i class= "fa-solid fa-circle-check text-emerald-600 opacity-0 peer-checked:opacity-100" ></ i >
                            </ div >
                        </ label >
                    </ div >
                </ div >

                < !--2.เลือกวัน-- >
                < div class= "bg-white p-5 rounded-xl shadow-sm border border-gray-200" >
                    < h3 class= "font-bold text-gray-800 text-lg mb-3 flex items-center text-blue-900" >
                        < span class= "bg-amber-400 text-slate-900 rounded-full w-6 h-6 inline-flex items-center justify-center text-xs mr-2" > 2 </ span >
                        เลือกวันที่ต้องการเล่น
                    </ h3 >
                    < div >
                        < input type = "date" id = "booking-date" onchange = "updateBookingGrid()"
                            class= "w-full p-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-amber-400 font-medium text-gray-700" >
                    </ div >
                </ div >

                < !--สรุปรายการเลือก-- >
                < div class= "bg-amber-50 p-5 rounded-xl border border-amber-200" >
                    < h4 class= "font-bold text-amber-900 mb-2 border-b border-amber-200 pb-2" > สรุปการเลือกจอง </ h4 >
                    < div class= "space-y-1 text-sm text-amber-900" >
                        < p >< strong > สนาม:</ strong > < span id = "summary-field" class= "font-semibold text-blue-900" > -</ span ></ p >
                        < p >< strong > วันที่:</ strong > < span id = "summary-date" class= "font-semibold text-blue-900" > -</ span ></ p >
                        < p >< strong > เวลา:</ strong > < span id = "summary-time" class= "font-semibold text-blue-900" > ยังไม่ได้เลือก </ span ></ p >
                        < p class= "text-xs text-amber-700 mt-2" > *กำหนดเวลาเล่นรอบละ 1 ชั่วโมง </ p >
                    </ div >
                    < button id = "btn-confirm-booking" onclick = "confirmBooking()" disabled
                        class= "w-full mt-4 bg-gray-300 text-gray-500 font-bold py-2.5 rounded-lg transition cursor-not-allowed" >
                        ยืนยันการจองสนาม
                    </ button >
                </ div >

            </ div >

            < !--ด้านขวา: ตารางเวลา และประวัติการจอง -->
            <div class= "lg:col-span-2 space-y-6" >


                < div class= "bg-white p-5 rounded-xl shadow-sm border border-gray-200" >
                    < div class= "flex flex-col sm:flex-row justify-between sm:items-center mb-4 pb-3 border-b border-gray-100 gap-2" >
                        < h3 class= "font-bold text-gray-800 text-lg flex items-center text-blue-900" >
                            < span class= "bg-amber-400 text-slate-900 rounded-full w-6 h-6 inline-flex items-center justify-center text-xs mr-2" > 3 </ span >
                            ตารางเวลา(06:00 - 22:00 น.)
                        </ h3 >
                        < !--Legend สัญลักษณ์สี-- >
                        < div class= "flex items-center space-x-4 text-xs font-medium" >
                            < div class= "flex items-center space-x-1" >
                                < span class= "w-3.5 h-3.5 bg-white border border-gray-400 rounded" ></ span >
                                < span > ว่าง(สีขาว) </ span >
                            </ div >
                            < div class= "flex items-center space-x-1" >
                                < span class= "w-3.5 h-3.5 bg-red-600 rounded" ></ span >
                                < span > จองแล้ว(สีแดง) </ span >
                            </ div >
                            < div class= "flex items-center space-x-1" >
                                < span class= "w-3.5 h-3.5 bg-amber-400 rounded" ></ span >
                                < span > กำลังเลือก </ span >
                            </ div >
                        </ div >
                    </ div >

                    < !--Time Grid-- >
                    < div id = "time-grid" class= "grid grid-cols-2 sm:grid-cols-4 gap-3" >
                        < !--ปุ่มเวลาจะถูกสร้างด้วย JS-- >
                    </ div >
                </ div >

                < !--ประวัติการจองของคุณ-- >
                < div class= "bg-white p-5 rounded-xl shadow-sm border border-gray-200" >
                    < h3 class= "font-bold text-gray-800 text-lg mb-3 flex items-center text-blue-900" >
                        < i class= "fa-solid fa-clock-rotate-left mr-2" ></ i > ประวัติการจองของคุณ
                    </ h3 >
                    < div id = "my-bookings-list" class= "space-y-2 max-h-60 overflow-y-auto" >
                        < !--รายการจองจะแสดงที่นี่-- >
                    </ div >
                </ div >

            </ div >

        </ div >
    </ section >

    < !--FOOTER-- >
    < footer class= "bg-gray-800 text-gray-400 py-4 text-center text-sm" >
        < p >© ระบบจองสนามฟุตบอล มหาวิทยาลัยมหาสารคาม(Mahasarakham University)</ p >
    </ footer >

    < !--MODAL: FORGOT PASSWORD(กล่องรีเซ็ตรหัสผ่าน) -->
    < div id = "modal-forgot" class= "fixed inset-0 bg-black bg-opacity-50 hidden z-50 flex items-center justify-center p-4" >
        < div class= "bg-white rounded-xl shadow-xl max-w-md w-full p-6 relative" >
            < button onclick = "closeForgotModal()" class= "absolute top-4 right-4 text-gray-400 hover:text-gray-600" >
                < i class= "fa-solid fa-xmark text-xl" ></ i >
            </ button >
            < h3 class= "text-xl font-bold text-gray-800 mb-2" > ลืมรหัสผ่าน </ h3 >
            < p class= "text-sm text-gray-600 mb-4" > ระบุอีเมล มมส.ของคุณเพื่อตั้งรหัสผ่านใหม่ </ p >

            < form onsubmit = "handleForgotPassword(event)" class= "space-y-4" >
                < div >
                    < label class= "block text-sm font-medium text-gray-700 mb-1" > อีเมล มมส.</ label >
                    < input type = "email" id = "forgot-email" required placeholder = "66xxxxxxxx@msu.ac.th"
                        class= "w-full px-3 py-2 border rounded-lg text-sm focus:ring-2 focus:ring-amber-400" >
                </ div >
                < div >
                    < label class= "block text-sm font-medium text-gray-700 mb-1" > รหัสผ่านใหม่(New Password) </ label >
                    < input type = "password" id = "forgot-new-password" required placeholder = "ตั้งรหัสผ่านใหม่ของคุณ"
                        class= "w-full px-3 py-2 border rounded-lg text-sm focus:ring-2 focus:ring-amber-400" >
                </ div >
                < button type = "submit" class= "w-full bg-amber-500 hover:bg-amber-600 text-white font-medium py-2 rounded-lg transition text-sm" >
                    บันทึกรหัสผ่านใหม่
                </ button >
            </ form >
        </ div >
    </ div >

    < !--JAVASCRIPT LOGIC-- >
    < script >
        // --- 1. GLOBAL STATE & CONFIGURATIONS ---
        let currentUser = null;
let selectedTimeSlot = null;

// ช่วงเวลาที่เปิดให้จอง: 06.00 - 22.00 น. (รอบละ 1 ชม.)
const timeSlots = [
    "06:00 - 07:00", "07:00 - 08:00", "08:00 - 09:00", "09:00 - 10:00",
            "10:00 - 11:00", "11:00 - 12:00", "12:00 - 13:00", "13:00 - 14:00",
            "14:00 - 15:00", "15:00 - 16:00", "16:00 - 17:00", "17:00 - 18:00",
            "18:00 - 19:00", "19:00 - 20:00", "20:00 - 21:00", "21:00 - 22:00"
];

const fieldNames = {
            'blue': 'สนามฟ้า (สนามใหญ่)',
            'red': 'สนามแดง (สนามลู่วิ่ง)',
            'small': 'สนามเล็ก (หญ้าเทียม)'
        };

// เริ่มต้นการทำงานเมื่อโหลดหน้าเว็บ
window.onload = function() {
    // ตั้งค่าวันที่เริ่มต้นในปฏิทินเป็น "วันนี้"
    const today = new Date().toISOString().split('T')[0];
    const dateInput = document.getElementById('booking-date');
    dateInput.value = today;
    dateInput.min = today; // ห้ามเลือกวันที่ย้อนหลัง

    // สร้างข้อมูลจำลองการจองเบื้องต้นใน LocalStorage (หากยังไม่มี)
    if (!localStorage.getItem('msu_bookings'))
    {
        const initialBookings = [
                    { id: 1, field: 'blue', date: today, time: '17:00 - 18:00', user: 'other@msu.ac.th' },
                    { id: 2, field: 'blue', date: today, time: '18:00 - 19:00', user: 'other2@msu.ac.th' },
                    { id: 3, field: 'red', date: today, time: '16:00 - 17:00', user: 'other3@msu.ac.th' }
                ];
        localStorage.setItem('msu_bookings', JSON.stringify(initialBookings));
    }

    if (!localStorage.getItem('msu_users'))
    {
        localStorage.setItem('msu_users', JSON.stringify({ }));
    }
}
;

// --- 2. AUTHENTICATION (ระบบล็อกอิน/ตรวจรหัสผ่าน) ---
function handleLogin(e)
{
    e.preventDefault();
    const email = document.getElementById('login-email').value.trim();
    const password = document.getElementById('login-password').value;

    // ตรวจสอบโดเมนอีเมลนิสิต/บุคลากร มมส.
    if (!email.endsWith('@msu.ac.th') && !email.endsWith('@student.msu.ac.th'))
    {
        showToast('ต้องใช้อีเมล มมส. (@msu.ac.th หรือ @student.msu.ac.th) เท่านั้น', 'error');
        return;
    }

    let users = JSON.parse(localStorage.getItem('msu_users')) || { }
    ;

    // กรณีล็อกอินด้วยอีเมลนี้เป็นครั้งแรก -> ทำการผูกอีเมลเข้ากับ password ที่ป้อนเข้ามา
    if (!users[email])
    {
        users[email] = password;
        localStorage.setItem('msu_users', JSON.stringify(users));
        showToast('ลงทะเบียนรหัสผ่านของคุณเรียบร้อยแล้ว!', 'success');
    }
    else
    {
        // หากเคยใช้งานแล้ว -> ตรวจสอบว่า Password ตรงกับที่ตั้งไว้หรือไม่
        if (users[email] !== password)
        {
            showToast('รหัสผ่านไม่ถูกต้อง! กรุณากรอกรหัสผ่านที่คุณตั้งไว้', 'error');
            return;
        }
    }

    // ล็อกอินสำเร็จ
    currentUser = email;
    document.getElementById('display-user-email').innerText = email;
    document.getElementById('login-section').classList.add('hidden');
    document.getElementById('booking-section').classList.remove('hidden');
    document.getElementById('user-info-bar').classList.remove('hidden');

    showToast('เข้าสู่ระบบสำเร็จ ยินดีต้อนรับครับ', 'success');
    updateBookingGrid();
    renderMyBookings();
}

function logout()
{
    currentUser = null;
    document.getElementById('login-form').reset();
    document.getElementById('booking-section').classList.add('hidden');
    document.getElementById('user-info-bar').classList.add('hidden');
    document.getElementById('login-section').classList.remove('hidden');
    showToast('ออกจากระบบเรียบร้อยแล้ว', 'info');
}

// --- FORGOT PASSWORD MODAL ---
function openForgotModal()
{
    document.getElementById('modal-forgot').classList.remove('hidden');
}

function closeForgotModal()
{
    document.getElementById('modal-forgot').classList.add('hidden');
}

function handleForgotPassword(e)
{
    e.preventDefault();
    const email = document.getElementById('forgot-email').value.trim();
    const newPassword = document.getElementById('forgot-new-password').value;

    if (!email.endsWith('@msu.ac.th') && !email.endsWith('@student.msu.ac.th'))
    {
        showToast('กรุณากรอกอีเมล มมส. ให้ถูกต้อง', 'error');
        return;
    }

    let users = JSON.parse(localStorage.getItem('msu_users')) || { }
    ;
    users[email] = newPassword;
    localStorage.setItem('msu_users', JSON.stringify(users));

    showToast('เปลี่ยนรหัสผ่านสำเร็จแล้ว! สามารถใช้รหัสใหม่ล็อกอินได้ทันที', 'success');
    closeForgotModal();
}

// --- 3. BOOKING LOGIC & GRID RENDER ---
function updateBookingGrid()
{
    const selectedField = document.querySelector('input[name="field-choice"]:checked').value;
    const selectedDate = document.getElementById('booking-date').value;

    // อัปเดตข้อมูลสรุป
    document.getElementById('summary-field').innerText = fieldNames[selectedField];
    document.getElementById('summary-date').innerText = selectedDate || '-';

    // รีเซ็ตช่วงเวลาที่เลือกไว้
    selectedTimeSlot = null;
    document.getElementById('summary-time').innerText = 'ยังไม่ได้เลือก';
    const confirmBtn = document.getElementById('btn-confirm-booking');
    confirmBtn.disabled = true;
    confirmBtn.className = "w-full mt-4 bg-gray-300 text-gray-500 font-bold py-2.5 rounded-lg transition cursor-not-allowed";

    // ดึงข้อมูลการจองจาก LocalStorage
    const allBookings = JSON.parse(localStorage.getItem('msu_bookings')) || [];

    // กรองหาเวลาที่ถูกจองแล้วสำหรับ "สนาม" และ "วันที่" เลือก
    const bookedTimes = allBookings
        .filter(b => b.field === selectedField && b.date === selectedDate)
        .map(b => b.time);

    // วาดปุ่มเวลา (Grid)
    const gridContainer = document.getElementById('time-grid');
    gridContainer.innerHTML = '';

    timeSlots.forEach(slot => {
    const isBooked = bookedTimes.includes(slot);
    const slotCard = document.createElement('div');

    if (isBooked)
    {
        // ช่องถูกจองแล้ว -> สีแดง (Red)
        slotCard.className = "bg-red-600 text-white p-3 rounded-lg border border-red-700 flex flex-col items-center justify-center shadow-sm opacity-90 cursor-not-allowed";
        slotCard.innerHTML = `
                        < span class= "text-sm font-bold" >< i class= "fa-solid fa-ban text-xs mr-1" ></ i >${ slot}</ span >
                        < span class= "text-[10px] bg-red-800 px-2 py-0.5 rounded-full mt-1" > จองแล้ว </ span >
                    `;
                } else
{
    // ช่องว่าง -> สีขาว (White)
    slotCard.className = "bg-white text-gray-800 hover:border-amber-400 p-3 rounded-lg border-2 border-gray-200 flex flex-col items-center justify-center shadow-sm cursor-pointer transition transform hover:-translate-y-0.5 slot-available";
    slotCard.setAttribute('data-time', slot);
    slotCard.onclick = () => selectTimeSlot(slot, slotCard);
    slotCard.innerHTML = `
                        < span class= "text-sm font-bold" >${ slot}</ span >
                        < span class= "text-[10px] text-emerald-600 bg-emerald-50 border border-emerald-200 px-2 py-0.5 rounded-full mt-1" > ว่าง </ span >
                    `;
                }

                gridContainer.appendChild(slotCard);
            });
        }

        function selectTimeSlot(slot, element)
{
    // ล้างสีไฮไลต์ของการเลือกเดิม
    document.querySelectorAll('.slot-available').forEach(el => {
        el.classList.remove('bg-amber-400', 'text-slate-900', 'border-amber-500');
        el.classList.add('bg-white', 'text-gray-800');
    });

    // ไฮไลต์ช่องที่เลือกใหม่ -> เปลี่ยนเป็นสีเหลือง
    element.classList.remove('bg-white', 'text-gray-800');
    element.classList.add('bg-amber-400', 'text-slate-900', 'border-amber-500');

    selectedTimeSlot = slot;
    document.getElementById('summary-time').innerText = slot + " น.";

    // เปิดปุ่มยืนยัน
    const confirmBtn = document.getElementById('btn-confirm-booking');
    confirmBtn.disabled = false;
    confirmBtn.className = "w-full mt-4 bg-amber-500 hover:bg-amber-600 text-slate-900 font-bold py-2.5 rounded-lg transition shadow-md cursor-pointer";
}

function confirmBooking()
{
    if (!selectedTimeSlot) return;

    const selectedField = document.querySelector('input[name="field-choice"]:checked').value;
    const selectedDate = document.getElementById('booking-date').value;

    let allBookings = JSON.parse(localStorage.getItem('msu_bookings')) || [];

    const newBooking = {
                id: Date.now(),
                field: selectedField,
                date: selectedDate,
                time: selectedTimeSlot,
                user: currentUser
            };

allBookings.push(newBooking);
localStorage.setItem('msu_bookings', JSON.stringify(allBookings));

showToast('จองสนามสำเร็จเรียบร้อยแล้ว!', 'success');
updateBookingGrid();
renderMyBookings();
        }

        function cancelBooking(id)
{
    if (confirm('คุณต้องการยกเลิกการจองนี้ใช่หรือไม่?'))
    {
        let allBookings = JSON.parse(localStorage.getItem('msu_bookings')) || [];
        allBookings = allBookings.filter(b => b.id !== id);
        localStorage.setItem('msu_bookings', JSON.stringify(allBookings));

        showToast('ยกเลิกรายการจองเรียบร้อยแล้ว', 'info');
        updateBookingGrid();
        renderMyBookings();
    }
}

function renderMyBookings()
{
    const container = document.getElementById('my-bookings-list');
    const allBookings = JSON.parse(localStorage.getItem('msu_bookings')) || [];

    const myBookings = allBookings.filter(b => b.user === currentUser);

    if (myBookings.length === 0)
    {
        container.innerHTML = `< p class= "text-xs text-gray-400 text-center py-4" > คุณยังไม่มีรายการจองสนาม </ p >`;
return;
            }

            container.innerHTML = '';
myBookings.forEach(item => {
    const card = document.createElement('div');
    card.className = "flex items-center justify-between p-3 bg-gray-50 border rounded-lg text-sm";
    card.innerHTML = `
                    < div >
                        < p class= "font-bold text-blue-900" >${ fieldNames[item.field]}</ p >
                        < p class= "text-xs text-gray-600" >< i class= "fa-regular fa-calendar mr-1" ></ i >${ item.date} | < i class= "fa-regular fa-clock mr-1" ></ i >${ item.time}
น.</ p >
                    </ div >
                    < button onclick = "cancelBooking(${item.id})" class= "text-xs text-red-600 hover:text-red-800 bg-red-50 hover:bg-red-100 px-2 py-1 rounded border border-red-200 transition" >
                        < i class= "fa-solid fa-trash-can mr-1" ></ i > ยกเลิก
                    </ button >
                `;
container.appendChild(card);
            });
        }

        // --- TOAST NOTIFICATION UTILITY ---
        function showToast(message, type = 'info')
{
    const toast = document.getElementById('toast');
    const toastIcon = document.getElementById('toast-icon');
    const toastMsg = document.getElementById('toast-msg');

    toastMsg.innerText = message;

    if (type === 'success')
    {
        toast.className = "fixed top-5 right-5 z-50 flex items-center p-4 mb-4 text-gray-700 bg-white rounded-lg shadow-lg border-l-4 border-emerald-500 transition-all duration-300";
        toastIcon.className = "inline-flex items-center justify-center flex-shrink-0 w-8 h-8 rounded-lg bg-emerald-100 text-emerald-500";
        toastIcon.innerHTML = `< i class= "fa-solid fa-check" ></ i >`;
            } else if (type === 'error')
{
    toast.className = "fixed top-5 right-5 z-50 flex items-center p-4 mb-4 text-gray-700 bg-white rounded-lg shadow-lg border-l-4 border-red-500 transition-all duration-300";
    toastIcon.className = "inline-flex items-center justify-center flex-shrink-0 w-8 h-8 rounded-lg bg-red-100 text-red-500";
    toastIcon.innerHTML = `< i class= "fa-solid fa-triangle-exclamation" ></ i >`;
            } else
{
    toast.className = "fixed top-5 right-5 z-50 flex items-center p-4 mb-4 text-gray-700 bg-white rounded-lg shadow-lg border-l-4 border-blue-500 transition-all duration-300";
    toastIcon.className = "inline-flex items-center justify-center flex-shrink-0 w-8 h-8 rounded-lg bg-blue-100 text-blue-500";
    toastIcon.innerHTML = `< i class= "fa-solid fa-info" ></ i >`;
            }

            toast.classList.remove('translate-y-[-100px]', 'opacity-0');
setTimeout(hideToast, 4000);
        }

        function hideToast()
{
    const toast = document.getElementById('toast');
    toast.classList.add('translate-y-[-100px]', 'opacity-0');
}
    </ script >
</ body >
</ html >