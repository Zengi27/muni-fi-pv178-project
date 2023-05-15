using AutoMapper;
using DbModel;
using ExpenseManagerAPI.DTOs.Expense;
using ExpenseManagerAPI.Services.User;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManagerAPI.Services.Expense;

public class ExpenseService
{
    private readonly UserManager<DbModel.Entities.User> _userManager;
    private readonly UserService _userService;
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseService(UserManager<DbModel.Entities.User> userManager, UserService userService ,ExpenseManagerDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResult<ExpenseDto>> AddExpenseForUser(string username, AddExpenseDto expenseDto)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return ServiceResult<ExpenseDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var expense = _mapper.Map<DbModel.Entities.Expense>(expenseDto);
        expense.UserId = user.Id;
        expense.User = user;
        
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        return ServiceResult<ExpenseDto>.Success(_mapper.Map<ExpenseDto>(expense));
    }

    public async Task<ServiceResult<ExpenseDto>> GetUserExpenseById(string username, int expenseId)
    {
        var user = await _userService.GetCurrentUser(username);
        
        if (user == null)
        {
            return ServiceResult<ExpenseDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var expense = user.Expenses.FirstOrDefault(e => e.Id == expenseId);
        
        if (expense == null)
        {
            return ServiceResult<ExpenseDto>.Failure($"Expense with id: {expenseId} not found.", ResultCode.NotFound);
        }

        var expenseDto = _mapper.Map<ExpenseDto>(expense);

        return ServiceResult<ExpenseDto>.Success(expenseDto);
    }

    public async Task<ServiceResult<IEnumerable<ExpenseDto>>> GetUserExpenses(string username)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IEnumerable<ExpenseDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(user.Expenses);

        return ServiceResult<IEnumerable<ExpenseDto>>.Success(expenseDtos);
    }

    public async Task<ServiceResult<bool>> DeleteExpense(string username, int expenseId)
    {
        var user = await _userService.GetCurrentUser(username);
        
        if (user == null)
        {
            return ServiceResult<bool>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var expense = user.Expenses.FirstOrDefault(e => e.Id == expenseId);
        
        if (expense == null)
        {
            return ServiceResult<bool>.Failure("Expense not found.", ResultCode.NotFound);
        }
        
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Success(true, ResultCode.NoContent);
    }

    public async Task<ServiceResult<ExpenseDto>> UpdateExpense(string username, ExpenseDto expenseDto)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<ExpenseDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        } 
        
        var expense = user.Expenses.FirstOrDefault(e => e.Id == expenseDto.Id);
        
        if (expense == null)
        {
            return ServiceResult<ExpenseDto>.Failure("Expense not found.", ResultCode.NotFound);
        }

        expense.Amount = expenseDto.Amount;
        expense.Date = expenseDto.Date;
        expense.Description = expenseDto.Description;

        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
        
        return ServiceResult<ExpenseDto>.Success(_mapper.Map<ExpenseDto>(expense));
    }

    public async Task<ServiceResult<IEnumerable<ExpenseDto>>> FilterByDate(string username, DateTime startDate, DateTime endDate)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IEnumerable<ExpenseDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        } 
        
        var expenses = user.Expenses
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .Select(e => _mapper.Map<ExpenseDto>(e));
        
        return ServiceResult<IEnumerable<ExpenseDto>>.Success(expenses);
    }
    
    public async Task<ServiceResult<IEnumerable<ExpenseDto>>> GetMostExpensiveExpenses(string username, int count)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IEnumerable<ExpenseDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var expenses = user.Expenses
            .OrderByDescending(e => e.Amount)
            .Take(count)
            .Select(e => _mapper.Map<ExpenseDto>(e));

        return ServiceResult<IEnumerable<ExpenseDto>>.Success(expenses);
    }
    
    public async Task<ServiceResult<IEnumerable<ExpenseDto>>> GetCheapestExpenses(string username, int count)
    {
        var user = await _userService.GetCurrentUser(username);
    
        if (user == null)
        {
            return ServiceResult<IEnumerable<ExpenseDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var expenses = user.Expenses
            .OrderBy(e => e.Amount)
            .Take(count)
            .Select(e => _mapper.Map<ExpenseDto>(e));
    
        return ServiceResult<IEnumerable<ExpenseDto>>.Success(expenses);
    }
    
    public async Task<ServiceResult<ExpenseReportDto>> GetMonthlyExpenseReport(string username, int year, int month)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<ExpenseReportDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var expenses = user.Expenses
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .ToList();

        var monthlyReport = new ExpenseReportDto
        {
            Year = year,
            Month = month,
            TotalAmount = expenses.Sum(e => e.Amount),
            Expenses = expenses.Select(e => _mapper.Map<ExpenseDto>(e))
        };

        return ServiceResult<ExpenseReportDto>.Success(monthlyReport);
    }

    public async Task<ServiceResult<ExpenseReportDto>> GetYearlyExpenseReport(string username, int year)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<ExpenseReportDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var expenses = user.Expenses
            .Where(e => e.Date.Year == year)
            .ToList();

        var monthlyReport = new ExpenseReportDto
        {
            Year = year,
            TotalAmount = expenses.Sum(e => e.Amount),
            Expenses = expenses.Select(e => _mapper.Map<ExpenseDto>(e))
        };

        return ServiceResult<ExpenseReportDto>.Success(monthlyReport);
    }
    
    public async Task<ServiceResult<decimal>> GetTotalExpense(string username)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<decimal>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var totalExpense = user.Expenses
            .Sum(e => e.Amount);

        return ServiceResult<decimal>.Success(totalExpense);
    }
    
}