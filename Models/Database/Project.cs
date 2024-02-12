namespace Translator_Project_Management.Models.Database
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }  = String.Empty;
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly DueDate { get; set; }
        public string Status { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        public ICollection<Models.Database.File> Files { get; set; } = new List<Models.Database.File>();
    }
}
