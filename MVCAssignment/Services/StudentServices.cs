using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCAssignment.Models;
using NHibernate;

namespace MVCAssignment.Services
{
    public interface IStudentService
    {
        Student GetStudent(int id);
        Course GetCourse(int id);
        Student GetStudentWithCourseDetails(int id);
        IList<Student> GetStudentList();
        IList<Course> GetCourseList();
        void CreateStudent(Student std);
        void UpdateStudent(Student std);
        void DeleteStudent(int id);
    }
    public class StudentServices : IStudentService
    {

        public Student GetStudent(int id)
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
                return student;
            }
        }

        public Course GetCourse(int id)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var course = session.Get<Course>(id);
                return course;
            }
        }

        public Student GetStudentWithCourseDetails(int id)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var student = session.Get<Student>(id);
                if (student.MainCourse != null)
                {
                    student.MainCourse = session.Get<Course>(student.MainCourse.CourseId);

                }
                if (student.SupplementaryCourse != null)
                {
                    student.SupplementaryCourse = session.Get<Course>(student.SupplementaryCourse.CourseId);

                }
                return student;
            }
        }

        public IList<Student> GetStudentList()
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var students = session.Query<Student>().ToList();
                return students;
            }
        }

        public IList<Course> GetCourseList()
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var courses = session.Query<Course>().ToList();
                return courses;
            }
        }

        public void CreateStudent(Student student)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                var courseList = session.Query<Course>().ToList();
                var MCourse = courseList.FirstOrDefault(s => s.CourseId == student.MainCourseId);
                var SCourse = courseList.FirstOrDefault(s => s.CourseId == student.SupplementaryCourseId);
                Student std = new Student(
                    student.StudentName, student.Age, student.BloodGroup, student.Gender,
                    student.Image, MCourse, SCourse);
                session.Save(std);
            }
        }

        public void UpdateStudent(Student student)
        {
            using (ISession session = NHibernateSessions.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var courseList = session.Query<Course>().ToList();
                    var MCourse = courseList.FirstOrDefault(s => s.CourseId == student.MainCourseId);
                    var SCourse = courseList.FirstOrDefault(s => s.CourseId == student.SupplementaryCourseId);

                    student.MainCourse = MCourse;
                    student.SupplementaryCourse = SCourse;
                    session.Update(student);
                    transaction.Commit();
                }
            }
        }

        public void DeleteStudent(int id)
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
        }
    }

}