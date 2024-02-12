﻿using Translator_Project_Management.Models.Database;
using Translator_Project_Management.Models.Presentation;

namespace Translator_Project_Management.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        //Gets all projects
        IEnumerable<Project> GetAll();

        //Gets project by ID
        Project GetById(int projectId);

        //Inserts a new project record
        void Insert(Project project);

        //Updates a project record
        void Update(Project project);

        //Deletes a project record
        void Delete(int projectId);
    }
}