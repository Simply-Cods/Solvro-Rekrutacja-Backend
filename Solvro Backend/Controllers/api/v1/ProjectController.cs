using Microsoft.AspNetCore.Mvc;
using Solvro_Backend.DTOs;
using Solvro_Backend.Models.Views;
using Solvro_Backend.Repositories;
using Solvro_Backend.Models.Database;

namespace Solvro_Backend.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectRepository _projectRepository;
        private IUserRepository _userRepository;
        private IProjectMemberMappingRepository _projectMemberMappingRepository;

        public ProjectController(IServiceProvider serviceProvider) 
        {
            _projectRepository = serviceProvider.GetRequiredService<IProjectRepository>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _projectMemberMappingRepository = serviceProvider.GetRequiredService<IProjectMemberMappingRepository>();
        }

        [HttpGet("project")]
        public IActionResult GetAllProjects()
        {
            var dbResult = _projectRepository.GetAllProjects();
            List<ProjectView> views = new();
            if(dbResult.Count > 0)
            {
                views = dbResult.Select(p => new ProjectView(p)).ToList();
            }
            return Ok(views);
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            User? owner = _userRepository.GetUser(dto.OwnerId);
            if(owner == null)
                return NotFound($"User with the ID of {dto.OwnerId} does not exist and cannot be assigned as owner");
            

            List<User> members = new();
            foreach(long userId in dto.MemberIds)
            {
                User? member = _userRepository.GetUser(userId);
                if (member == null)
                    return NotFound($"User with the ID of {userId} does not exist and cannot be assigned as member");
                    
                members.Add(member);
            }

            Project project = new()
            {
                Owner = owner,
                Name = dto.Name
            };
            project = await _projectRepository.CreateProject(project);

            List<ProjectMemberMapping> mappings = new();

            foreach(User user in members)
            {
                ProjectMemberMapping mapping = new()
                {
                    Project = project,
                    User = user
                };
                mappings.Add(mapping);
            }
            try
            {
                await _projectMemberMappingRepository.BulkCreateMapping(mappings);
            } catch (Exception ex)
            {
                return StatusCode(207, new object[] {
                    StatusCode(201, new ProjectView(project)),
                    StatusCode(500, ex.Message)
                });
            }
            
            return StatusCode(201, new ProjectView(project));
        }

        [HttpGet("project/{projectId}")]
        public IActionResult GetProject([FromRoute] long projectId)
        {
            Project? project = _projectRepository.GetProject(projectId);
            if (project == null) return NotFound($"Project with the ID of {projectId} does not exist");
            return Ok(new ProjectFullView(project));
        }
    }
}
