using LoLTournaments.Application.Infrastructure;
using LoLTournaments.Domain.Abstractions;
using LoLTournaments.Domain.Entities;
using LoLTournaments.Infrastructure.Presistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Di Setup
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
        o.User.RequireUniqueEmail = true;
        o.User.AllowedUserNameCharacters = builder.Configuration.GetSection("AllowedUserNameCharacters").Get<string>();
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Setup compression
builder.Services.AddResponseCompression(o =>
{
    o.MimeTypes = new[]
    {
        "application/octet-stream", "application/wasm", "application/data", "application/vnd.unity",
        "application/x-gzip"
    };
    o.Providers.Add<BrotliCompressionProvider>();
    o.Providers.Add<GzipCompressionProvider>();
    o.EnableForHttps = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

ApplicationInstaller.Install(builder.Services, builder.Configuration);
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddResponseCaching();
builder.Services.AddControllersWithViews();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Di Setup

var app = builder.Build();

app.UseResponseCompression();
app.UseHttpsRedirection();

var provider = new FileExtensionContentTypeProvider();

provider.Mappings.Remove(".unityweb");
provider.Mappings.Remove(".wasm");
provider.Mappings.Remove(".data");
provider.Mappings.Remove(".js");

provider.Mappings.Add(".wasm", "application/wasm");
provider.Mappings.Add(".unityweb", "application/octet-stream");
provider.Mappings.Add(".data", "application/octet-stream");
provider.Mappings.Add(".js", "application/javascript");

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,

    OnPrepareResponse = context =>
    {
        if (context.Context.Request.Path.Value != null && context.Context.Request.Path.Value.EndsWith(".unityweb"))
            context.Context.Response.Headers.Add("content-encoding", "br");
    }
});

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
// app.UseEndpoints(endpoints => endpoints.MapControllers());
app.MapGet("/", () => "Hello World!");
app.Run();