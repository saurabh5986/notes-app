using NotesApp.Business;
using NotesApp.Business.Models;
using System;
using System.Web.Mvc;

namespace NotesApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        // GET: /Account/Login
        [Route("sign-in")]
        [Route("")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [Route("sign-in")]
        [Route("")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _accountRepo.Login(model, RequestHelpers.RequestIPAddress());
            var valid = _accountRepo.CheckValidForTransaction(result.UserId, RequestHelpers.RequestIPAddress());
            if (!valid)
            {
                return RedirectToAction("IpBlocked", "Account");
            }
            if (result != null)
            {
                SetUserSession(result);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Incorrect Username or Password");
                return View(model);
            }
        }

        public ActionResult IpBlocked()
        {
            if (HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [Route("blocked-ip-list")]
        public ActionResult BlockedIpList()
        {
            if (HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.BlockedIpList = "active";
            var BLockedIps = _accountRepo.GetBlockedIpList();
            return View(BLockedIps);
        }

        public void SetUserSession(User userdata)
        {
            HttpContext.Session["UserId"] = userdata.UserId;
            HttpContext.Session["Role"] = userdata.UserType;
            HttpContext.Session["FirstName"] = userdata.FirstName;
            HttpContext.Session["LastName"] = userdata.LastName;
        }

        public void ClearUserSession()
        {
            HttpContext.Session["Role"] = null;
            HttpContext.Session["UserId"] = null;
            HttpContext.Session["FirstName"] = null;
            HttpContext.Session["LastName"] = null;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Notes");
        }

        [Route("sign-up")]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [Route("sign-up")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IpAddress = RequestHelpers.RequestIPAddress(),
                    Password = model.Password,
                    Status = true,
                    CreatedUTC = DateTime.UtcNow,
                    UserType = UserTypeEnum.User.ToString(),
                };
                var result = _accountRepo.Register(user);
                if (result)
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            ClearUserSession();
            return RedirectToAction("Login", "Account");
        }
    }
}