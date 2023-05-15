using ExpenseManagerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
    {
        switch (serviceResult.ResultCode)
        {
            case ResultCode.Ok:
                return Ok(serviceResult.Data);
            case ResultCode.NoContent:
                return NoContent();
            case ResultCode.NotFound:
                return NotFound(serviceResult.ErrorMessage);
            case ResultCode.Unauthorized:
                return Unauthorized(serviceResult.ErrorMessage);
            default:
                return StatusCode(500);
        }
    }
}