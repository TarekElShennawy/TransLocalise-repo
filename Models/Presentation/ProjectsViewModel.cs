namespace Translator_Project_Management.Models.Presentation
{
	public class ProjectsViewModel
	{
		public List<ProjectDetailsViewModel> Details { get; set; }
		public SetLinesViewModel SetLines { get; set; } = new SetLinesViewModel();
		public ExportFileViewModel ExportFile { get; set; } = new ExportFileViewModel();
	}
}
