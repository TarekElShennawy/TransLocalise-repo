using Microsoft.EntityFrameworkCore;
using Translator_Project_Management.Database;
using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Repositories.Interfaces;

namespace Translator_Project_Management.Repositories
{
	public class ProjectRepository : IProjectRepository
	{
		private readonly LocDbContext _dbContext;

        public ProjectRepository(LocDbContext dbContext)
        {
            _dbContext = dbContext;
        }

		public Project GetById(int projectId)
		{
			return _dbContext.Projects.Find(projectId);
		}

		public IQueryable<Project> GetAll()
		{
			return _dbContext.Projects;
							
		}

        public void Insert(Project project)
		{
			_dbContext.Projects.Add(project);
			_dbContext.SaveChanges();
		}

		public void Update(Project project)
		{
			_dbContext.Projects.Update(project);
			_dbContext.SaveChanges();
		}

		public void Delete(int projectId)
		{
			var project = _dbContext.Projects.Find(projectId);
			if (project != null)
			{
				_dbContext.Projects.Remove(project);
				_dbContext.SaveChanges();
			}
		}
    }
}
