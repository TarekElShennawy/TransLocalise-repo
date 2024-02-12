namespace Translator_Project_Management.Models.Database
{
    public class File
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }		
	}
}
