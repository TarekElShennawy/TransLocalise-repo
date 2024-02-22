namespace Translator_Project_Management.Models.Presentation
{
    public class TaskDetailsViewModel
    {
        //The name of the task, currently set as the file's name.
        public string Name { get; set; }

        //A list of UserTask which comprises of the source line, the translation set by user and other properties.
        public List<UserTask> Tasks { get; set; }
    }
}
