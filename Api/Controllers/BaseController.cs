using Contracts.HttpContext;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<IActionResult> SafeExecute(Func<Task<IActionResult>> action, CancellationToken cancellationToken)
        {
            try
            {
                return await action();
            }
            catch (Exception)
            {
                ErrorResponse errorResponse = new()
                {
                    Code = ErrorCode.InternalServerError,
                    Message = "Ungandled error"
                };

                return ToActionResult(errorResponse);
            }
        }

        protected IActionResult ToActionResult(ErrorResponse errorResponse)
        {
            return StatusCode((int)errorResponse.Code / 100, errorResponse);
        }
    }
}