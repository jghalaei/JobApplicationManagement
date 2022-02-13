using JobApplicationManagement.DB;
using JobApplicationManagement.Repositories;
using JobApplicationManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));


string conStr = builder.Configuration.GetConnectionString("JobAppConnectionString");
builder.Services.AddDbContext<JobAppDbContext>(opt => opt.UseSqlServer(conStr));
builder.Services.AddTransient<JobApplicationService>();
builder.Services.AddTransient<StatusHistoryService>();
builder.Services.AddTransient<JobApplicationRepository>();
builder.Services.AddTransient<StatusHistoryRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
