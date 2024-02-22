using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Translator_Project_Management.Importers;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Models.Presentation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Controllers
{
    [Authorize(Roles = "Manager")] //Projects is only accessible on user log-in for managers
	public class ProjectsController : Controller
    {
        public IEnumerable<IImporter> _importers;

        private readonly IProjectRepository _projectRepository;
		private readonly IClientRepository _clientRepository;
		private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
		private readonly ILanguageRepository _languageRepository;
        private readonly ILineRepository<SourceLine> _sourceLineRepository;
        private readonly IUserSourceLineMappingRepository _userToSourceMappingRepository;

        private readonly UserManager<User> _userManager;

        public ProjectsController(UserManager<User> userManager, IEnumerable<IImporter> importers, IProjectRepository projectRepository, IUserRepository userRepository,
            IClientRepository clientRepository, IFileRepository fileRepository, ILanguageRepository languageRepository, ILineRepository<SourceLine> sourceLineRepository,
			IUserSourceLineMappingRepository userToSourceMappingRepository)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _languageRepository = languageRepository;
            _sourceLineRepository = sourceLineRepository;
            _userToSourceMappingRepository = userToSourceMappingRepository;

			_importers = importers;

            _userManager = userManager;
        }

        // GET: ProjectsController
        public ActionResult Index() 
        {
            List<ProjectDetailsViewModel> projectVMs = new List<ProjectDetailsViewModel>();

            List<Project> allProjects = _projectRepository.GetAll().ToList();                

            foreach (Project project in allProjects)
            {
                List<Models.Database.File> projectFiles = _fileRepository.GetByProjectId(project.Id).ToList();

                ProjectDetailsViewModel projectVM = new ProjectDetailsViewModel()
                {
                    Project = project,
                    Details = new ProjectDetails()
                    {
                        ClientName = _clientRepository.GetById(project.ClientId).Name,
                        ManagerName = _userRepository.GetManagerById(project.ManagerId).FirstName,
                        Files = projectFiles,
                        FileCount = projectFiles.Count()
                    }
                };

                projectVMs.Add(projectVM);
            }           

            ViewData["projects"] = projectVMs;

			// Get all users
			List<User> allUsers = _userRepository.GetAll().ToList();

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
            ViewData["projectStatuses"] = new SelectList(GetProjectStatuses(), "Status", "Status");
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

        private static List<ProjectStatus> GetProjectStatuses()
        {
            return new List<ProjectStatus>
            {
                new ProjectStatus { Id = 1, Status = "Not Started"},
                new ProjectStatus { Id = 2, Status = "In Progress"},
                new ProjectStatus { Id = 3, Status = "Completed"},
                new ProjectStatus { Id = 4, Status = "On Hold"}
            };
        }

        public ActionResult AddFile([FromForm] IFormFile file, int projectId)
        {
            IImporter selectedImporter = null;

            foreach(IImporter importer in _importers)
            {
                if(importer.IsValidImporter(file))
                {
                    selectedImporter = importer;
                    break;
                }
            }

            if(selectedImporter != null) {

                try
                {
					using (selectedImporter)
					{
					    LocFile parsedFile = selectedImporter.ParseFile(file);
					    selectedImporter.UploadToDb(projectId, parsedFile);
					}
				}
                catch
                {
                
                }
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
            List<int> sourceLines = _sourceLineRepository.GetForFile(fileId).Select(sl => sl.Id).ToList();

            if(!string.IsNullOrEmpty(selectedUserId) && sourceLines != null && sourceLines.Any())
            { 
                _userToSourceMappingRepository.AssignSourceLinesToUser(selectedUserId, sourceLines);
			}

			return RedirectToAction("Index");
        }
    }
}