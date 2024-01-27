using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Smtp;
using FMJ_Backend;
using FMJ_Backend.Datos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),options => options.CommandTimeout(180));
});



builder.Services.Configure<ServicioCorreoConfig>(builder.Configuration.GetSection("ServicioCorreoConfig"));

/*builder.Services.Configure<KestrelServerOptions>(options => {
options.Limits.MaxRequestHeadersTotalSize = 52428800; // Tamaño en bytes
});*/
builder.Services.Configure<HttpSysOptions>(options => { options.MaxRequestBodySize = 52428800; });
builder.Services.AddCors(options => {
    options.AddPolicy(
            name: "AllOrigins",
            builder => {
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "FMJ",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TOKEN"))
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseCors("AllOrigins");

app.UseAuthentication();

app.UseAuthorization();



app.MapControllers();





app.Run();
