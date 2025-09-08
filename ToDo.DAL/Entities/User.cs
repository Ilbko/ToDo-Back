using Microsoft.AspNetCore.Identity;

namespace ToDo.DAL.Entities;

public class User : IdentityUser<long>
{    
    public DateTime CreatedAt { get; init; }
}