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
        public ActionResult Create([Bind(Include = "StudentName, Age, Image, BloodGroup, Gender")] Student student)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
 
                if (ModelState.IsValid)
                {
                    session.Save(student);
                    return RedirectToAction("Index");
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
                    return View(student);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(
            Include = "StudentName, Age, Image, BloodGroup, Gender")] int? id, Student student)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var currentStudent = session.Get<Student>(id);
                currentStudent.Age = student.Age;
                currentStudent.StudentName = student.StudentName;
                currentStudent.BloodGroup = student.BloodGroup;
                currentStudent.Gender = student.Gender;
                currentStudent.Image = student.Image;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(currentStudent);
                    transaction.Commit();
                }
                return RedirectToAction("Index");
            }
        }
    }
}