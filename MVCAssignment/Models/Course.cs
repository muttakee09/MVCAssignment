using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAssignment.Models
{
    public class Course
    {
        public virtual string CourseName { get; set; }
        public virtual string CourseCode { get; set; }
        public virtual decimal CourseCredit { get; set; }
    }
}