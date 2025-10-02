using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
