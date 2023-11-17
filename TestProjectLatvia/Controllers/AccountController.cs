using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Polly;
using TestProjectLatvia.Domains;
using TestProjectLatvia.FluentValidation;
using TestProjectLatvia.Services.Interfaces;
using TestProjectLatvia.ViewModels;
using Task = TestProjectLatvia.Domains.Task;

namespace TestProjectLatvia.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IToastNotification _toastNotification;
    private readonly SignInManager<User> _signInManager;

    public AccountController(IToastNotification toastNotification, IUserRepository userRepository, SignInManager<User> signInManager)
    { 
        _toastNotification = toastNotification;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        var validationResult = await new RegisterModelValidator().ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
        }
        try
        {
            if (ModelState.IsValid) await _userRepository.Register(model);
            _toastNotification.AddSuccessToastMessage("Registration successfully!");
            return RedirectToAction("Index", "Home");
        }
        catch(Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
    }

    public IActionResult RegisterManager() => View();


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterManager(RegisterModel model)
    {
        var validationResult = await new RegisterModelValidator().ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(model);
        }
        try
        {
            await _userRepository.RegisterManager(model);
         
            _toastNotification.AddSuccessToastMessage("Registration successfully!");

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
    }

    public IActionResult Login() => View();


    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var validationResult = await new LoginModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(model);
        }
        var retryPolicy = Policy.Handle<Exception>()
            .RetryAsync(3, (exception, retryCount) =>
            {
                Console.WriteLine($"An exception occurred during login. Retry attempt: {retryCount}. Exception: {exception}");
            });
        try
        {
            var signInResult = await retryPolicy.ExecuteAsync(async () =>
            {
                var result = await _userRepository.Login(model);
                return result;
            });
            return RedirectToAction("Index", "Task");
        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(model);
        }
        
    }

    public IActionResult AccessDenied() => RedirectToAction("Index", "Home");

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Main", "Account");
    }
    public async Task<IActionResult> Main()
    {
        return User.Identity!.IsAuthenticated ? RedirectToAction("Index", "Home") : View();
    }
    public IActionResult CreateUser() => View();
    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserModel models)
    {
        var validationResult = await new CreateModelValidator().ValidateAsync(models);

        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return View(models);
        }
        try
        {
            await _userRepository.CreateUser(models);

            _toastNotification.AddSuccessToastMessage("Registration successfully!");

            return RedirectToAction("Index", "Home");

        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage(ex.Message);
            return View(models);
        }
    }
}
