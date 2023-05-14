using AutoMapper;
using DbModel;
using DbModel.Entities;
using ExpenseManagerAPI.DTOs.Expense;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManagerAPI.Services;

public class ExpenseService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthService _authService;
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseService(UserManager<User> userManager, AuthService authService ,ExpenseManagerDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _authService = authService;
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
        
        var expense = _mapper.Map<Expense>(expenseDto);
        expense.UserId = user.Id;
        expense.User = user;
        
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();

        return ServiceResult<ExpenseDto>.Success(_mapper.Map<ExpenseDto>(expense));
    }

    public async Task<ServiceResult<ExpenseDto>> GetUserExpenseById(string username, int expenseId)
    {
        var user = await _authService.GetCurrentUser(username);
        
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
        var user = await _authService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IEnumerable<ExpenseDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var expenseDtos = _mapper.Map<IEnumerable<ExpenseDto>>(user.Expenses);

        return ServiceResult<IEnumerable<ExpenseDto>>.Success(expenseDtos);
    }

    public async Task<ServiceResult<bool>> DeleteExpense(string username, int expenseId)
    {
        var user = await _authService.GetCurrentUser(username);
        
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
        var user = await _authService.GetCurrentUser(username);

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
}