using Microsoft.AspNetCore.Identity;
using TestProjectLatvia.Data;
using TestProjectLatvia.Domains;
using TestProjectLatvia.ExtensionFunctions;
using TestProjectLatvia.Services.Interfaces;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.Services.Implementations;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserRepository(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<RegisterModel> Register(RegisterModel model)
    {
        if (!CheckEmail.IsValidEmail(model.Email))
            throw new Exception("Invalid email address format");
        var existUser = await _userManager.FindByEmailAsync(model.Email);
        if (existUser != null)
            throw new Exception("Email already taken ");
        var user = new User { UserName = model.Name, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "USER");
            await _context.SaveChangesAsync();
        }
        foreach (var error in result.Errors)
            throw new Exception($"{error.Description}");

        return model ?? new RegisterModel();

    }

    public async Task<RegisterModel> RegisterManager(RegisterModel model)
    {
        if (!CheckEmail.IsValidEmail(model.Email))
            throw new Exception("Invalid email address format");
        var existUser = await _userManager.FindByEmailAsync(model.Email);
        if (existUser != null)
            throw new Exception("Email already taken ");
        var user = new User
        {
            UserName = model.Name,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        Console.WriteLine(result.Errors);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "MANAGER");
            await _context.SaveChangesAsync();
        }
        foreach (var error in result.Errors) throw new Exception($"{error.Description}");

        return model ?? new RegisterModel();
    }

    public async Task<SignInResult> Login(LoginModel model)
    {
        if (!CheckEmail.IsValidEmail(model.Email))
            throw new Exception("Invalid email address format");
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) throw new Exception("Invalid email or password");
        var passResult = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!passResult)
            throw new Exception("Invalid email or password");

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        if (!result.Succeeded)
            throw new Exception("Invalid email or password");

        return result;

    }

    public async Task<CreateUserModel> CreateUser(CreateUserModel model)
    {
        if (!CheckEmail.IsValidEmail(model.Email))
            throw new Exception("Invalid email address format");
        var existUser = await _userManager.FindByEmailAsync(model.Email);
        if (existUser != null)
            throw new Exception("Email already taken ");
        var user = new User
        {
            UserName = model.Name,
            Email = model.Email,
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        Console.WriteLine(result.Errors);
        if (model.Role == ERole.MANAGER)
        {
            if (!result.Succeeded) return model ?? new CreateUserModel();
            await _userManager.AddToRoleAsync(user, "MANAGER");
            await _context.SaveChangesAsync();
        }
        else if (model.Role == ERole.USER)
        {
            if (!result.Succeeded) return model ?? new CreateUserModel();
            await _userManager.AddToRoleAsync(user, "USER");
            await _context.SaveChangesAsync();
        }
        else if (model.Role == ERole.ADMIN)
        {
            if (!result.Succeeded) return model ?? new CreateUserModel();
            await _userManager.AddToRoleAsync(user, "ADMIN");
            await _context.SaveChangesAsync();
        }
        return model ?? new CreateUserModel();

    }
}