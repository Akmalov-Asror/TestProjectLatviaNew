using Microsoft.AspNetCore.Identity;

namespace TestProjectLatvia.Domains;

public class User : IdentityUser
{
    public List<Task> Tasks { get; set; }
}