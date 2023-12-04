using LoLTournaments.Application.Infrastructure;
using LoLTournaments.Application.Services;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Infrastructure.Presistence;
using LoLTournaments.Shared.Abstractions;
using LoLTournaments.Shared.Common;
using LoLTournaments.WebApi.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgresDbConnection"),
            o =>
            {
                o.CommandTimeout(360);
                o.EnableRetryOnFailure();
                o.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => //CookieAuthenticationOptions
    {
        options.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddIdentity<UserEntity, IdentityRole>(o => 
    {
        o.SignIn.RequireConfirmedAccount = false;
        o.Password.RequireDigit = false;
        o.Password.RequireUppercase = false;
        o.Password.RequiredLength = 4;
        o.Password.RequireLowercase = false;
        o.Password.RequireNonAlphanumeric = false;
        o.User.RequireUniqueEmail = false;
        o.User.AllowedUserNameCharacters = builder.Configuration.GetSection("AllowedUserNameCharacters").Get<string>();
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Setup compression
builder.Services.AddResponseCompression(o =>
{
    o.MimeTypes = new[]
    {
        "application/octet-stream",
        "application/wasm", 
        "application/data", 
        "application/vnd.unity",
        "application/gzip",
        "application/x-gzip",
    };
    o.Providers.Add<BrotliCompressionProvider>();
    o.Providers.Add<GzipCompressionProvider>();
    o.EnableForHttps = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

ApplicationDi.Install(builder.Services, builder.Configuration);
builder.Services.AddResponseCaching();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = VersionInfo.SolutionName, Version = VersionInfo.APIVersion });
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();


DefaultSharedLogger.Initialize(app.Services.GetRequiredService<ISharedLogger>());
app.UseFileServer();
app.UseResponseCompression();
app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();

provider.Mappings.Add(".wasm.br", "application/wasm");
provider.Mappings.Add(".wasm.gz", "application/wasm");
provider.Mappings.Add(".unityweb", "application/octet-stream");
provider.Mappings.Add(".js.unityweb", "application/javascript");
provider.Mappings.Add(".data", "application/octet-stream");
provider.Mappings.Add(".data.br", "application/octet-stream");
provider.Mappings.Add(".data.gz", "application/octet-stream");
provider.Mappings.Add(".js.br", "application/javascript");
provider.Mappings.Add(".js.gz", "application/javascript");

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});


app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    o.DocumentTitle = $"{VersionInfo.SolutionName}";
    o.RoutePrefix = "swagger";
});
app.UseCors("AllowAll");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
var appTask = app.RunAsync();
app.Services.CreateScope().ServiceProvider.GetService<BootstrapService>();
await appTask;