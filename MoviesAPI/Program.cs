using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MoviesAPI;
using MoviesAPI.APIBehavior;
using MoviesAPI.Filters;
using MoviesAPI.Helpers;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
    options.Filters.Add(typeof(ParseBadRequest));
}).ConfigureApiBehaviorOptions(BadRequestBehavior.Parse);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetValue<string>("DefaultConnection"), sqlOptions => sqlOptions.UseNetTopologySuite());
});
builder.Services.AddTransient<CustomActionFilter>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton(provider => new MapperConfiguration(config =>
    {
        var geometry = provider.GetRequiredService<GeometryFactory>();
        config.AddProfile(new AutoMapperHelper(geometry));
    }).CreateMapper());
builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
builder.Services.AddScoped<IFileStorageService, InAppStorageService>();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddResponseCaching();
builder.Services.AddCors(options =>
{
    var frontedUrl = builder.Configuration.GetValue<string>("frontend_Url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontedUrl).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
    });
});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["keyjwt"])),
        ClockSkew = TimeSpan.Zero

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Loglama için startup kullandý benim nasýl birþey kullanmam lazým
//app.Use(async (context, next) =>
//{
//    using (var swapStream = new MemoryStream())
//    {
//        var originalResponseBody = context.Response.Body;
//        context.Response.Body = swapStream;
//        await next.Invoke();
//        swapStream.Seek(0, SeekOrigin.Begin);
//        string responseBody = new StreamReader(swapStream).ReadToEnd();
//        swapStream.Seek(0, SeekOrigin.Begin);
//        await swapStream.CopyToAsync(originalResponseBody);
//        context.Response.Body = originalResponseBody;
//        //logger.LogInformation(responseBody);
//    }

//});
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
//defaultta yoktu

//app.UseResponseCaching();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
