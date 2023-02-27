namespace Contracts.HttpContext
{
    public enum ErrorCode
    {
        BadRequest = 40000,
        UsernameIsAlreadyInUse = 40901,
        InternalServerError = 50000,
        DbFailureError = 50001
    }

    public class ErrorResponse
    {
        public ErrorCode Code { get; init; }
        public string? Message { get; init; }
    }
}