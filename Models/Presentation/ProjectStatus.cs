namespace Translator_Project_Management.Models.Presentation
{
    public class ProjectStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public static List<ProjectStatus> GetProjectStatuses()
        {
            return new List<ProjectStatus>
            {
                new ProjectStatus { Id = 1, Status = "Not Started"},
                new ProjectStatus { Id = 2, Status = "In Progress"},
                new ProjectStatus { Id = 3, Status = "Completed"},
                new ProjectStatus { Id = 4, Status = "On Hold"}
            };
        }
    }
}
