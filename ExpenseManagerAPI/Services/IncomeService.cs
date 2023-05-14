using AutoMapper;
using DbModel;
using DbModel.Entities;
using ExpenseManagerAPI.DTOs.Income;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManagerAPI.Services;

public class IncomeService
{
    private readonly UserManager<User> _userManager;
    private readonly AuthService _authService;
    private readonly ExpenseManagerDbContext _context;
    private readonly IMapper _mapper;

    public IncomeService(UserManager<User> userManager, AuthService authService, ExpenseManagerDbContext context, IMapper mapper)
    {
        _userManager = userManager;
        _authService = authService;
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
        
        var income = _mapper.Map<Income>(incomeDto);
        income.UserId = user.Id;
        income.User = user;
        
        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync();

        return ServiceResult<IncomeDto>.Success(_mapper.Map<IncomeDto>(income));
    }

    public async Task<ServiceResult<IncomeDto>> GetUserIncomeById(string username, int incomeId)
    {
        var user = await _authService.GetCurrentUser(username);
        
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
}