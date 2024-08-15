using JobPortalApplication.Data;
using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Models.Users;
using JobPortalApplication.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JobPortalApplication.Controllers
{
    public class JobListingController : Controller
    {
        private readonly DatabaseContext _context;

        private readonly ILogger<JobListingController> _logger;

        public JobListingController(DatabaseContext context, ILogger<JobListingController> logger)
        {
            _context = context;
            _logger = logger;


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
        public IActionResult getJob(string searchTerm, string workType, string city, int page = 1)
        {
            const int PageSize = 3;
           var jobList = _context.job.Include(c => c.Company)
                             .OrderByDescending(j => j.EntryDate) 
                             .ToList();

           // var jobList = _context.job.Include(c => c.Company).ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                jobList = jobList.Where(j => j.JobTitle.ToLower().Contains(searchTerm.ToLower().Substring(0, Math.Min(3, searchTerm.Length)))).ToList();
            }
            if (!string.IsNullOrEmpty(workType))
            {
                jobList = jobList.Where(j => j.WorkType.Contains(workType)).ToList();
            }
            if (!string.IsNullOrEmpty(city))
            {
                string lowerCaseCity = city.ToLower();
                jobList = jobList.Where(j => j.Address.ToLower().Contains(lowerCaseCity)).ToList();
            }

            ViewBag.JobCount = jobList.Count;
            if (string.IsNullOrEmpty(searchTerm) && string.IsNullOrEmpty(workType) && string.IsNullOrEmpty(city))
            {
                // jobList = _context.job.Include(c => c.Company).ToList();
                jobList = _context.job.Include(c => c.Company)
                            .OrderByDescending(j => j.EntryDate)  
                             .ToList();
            }

            // Pagination logic
            var totalJobs = jobList.Count();
            var totalPages = (int)Math.Ceiling((double)totalJobs / PageSize);

            var paginatedJobs = jobList.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedJobs);
        }

        [HttpPost]
        public IActionResult GetJobs(int jobid)
        {
            var jobs = _context.job.ToList();
            var applications = _context.jobapplication.ToList();


            if (jobs.Count == 0 || applications.Count==0)
            {
                // Serialize the empty JObject to a JSON string
                JObject emptyJson = new JObject();
                string jsonString = JsonConvert.SerializeObject(emptyJson);

                return new ContentResult
                {
                    Content = jsonString,
                    ContentType = "application/json"
                }; ;
            }

            /*
            var jobsByMonth = jobs.GroupBy(j => j.EntryDate.Month)
                             .ToDictionary(g => g.Key, g => g.Count());

            */
            var jobsByMonth = jobs
    .Where(j => j.EntryDate.HasValue)  // Filter out jobs where EntryDate is null
    .GroupBy(j => j.EntryDate.Value.Month)  // Access Month property after ensuring EntryDate has value
    .ToDictionary(g => g.Key, g => g.Count());



            var applicationsByMonth = applications
    .Where(a => a.EntryDate.HasValue)  // Filter out applications where EntryDate is null
    .GroupBy(a => a.EntryDate.Value.Month)  // Access Month property after ensuring EntryDate has value
    .ToDictionary(g => g.Key, g => g.Count());

            /*
            var applicationsByMonth = applications.GroupBy(a => a.EntryDate.Month)
                                                  .ToDictionary(g => g.Key, g => g.Count());

            */

            var jobsDictionary = Enumerable.Range(1, 12).ToDictionary(i => i, i => 0);
            var applicationsDictionary = Enumerable.Range(1, 12).ToDictionary(i => i, i => 0);

            foreach (var a in jobsByMonth)
            {
                jobsDictionary[a.Key] = a.Value;
            }

            foreach (var a in applicationsByMonth)
            {
                applicationsDictionary[a.Key] = a.Value;
            }

            var jobData = jobsDictionary.Values.ToArray();
            var applicationData = applicationsDictionary.Values.ToArray();


            var options = new
            {
                series = new[]
            {
                new { name = "Jobs", data = jobData },
                new { name = "Job Applications", data = applicationData }
            }
            };

            string jsonOptions = JsonConvert.SerializeObject(options, Formatting.Indented);

            return new ContentResult
            {
                Content = jsonOptions,
                ContentType = "application/json"
            };
        }

        [HttpPost]
        public IActionResult GetUserData()
        {
            var users = _context.userdata.ToList();
            var company = _context.company.ToList();


            if (users.Count == 0 || company.Count == 0)
            {
                // Serialize the empty JObject to a JSON string
                JObject emptyJson = new JObject();
                string jsonString = JsonConvert.SerializeObject(emptyJson);

                return new ContentResult
                {
                    Content = jsonString,
                    ContentType = "application/json"
                }; ;
            }
            var userCount = users.Count();
            var companyCount = company.Count();

            /*
                        var usersByMonth = users
                            .Where(u => u.EntryDate.HasValue)
                            .GroupBy(u => u.EntryDate.Value.Month)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var companyByMonth = company
                         .Where(u => u.EntryDate.HasValue)
                         .GroupBy(u => u.EntryDate.Value.Month)
                         .ToDictionary(g => g.Key, g => g.Count());
            */
            /*
            var usersDictionary = Enumerable.Range(1, 12).ToDictionary(i => i, i => 0);
            var companyDictionary = Enumerable.Range(1, 12).ToDictionary(i => i, i => 0);

            foreach (var a in usersByMonth)
            {
                usersDictionary[a.Key] = a.Value;
            }

            foreach (var a in companyByMonth)
            {
                companyDictionary[a.Key] = a.Value;
            }

            var userdata = usersDictionary.Values.ToArray();
            var companydata = companyDictionary.Values.ToArray();
            */
            var options = new
            {
                series = new[]
                  {
            new { name = "User", data = userCount },
            new { name = "Company", data = companyCount }
        }
            };

            string jsonOptions = JsonConvert.SerializeObject(options, Formatting.Indented);

            return new ContentResult
            {
                Content = jsonOptions,
                ContentType = "application/json"
            };
        }

        /* -- for display single job in user side --*/
        public IActionResult SingleJob(int id)
        {
            var job = _context.job.Include(c => c.Company)
                                    .ThenInclude(c => c.industry)
                                   .FirstOrDefault(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }


            var industry = _context.industry.FirstOrDefault(i => i.IndustryName == job.Industry);
            var industryName = industry?.IndustryName;

            ViewBag.IndustryName = industryName;
            Console.WriteLine($"Job Id: {job.JobId}, Industry: {job.Industry}, IndustryName: {industryName}");

            return View(job);
        }

        public IActionResult SingleJob2(int id)
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
        public async Task<IActionResult> SaveJob(int jobId, int entryStatus = 1)
        {
            HttpContext.Session.SetInt32("JobId", jobId);

            return await SaveOrApplyJob(jobId, entryStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyJob(int jobId, int entryStatus = 2)
        {
            HttpContext.Session.SetInt32("JobId", jobId);

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

            var job = await _context.job.FirstOrDefaultAsync(j => j.JobId == jobId);

            if (job == null)
            {
                TempData["ErrorMessage"] = "Job not found.";
                return RedirectToAction("SingleJob", new { id = jobId });
            }

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
                if (entryStatus == 2)
                {
                    job.PositionCompleted = (job.PositionCompleted ?? 0) + 1;
                }
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
                return RedirectToAction("Login", "Login");
            }

            var jobApplications = await _context.jobapplication
                .Include(j => j.Job)
                    .ThenInclude(j => j.Company)
                .Where(j => j.UserId == userId.Value && j.EntryStatus == 1)
                .ToListAsync();

            var jobs = jobApplications.Select(ja => ja.Job).ToList();

            ViewBag.JobCount = jobs.Count;
            ViewBag.HttpContext = HttpContext; // Pass the HttpContext to the view

            return View(jobs);
        }

        [Authorize]
        public async Task<IActionResult> AppliedJobUser()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            var appliedJobApplications = await _context.jobapplication
                .Include(j => j.Job)
                    .ThenInclude(j => j.Company)
                      .Include(j => j.User)

                .Where(j => j.UserId == userId.Value && j.EntryStatus == 2)
                .ToListAsync();

            var appliedJobs = appliedJobApplications.Select(ja => ja.Job).ToList();

            ViewBag.JobCount = appliedJobs.Count;
            return View(appliedJobApplications);
        }
        [HttpPost]
        public IActionResult jobApplication()
        {
            var jobapp = _context.jobapplication.ToList();

            var json = JsonConvert.SerializeObject(jobapp);
            return new ContentResult
            {
                Content = json,
                ContentType = "application/json"
            };
        }



        [Authorize]
        public async Task<IActionResult> AppliedJob()
        {
            int? employerUserId = HttpContext.Session.GetInt32("UserId");

            if (!employerUserId.HasValue)
            {
                return RedirectToAction("Login", "Login");
            }

            // Fetch jobs posted by this employer
            var employerJobs = await _context.job
                .Where(j => j.Company.UserId == employerUserId.Value)
                .Select(j => j.JobId)
                .ToListAsync();

            if (!employerJobs.Any())
            {
                ViewData["ErrorMessage"] = "No jobs found for the current employer.";
                return View(Enumerable.Empty<JobApplication>());
            }

            var jobApplications = await _context.jobapplication
                .Include(j => j.Job)
                .ThenInclude(j => j.Company)
                .Include(j => j.User)
                .Where(j => j.EntryStatus == 2 && j.JobId.HasValue && employerJobs.Contains(j.JobId.Value))
                .ToListAsync();

            if (!jobApplications.Any())
            {
                ViewData["ErrorMessage"] = "No job applications found  for the current employer's jobs.";
            }

            return View(jobApplications);
        }

        [Authorize]
        public async Task<IActionResult> UserDetails(int? userId)
        {
            if (userId == null)
            {

                return NotFound();
            }

            var userdata = await _context.userdata
                .Include(u => u.UserModel)
                .Include(u => u.course)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userdata == null)
            {
                return NotFound();
            }

            return View(userdata);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            var japp = await _context.jobapplication.FindAsync(id);

            if (japp == null)
            {
                return NotFound();
            }
            if (japp.ApplicationStatus == null)
            {
                japp.ApplicationStatus = status == 1 ? 0 : 1;
            }
            else
            {
                // Toggle the status
                japp.ApplicationStatus = status;
            }

            _context.Update(japp);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AppliedJob));
        }



        public async Task<IActionResult> ScheduleInterview(int userId, int jobId)
        {
            var user = _context.usermodel.FirstOrDefault(u => u.UserId == userId);
            var job = _context.job.FirstOrDefault(j => j.JobId == jobId);

            if (user == null || job == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Retrieve the list of users and jobs
            var users = await _context.usermodel.ToListAsync();
            var jobs = await _context.job.ToListAsync();

            // Convert users and jobs to SelectListItems

            var userItems = users.Select(u => new SelectListItem { Value = u.UserId.ToString(), Text = u.Name });
            var jobItems = jobs.Select(j => new SelectListItem { Value = j.JobId.ToString(), Text = j.JobTitle });


            // Pass SelectListItems to the view
            ViewBag.UserId = userItems;
            ViewBag.JobId = jobItems;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleInterview(InterviewDto interviewDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(interviewDto);
                }

                var interview = new Interview
                {
                    UserId = interviewDto.UserId,
                    JobId = interviewDto.JobId,
                    InterviewDate = interviewDto.InterviewDate,
                    Time = interviewDto.Time,
                    Location = interviewDto.Location,
                    Status = interviewDto.Status,
                    EntryDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Remark = interviewDto.Remark
                };

                _context.interview.Add(interview);
                await _context.SaveChangesAsync();
                ViewBag.SuccessMessage = "Interview scheduled successfully!";
                return View(interviewDto);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error occurred while scheduling interview.");

                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction(nameof(ScheduleInterview));
            }
        }

        public async Task<IActionResult> UserInterviews()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["ErrorMessage"] = "User not logged in.";
                return RedirectToAction("Login", "Login");
            }

            var interviews = await _context.interview
                .Include(i => i.Job)
                .ThenInclude(j => j.Company)
                .Where(i => i.UserId == userId)
                .ToListAsync();

            if (interviews == null || !interviews.Any())
            {
                TempData["ErrMessage"] = "No interviews found for the logged-in user.";
                return RedirectToAction("UserInterviews");
            }
           
            return View(interviews);
        }
    }
}

