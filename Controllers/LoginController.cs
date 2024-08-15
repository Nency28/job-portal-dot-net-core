using JobPortalApplication.Data;
using JobPortalApplication.Models.CompanyModel;
using JobPortalApplication.Models.Users;
using JobPortalApplication.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortalApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly DatabaseContext _context;
        IWebHostEnvironment hostingenvironment;

        public LoginController(DatabaseContext context, IWebHostEnvironment hostingenvironment)
        {
            _context = context;
            this.hostingenvironment = hostingenvironment;

        }

        /*
        public IActionResult Register2()
        {
            var combinedViewModel = new LoginViewModel
            {
                UserModel = new UserModel(),
                Userdto = new Userdto(),
                CompanyDto = new CompanyDto()
            };

            return View(combinedViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register2(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.UserModel.EntryDate = DateTime.Now;
                    viewModel.UserModel.UpdateDate = DateTime.Now;


                    
                    _context.usermodel.Add(viewModel.UserModel);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetInt32("UserId", viewModel.UserModel.UserId);

                    if (viewModel.UserModel.utype == 1)
                    {
                        TempData["TabToActivate"] = "contact"; // Tab id you want to activate

                        // return RedirectToAction("UserM");
                    }
                    else if (viewModel.UserModel.utype == 2)
                    {
                        TempData["TabToActivate"] = "contact"; // Tab id you want to activate

                        //return RedirectToAction("Employer");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(viewModel);
                }
            }
            // ViewBag.Countries = _context.country.ToList();

            return View(viewModel);
        }

        public IActionResult UserM()
        {
            var viewModel = new LoginViewModel
            {
                Userdto = new Userdto(),
                CompanyDto = new CompanyDto()
            };
            ViewBag.Courses = _context.course.ToList();

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserM(LoginViewModel viewModel)
        {

            string fileName = "";
            if (viewModel.Userdto.File != null)
            {
                string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "Files");
                fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewModel.Userdto.File.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Userdto.File.CopyToAsync(stream);
                }
            }
            int? userId = HttpContext.Session.GetInt32("UserId");


            if (userId.HasValue)
            {
                try
                {
                    var newUserDetails = new Userdata
                    {
                        UserId = userId.Value,
                        Gender = viewModel.Userdto.Gender,
                        Dob = viewModel.Userdto.Dob,
                        MartialStatus = viewModel.Userdto.MartialStatus,
                        Category = viewModel.Userdto.Category,
                        Address = viewModel.Userdto.Address,
                        Language = viewModel.Userdto.Language,
                        Hometown = viewModel.Userdto.Hometown,
                        Pincode = viewModel.Userdto.Pincode,
                        Course = viewModel.Userdto.Course,
                        Duration = viewModel.Userdto.Duration,
                        Grade = viewModel.Userdto.Grade,
                        Skills = viewModel.Userdto.Skills,
                        Resume = fileName
                    };

                    _context.userdata.Add(newUserDetails);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(viewModel);
                }
            }
            ViewBag.Courses = _context.course.ToList();

            return View(viewModel);
        }


        public IActionResult Employer()
        {
            var viewModel = new LoginViewModel
            {
                UserModel = new UserModel(),
                Userdto = new Userdto()
            };
            ViewBag.Industry = _context.industry.ToList();

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Employer(LoginViewModel viewModel)
        {
            String fileName = "";
            if (viewModel.CompanyDto.ImageFile != null)
            {
                String uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "Image");
                fileName = Guid.NewGuid().ToString() + "_" + viewModel.CompanyDto.ImageFile.FileName;
                string filepath = Path.Combine(uploadFolder, fileName);
                viewModel.CompanyDto.ImageFile.CopyTo(new FileStream(filepath, FileMode.Create));
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var company = new Company
                {
                    UserId = userId.Value,
                    CompanyName = viewModel.CompanyDto.CompanyName,
                    Industry = viewModel.CompanyDto.Industry,
                    Website = viewModel.CompanyDto.Website,
                    Location = viewModel.CompanyDto.Location,
                    Email = viewModel.CompanyDto.Email,
                    Mobile = viewModel.CompanyDto.Mobile,
                    Status = viewModel.CompanyDto.Status,
                    Description = viewModel.CompanyDto.Description,
                    Image = fileName
                };

                _context.company.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        */
     

        public IActionResult Register()
        {
            try
            {
                ViewBag.CandidateCount = _context.usermodel.Count(u => u.utype == 1);
            }
            catch (Exception ex)
            {
                ViewBag.CandidateCount = 0; 
            }
            return View();



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.usermodel.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        ViewBag.CandidateCount = _context.usermodel.Count(u => u.utype == 1);

                        return View(model);
                    }

                    model.EntryDate = DateTime.Now;
                    model.UpdateDate = DateTime.Now;



                    _context.usermodel.Add(model);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetInt32("UserId", model.UserId);

                    if (model.utype == 1)
                    {
                        return RedirectToAction("UserM");
                    }
                    else if (model.utype == 2)
                    {
                        return RedirectToAction("Employer");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    ViewBag.CandidateCount = _context.usermodel.Count(u => u.utype == 1);

                    return View(model);
                }
            }
            // ViewBag.Countries = _context.country.ToList();
            ViewBag.CandidateCount = _context.usermodel.Count(u => u.utype == 1);


            return View(model);
        }

        public IActionResult UserM()
        {
            ViewBag.Courses = _context.course.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserM(Userdto userdto)
        {

            string fileName = "";
            if (userdto.File != null)
            {
                string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "Files");
                fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(userdto.File.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userdto.File.CopyToAsync(stream);
                }
            }
            int? userId = HttpContext.Session.GetInt32("UserId");
         //   var qualifications = userdto.Qualification != null ? string.Join(",", userdto.Qualification) : string.Empty;


            if (userId.HasValue)
            {
                try
                {
                    var newUserDetails = new Userdata
                    {
                        UserId = userId.Value,
                        Gender = userdto.Gender,
                        Dob = userdto.Dob,
                        MartialStatus = userdto.MartialStatus,
                        Category = userdto.Category,
                        Address = userdto.Address,
                        Language = userdto.Language,
                        Hometown = userdto.Hometown,
                        Qualification = userdto.Qualification,

                        Pincode = userdto.Pincode,
                        Course = userdto.Course,
                        Duration = userdto.Duration,
                        Grade = userdto.Grade,
                        Skills = userdto.Skills,
                        Resume = fileName
                    };

                    _context.userdata.Add(newUserDetails);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Success", "Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving data. Please try again later.");
                    return View(userdto);
                }
            }
            ViewBag.Courses = _context.course.ToList();

            return View(userdto);
        }


        public IActionResult Employer()
        {
            ViewBag.industry = _context.industry.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Employer(CompanyDto companydto)
        {
            String fileName = "";
            if (companydto.ImageFile != null)
            {
                String uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "Image");
                fileName = Guid.NewGuid().ToString() + "_" + companydto.ImageFile.FileName;
                string filepath = Path.Combine(uploadFolder, fileName);
                companydto.ImageFile.CopyTo(new FileStream(filepath, FileMode.Create));
            }
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                var company = new Company
                {
                    UserId = userId.Value,
                    CompanyName = companydto.CompanyName,
                    Industry = companydto.Industry,
                    Website = companydto.Website,
                    Location = companydto.Location,
                    Email = companydto.Email,
                    Mobile = companydto.Mobile,
                    //  Status = companydto.Status,
                    Description = companydto.Description,
                    Image = fileName
                };


                _context.company.Add(company);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success", "Login");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

       
        public IActionResult Index()
        {


            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var isAdmin = HttpContext.Session.GetString("utype") == "3";

            if (isAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }

            var company = _context.company.FirstOrDefault(c => c.UserId == userId);
            if (company != null)
            {
                ViewBag.CompanyName = company.CompanyName; 

                ViewBag.CompanyId = company.CompanyId;
            }

            return View();
        }

        public IActionResult EditCompany(int id)
        {
            var company = _context.company.FirstOrDefault(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult EditCompany(int id, CompanyDto companyDto)
        {
            var company = _context.company.FirstOrDefault(c => c.CompanyId == id);
            if (company == null)
            {
                return NotFound();
            }

            String fileName = company.Image;

            if (companyDto.ImageFile != null)
            {
                String uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "Image");
                fileName = Guid.NewGuid().ToString() + "_" + companyDto.ImageFile.FileName;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    companyDto.ImageFile.CopyTo(fileStream);
                }
            }

            var userEmail = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] = "User email is not available in session.";
                return RedirectToAction("Login", "Login");
            }

            var employer = _context.usermodel.FirstOrDefault(e => e.Email == userEmail);
            if (employer == null)
            {
                TempData["ErrorMessage"] = "Employer not found.";
                return RedirectToAction("Login", "Login");
            }

            company.CompanyName = companyDto.CompanyName;
            company.Website = companyDto.Website;
            company.Location = companyDto.Location;
            company.Email = companyDto.Email;
            company.Mobile = companyDto.Mobile;
            company.Description = companyDto.Description;
            company.Image = fileName;
            company.UserId = employer.UserId;

            _context.SaveChanges();
            HttpContext.Session.SetString("CompanyName", company.CompanyName);
            HttpContext.Session.SetString("CompanyLogo", company.Image);
            return RedirectToAction("EditCompany", "Login");
        }

        public IActionResult Success()
        {
            return View();


        }


        public IActionResult Login()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.Session.Clear();

                if (model.Email == "admin@gmail.com" && model.Password == "admin")
                {
                    var adminIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Email) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var adminPrincipal = new ClaimsPrincipal(adminIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);



                    HttpContext.Session.SetString("utype", "3");


                    return RedirectToAction("AdminDashboard", "Admin");
                }

                var user = await _context.usermodel.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    var userIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Email) }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var userPrincipal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("Email", user.Email); 

                    if (user.utype == 2)
                    {
                        var company = await _context.company.FirstOrDefaultAsync(c => c.UserId == user.UserId);

                        if (company != null)
                        {
                            HttpContext.Session.SetString("CompanyName", company.CompanyName);
                            HttpContext.Session.SetString("CompanyLogo", company.Image);
                            HttpContext.Session.SetInt32("CompanyId", company.CompanyId);
                        }
                    }

                    if (user.utype == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.utype == 2)
                    {
                        return RedirectToAction("CompanyDashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
        
       
    }
}
    

