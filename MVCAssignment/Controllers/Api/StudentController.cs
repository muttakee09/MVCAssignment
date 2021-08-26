using MVCAssignment.Models;
using MVCAssignment.Services;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCAssignment.Api.Controllers
{
    public class StudentController : ApiController
    {
        // GET api/<controller>

        public StudentController() { }

        public IHttpActionResult Get()
        {
            try
            {
                using (ISession session = NHibernateSessions.OpenSession())
                {
                    var student = session.Query<Student>().ToList();
                    return Ok(student);
                }
            }

            catch(Exception ex)
            {
                return NotFound();
            }
            
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var student = session.Get<Student>(id);
                if (student.MainCourse != null)
                {
                    student.MainCourseId = student.MainCourse.CourseId;

                }
                if (student.SupplementaryCourse != null)
                {
                    student.SupplementaryCourseId = student.SupplementaryCourse.CourseId;

                }
                return Ok(student);
            }
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}