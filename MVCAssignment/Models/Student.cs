using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCAssignment.Models
{

    public class Student
    {
        [Display(Name = "Student Id")]
        public virtual int StudentId { get; set; }
        [Display(Name = "Student Name")]
        public virtual string StudentName { get; set; }
        [Range(0, 200)]
        public virtual int Age { get; set; }
        [Display(Name = "Blood Group")]
        public virtual BloodGroupType BloodGroup { get; set; }
        public virtual GenderType Gender { get; set; }
        public virtual string Image { get; set; }
        [Display(Name = "Main course")]
        public virtual int? MainCourseId { get; set; }
        [Display(Name = "Supplementary course")]
        public virtual int? SupplementaryCourseId { get; set; }
        [ForeignKey("MainCourse"), Display(Name = "Main course")]
        public virtual Course MainCourse { get; set; }
        [ForeignKey("SupplementaryCourse"), Display(Name = "Supplementary course")]
        public virtual Course SupplementaryCourse { get; set; }

        public Student() { }

        public Student(string studentName, int age, BloodGroupType bloodGroup,
           GenderType gender, string image, Course mainCourseEntity, Course supplementaryCourseEntity)
        {
            StudentName = studentName;
            Age = age;
            BloodGroup = bloodGroup;
            Gender = gender;
            Image = image;
            MainCourse = mainCourseEntity;
            SupplementaryCourse = supplementaryCourseEntity;
        }
    }
}