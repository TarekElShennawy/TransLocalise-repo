using MySqlConnector;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class LineRepository : ILineRepository
	{
		private readonly MySqlDatabase _db;

		private string c_InsertSourceLine = @"INSERT INTO source_text (line_id, file_id, text, lang_id) VALUES (@line_id, @file_id, @text, @lang_id)";
		private string c_InsertTranslationLine = @"INSERT INTO translations (line_id, file_id, text, lang_id) VALUES (@line_id, @file_id, @text, @lang_id)";

		public LineRepository(MySqlDatabase db)
        {
			_db = db;
        }

		public void Delete(int lineId)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Line> GetAllSourceLines()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Line> GetAllTranslations()
		{
			throw new NotImplementedException();
		}

		public Line GetById(int lineId)
		{
			throw new NotImplementedException();
		}

		public void Insert(Line line, MySqlTransaction transaction)
		{
			MySqlCommand cmd = this._db.Connection.CreateCommand();
			cmd.Transaction = transaction;

			if(line.IsTranslation)
			{
				cmd.CommandText = c_InsertTranslationLine;
			}
			else
			{
				cmd.CommandText = c_InsertSourceLine;
			}

			cmd.Parameters.AddWithValue("@line_id", line.LineId);
			cmd.Parameters.AddWithValue("@file_id", line.FileId);
			cmd.Parameters.AddWithValue("@text", line.Text);
			cmd.Parameters.AddWithValue("@lang_id", line.LangId);

			cmd.ExecuteNonQuery();
		}

		public void Update(Line line)
		{
			throw new NotImplementedException();
		}
	}
}
