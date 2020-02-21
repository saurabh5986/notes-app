using NotesApp.Business;
using NotesApp.Business.Models;
using System;
using System.Web.Mvc;

namespace NotesApp.Controllers
{
    public class NotesController : Controller
    {
        private readonly INotesRepository _notesRepo;
        private readonly IAccountRepository _accountRepo;

        public NotesController(INotesRepository notesRepo, IAccountRepository accountRepo)
        {
            _notesRepo = notesRepo;
            _accountRepo = accountRepo;
        }

        // GET: Notes
        [Route("note-list")]
        public ActionResult Index()
        {
            ViewBag.ActiveNoteList = "active";
            if (HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = HttpContext.Session["UserId"].ToString();
            NotesResultView resultView = new NotesResultView();
            resultView.Notes = _notesRepo.GetAllNotes(Convert.ToInt32(userId), null, null);
            return View(resultView);
        }

        [Route("note-list")]
        [HttpPost]
        public ActionResult Index(NotesResultView model)
        {
            ViewBag.ActiveNoteList = "active";
            if (HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = HttpContext.Session["UserId"].ToString();
            NotesResultView resultView = new NotesResultView();
            resultView.Notes = _notesRepo.GetAllNotes(Convert.ToInt32(userId), model.FromDate, model.ToDate);
            if (model.FromDate != null)
                resultView.FromDate = Convert.ToDateTime(model.FromDate);
            if (model.ToDate != null)
                resultView.ToDate = Convert.ToDateTime(model.ToDate);
            return View(resultView);
        }

        [Route("add-note")]
        public ActionResult Add()
        {
            ViewBag.ActiveAddNote = "active";
            if (HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = HttpContext.Session["UserId"].ToString();
            return View(new NoteViewModel());
        }

        [Route("add-note")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(NoteViewModel noteView, string submitButton)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session["UserId"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var userId = HttpContext.Session["UserId"].ToString();
                var noteToAdd = new Note
                {
                    UserId = Convert.ToInt32(userId),
                    CreatedUTC = DateTime.UtcNow,
                    NoteText = noteView.NoteText
                };
                var valid = _accountRepo.CheckValidForTransaction(Convert.ToInt32(userId), RequestHelpers.RequestIPAddress());
                if (!valid)
                {
                    return RedirectToAction("IpBlocked", "Account");
                }
                var isSuccess = _notesRepo.AddNote(noteToAdd, RequestHelpers.RequestIPAddress());
                if (isSuccess && submitButton == "Save")
                {
                    return RedirectToAction("Index", "Notes");
                }
                else
                {
                    return RedirectToAction("Add", "Notes");
                }
            }
            else
            {
                return View(noteView);
            }
        }
    }
}