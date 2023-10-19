using Microsoft.AspNetCore.Mvc;

namespace Solvro_Backend.Models
{
    public class ApiResponse : IActionResult
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Response { get; set; }

        public ApiResponse(int statusCode, string? message = null, object? response = null)
        {
            StatusCode = statusCode;
            Message = message;
            Response = response;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var tmp = new ObjectResult(this) { StatusCode = StatusCode };
            await tmp.ExecuteResultAsync(context);
        }

        public static ApiResponse NotFound(object? response) 
        {
            return new ApiResponse(404, "Not Found", response);
        }

        public static ApiResponse Ok(object? response)
        {
            return new ApiResponse(200, "Ok", response);
        }

        public static ApiResponse Created(object response)
        {
            return new ApiResponse(201, "Created", response);
        }

        public static ApiResponse ServerError(string message, object response)
        {
            return new ApiResponse(500, message, response);
        }

        public static ApiResponse MultiStatus(List<ApiResponse> responses)
        {
            return new ApiResponse(207, "Multi Status", responses);
        }
    }
}
