using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.DAL.Entities;

namespace ToDo.DAL.Data;

public class ToDoDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
       : base(options)
    {
    }

    public DbSet<ToDoTask> ToDoTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ToDoDbContext).Assembly);
    }
}