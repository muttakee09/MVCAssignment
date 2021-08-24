using MVCAssignment.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCAssignment.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var students = session.Query<Student>().ToList();
                return View(students);
            }
        }

        public ActionResult Details(int? id)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                ViewBag.Message = "Student ID: " + id;
                var student = session.Get<Student>(id);
                if (student.MainCourse != null)
                {
                    student.MainCourse = session.Get<Course>(student.MainCourse.CourseId);

                }
                if (student.SupplementaryCourse != null)
                {
                    student.SupplementaryCourse = session.Get<Course>(student.SupplementaryCourse.CourseId);

                }
                return View(student);
            }
        }

        public ActionResult Create()
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var courseList = session.Query<Course>()
                    .ToList();
                ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                /*var courseList = session.Query<Course>()
                    .Select((r) => new SelectListItem
                    { Text = r.CourseCode + " - " + r.CourseName, Value = r.CourseId.ToString()
                    })
                    .ToList();
                ViewBag.CourseList = courseList;*/
                return View();
            }
            
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

        public ActionResult Edit(int id)
        {
            if (id != null)
            {
                using (ISession session = NHibernateSessions.OpenSession())
                {
                    ViewBag.Message = "Student ID: " + id;
                    var student = session.Get<Student>(id);
                    var courseList = session.Query<Course>()
                     .ToList();
                    if (student.MainCourse != null)
                    {
                        student.MainCourseId = student.MainCourse.CourseId;

                    }
                    if (student.SupplementaryCourse != null)
                    {
                        student.SupplementaryCourseId = student.SupplementaryCourse.CourseId;

                    }
                    ViewBag.CourseList = new SelectList(courseList, "CourseId", "CourseName");
                    return View(student);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(
            Include = "StudentName, Age, Image, BloodGroup, Gender, MainCourse, SupplementaryCourse")] int? id, Student student, HttpPostedFileBase image)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            using (ISession session = NHibernateSessions.OpenSession())
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

                    var MCourse = courseList.First(s => s.CourseId == student.MainCourseId);
                    var SCourse = courseList.First(s => s.CourseId == student.SupplementaryCourseId);
                    
                    student.MainCourse = MCourse;
                    student.SupplementaryCourse = SCourse;
                    session.Evict(student);
                    session.Update(student);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    return View();
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
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var student = session.Get<Student>(id);
                if (student == null)
                {
                    return HttpNotFound();
                }
                return View(student);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Student student)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var std = session.Get<Student>(id);
                    session.Delete(std);
                    transaction.Commit();
                }
            }
            return RedirectToAction("Index");
        }
    }
}