using Coders_Back.Domain.DataAbstractions;
using Coders_Back.Domain.DTOs.Input;
using Coders_Back.Domain.DTOs.Output;
using Coders_Back.Domain.Entities;
using Coders_Back.Domain.Interfaces;

namespace Coders_Back.Domain.Services;

public class ProjectService : IProjectService
{
    private readonly IRepository<Project> _projects;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IRepository<Project> projects, IUnitOfWork unitOfWork)
    {
        _projects = projects;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProjectOutput>> GetAll()
    {
        var projects = await _projects.GetAll();
        return projects.Select(p => new ProjectOutput(p)).ToList();
    }

    public async Task<ProjectOutput?> GetById(Guid projectId)
    {
        var project = await _projects.GetById(projectId);
        return project is null ? null : new ProjectOutput(project);
    }

    public async Task<ProjectOutput> Create(ProjectInput projectInput)
    {
        var project = new Project{
            Name = projectInput.Name,
            Description = projectInput.Description,
            GithubUrl = projectInput.GithubUrl,
            DiscordUrl = projectInput.DiscordUrl,
            //TODO: OwnerId = pega_id_user_logado
        };

        await _projects.Insert(project);
        
        return new ProjectOutput(project);
    }

    public async Task<bool> Update(Guid projectId)
    {
        var project = await _projects.GetById(projectId);
        if (project is null) return false;
        _projects.Update(project);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Guid projectId)
    {
        var project = await _projects.GetById(projectId);
        if (project is null) return false;
        await _projects.Delete(projectId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}