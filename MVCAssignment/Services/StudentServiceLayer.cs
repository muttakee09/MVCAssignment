using System;
using System.IO;
using System.Web;
using MVCAssignment.Models;


namespace MVCAssignment.Services
{
    public interface IStudentServiceLayer
    {
        Student UploadImage(Student student, string address, HttpPostedFileBase image, string defaultImage = null);
    }
    public class StudentServiceLayer : IStudentServiceLayer
    {
        public Student UploadImage(Student student, string address, HttpPostedFileBase image, string defaultImage = null)
        {
            if (image != null && image.ContentLength > 0)
            {
                string fileName = Path.GetFileName(image.FileName);
                string path = Path.Combine(address, DateTime.Now.ToString("yyMMddHHmmss-") + fileName);
                image.SaveAs(path);
                student.Image = Path.GetFileName(DateTime.Now.ToString("yyMMddHHmmss-") + image.FileName);

            }
            else
            {
                student.Image = defaultImage;
            }
            return student;
        }
    }
}