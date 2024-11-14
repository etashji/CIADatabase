using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CIADatabase.Areas.Users.Models;
using CIADatabase.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace CIADatabase.Areas.Users.Controllers
{
    [Authorize]  // This ensures all actions require an authenticated user
    public class UsersController : Controller
    {
        private CIADatabaseContext db = new CIADatabaseContext();

        // GET: Users/Users
        public ActionResult Index()
        {
            var users = db.Users.OrderBy(u => u.Username).ToList(); // List alphabetically by username
            return View(users);
        }

        // GET: Users/Users/Details/5
        public ActionResult Details(int? id)
        {
            // If `id` is null, retrieve it from the authentication ticket
            if (!id.HasValue)
            {
                id = GetCurrentUserId();
                if (!id.HasValue)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // Access restricted if no valid ID is found
                }
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: Users/Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous] // Allow anonymous access to create action
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Age,Email,Username,Password,ConfirmPassword")] User user)
        {
            if (user.Age < 18)
            {
                ModelState.AddModelError("Age", "You are too young to access the application.");
                return View(user);
            }

            if (db.Users.Any(u => u.Username == user.Username))
            {
                return RedirectToAction("Login");  // Redirect to login if username is taken
            }

            user.FirstName = Capitalize(user.FirstName);
            user.LastName = Capitalize(user.LastName);
            user.HashedPassword = HashPassword(user.Password);

            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Users/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                id = GetCurrentUserId();
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Password,ConfirmPassword")] User user, string newPassword)
        {
            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Age");
            ModelState.Remove("Email");
            ModelState.Remove("Username");

            int? userIdFromAuthTicket = GetCurrentUserId();
            if (!userIdFromAuthTicket.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // Access restricted
            }

            var existingUser = db.Users.Find(userIdFromAuthTicket.Value);
            if (existingUser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // Access restricted
            }

            if (ModelState.IsValid)
            {
                newPassword = user.Password;
                if (!string.IsNullOrEmpty(newPassword))
                {
                    existingUser.HashedPassword = HashPassword(newPassword);
                    db.Entry(existingUser).Property(u => u.HashedPassword).IsModified = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    db.Configuration.ValidateOnSaveEnabled = true;
                }
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); // Access restricted
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Users/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }

            // Log the user out after deleting the account
            FormsAuthentication.SignOut();
            Session.Clear();

            return RedirectToAction("Login", "Users");
        }

        // Login and Logout Actions
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string identifier, string password)
        {
            if (string.IsNullOrEmpty(identifier) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username/Email and Password are required.");
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Username == identifier || u.Email == identifier);

            if (user == null || !VerifyPassword(password, user.HashedPassword))
            {
                ModelState.AddModelError("", "Invalid username/email or password.");
                return View();
            }

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                user.UserId.ToString()
            );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(authCookie);

            return Redirect("~/");
        }

        // GET: Users/Logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        // Helper Methods
        private int? GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var authTicket = (FormsIdentity)User.Identity;
                return int.Parse(authTicket.Ticket.UserData);
            }

            return null;
        }

        private string Capitalize(string name) => string.IsNullOrEmpty(name) ? name : char.ToUpper(name[0]) + name.Substring(1).ToLower();

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHashedPassword) => HashPassword(enteredPassword) == storedHashedPassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
