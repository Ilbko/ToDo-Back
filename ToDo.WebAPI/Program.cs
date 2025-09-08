using Microsoft.EntityFrameworkCore;
using ToDo.DAL.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseSqlServer(connectionString, opt =>
    {
        opt.MigrationsAssembly(typeof(ToDoDbContext).Assembly.GetName().Name);
        opt.MigrationsHistoryTable("__EFMigrationsHistory", schema: "entity_framework");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
