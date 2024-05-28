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
                if (company == null)
                {
                    return RedirectToAction("Error", "Home");
                }



                var j = new Job
                {
                    UserId = userId.Value,
                    CompanyId = companyId.Value,
                    JobTitle = jobDto.JobTitle,
                    Description = jobDto.Description,
                    Position = jobDto.Position,
                    PositionCompleted = jobDto.PositionCompleted,
                    Course= jobDto.Course,
                    QuaId = jobDto.QuaId,
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
                    return RedirectToAction("Index", "PostJob"); 
            
            
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
        /*
        public IActionResult ViewJob()
        {
            var joblist = _context.job.ToList();
            return View(joblist);
        }
       */

    }

}
