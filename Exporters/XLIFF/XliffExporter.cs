using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Localisation;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Exporters.XLIFF
{
	public class XliffExporter : ExporterBaseClass
	{
		public XliffExporter(IFileRepository fileRepository)
			: base(fileRepository)
		{
			fileType = "xliff";
		}

		public override string WriteFile(LocFile file)
		{
			XDocument xliffDoc = new XDocument();

			XNamespace xsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";

			XElement rootElement = new XElement(
					"xliff",
					new XAttribute("version", "1.2"),
					new XAttribute(XNamespace.Xmlns + "xsi", xsiNamespace),
					new XAttribute(xsiNamespace + "schemaLocation",
					"urn:oasis:names:tc:xliff:document:1.2 xliff-core-1.2.xsd")
					);

			//Assumes that all lines have the same source-target langs (considering it is set per file in XLIFF)
			//Therefore fetches the lang codes from the first source/translation lines from the file's lines
			XElement fileElement = new XElement(
				"file",
				new XAttribute("original", $"{file.Name}.xliff"),
				new XAttribute("source-language", $"{file.Lines.First().SourceLang}"),
				new XAttribute("target-language", $"{file.Lines.First().TargetLang}")
				);

			XElement bodyElement = new XElement("body");
			foreach (var line in file.Lines)
			{
				XElement unitElement = new XElement(
					"unit",
					new XAttribute("id", $"{line.LineId}")
					);
				unitElement.Add(new XElement("source", line.SourceText));
				unitElement.Add(new XElement("target", line.TargetText));
				bodyElement.Add(unitElement);
			}

			fileElement.Add(bodyElement);
			rootElement.Add(fileElement);
			xliffDoc.Add(rootElement);

			return xliffDoc.ToString();
		}
	}
}
