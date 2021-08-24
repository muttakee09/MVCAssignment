using MVCAssignment.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCAssignment.Services;

namespace MVCAssignment.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
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
                var courseList = session.Query<Course>()
                    .ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                try
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(image.FileName);
                        string path = Path.Combine(Server.MapPath("~/Photos"), fileName);
                        image.SaveAs(path);
                        student.Image = Path.GetFileName(image.FileName);

                    }
                    if (ModelState.IsValid)
                    {
                        var MCourse = courseList.FirstOrDefault(s => s.CourseId == student.MainCourseId);
                        var SCourse = courseList.FirstOrDefault(s => s.CourseId == student.SupplementaryCourseId);
                        Student std = new Student(
                            student.StudentName, student.Age, student.BloodGroup, student.Gender,
                            student.Image, MCourse, SCourse);
                        session.Save(std);
                        return RedirectToAction("Index");
                    }
                }
                catch(Exception ex)
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
            
            using (ISession session = NHibernateSessions.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        var alreadySavedStudent = session.Get<Student>(id);
                        var courseList = session.Query<Course>()
                            .ToList();
                        ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");

                        if (image != null && image.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(image.FileName);
                            string path = Path.Combine(Server.MapPath("~/Photos"), fileName);
                            image.SaveAs(path);
                            student.Image = Path.GetFileName(image.FileName);

                        }
                        else
                        {
                            student.Image = alreadySavedStudent.Image;
                        }
                        session.Evict(alreadySavedStudent);

                        var MCourse = courseList.FirstOrDefault(s => s.CourseId == student.MainCourseId);
                        var SCourse = courseList.FirstOrDefault(s => s.CourseId == student.SupplementaryCourseId);

                        student.MainCourse = MCourse;
                        student.SupplementaryCourse = SCourse;
                        session.Update(student);
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        ViewBag.Message = e.Message;
                        return View();
                    }
                }
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