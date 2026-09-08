// using jvPo.dbRepo;
using Microsoft.EntityFrameworkCore;
using jvPo.Models;
using jvPo.Application.Services;
using jvPo.Application.Interface;
using jvPo.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DevExpress.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter your JWT token."
    });

    options.AddSecurityRequirement(document =>
        new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddMvc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ApplicationDbContext>(ops => ops.UseSqlServer(builder.Configuration.GetConnectionString("DefCon")));
builder.Services.AddInfrastructure();
builder.Services.AddCors(ops => ops.AddPolicy("AllowFrontend", ops =>
{
    ops.WithOrigins().AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(ops =>
{
    ops.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("101", policy =>
    {
        policy.RequireClaim("CompanyCode", "101");
    });
});

//builder.Services.AddTransient<dbRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () =>
{
    return Results.Redirect("/Account/Login");
});
app.MapRazorPages();
app.Run();

// try
// {
//     var repo = app.Services.GetRequiredService<dbRepo>();
//     var addresses = repo.getAll();

//     if (addresses != null && addresses.Any())
//     {
//         Console.WriteLine($"Success! Found {addresses.Count()} records.");
//         foreach (var addr in addresses)
//         {
//             // Print one or two properties to verify data mapping
//             Console.WriteLine($"Address: {addr.Address}");
//         }
//     }
//     else
//     {
//         Console.WriteLine("Connection worked, but the table is empty.");
//     }
// }
// catch (Exception ex)
// {
//     // THIS is the part that will tell you if the SQL 2000 server is rejecting you
//     Console.WriteLine("--- CONNECTION FAILED ---");
//     Console.WriteLine($"Error Message: {ex.Message}");
//     Console.WriteLine($"Stack Trace: {ex.StackTrace}");
// }