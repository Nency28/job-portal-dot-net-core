using JobPortalApplication.Data;
using JobPortalApplication.Models.CompanyModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalApplication.Controllers
{
    public class InterviewController : Controller
    {
        private readonly DatabaseContext _context;


        public InterviewController(DatabaseContext context)
        {
            _context = context;

        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ScheduleInterview(int userId, int jobId)
        {
            var user = await _context.usermodel.FindAsync(userId);
            var job = await _context.job.FindAsync(jobId);

            if (user == null || job == null)
            {
                return NotFound();
            }

            ViewBag.UserId = userId;
            ViewBag.JobId = jobId;
            ViewBag.UserName = user.Name;
            ViewBag.JobTitle = job.JobTitle;

            return View();
        }

        // POST: Interview/ScheduleInterview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleInterview([Bind("UserId,JobId,InterviewDate,Time,Location,Remark")] Interview interview)
        {
            if (ModelState.IsValid)
            {
                interview.EntryDate = DateTime.Now;
                interview.UpdateDate = DateTime.Now;

                _context.Add(interview);
                await _context.SaveChangesAsync();
                return RedirectToAction("AppliedJob","Joblisting"); // Redirect to a suitable action, e.g., Index or List of Interviews
            }

            var user = await _context.usermodel.FindAsync(interview.UserId);
            var job = await _context.job.FindAsync(interview.JobId);

            ViewBag.UserId = interview.UserId;
            ViewBag.JobId = interview.JobId;
            ViewBag.UserName = user?.Name;
            ViewBag.JobTitle = job?.JobTitle;

            return View(interview);
        }


    }
}
