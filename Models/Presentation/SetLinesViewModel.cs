using Microsoft.AspNetCore.Mvc.Rendering;

namespace Translator_Project_Management.Models.Presentation
{
	public class SetLinesViewModel
	{
		//This is a string in order to pass it from the view to the controller and parse it into a List<int> before passing it to the repository
		public string SourceLineIds { get; set; }
		public SelectList UserSelections { get; set; }
		public string SelectedUserId { get; set; }
	}
}
