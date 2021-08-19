using MVCAssignment.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
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
                ViewBag.Message = "Your application description page no." + id;
                var student = session.Get<Student>(id);
                return View(student);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentName, Age, Gender, BloodGroup, Image")] Student student)
        {
            return View();
        }
    }
}