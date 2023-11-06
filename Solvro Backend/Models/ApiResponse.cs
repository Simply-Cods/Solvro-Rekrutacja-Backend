using Microsoft.AspNetCore.Mvc;

namespace Solvro_Backend.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// Response data
        /// </summary>
        public T Data { get; set; }

        public ApiResponse(int statusCode, T data, string? massage = null) : base(statusCode, massage) 
        { 
            Data = data;
        }

        public static ApiResponse<T> NotFound(T data)
        {
            return new ApiResponse<T>(404, data, "Not Found");
        }

        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>(200, data, "OK");
        }

        public static ApiResponse<T> Created(T data)
        {
            return new ApiResponse<T>(201, data, "Created");
        }

        public static ApiResponse<T> BadRequest(T data)
        {
            return new ApiResponse<T>(400, data, "Bad Request");
        }

        public static ApiResponse<T> ServerError(T data)
        {
            return new ApiResponse<T>(500, data, "Internal Server Error");
        }
        
    }

    public class ApiResponse : IActionResult
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Usually the http status code name
        /// </summary>
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var tmp = new ObjectResult(this) { StatusCode = StatusCode };
            await tmp.ExecuteResultAsync(context);
        }

        public static ApiResponse NotFound() 
        {
            return new ApiResponse(404, "Not Found");
        }

        public static ApiResponse Ok()
        {
            return new ApiResponse(200, "Ok");
        }

        public static ApiResponse<IEnumerable<ApiResponse>> MultiStatus(IEnumerable<ApiResponse> data)
        {
            return new ApiResponse<IEnumerable<ApiResponse>>(207, data, "Multi Status");
        }
    }
}
