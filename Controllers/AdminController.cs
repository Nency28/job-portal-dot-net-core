using JobPortalApplication.Data;
using JobPortalApplication.Models;
using JobPortalApplication.Models.Users;

using JobPortalApplication.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JobPortalApplication.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly DatabaseContext _context;

        public AdminController(DatabaseContext context)
        {
            _context = context;
        }
        [Authorize]

        public IActionResult Index()
        {
            

            return View();


        }
        public IActionResult AdminDashboard()
        {

            int totalUsers = _context.userdata.Count();
            ViewBag.TotalUsers = totalUsers;

            int totalCompany = _context.company.Count();
            ViewBag.TotalCompany = totalCompany;

            int totalJob = _context.job.Count();
            ViewBag.TotalJob = totalJob;

            int appliedJob = _context.jobapplication.Count();
            ViewBag.TotalAppliedJob = appliedJob;

            int totalInterview = _context.interview.Count();
            ViewBag.TotalInterview = totalInterview;



            return View();


        }
        [Authorize]
        public async Task<IActionResult> CompanyDashboard()
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

            var totalJobsCount = employerJobs.Count;

            if (!employerJobs.Any())
            {
                ViewBag.ErrorMessage = "No jobs found for the current employer.";
                ViewBag.AppliedJobCount = 0; 
                ViewBag.TotalJobsCount = 0; 
                return View();
            }

            var appliedJobCount = await _context.jobapplication
                .Where(j => j.EntryStatus == 2 && j.JobId.HasValue && employerJobs.Contains(j.JobId.Value))
                .CountAsync();

            ViewBag.AppliedJobCount = appliedJobCount;
            ViewBag.TotalJobsCount = totalJobsCount;

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        //----------------------COUNTRY----------------------------
        
        public IActionResult AddCountry()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCountry([Bind("CountryName, Status")] Country c)
        {
            if (ModelState.IsValid)
            {
                _context.country.Add(c);
                _context.SaveChanges();
                return RedirectToAction(nameof(ViewCountry));
            }
            return View(c);
        }
        public async Task<IActionResult> ViewCountry()
        {
            var countries = await _context.country.ToListAsync();
            return View(countries);
        }
        public IActionResult EditCountry(int id)
        {
            var country = _context.country.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCountry(int id, Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewCountry));
            }

            return View(country);
        }

        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewCountry));
        }
        
        //-----------------End country
        //----------------------STATE----------------------------

        
        public IActionResult AddState()
        {
            ViewBag.Countries = _context.country.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState( StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var state = new State
                {
                    StateName = model.StateName,
                    CountryId = model.CountryId,
                    Status = model.Status
                };

                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewState));
            }

            ViewBag.Countries = _context.country.ToList();
            return View(model);
        }

        public IActionResult ViewState()
        {
            var states = _context.state.Include(s => s.Country).ToList();
            return View(states);
        }


        public IActionResult EditState(int id)
        {
            var state = _context.state.Find(id);
            if (state == null)
            {
                return NotFound();
            }
            ViewBag.Countries = _context.country.ToList();
            return View(state);
        }

        [HttpPost]
        public IActionResult getStates(int countryid)
        {
            // var states = _context.state.Include(s => s.Country).ToList();
            
            var states = _context.state.Where(s => s.CountryId == countryid).ToList();
            var json = JsonConvert.SerializeObject(states);
            
            return new ContentResult
            {
                Content = json
            };
                
        }
        [HttpPost]
        public IActionResult getCountry()
        {
            var countries =  _context.country.ToList();
            var json = JsonConvert.SerializeObject(countries);

            return new ContentResult
            {
                Content = json
            };
        }
        [HttpPost]
        public IActionResult getCity(int stateid)
        {
            // var states = _context.state.Include(s => s.Country).ToList();

            var city = _context.city.Where(s => s.StateId == stateid).ToList();
            var json = JsonConvert.SerializeObject(city);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, State s)
        {
            if (id != s.StateId)
            {

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(s);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewState));
            }

            ViewBag.Countries = _context.country.ToList();
            return View(s);
        }

        public async Task<IActionResult> DeleteState(int id)
        {
            var state = await _context.state.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.state.Remove(state);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewState));
        }
        
        //-------------End State--------------------


        //------------------CITY-------------------
        
        public IActionResult AddCity()
        {

            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
           // ViewBag.StatesByCountry = _context.state.GroupBy(s => s.CountryId).ToDictionary(g => g.Key, g => g.ToList());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(int countryId, CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var city = new City
                {
                    CityName = model.CityName,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    Status = model.Status
                };

                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewCity));
            }
           // ViewBag.States = _context.state.Where(s => s.CountryId == countryId).ToList();

            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
            return View(model);
        } 

        /*
        public IActionResult GetStates(int countryId)
        {
            var states = _context.state.Where(s => s.CountryId == countryId).ToList();
            return PartialView("_StatesDropdownPartial", states);
        } */
        
        public IActionResult ViewCity()
        {
            //var city = _context.city.Include(s => s.Country).ToList();
           // return View(city);
              var cities = _context.city.Include(c => c.Country).Include(c => c.State).ToList();
              return View(cities);
        }
        /*
        public IActionResult EditCity(int id)
        {
            var city = _context.city.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewCity));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.city.Any(e => e.CityId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                // Log the ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
            return View(city);
        } */


        public IActionResult EditCity(int id)
        {
            var city = _context.city.Find(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewCity));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine("Error updating city: " + ex.Message);
                    throw;
                }
            }
            else
            {
                // Log the ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("ModelState Error: " + error.ErrorMessage);
                }
            }

            ViewBag.Countries = _context.country.ToList();
            ViewBag.States = _context.state.ToList();
            return View(city);
        }

        private bool CityExists(int id)
        {
            return _context.city.Any(e => e.CityId == id);
        } /* 
        public IActionResult DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = _context.city.Include(c => c.Country).Include(c => c.State).FirstOrDefault(c => c.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        } */

        public async Task<IActionResult> DeleteCity(int id)
        {
            var city = await _context.city.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.city.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewCity));
        }

        //---------------------------End City---------------------------
        [Authorize]

        public IActionResult Master()
        {
            return View();

        }

        //----------------Industry---------------------
        public IActionResult AddIndustry()
        {
            return View();
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddIndustry(Industry model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.EntryDate = DateTime.Now;
                    model.UpdateDate = DateTime.Now;



                    _context.industry.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ViewIndustry));


                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ViewIndustry()
        {
            var industry = await _context.industry.ToListAsync();
            return View(industry);
        }

        public IActionResult EditIndustry(int id)
        {
            var industry = _context.industry.Find(id);
            if (industry == null)
            {
                return NotFound();
            }
            return View(industry);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditIndustry(int id, Industry industry)
        {
            if (id != industry.IndustryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingIndustry = await _context.industry.FindAsync(id);
                    if (existingIndustry == null)
                    {
                        return NotFound();
                    }

                    industry.EntryDate = existingIndustry.EntryDate;

                    industry.UpdateDate = DateTime.Now;

                    _context.Entry(existingIndustry).CurrentValues.SetValues(industry);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewIndustry));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating data. Please try again later.");
                    return View(industry);
                }
            }

            return View(industry);
        }
        public async Task<IActionResult> DeleteIndustry(int id)
        {
            var industry = await _context.industry.FindAsync(id);
            if (industry == null)
            {
                return NotFound();
            }

            _context.industry.Remove(industry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewIndustry));
        }


        //-------------End Industry----------

        //------------Add Course-------------

        public IActionResult AddCourse()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(Course model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    



                    _context.course.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ViewCourse));


                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(model);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> ViewCourse()
        {
            var courses = await _context.course.ToListAsync();
            return View(courses);
        }

        public IActionResult EditCourse(int id)
        {
            var course = _context.course.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewCourse));
            }

            return View(course);
        }
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewCourse));
        }



        //------------End Course-----------

        //------------Add Board/University-------------

        public IActionResult AddQualification()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQualification(Qualification model)
        {
            if (ModelState.IsValid)
            {
                try
                {




                    _context.qualification.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ViewQualification));


                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(model);
                }
            }

            return View(model);
        }
        public async Task<IActionResult> ViewQualification()
        {
            var qualification = await _context.qualification.ToListAsync();
            return View(qualification);
        }
        public IActionResult EditQualification(int id)
        {
            var qualification = _context.qualification.Find(id);
            if (qualification == null)
            {
                return NotFound();
            }
            return View(qualification);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQualification(int id, Qualification qualification)
        {
            if (id != qualification.QuaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(qualification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewQualification));
            }

            return View(qualification);
        }
        public async Task<IActionResult> DeleteQualification(int id)
        {
            var qualification = await _context.qualification.FindAsync(id);
            if (qualification == null)
            {
                return NotFound();
            }

            _context.qualification.Remove(qualification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewQualification));
        }



        //------------End Board/University-----------

        public IActionResult ChangePassword()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                var model = new ChangePasswordModel { UserId = userId.Value };
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login"); 
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.usermodel.FindAsync(model.UserId);

                    if (user != null && user.Password == model.CurrentPassword)
                    {
                        user.Password = model.NewPassword;
                        user.UpdateDate = DateTime.Now; 

                        await _context.SaveChangesAsync(); 
                        var successMessage = "Password changed successfully!";
                        return Content($"<script>alert('{successMessage}'); window.location.href = '/Admin/Index';</script>", "text/html");
                    }
                    else
                    {
                        ModelState.AddModelError("CurrentPassword", "Incorrect current password.");
                        return View(model);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while changing the password. Please try again later.");
                    return View(model);
                }
            }
            return View(model);
        }

        /* -- for display company list in Company side  (Edit, Delete) ---*/


        public async Task<IActionResult> ViewEmployerCompany()
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
            var company = await _context.company
                               .Include(c => c.industry)
                               .FirstOrDefaultAsync(c => c.UserId == user.UserId);

            if (company != null)
            {
                HttpContext.Session.SetString("CompanyName", company.CompanyName);
                HttpContext.Session.SetString("CompanyLogo", company.Image);

                Console.WriteLine("CompanyName: " + company.CompanyName);
                Console.WriteLine("CompanyLogo: " + company.Image);
            }
            else
            {
                HttpContext.Session.Remove("CompanyName");
                HttpContext.Session.Remove("CompanyLogo");
            }

            var companies = await _context.company
                                          .Include(c => c.industry)
                                          .Where(c => c.UserId == user.UserId)
                                          .ToListAsync();
            
            return View(companies);
        }
        /* -- for display company list in admin side (Approval) ---*/
        public IActionResult ViewCompany()
        {
            var companies = _context.company.Include(c => c.industry).ToList();
            return View(companies); 
        }

        /* -- for display User list in admin side (Approval) ---*/

        public IActionResult ViewUser()
        {
            var users = _context.userdata.Include(c => c.course).Include(c => c.UserModel).ToList();
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, int status)
        {
            var company = await _context.company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }
            if (company.Status == null)
            {
                company.Status = status == 1 ? 0 : 1;
            }
            else
            {
                // Toggle the status
                company.Status = status;
            }

            _context.Update(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewCompany));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus2(int id, int status)
        {
            var user = await _context.userdata.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            if (user.Status == null)
            {
                user.Status = status == 1 ? 0 : 1;
            }
            else
            {
                // Toggle the status
                user.Status = status;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewUser));
        }


        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
