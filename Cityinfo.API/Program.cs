using Cityinfo.API;
using Cityinfo.API.Entities;
using Cityinfo.API.Repository;
using Cityinfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers(options =>
{

    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();
//builder.Services.AddMvc();
//builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("CityInfoApiBearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "???? ???? JWT ??? ?? ???? ????"
    });

    setupAction.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth" }
            }, new List<string>() }
    });
});


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//builder.Services.AddTransient<IMailService,LocalMailService>();
//builder.Services.AddTransient<IMailService, CloudMailService>();
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddDbContext<CityinfoDbContex>(option =>
{
    option.UseSqlite(
            builder.Configuration["ConnectionStrings:CityConnectionString"]
        );
});

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(Option =>
    {
        Option.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"])
                )
        };
    });



var app = builder.Build();




//builder.Services.AddKeyedTransient<>();







//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello TopLearn");
//});


#region Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




// Controller/Action/id?

app.UseEndpoints(Endpoint =>
{
    Endpoint.MapControllers();
});


#endregion

app.Run();
