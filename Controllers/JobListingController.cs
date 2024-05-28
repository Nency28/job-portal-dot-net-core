using JobPortalApplication.Data;
using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalApplication.Controllers
{
    public class JobListingController : Controller
    {
        private readonly DatabaseContext _context;


        public JobListingController(DatabaseContext context)
        {
            _context = context;

        }
        // GET: JobListingController
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostJob()
        {
            return View();
        }



        /* -- for display Job list in Company side  (Edit, Delete) ---*/


        public async Task<IActionResult> ViewEmployerJob()
        {
            var email = User.Identity.Name;

            if (email == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = await _context.usermodel.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var job = await _context.job
                                          .Include(c => c.Company)
                                          .Where(c => c.UserId == user.UserId)
                                          .ToListAsync();

            return View(job);
        }

        /* -- for display job listing in user side */
        public IActionResult ViewJob()
        {
            var joblist = _context.job.Include(c => c.Company).ToList();
            ViewBag.JobCount = joblist.Count;

            return View(joblist);
        }

        /* -- for display single job in user side --*/
        public IActionResult SingleJob(int id)
        {
            var job = _context.job.Include(c => c.Company)
                                   .FirstOrDefault(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJob(int jobId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login", new { returnUrl = Url.Action("SaveJob", new { jobId }) });
            }

            var application = new JobApplication
            {
                JobId = jobId,
                UserId = userId.Value,
                EntryDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                EntryStatus = 1

            };

            _context.jobapplication.Add(application);
            await _context.SaveChangesAsync();

            return RedirectToAction("SingleJob", new { id = jobId });
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveJob(int jobId, int entryStatus)
        {
            return await SaveOrApplyJob(jobId, entryStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyJob(int jobId, int entryStatus)
        {
            return await SaveOrApplyJob(jobId, entryStatus);
        }

        private async Task<IActionResult> SaveOrApplyJob(int jobId, int entryStatus)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login", new { returnUrl = Url.Action(entryStatus == 1 ? "SaveJob" : "ApplyJob", new { jobId, entryStatus }) });
            }

            var existingApplication = await _context.jobapplication
                .FirstOrDefaultAsync(a => a.JobId == jobId && a.UserId == userId.Value);

            if (existingApplication != null)
            {
                if (existingApplication.EntryStatus == 2 && entryStatus == 1)
                {
                    TempData["ErrorMessage"] = "You have already applied for this job. You cannot save it again.";
                    return RedirectToAction("SingleJob", new { id = jobId });
                }

                existingApplication.EntryStatus = entryStatus;
                existingApplication.UpdateDate = DateTime.Now;
            }
            else
            {
                var application = new JobApplication
                {
                    JobId = jobId,
                    UserId = userId.Value,
                    EntryDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    EntryStatus = entryStatus
                };
                _context.jobapplication.Add(application);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = entryStatus == 1 ? "Job saved successfully!" : "Job application submitted successfully!";

            return RedirectToAction("SingleJob", new { id = jobId });
        }

        [Authorize]
        public async Task<IActionResult> MyJobs()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var jobApplications = await _context.jobapplication
                .Include(j => j.Job)
                    .ThenInclude(j => j.Company)
                .Where(j => j.UserId == userId.Value && j.EntryStatus == 1)
                .ToListAsync();

            var jobs = jobApplications.Select(ja => ja.Job).ToList();

            ViewBag.JobCount = jobs.Count;
            return View(jobs);
        }



    }
}

