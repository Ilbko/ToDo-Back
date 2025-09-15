using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ToDo.BLL;
using ToDo.BLL.Commands.ToDoTasks.Create;
using ToDo.BLL.Commands.ToDoTasks.Update;
using ToDo.BLL.Validators.ToDoTasks;
using ToDo.DAL.Data;
using ToDo.DAL.Repositories.Interfaces.Base;
using ToDo.DAL.Repositories.Realizations.Base;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(BllAssemblyMarker).Assembly);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(BllAssemblyMarker).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<BllAssemblyMarker>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

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

app.MapControllers();

app.Run();
