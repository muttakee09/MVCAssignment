using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAssignment.Models
{

    public class Student
    {
        public virtual int StudentId { get; set; }
        public virtual string StudentName { get; set; }
        public virtual int Age { get; set; }
        public virtual BloodGroupType BloodGroup { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual string Image { get; set; }
        public virtual Course MainCourse { get; set; }
        public virtual Course SupplementaryCourse { get; set; }
    }
}