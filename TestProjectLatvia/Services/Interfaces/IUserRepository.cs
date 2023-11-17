using Microsoft.AspNetCore.Identity;
using TestProjectLatvia.ViewModels;

namespace TestProjectLatvia.Services.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Registers a new user with the provided RegisterModel.
    /// </summary>
    /// <param name="model">RegisterModel object containing registration information for the user.</param>
    /// <returns>
    ///     Returns a Task containing the RegisterModel representing the newly registered user.
    ///     The method initiates the registration process for a user based on the provided model.
    /// </returns>
    Task<RegisterModel> Register(RegisterModel model);
    /// <summary>
    /// Registers a new administrator with the provided RegisterModel.
    /// </summary>
    /// <param name="model">RegisterModel object containing registration information for the administrator.</param>
    /// <returns>
    ///     Returns a Task containing the RegisterModel representing the newly registered administrator.
    ///     The method initiates the registration process for an administrator based on the provided model.
    /// </returns>
    Task<RegisterModel> RegisterManager(RegisterModel model);
    /// <summary>
    /// Attempts to authenticate a user by verifying the provided email and password.
    /// </summary>
    /// <param name="model">LoginModel object containing user credentials (email and password).</param>
    /// <returns>
    ///     Returns a Task containing the SignInResult, indicating the outcome of the sign-in attempt.
    ///     Successful authentication results in a SignInResult with details about the success.
    ///     Throws exceptions for invalid email format, non-existent user, or incorrect password.
    /// </returns>
    Task<SignInResult> Login(LoginModel model);
    Task<CreateUserModel> CreateUser(CreateUserModel model);
}