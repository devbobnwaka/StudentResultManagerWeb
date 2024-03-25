using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using StudentResultManager.Data;
using StudentResultManager.Models;
using StudentResultManager.Models.Dto;
using System;

namespace StudentResultManager.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext dbContext,
            IUserStore<ApplicationUser> userStore,
            ILogger<AccountController> logger,
            IWebHostEnvironment hostingEnvironment
            )
        {
            _dbContext = dbContext;
            _userStore = userStore;
            _userManager = userManager;
            _emailStore = GetEmailStore();
            _hostingEnvironment = hostingEnvironment;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email,
                           model.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        private async Task<string> ProcessPhoto(CreateStudentViewModel studentModel)
        {
            if (studentModel.PhotoPath != null)
            {
                try
                {
                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    // Check if the directory exists, if not, create it
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "-" + studentModel.PhotoPath.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await studentModel.PhotoPath.CopyToAsync(fileStream);
                    } 
                    return uniqueFileName;
                } catch (Exception ex)
                {
                    Console.WriteLine($"Error processing photo: {ex.Message}");
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(CreateStudentViewModel studentModel)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = Activator.CreateInstance<ApplicationUser>();
                user.FirstName = studentModel.FirstName;
                user.LastName = studentModel.LastName;

                await _userStore.SetUserNameAsync(user, studentModel.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, studentModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, studentModel.Password);
                if (result.Succeeded)
                {
                    string? uniqueFilePath = await ProcessPhoto(studentModel);
                    Student student = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Class = studentModel.Class,
                        EnrollmentDate = studentModel.EnrollmentDate,
                        ContactInformation = studentModel.ContactInformation,
                        DateOfBirth = studentModel.DateOfBirth,
                        PhotoPath = uniqueFilePath,
                        ApplicationUser = user
                    };
                    await _dbContext.Students.AddAsync(student);
                    await _dbContext.SaveChangesAsync();
                    var res = await _signInManager.PasswordSignInAsync(studentModel.Email, studentModel.Password, true, lockoutOnFailure: false);
                    if (res.Succeeded)
                    {
                        _logger.LogInformation("Newly registered User logged in.");
                        return RedirectToAction("Index", "Home");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

    }
}
