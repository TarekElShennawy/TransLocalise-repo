using Translator_Project_Management.Models.Database;

namespace Translator_Project_Management.Models.Presentation
{
    public class ProjectDetailsViewModel
    {
        public Project Project { get; set; }
        public ProjectDetails Details { get; set; }
    }

    public class ProjectDetails
    {
        public string ClientName { get; set; }
        public string ManagerName { get; set; }

        public List<Database.File> Files { get; set; }
        public int FileCount { get; set; } = 0;
    }
}
