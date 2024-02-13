using Newtonsoft.Json;

namespace Translator_Project_Management.Models.Localisation
{
	public class LocLine
	{
		[JsonProperty("id")]
		public string LineId { get; set; }
		[JsonProperty("source")]
		public string SourceText { get; set; }
		[JsonProperty("target")]
		public string TargetText { get; set; }
		[JsonProperty("source_language")]
		public string SourceLang { get; set; }
		[JsonProperty("target_language")]
		public string TargetLang { get; set; }
	}
}
