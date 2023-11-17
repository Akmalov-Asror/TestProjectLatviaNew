using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestProjectLatvia.Domains;

namespace TestProjectLatvia.Data;

/// <summary>
/// Configuration class for seeding IdentityRole entities into the database using Entity Framework Core's IEntityTypeConfiguration.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    /// <summary>
    /// Initializes a new instance of the RoleConfiguration class with the provided services.
    /// </summary>
    /// <param name="services">The service provider.</param>
    public RoleConfiguration(IServiceProvider services) => this.Services = services;

    /// <summary>
    /// Gets or sets the service provider.
    /// </summary>
    public IServiceProvider Services { get; set; }

    /// <summary>
    /// Configures the entity of type IdentityRole.
    /// </summary>
    /// <param name="builder">The entity type builder for IdentityRole.</param>
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        // Retrieves the RoleManager<IdentityRole> service from the service provider.
        var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();

        // Retrieves role names from the ERole enum and creates IdentityRole instances.
        var roles = Enum.GetNames<ERole>().Select(x => new IdentityRole(x.ToUpper()) { NormalizedName = roleManager.NormalizeKey(x.ToUpper()) });

        // Configures seeding of IdentityRole entities with data obtained from roles.
        builder.HasData(roles);
    }
}