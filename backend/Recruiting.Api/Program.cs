using Microsoft.EntityFrameworkCore;
using Recruiting.Application.Interfaces;
using Recruiting.Application.Services;
using Recruiting.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Recruiting.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
builder.Services.AddScoped<IUserService, UserService>();

// Global error handling
builder.Services.AddExceptionHandler(options =>
{
    options.ExceptionHandlingPath = "/error";
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
