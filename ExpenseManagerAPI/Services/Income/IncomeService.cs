using AutoMapper;
using DbModel;
using ExpenseManagerAPI.DTOs.Income;
using ExpenseManagerAPI.Services.User;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManagerAPI.Services.Income;

public class IncomeService
{
    private readonly UserManager<DbModel.Entities.User> _userManager;
    private readonly UserService _userService;
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public IncomeService(UserManager<DbModel.Entities.User> userManager, UserService userService, ExpenseManagerDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ServiceResult<IncomeDto>> AddIncomeForUser(string username, AddIncomeDto incomeDto)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return ServiceResult<IncomeDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var income = _mapper.Map<DbModel.Entities.Income>(incomeDto);
        income.UserId = user.Id;
        income.User = user;
        
        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync();

        return ServiceResult<IncomeDto>.Success(_mapper.Map<IncomeDto>(income));
    }

    public async Task<ServiceResult<IncomeDto>> GetUserIncomeById(string username, int incomeId)
    {
        var user = await _userService.GetCurrentUser(username);
        
        if (user == null)
        {
            return ServiceResult<IncomeDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var income = user.Incomes.FirstOrDefault(i => i.Id == incomeId);
        
        if (income == null)
        {
            return ServiceResult<IncomeDto>.Failure($"Income with id: {incomeId} not found.", ResultCode.NotFound);
        }

        var incomeDto = _mapper.Map<IncomeDto>(income);

        return ServiceResult<IncomeDto>.Success(incomeDto);
    }
    
    public async Task<ServiceResult<IEnumerable<IncomeDto>>> GetUserIncomes(string username)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IEnumerable<IncomeDto>>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var incomeDtos = _mapper.Map<IEnumerable<IncomeDto>>(user.Incomes);

        return ServiceResult<IEnumerable<IncomeDto>>.Success(incomeDtos);
    }
    
    public async Task<ServiceResult<bool>> DeleteIncome(string username, int incomeId)
    {
        var user = await _userService.GetCurrentUser(username);
        
        if (user == null)
        {
            return ServiceResult<bool>.Failure("You are not authorized", ResultCode.Unauthorized);
        }
        
        var income = user.Incomes.FirstOrDefault(i => i.Id == incomeId);
        
        if (income == null)
        {
            return ServiceResult<bool>.Failure("Income with id: {incomeId} not found.", ResultCode.NotFound);
        }
        
        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();

        return ServiceResult<bool>.Success(true, ResultCode.NoContent);
    }
    
    public async Task<ServiceResult<IncomeDto>> UpdateIncome(string username, IncomeDto incomeDto)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<IncomeDto>.Failure("You are not authorized", ResultCode.Unauthorized);
        } 
        
        var income = user.Incomes.FirstOrDefault(i => i.Id == incomeDto.Id);
        
        if (income == null)
        {
            return ServiceResult<IncomeDto>.Failure("Income not found.", ResultCode.NotFound);
        }

        income.Amount = incomeDto.Amount;
        income.Date = incomeDto.Date;
        income.Description = incomeDto.Description;

        _context.Incomes.Update(income);
        await _context.SaveChangesAsync();
        
        return ServiceResult<IncomeDto>.Success(_mapper.Map<IncomeDto>(income));
    }
    
    public async Task<ServiceResult<decimal>> GetTotalIncome(string username)
    {
        var user = await _userService.GetCurrentUser(username);

        if (user == null)
        {
            return ServiceResult<decimal>.Failure("You are not authorized", ResultCode.Unauthorized);
        }

        var totalIncome = user.Incomes
            .Sum(i => i.Amount);

        return ServiceResult<decimal>.Success(totalIncome);
    }
}