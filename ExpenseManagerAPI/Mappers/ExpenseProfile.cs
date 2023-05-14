using AutoMapper;
using DbModel.Entities;
using ExpenseManagerAPI.DTOs.Expense;

namespace ExpenseManagerAPI.Mappers;

public class ExpenseProfile : Profile
{
    public ExpenseProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<AddExpenseDto, Expense>();
    }
}