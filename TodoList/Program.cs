using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TodoList.Endpoints;
using TodoList.Entity;
using TodoList.Services;
using TodoList.Validation;

var builder = WebApplication.CreateBuilder(args);
var secretKey = ApiSettings.GenerateSecretByte();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<TaskToDo>, TaskToDoValidator>();

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("Admin"));
});
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

//Allowing cors because different fe and be domain
app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/auth/v1")
    .MapAuthApiV1()
    .WithTags("Auth Endpoints");

app.MapGroup("/tasks/v1")
    .MapTaskApiV1()
    .RequireAuthorization()
    .WithTags("Tasks Endpoints");

app.MapGroup("/admin/user/v1")
    .MapUserApiV1()
    .RequireAuthorization("Admin")
    .WithTags("User Endpoints");

app.Run();


