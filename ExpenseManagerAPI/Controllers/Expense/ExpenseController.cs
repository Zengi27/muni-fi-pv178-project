using ExpenseManagerAPI.DTOs.Expense;
using ExpenseManagerAPI.Services;
using ExpenseManagerAPI.Services.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagerAPI.Controllers.Expense;

[ApiController]
[Authorize]
[Route("expenses")]
public class ExpenseController : BaseController
{
    private readonly ExpenseService _expenseService;

    public ExpenseController(ExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddExpense(AddExpenseDto addExpenseDto)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.AddExpenseForUser(username, addExpenseDto);

        switch (serviceResult.ResultCode)
        {
            case ResultCode.Ok:
                return CreatedAtAction(nameof(GetExpenseById), new { id = serviceResult.Data!.Id }, serviceResult.Data);
            case ResultCode.Unauthorized:
                return Unauthorized(serviceResult.ErrorMessage);
            default:
                return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpenseById(int id)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetUserExpenseById(username, id);
        
        return HandleServiceResult(serviceResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses()
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetUserExpenses(username);

        return HandleServiceResult(serviceResult);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.DeleteExpense(username, id);

        return HandleServiceResult(serviceResult);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateExpense(ExpenseDto expenseDto)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.UpdateExpense(username, expenseDto);

        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("filter-by-date")]
    public async Task<IActionResult> GetExpensesByDateRange(DateTime startDate, DateTime? endDate = null)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.FilterByDate(username, startDate, endDate ?? DateTime.UtcNow);
    
        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("most-expensive/{count}")]
    public async Task<IActionResult> GetMostExpensiveExpenses(int count)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetMostExpensiveExpenses(username, count);

        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("cheapest/{count}")]
    public async Task<IActionResult> GetCheapestExpenses(int count)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetCheapestExpenses(username, count);

        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("monthly-report/{year}/{month}")]
    public async Task<IActionResult> GetMonthlyExpenseReport(int year, int month)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetMonthlyExpenseReport(username, year, month);

        return HandleServiceResult(serviceResult);
    }
    
    [HttpGet("yearly-report/{year}")]
    public async Task<IActionResult> GetYearlyExpenseReport(int year)
    {
        var username = GetUsername();

        if (username == null)
        {
            return BadRequest();
        }
        
        var serviceResult = await _expenseService.GetYearlyExpenseReport(username, year);

        return HandleServiceResult(serviceResult);
    }
}