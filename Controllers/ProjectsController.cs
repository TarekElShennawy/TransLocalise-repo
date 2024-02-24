using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Database;
using Translator_Project_Management.Importers;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Presentation;
using Translator_Project_Management.Repositories.Interfaces;
using Translator_Project_Management.Services;
using File = Translator_Project_Management.Models.Database.File;

namespace Translator_Project_Management.Controllers
{
    [Authorize(Roles = "Manager")] //Projects is only accessible on user log-in for managers
	public class ProjectsController : Controller
	{
		public IEnumerable<IImporter> _importers;

		private readonly IProjectRepository _projectRepository;
		private readonly IClientRepository _clientRepository;
		private readonly IFileRepository _fileRepository;
		private readonly IUserSourceLineMappingRepository _userToSourceMappingRepository;
		private readonly LocDbContext _dbContext;

		private readonly UserManager<User> _userManager;

		public ProjectsController(UserManager<User> userManager, IEnumerable<IImporter> importers, IProjectRepository projectRepository, IClientRepository clientRepository,
			IFileRepository fileRepository, IUserSourceLineMappingRepository userToSourceMappingRepository, LocDbContext dbContext)
		{
			_projectRepository = projectRepository;
			_clientRepository = clientRepository;
			_fileRepository = fileRepository;
			_userToSourceMappingRepository = userToSourceMappingRepository;

			_importers = importers;

			_userManager = userManager;
			_dbContext = dbContext;
		}

		// GET: ProjectsController
		public ActionResult Index()
		{
			List<ProjectDetailsViewModel> projectVMs = _projectRepository.GetAll()
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

			ViewData["projects"] = projectVMs;

			// Get all users
			List<User> allUsers = _userManager.Users.ToList();

			// Create SelectList from users
			ViewData["users"] = new SelectList(allUsers, "Id", "FirstName");

			return View();
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
			return RedirectToAction("Index");
		}
		

		public ActionResult AddFile([FromForm] IFormFile file, int projectId)
		{
			var fileImporterService = new FileImportService(_importers);

			try
			{
				if (fileImporterService.AddFile(file, projectId))
				{
					//Redirect back to Index on success
					return RedirectToAction("Index");
				}
				else
				{
					//TODO: Redirect to failure page, currently redirects back to Index
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				//Log exception
			}

			return RedirectToAction("Index");
		}

		public ActionResult DeleteFile([FromForm] int fileId)
		{
			_fileRepository.Delete(fileId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult SetLinesToUser([FromForm] int fileId, string selectedUserId)
		{
			File file = _fileRepository.GetById(fileId)
									   .Include(f => f.SourceLines)
									   .FirstOrDefault();

			if(file != null)
			{
				List<int> sourceLineIds = file.SourceLines.Select(sl => sl.Id).ToList();

				if (!string.IsNullOrEmpty(selectedUserId) && sourceLineIds != null && sourceLineIds.Any())
				{
					_userToSourceMappingRepository.AssignSourceLinesToUser(selectedUserId, sourceLineIds);
				}
			}			

			return RedirectToAction("Index");
		}
	}
}