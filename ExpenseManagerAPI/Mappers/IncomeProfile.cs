using AutoMapper;
using DbModel.Entities;
using ExpenseManagerAPI.DTOs.Income;

namespace ExpenseManagerAPI.Mappers;

public class IncomeProfile : Profile
{
    public IncomeProfile()
    {
        CreateMap<Income, IncomeDto>();
        CreateMap<AddIncomeDto, Income>();
    }
}