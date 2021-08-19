using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAssignment.Models
{
    public class Course
    {
        public virtual int CourseId { get; set; }
        public virtual string CourseName { get; set; }
        public virtual string CourseCode { get; set; }
        public virtual decimal CourseCredit { get; set; }
        protected virtual ICollection<Student> MainStudents { get; set; }
        protected virtual ICollection<Student> SupplementaryStudents { get; set; }
    }
}