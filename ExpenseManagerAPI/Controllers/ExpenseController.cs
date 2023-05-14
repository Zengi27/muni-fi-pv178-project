using ExpenseManagerAPI.DTOs.Expense;
using ExpenseManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers;

[ApiController]
[Authorize]
[Route("expenses")]
public class ExpenseController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpenseController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense(AddExpenseDto addExpenseDto)
    {
        var username = User.Identity.Name;
        var serviceResult = await _expenseService.AddExpenseForUser(username, addExpenseDto);

        switch (serviceResult.ResultCode)
        {
            case ResultCode.Ok:
                return CreatedAtAction(nameof(GetExpenseById), new { id = serviceResult.Data.Id }, serviceResult.Data);
            case ResultCode.Unauthorized:
                return Unauthorized(serviceResult.ErrorMessage);
            default:
                return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(int id)
    {
        var username = User.Identity.Name;
        var serviceResult = await _expenseService.GetUserExpenseById(username, id);
        
        return HandleServiceResult(serviceResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var username = User.Identity.Name;
        var serviceResult = await _expenseService.GetUserExpenses(username);

        return HandleServiceResult(serviceResult);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var username = User.Identity.Name;
        var serviceResult = await _expenseService.DeleteExpense(username, id);

        return HandleServiceResult(serviceResult);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateExpense(ExpenseDto expenseDto)
    {
        var username = User.Identity.Name;
        var serviceResult = await _expenseService.UpdateExpense(username, expenseDto);

        return HandleServiceResult(serviceResult);
    }
    
    private IActionResult HandleServiceResult<T>(ServiceResult<T> serviceResult)
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