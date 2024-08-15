using JobPortalApplication.Data;
using JobPortalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JobPortalApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;


      
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _context = context;

            _logger = logger;
        }

        public ActionResult Index(string searchTerm, string workType, string city, int page = 1)
        {
            const int PageSize = 5;

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

            var candidateCount = _context.usermodel.Count(u => u.utype == 1);
            ViewBag.CandidateCount = candidateCount;

            var companyCount = _context.usermodel.Count(u => u.utype == 2);
            ViewBag.CompanyCount = companyCount;

            var jobApplicationCount = _context.jobapplication.Count();
            ViewBag.JobApplicationCount = jobApplicationCount;

            // Pagination logic
            var totalJobs = jobList.Count();
            var totalPages = (int)Math.Ceiling((double)totalJobs / PageSize);

            var paginatedJobs = jobList.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedJobs);
        }
        public IActionResult Edit()
        {
            return View();
        }
        /*
        public IActionResult SingleJob(int id)
        {
            var job = _context.job.Include(c => c.Company)
                                   .FirstOrDefault(j => j.JobId == id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }*/


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}