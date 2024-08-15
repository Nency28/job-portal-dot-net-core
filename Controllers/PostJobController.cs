using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Newtonsoft.Json;


namespace JobPortalApplication.Controllers
{
    public class PostJobController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<PostJobController> _logger;

        public PostJobController(DatabaseContext context, ILogger<PostJobController> logger)
        {
            _context = context;
            _logger = logger;

        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: PostJobController/Create







        [HttpPost]
        public IActionResult getIndustry()
        {
            var industries = _context.industry.ToList();
            var json = JsonConvert.SerializeObject(industries);

            return new ContentResult
            {
                Content = json
            };
        }


        [HttpPost]
        public IActionResult getQualification()
        {
            var qualification = _context.qualification.ToList();
            var json = JsonConvert.SerializeObject(qualification);

            return new ContentResult
            {
                Content = json
            };
        }

        [HttpPost]
        public IActionResult getCourse()
        {
            var course = _context.course.ToList();
            var json = JsonConvert.SerializeObject(course);

            return new ContentResult
            {
                Content = json
            };
        }
        public async Task<IActionResult> GetJobsCount()
        {
            var jobsCount = await _context.job.CountAsync();
            return Json(new { count = jobsCount });
        }

        public IActionResult PostJob()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");
            if (!companyId.HasValue)
            {
                return RedirectToAction("Error", "Home");
            }

            var company = _context.company.FirstOrDefault(c => c.CompanyId == companyId);
            if (company == null)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.CompanyStatus = company.Status;
            ViewBag.JobsCount = _context.job.Count();


            return View();


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostJob(JobDto jobDto)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (userId.HasValue && companyId.HasValue)
            {

                var company = await _context.company.FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Status == 1);

                if (company != null)
                {
                    ViewBag.CompanyStatus = company.Status ?? 0; // Default to 0 if status is null
                }
                else
                {
                    ViewBag.CompanyStatus = 0; // Default to 0 if company is not found
                }

                var qualifications = jobDto.QuaId != null ? string.Join(",", jobDto.QuaId) : string.Empty;


                


                var j = new Job
                {
                    UserId = userId.Value,
                    CompanyId = companyId.Value,
                    JobTitle = jobDto.JobTitle,
                    Description = jobDto.Description,
                    Position = jobDto.Position,
                    PositionCompleted = jobDto.PositionCompleted,
                    Course= jobDto.Course,
                    //QuaId = jobDto.QuaId,
                   // Course = course,
                    QuaId = qualifications,
                    Timing = jobDto.Timing,
                    Address = jobDto.Address,
                   Industry = jobDto.Industry,
                    WorkMode = jobDto.WorkMode,
                    WorkType = jobDto.WorkType,
                    Salary = jobDto.Salary,
                    Gender = jobDto.Gender,
                    Experience = jobDto.Experience,
                    InterviewMode = jobDto.InterviewMode,
                    Language = jobDto.Language,
                    EntryDate = DateTime.Now, 
                     UpdateDate = DateTime.Now
                    };





                _context.job.Add(j);
                    await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Job posted successfully!";
                return RedirectToAction(nameof(PostJob));
              //  return RedirectToAction("Index", "PostJob"); 
            
            
               /*
            catch (Exception ex)
                {   
                    ModelState.AddModelError("", "An error occurred while saving the job.  ");
                    return View(jobDto); 
                }
            */
             
            }
            else
            {
             
                return RedirectToAction("Error", "Home"); 
            }
           
        }


        public IActionResult EditJob(int id)
        {
            var job = _context.job
                .Include(j => j.Company)
                .FirstOrDefault(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }

        // POST: Job/EditJob/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditJob(int id, Job job)
        {
            var existingJob = _context.job.FirstOrDefault(j => j.JobId == id);
            if (existingJob == null)
            {
                return NotFound();
            }

            // Update fields
            existingJob.JobTitle = job.JobTitle;
            existingJob.Description = job.Description;
            existingJob.Position = job.Position;
            existingJob.PositionCompleted = job.PositionCompleted;
            existingJob.QuaId = job.QuaId;
            existingJob.Course = job.Course;
            existingJob.Timing = job.Timing;
            existingJob.Address = job.Address;
            existingJob.Industry = job.Industry;
            existingJob.WorkMode = job.WorkMode;
            existingJob.WorkType = job.WorkType;
            existingJob.Salary = job.Salary;
            existingJob.Gender = job.Gender;
            existingJob.Experience = job.Experience;
            existingJob.InterviewMode = job.InterviewMode;
            existingJob.Language = job.Language;

            existingJob.UpdateDate = DateTime.Now;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Job updated successfully!";
            return RedirectToAction("ViewEmployerJob", "JobListing"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.job.Remove(job);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Job deleted successfully!";
            return RedirectToAction("ViewEmployerJob", "JobListing"); 
        }

    }

}
