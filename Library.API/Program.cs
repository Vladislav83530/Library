using Library.API.Middlewares;
using Library.BLL.Services;
using Library.BLL.Services.Interfaces;
using Library.DAL.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "LibraryDB"));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<RequestLoggingMiddleware>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ILibraryService, LibraryService>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();