using MVCAssignment.Models;
using MVCAssignment.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCAssignment.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IStudentServiceLayer _studentServiceLayer;

        public StudentController(IStudentService studentService, IStudentServiceLayer studentServiceLayer)
        {
            _studentService = studentService;
            _studentServiceLayer = studentServiceLayer;
        }

        // GET: Student
        public ActionResult Index()
        {
            return View(_studentService.GetStudentList());
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Student s = _studentService.GetStudentWithCourseDetails((int)id);
                return View(s);
            }
            else
            {
                return HttpNotFound();
            }

        }

        public ActionResult Create()
        {
            IList<Course> courseList = _studentService.GetCourseList();
            ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(
            Include = "StudentName, Age, Image, BloodGroup, Gender, MainCourseId, SupplementaryCourseId")] Student student,
            HttpPostedFileBase image)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                IList<Course> courseList = _studentService.GetCourseList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                try
                {
                    if (ModelState.IsValid)
                    {
                        student = _studentServiceLayer.UploadImage(student,
                            Path.Combine(Server.MapPath("~/Photos")), image);
                        _studentService.CreateStudent(student);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View();
                }
                return View(student);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Student student = _studentService.GetStudent((int)id);
                IList<Course> courseList = _studentService.GetCourseList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                return View(student);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(
            Include = "StudentName, Age, Image, BloodGroup, Gender, MainCourse, SupplementaryCourse")] int? id,
            Student student, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                IList<Course> courseList = _studentService.GetCourseList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                Student alreadySavedStudent = _studentService.GetStudent((int)id);
                
                var studentImage = alreadySavedStudent.Image ?? null;
                if (studentImage != null)
                {
                    _studentServiceLayer.UploadImage(student, Path.Combine(Server.MapPath("~/Photos")), image,
                        alreadySavedStudent.Image);
                }
                else
                {
                    _studentServiceLayer.UploadImage(student, Path.Combine(Server.MapPath("~/Photos")), image);
                }
                
                _studentService.UpdateStudent(student);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }

        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student s = _studentService.GetStudent((int)id);
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
        {
            _studentService.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}