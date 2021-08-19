using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAssignment.Models
{
    public enum BloodGroupType
    {
        APositive,
        BPositive,
        ABPositive,
        OPositive,
        ANegative,
        BNegative,
        ABNegative,
        ONegative,
    }
    public enum GenderType
    {
        Male,
        Female
    }
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int Age { get; set; }
        public BloodGroupType BloodGroup { get; set; }
        public GenderType Gender { get; set; }
        public string Image { get; set; }
        public Course MainCourse { get; set; }
        public Course SupplementaryCourse { get; set; }
    }
}