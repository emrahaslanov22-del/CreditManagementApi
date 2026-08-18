using AutoMapper;
using CreditManagementApi.Dtos;
using CreditManagementApi.Entities;

namespace CreditManagementApi.Mapper
{
    public class DebtorMapper : Profile
    {
        public DebtorMapper()
        {
            CreateMap<Debtor, DebtorDto>();
            CreateMap<CreateDebtorDto, Debtor>();
            CreateMap<UpdateDebtorDto, Debtor>();
        }

    }
}
