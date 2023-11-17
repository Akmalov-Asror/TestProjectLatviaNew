using Microsoft.AspNetCore.Identity;
using TestProjectLatvia.Domains;
using Task = System.Threading.Tasks.Task;

namespace TestProjectLatvia.Data;

public class Seed
{
    /// <summary>
    /// Seeds roles and users into the database.
    /// </summary>
    /// <param name="serviceProvider">The service provider for obtaining necessary services.</param>
    public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
    {
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            try
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await SeedRolesAsync(roleManager);

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await SeedUsersAsync(userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during seeding: {ex.Message}");
            }
        }
    }
    /// <summary>
    /// Seeds roles into the database.
    /// </summary>
    /// <param name="roleManager">Role manager for managing identity roles.</param>
    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        await CreateRoleAsync(roleManager, ERole.ADMIN);
        await CreateRoleAsync(roleManager, ERole.MANAGER);
        await CreateRoleAsync(roleManager, ERole.USER);
    }
    /// <summary>
    /// Creates a role if it does not exist.
    /// </summary>
    /// <param name="roleManager">Role manager for managing identity roles.</param>
    /// <param name="role">The role to create.</param>
    private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, ERole role)
    {
        var roleName = role.ToString();

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
            Console.WriteLine($"{roleName} role created successfully.");
        }
    }
    /// <summary>
    /// Seeds users into the database.
    /// </summary>
    /// <param name="userManager">User manager for managing identity users.</param>
    private static async Task SeedUsersAsync(UserManager<User> userManager)
    {
        await CreateUserAsync(userManager, "Jenny@gmail.com", "Jenny", "A0601221a_", ERole.ADMIN);
        await CreateUserAsync(userManager, "Coma@gmail.com", "Coma", "B0601221b_", ERole.MANAGER);
        await CreateUserAsync(userManager, "Vin@gmail.com", "Vin", "C0601221c_", ERole.USER);
    }
    /// <summary>
    /// Creates a user if it does not exist and adds them to a specified role.
    /// </summary>
    /// <param name="userManager">User manager for managing identity users.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="role">The role to assign to the user.</param>
    private static async Task CreateUserAsync(UserManager<User> userManager, string email, string userName, string password, ERole role)
    {
        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser == null)
        {
            var newUser = new User
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, role.ToString());
                Console.WriteLine($"{userName} user created and added to {role} role successfully.");
            }
            else Console.WriteLine($"Error creating {userName} user: {string.Join(", ", result.Errors)}");
        }
        else Console.WriteLine($"{userName} user already exists.");
    }
}