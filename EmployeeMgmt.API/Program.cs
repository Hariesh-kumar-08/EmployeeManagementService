
using EmployeeMgmt.Application;
using EmployeeMgmt.Application.Interfaces;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EmployeeManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
 builder.Services.AddScoped<IEmployeeRepository , EmployeeRepository>(); 
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero // Adds precise time validation
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            context.Token = accessToken;
            Console.WriteLine($"Received token: {accessToken}");
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            if (context.Exception is SecurityTokenExpiredException)
            {
                Console.WriteLine($"Token expired: {context.Exception.Message}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            if (token != null)
            {
                Console.WriteLine($"Token validated. Expiration: {token.ValidTo}, Issuer: {token.Issuer}");
                foreach (var claim in token.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"OnChallenge: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        },
       
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvcOrigin",
        builder => builder
            .WithOrigins("https://localhost:7056")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//services.AddScoped<IDepartmentRepository, DepartmentRepository>();
var app = builder.Build();

//Create the database and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeManagementDbContext>();
    AppDbInitializer.SeedData(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
