using MVCAssignment.Models;
using NHibernate;
using System;
using System.Collections.Generic;
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
                return View(student);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentName, Age, Gender, BloodGroup, Image")] Student student)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
 
                if (ModelState.IsValid)
                {
                    session.Save(student);
                    return RedirectToAction("Index");
                }
                return View(student);
                /*return View(student);
                var student = new Student
                {
                    StudentName = "Name",
                    Age = 11,
                    Gender = GenderType.Female,
                    BloodGroup = BloodGroupType.ANegative,
                    Image = null
                };*/
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
                    return View(student);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentName, Age, Gender, BloodGroup, Image")] int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var student = session.Get<Student>(id);
                student.StudentName = "Name";
                student.Age = 11;
                student.Gender = GenderType.Female;
                student.BloodGroup = BloodGroupType.ANegative;
                student.Image = null;
                session.Update(student);
                return RedirectToAction("Index");
            }
        }
    }
}