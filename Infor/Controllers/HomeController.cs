using Infor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace Infor.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context = new UserContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            try
            {
                if (user != null && user.Email != null && user.Password != null)
                {
                    var userObjFromDb = _context.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower() && x.Password == user.Password);
                    if (userObjFromDb != null)
                    {
                        Session["userId"] = userObjFromDb.UserId;
                        return RedirectToAction("MyProfile");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Credentials Supplied");
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return View();
        }

        public ActionResult MyProfile()
        {
            try
            {
                var userId = Convert.ToInt32(Session["userId"]);
                if (userId > 0)
                {
                    var userObjFromDb = _context.Users.FirstOrDefault(x => x.UserId == userId);
                    return View(userObjFromDb);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }    
        }

        [HttpPost]
        public ActionResult MyProfile(User user)
        {
            try
            {
                var userId = Convert.ToInt32(Session["userId"]);
                if (userId > 0)
                {
                    var userObjFromDb = _context.Users.FirstOrDefault(x => x.UserId == userId);
                    userObjFromDb.Title = user.Title;
                    userObjFromDb.Profile = user.Profile;
                    _context.SaveChanges();
                    SendEmail(userObjFromDb);
                    return View(userObjFromDb);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public void SendEmail(User user)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Hello " + user.Name);
                sb.AppendLine("Please find below updated details!");
                sb.AppendLine("Name: " + user.Name);
                sb.AppendLine("Email: " + user.Email);
                sb.AppendLine("Title: " + user.Title);
                sb.AppendLine("Profile: " + user.Profile);

                var from = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                var password = ConfigurationManager.AppSettings["Password"].ToString();
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                var host = ConfigurationManager.AppSettings["Host"].ToString();

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(from);
                message.To.Add(new MailAddress(user.Email));
                message.Subject = "Profile Updated Successfully1";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = sb.ToString();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(from, password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index");
        }


    }
}