using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CartImplementation.Models;

namespace CartImplementation.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Student obj)
        {

            var db = new EducationPlatformEntities();
            var student = (from i in db.Students
                           where (i.Email == obj.Email && i.Password == obj.Password)
                           select i).FirstOrDefault();
            if (student != null)
            {
                Session["studentId"] = student.Id.ToString();
                TempData["msg"] = Session["studentId"];
                return RedirectToAction("CourseList");
               
            }
            else
            {
                TempData["msg"] = "Wrong Email or Password";
                return View();
            }
            

        }
        public ActionResult CourseList()
        {
            var db = new EducationPlatformEntities();
            var couseList = db.Courses.ToList();
            return View(couseList);
        }
        public ActionResult Cart( int id)

        {
            var db=new EducationPlatformEntities();
            var courseId = id;
            var studentId = Session["studentId"];
           //var studentIdInt = Int32.Parse(studentId);
            if (studentId == null)
            {
                return RedirectToAction("Login");
            }
            
            
                var studentId2 = Session["studentId"].ToString();
                var studentIdInt = Int32.Parse(studentId2);
                var course=(from i in db.Courses where i.Id==courseId
                           select i).FirstOrDefault();

                
               var cart = new Cart()
               {
                   CourseId = courseId,
                   StudentId = studentIdInt,
                   Price = course.Price,
                   CourseName= course.Name,
                    
               };
             
                db.Carts.Add(cart);
                db.SaveChanges();

                var specificStudentCart=(from i in db.Carts
                                         where i.StudentId==studentIdInt select i).ToList();
           

                return View(specificStudentCart);
            
           
        }
    }
}