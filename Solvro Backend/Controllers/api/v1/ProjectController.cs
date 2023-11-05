using Microsoft.AspNetCore.Mvc;
using Solvro_Backend.DTOs;
using Solvro_Backend.Models.Views;
using Solvro_Backend.Models;
using Solvro_Backend.Repositories;
using Solvro_Backend.Models.Database;

using STask = Solvro_Backend.Models.Database.Task;
using Solvro_Backend.Enums;

namespace Solvro_Backend.Controllers
{
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IProjectRepository _projectRepository;
        private IUserRepository _userRepository;
        private IProjectMemberMappingRepository _projectMemberMappingRepository;
        private ITaskRepository _taskRepository;

        public ProjectController(IServiceProvider serviceProvider) 
        {
            _projectRepository = serviceProvider.GetRequiredService<IProjectRepository>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _projectMemberMappingRepository = serviceProvider.GetRequiredService<IProjectMemberMappingRepository>();
            _taskRepository = serviceProvider.GetRequiredService<ITaskRepository>();
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
            return ApiResponse.Ok(views);
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            User? owner = _userRepository.GetUser(dto.OwnerId!.Value);
            if (owner == null)
                return ApiResponse.NotFound($"User with the ID of {dto.OwnerId} does not exist and cannot be assigned as owner");            

            Project project = new()
            {
                Owner = owner,
                Name = dto.Name
            };
            project = await _projectRepository.CreateProject(project);

            ApiResponse okResult = ApiResponse.Created(new ProjectView(project));

            List<ProjectMemberMapping> mappings = new();
            List<User> members = new();
            List<ApiResponse> errors = new()
            {
                okResult // we add the ok result here to send it along with the errors
            };

            foreach (long userId in dto.MemberIds)
            {
                User? member = _userRepository.GetUser(userId);
                if (member == null)
                {
                    errors.Add(ApiResponse.NotFound($"User with the ID of {userId} does not exist and cannot be assigned as member"));
                    continue;
                }
                members.Add(member);
            }

            foreach (User user in members)
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
                errors.Add(ApiResponse.ServerError("Failed to create member mappings. Exception in response body", ex));
            }

            if(errors.Count > 1)
                return ApiResponse.MultiStatus(errors);
            
            
            return okResult;
        }

        [HttpGet("project/{projectId}")]
        public IActionResult GetProject([FromRoute] long projectId)
        {
            Project? project = _projectRepository.GetProject(projectId);
            if (project == null) 
                return ApiResponse.NotFound($"Project with the ID of {projectId} does not exist");
            return ApiResponse.Ok(new ProjectFullView(project));
        }

        [HttpGet("project/user")]
        public IActionResult GetProjectsForUser([FromQuery] long userId)
        {
            return ApiResponse.Ok(_projectRepository.GetProjectsForUser(userId).Select(p => new ProjectFullView(p)));
        }

        [HttpPost("project/{projectId}/task")]
        public async Task<IActionResult> CreateTask([FromRoute]long projectId, [FromBody] CreateTaskDto dto)
        {
            Project? project = _projectRepository.GetProject(projectId);
            if (project == null)
                return ApiResponse.NotFound($"Project with the ID of {projectId} does not exist");

            User? creator = _userRepository.GetUser(dto.CreatorId!.Value);
            if (creator == null)
                return ApiResponse.NotFound($"User with the ID of {dto.CreatorId} does not exist and cannot be assigned as owner");

            STask task = new()
            {
                Project = project,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                Estimation = dto.Estimation!.Value,
                Specialization = dto.Specialization!.Value,
                State = TaskState.Open,
                Creator = creator
            };

            ApiResponse potentialNotFound = null;
            if(dto.AssignedUserId != null)
            {
                User? assignedUser = _userRepository.GetUser(dto.AssignedUserId.Value);
                if (assignedUser == null)
                {
                    potentialNotFound = ApiResponse.NotFound($"User with the ID of {dto.AssignedUserId} does not exist and cannot be assigned");
                }
                else
                {
                    task.AssignedUser = assignedUser;
                    task.State = TaskState.ToDo;
                }
            }

            task = await _taskRepository.CreateTask(task);

            var successResponse = ApiResponse.Created(new TaskView(task));

            if(potentialNotFound != null)
            {
                return ApiResponse.MultiStatus(new()
                {
                    successResponse,
                    potentialNotFound
                });
            }

            return successResponse;
        }

        [HttpPut("project/{projectId}/task/{taskId}")]
        public async Task<IActionResult> UpdateTaskStatus([FromRoute] long projectId, [FromRoute] long taskId, [FromBody] UpdateTaskStatusDto dto)
        {
            (STask? task, bool projectExists) = _taskRepository.SelectTask(projectId, taskId);

            if(task == null)
            {
                return projectExists
                    ? ApiResponse.NotFound($"Task with the ID of {taskId} does not exist.")
                    : ApiResponse.NotFound($"Project with the ID of {projectId} does not exist.");
            }

            task.State = dto.State!.Value;

            if(task.State == TaskState.Done)
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                task.CompletedAt = null;
            }

            task = await _taskRepository.UpdateTask(task);

            return ApiResponse.Ok(new TaskView(task));
        }

        private static Dictionary<Guid, List<Assignment>> s_assignmentCache = new();

        [HttpPost("project/{projectId}/assignment")]
        public IActionResult GetAssignment([FromRoute] long projectId)
        {
            var propositions = _projectRepository.GetAssignment(projectId);
            if (propositions == null)
                return ApiResponse.NotFound($"Project with the ID of {projectId} doesn't exit.");

            var view = new AssignmentView(propositions);
            s_assignmentCache.Add(view.Id, propositions);

            return ApiResponse.Created(view);
        }

        [HttpPut("project/{projectId}/assignment/{assignmentId}")]
        public async Task<IActionResult> ActAssignemnt([FromRoute] long projectId, [FromRoute] Guid assignmentId, [FromQuery] bool? delete)
        {
            if (delete.HasValue && delete.Value)
            {
                s_assignmentCache.Remove(assignmentId);
                return ApiResponse.Ok(null);
            }

            var project = _projectRepository.GetProject(projectId);
            if (project == null)
                return ApiResponse.NotFound($"Project with the ID of {projectId} doesn't exist");
            if (!s_assignmentCache.ContainsKey(assignmentId))
                return ApiResponse.NotFound($"Assignment with the ID of {assignmentId} doesn't exist");

            bool success = await _projectRepository.ApplyAssignment(projectId, s_assignmentCache[assignmentId]);

            if (success)
                return ApiResponse.Ok(null);
            return ApiResponse.BadRequest($"Could not complete pairings, is this the project they were generated for?");
        }
    }
}
