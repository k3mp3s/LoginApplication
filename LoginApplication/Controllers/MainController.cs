using LoginApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace LoginApplication.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Login()
        {
            // show the login form
            return View();
        }

        public ActionResult Authenticate(User model)
        {
            User user = UserRepository.GetUser(model.Username);

            if (user != null)
            {
                if (model.Password == user.Password)
                {
                    // generate a random code
                    String code = MainController.RandomString(10);
                    user.Code = code;
                    user.Timeout = DateTime.Now.AddSeconds(60);

                    ViewBag.Username = user.Username;

                    // send mail
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(user.Email));
                    message.From = new MailAddress("login@loginapp.com");
                    message.Subject = "Authentication code";
                    message.Body = "Input the following code in the next 60 seconds: " + code;
                    message.IsBodyHtml = false;

                    var smtp = new SmtpClient();
                    smtp.Host = "localhost";
                    smtp.Send(message);
                    
                    return View("Authenticate");
                }
                else
                {
                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult Welcome(String username, String code)
        {
            User user = UserRepository.GetUser(username);

            if (user != null)
            {
                if (user.Timeout < DateTime.Now)
                {
                    // timeout, return to login
                    return View("Login");
                }

                if (code != user.Code)
                {
                    // incorrect code, return to login
                    return View("Login");
                }

                ViewBag.Username = user.Username;
                ViewBag.Email = user.Email;

                // successfull login
                return View();
            }
            else
            {
                // user not found, return to login
                return View("Login");
            }
        }

        // function for generating random character sequence
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}