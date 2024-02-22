using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Presentation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Controllers
{
    [Authorize(Roles = "User")] //Tasks is only accessible on user log-in for translators
    public class TasksController : Controller
    {
		private readonly IUserSourceLineMappingRepository _userToSourceMappingRepository;
		private readonly UserManager<User> _userManager;
        private readonly ILineRepository<TransLine> _translationRepository;

		public TasksController(IUserSourceLineMappingRepository userToSourceMappingRepository, ILineRepository<TransLine> translationRepository, UserManager<User> userManager)
        {
            _translationRepository = translationRepository;
            _userToSourceMappingRepository = userToSourceMappingRepository;
			_userManager = userManager;
        }

        public IActionResult Index()
        {
			string userId = _userManager.GetUserId(this.User);
			List<SourceLine> sourceLinesByFile = _userToSourceMappingRepository
                                                                    .GetUserSourceLines(userId)
                                                                    .ToList();
            
            List<UserTask> allUserTasks = new List<UserTask>();

            foreach(SourceLine line in sourceLinesByFile)
            {
                UserTask taskVM = new UserTask()
                {
                    Source = line.Text,
                    FileId = line.FileId,
                    SourceId = line.Id

                };

                allUserTasks.Add(taskVM);
			}

            ViewData["AssignedTasks"] = allUserTasks;
			return View();
        }

        public IActionResult SetLineTranslation([FromForm] UserTask task)
        {
            TransLine translation = new TransLine()
            {
                SourceId = task.SourceId,
                Text = task.Translation,
                FileId = task.FileId,
                LangId = 1 //TODO: SET LANG ID OF THE USER TASKS BY ALLOWING MANAGER TO PICK A LANGUAGE WHEN SETTING A TASK
            };

            _translationRepository.Insert(translation);

            return RedirectToAction("Index");
        }
    }
}
