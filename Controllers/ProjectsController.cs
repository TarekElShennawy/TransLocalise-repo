using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Translator_Project_Management.Importers.XML;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Models.Presentation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly XLIFFImporter _importer;

        private readonly IProjectRepository _projectRepository;
		private readonly IClientRepository _clientRepository;
		private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;

        public ProjectsController(XLIFFImporter importer, IProjectRepository projectRepository, IUserRepository userRepository, IClientRepository clientRepository, IFileRepository fileRepository)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _importer = importer;
        }

        // GET: ProjectsController
        public ActionResult Index() 
        {
            List<Project> allProjects = _projectRepository.GetAll().ToList();
            List<ProjectDetailsViewModel> projectVMs = new List<ProjectDetailsViewModel>();

            foreach(Project project in allProjects)
            {
                List<Models.Database.File> projectFiles = _fileRepository.GetByProjectId(project.Id).ToList();                

				ProjectDetailsViewModel projectVM = new ProjectDetailsViewModel()
                {
                    Project = project,
                    Details = new ProjectDetails()
                    {
						ClientName = _clientRepository.GetClientForProject(project.ClientId).Name,
						ManagerName = _userRepository.GetManagerForProject(project.ManagerId).FirstName,
                        Files = projectFiles,
                        FileCount = projectFiles.Count()
					}                    
                };

				projectVMs.Add(projectVM);
			}

            ViewData["projects"] = projectVMs;

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
            ViewData["clients"] = new SelectList(_clientRepository.GetAll().ToList(), "Id", "Name");
            ViewData["managers"] = new SelectList(_userRepository.GetAllManagers().ToList(), "Id", "FirstName");

            return View();
        }

        // POST: ProjectsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Project project)
        {
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
            if(_importer.IsValidImporter(file))
            {
                LocFile parsedFile = _importer.ParseFile(file);
				_importer.UploadToDb(projectId, parsedFile);
			}

            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile([FromForm] int fileId)
        {
            _fileRepository.Delete(fileId);

            return RedirectToAction("Index");
        }
    }
}