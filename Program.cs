using Translator_Project_Management.Database;
using Translator_Project_Management.Importers;
using Translator_Project_Management.Importers.XML;
using Translator_Project_Management.Repositories;
using Translator_Project_Management.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Models.Database;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

var connectionString = configuration.GetConnectionString("MySqlDatabase") ?? throw new InvalidOperationException("Connection string 'LocDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Adding DbContext (Entity ORM)
builder.Services.AddDbContext<LocDbContext>(options =>
            options.UseMySQL(connectionString));

//Adding Identity
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<LocDbContext>();

//Setting Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

//Repository services
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IFileRepository,  FileRepository>();
builder.Services.AddTransient<ILanguageRepository, LanguageRepository>();
builder.Services.AddTransient<ILineRepository<SourceLine>, SourceLineRepository>();
builder.Services.AddTransient<ILineRepository<TransLine>, TranslationRepository>();
builder.Services.AddTransient<IUserSourceLineMappingRepository, UserSourceLineMappingRepository>();

//Importer services
builder.Services.AddTransient<IImporter, XLIFFImporter>();
builder.Services.AddTransient<IImporter, JSONImporter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
	endpoints.MapDefaultControllerRoute().RequireAuthorization();
});

app.MapRazorPages();

app.Run();
