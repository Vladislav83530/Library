using Library.API.Middlewares;
using Library.BLL.Services;
using Library.BLL.Services.Interfaces;
using Library.DAL.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options=>options.AddPolicy(name: "LibraryOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
        }));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("LibraryOrigins");

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();