using Translator_Project_Management.Database;
using Translator_Project_Management.Importers;
using Translator_Project_Management.Importers.XML;
using Translator_Project_Management.Importers.JSON;
using Translator_Project_Management.Repositories;
using Translator_Project_Management.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Models.Database;
using Microsoft.AspNetCore.Identity;
using Translator_Project_Management.Exporters;
using Translator_Project_Management.Exporters.JSON;
using Translator_Project_Management.Exporters.XLIFF;
using Translator_Project_Management.Services;
using IEmailSender = Translator_Project_Management.Services.Interfaces.IEmailSender;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

string connectionString = configuration.GetConnectionString("MySqlDatabase") ?? throw new InvalidOperationException("Connection string 'LocDbContextConnection' not found.");

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

//Exporter services
builder.Services.AddTransient<IExporter, JSONExporter>();
builder.Services.AddTransient<IExporter, XliffExporter>();

//File import and export services
builder.Services.AddTransient<FileImportService>();
builder.Services.AddTransient<FileExportService>();

//Email service configuration with DI
builder.Services.AddTransient<IEmailSender>(provider =>
{
    var senderEmail = configuration.GetValue<string>("EmailService:Email");
    var senderPassword = configuration.GetValue<string>("EmailService:Password");
	return new EmailSender(senderEmail, senderPassword);
});

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
