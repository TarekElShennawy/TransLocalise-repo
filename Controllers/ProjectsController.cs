using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Presentation;
using Translator_Project_Management.Repositories.Interfaces;
using Translator_Project_Management.Services;

namespace Translator_Project_Management.Controllers
{
	[Authorize(Roles = "Manager")] //Projects is only accessible on user log-in for managers
	public class ProjectsController : Controller
	{
		private readonly IProjectRepository _projectRepository;
		private readonly IClientRepository _clientRepository;
		private readonly IFileRepository _fileRepository;
		private readonly IUserSourceLineMappingRepository _userToSourceMappingRepository;
		private readonly LocDbContext _dbContext;

		private readonly UserManager<User> _userManager;

		private readonly FileImportService _importService;
		private readonly FileExportService _exportService;

		public ProjectsController(UserManager<User> userManager, IProjectRepository projectRepository, IClientRepository clientRepository,
			IFileRepository fileRepository, IUserSourceLineMappingRepository userToSourceMappingRepository, LocDbContext dbContext,
			FileExportService exportService, FileImportService importService)
		{
			_projectRepository = projectRepository;
			_clientRepository = clientRepository;
			_fileRepository = fileRepository;
			_userToSourceMappingRepository = userToSourceMappingRepository;

			_userManager = userManager;
			_dbContext = dbContext;
			_exportService = exportService;
			_importService = importService;
		}

		// GET: ProjectsController
		public IActionResult Index()
		{
			return View(BuildProjectsViewModel());
		}

		// GET: ProjectsController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ProjectsController/Create
		public ActionResult Create()
		{
			ViewData["projectStatuses"] = new SelectList(ProjectStatus.GetProjectStatuses(), "Status", "Status");
			ViewData["clients"] = new SelectList(_clientRepository.GetAll().ToList(), "Id", "Name");

			return View();
		}

		// POST: ProjectsController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([FromForm] Project project)
		{
			project.ManagerId = _userManager.GetUserId(this.User);

			_projectRepository.Insert(project);

			//Redirect to the table of projects after creating a new project
			return RedirectToAction("Index");
		}

		// GET: ProjectsController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ProjectsController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// POST: ProjectsController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int projectId)
		{
			//Deleting project
			_projectRepository.Delete(projectId);

			SetAlertTempData("Project deleted successfully.", "alert-success");

			return RedirectToAction("Index");
		}
		
		//Adds file to a project
		public ActionResult AddFile([FromForm] IFormFile file, int projectId)
		{
			try
			{
				if (_importService.AddFile(file, projectId))
				{
					SetAlertTempData($"File: {file.FileName} added to the project successfully.", "alert-success");
				}
				else
				{
					SetAlertTempData($"File: {file.FileName} failed to get added to the project.", "alert-danger");
				}
			}
			catch (Exception ex)
			{
				SetAlertTempData($"File: {file.FileName} failed to be added with error: {ex.Message}.", "alert-danger");
			}

			return RedirectToAction("Index");
		}

		//Deletes file from a project
		public ActionResult DeleteFile([FromForm] int fileId)
		{
			_fileRepository.Delete(fileId);

			SetAlertTempData("File deleted successfully.", "alert-success");

			return RedirectToAction("Index");
		}

		//Sets the lines of a project to a selected user
		[HttpPost]
		public ActionResult SetLinesToUser(ProjectsViewModel projectsVM)
		{
			if(ModelState.IsValid)
			{		
				if (projectsVM.SetLines.SourceLineIds != null && projectsVM.SetLines.SourceLineIds.Any() && !string.IsNullOrEmpty(projectsVM.SetLines.SelectedUserId))
				{
					// Split the string into a list of strings
					List<string> stringListOfIds = projectsVM.SetLines.SourceLineIds.Split(',').ToList();

					// Convert each string using LINQ with Select
					List<int> listofIds = stringListOfIds.Select(int.Parse).ToList();

					_userToSourceMappingRepository.AssignSourceLinesToUser(projectsVM.SetLines.SelectedUserId, listofIds);

					SetAlertTempData("Lines assigned to the user successfully.", "alert-success");
				}
				else
				{
					SetAlertTempData("Lines failed to be assigned to the user.", "alert-danger");
				}				
			}

			return RedirectToAction("Index");
		}

		//Exports the selected file and emails it to the user
		[HttpPost]
		public ActionResult ExportFile(ProjectsViewModel projectsVM)
		{
			if(ModelState.IsValid)
			{
				//Get currently signed user's e-mail
				Task<User> user = _userManager.GetUserAsync(User);

				string userEmail = user.Result.Email;

				if(!string.IsNullOrEmpty(userEmail) && _exportService.ExportAndSendFile(projectsVM.ExportFile.FileId, userEmail))
				{
					SetAlertTempData($"File exported and e-mailed to: {userEmail}", "alert-success");
				}
				else
				{
					SetAlertTempData("File export failed.", "alert-danger");
				}
			}

			return RedirectToAction("Index");
		}

		//Sets the TempData used to provide alerts to the user in the View
		private void SetAlertTempData(string message, string? alertClass)
		{
			TempData["AlertMessage"] = message;
			TempData["AlertClass"] = alertClass ?? "alert-info";
		}

		//Builds the ProjectsViewModel to be used in the Index view
		private ProjectsViewModel BuildProjectsViewModel()
		{
			ProjectsViewModel projectsVM = new ProjectsViewModel();
			List<ProjectDetailsViewModel> projectDetailsVMs = GetProjectDetails();

			if (projectDetailsVMs != null)
			{
				projectsVM.Details = projectDetailsVMs;
				projectsVM.SetLines.UserSelections = new SelectList(_userManager.Users.ToList(), "Id", "FirstName"); // Create SelectList from users fetched

				return projectsVM;
			}

			return null;
		}

		//Fetches the ProjectDetailsViewModels used in the ProjectsViewModel for the View
		private List<ProjectDetailsViewModel> GetProjectDetails()
		{
			List<ProjectDetailsViewModel> projectDetailsVMs = _projectRepository.GetAll()
				.Include(p => p.Files)
				.ThenInclude(f => f.SourceLines)
				.Include(p => p.Client)
				.Include(p => p.Manager)
				.Select(project => new ProjectDetailsViewModel
				{
					Project = project,
					Details = new ProjectDetails()
					{
						ClientName = project.Client.Name,
						ManagerName = project.Manager.FirstName,
						Files = project.Files.ToList(),
						FileCount = project.Files.Count()
					}
				})
				.ToList();

			if(projectDetailsVMs != null && projectDetailsVMs.Any())
			{
				return projectDetailsVMs;
			}

			return null;
		}
	}
}