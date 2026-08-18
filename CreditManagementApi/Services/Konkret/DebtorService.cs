using AutoMapper;
using CreditManagementApi.Dtos;
using CreditManagementApi.Entities;
using CreditManagementApi.Models;
using CreditManagementApi.Repository.Abstract;
using CreditManagementApi.Services.Abstract;

namespace CreditManagementApi.Services.Konkret
{
    public class DebtorService : IDebtorService
    {
        private readonly IDebtorRepository _repositroy;
        private readonly IMapper _mapper;

        public DebtorService(IDebtorRepository repositroy,IMapper mapper)
        {
            _repositroy = repositroy;
            _mapper = mapper;
        }

        public async Task<Debtor> AddAsync(Debtor debtor)
        {
            var existingDebtor = await _repositroy.GetByFincodeAsync(debtor.Fincode);

            if (existingDebtor != null)
            {
                throw new InvalidOperationException("This FIN code already exists");
            }

            if (debtor.CurrentBalance > debtor.CreditBalance)
            {
                throw new InvalidOperationException("Current balance cannot be greater than credit balance");
            }

            if (debtor.CreditTermMonths <= 0)
            {
                throw new InvalidOperationException("Credit term must be greater than zero");
            }

            var result = await _repositroy.AddAsync(debtor);

            await _repositroy.SaveChangesAsync();

            return result;
        }

        public async Task<bool> DeleteDebtorAsync(Guid id)
        {
            var debtor = await _repositroy.GetDebtorAsync(id);

            if (debtor == null)
            {
                throw new KeyNotFoundException("Debtor was not found");
            }

            await _repositroy.DeleteDebtorAsync(id);
            await _repositroy.SaveChangesAsync();

            return true;

        }
        public Task<Debtor> GetByFincodeAsync(string fincode)
        {
            return _repositroy.GetByFincodeAsync(fincode);
        }

        public async Task<Debtor> GetDebtorAsync(Guid id)
        {
            var result = await _repositroy.GetDebtorAsync(id);

            if (result == null)
            {
                throw new KeyNotFoundException("Debtor was not found");
            }

            return result;
        }

        public async Task<Debtor> UpdateAsync(Debtor debtor)
        {
            var existingDebtor = await _repositroy.GetDebtorAsync(debtor.Id);

            if (existingDebtor == null)
            {
                throw new KeyNotFoundException("Debtor was not found");
            }

            if (existingDebtor.Fincode != debtor.Fincode)
            {
                throw new InvalidOperationException("FIN code cannot be changed");
            }

            if (debtor.CurrentBalance > debtor.CreditBalance)
            {
                throw new InvalidOperationException("Current balance cannot be greater than credit balance");
            }

            var result = await _repositroy.UpdateAsync(debtor);

            await _repositroy.SaveChangesAsync();

            return result;
        }

        public async Task<PageResultDto<DebtorDto>> GetAll(int page,int pageSize,string? fincode,string? firstName)
        {
            var result = await _repositroy
                .GetAll(
                page,
                pageSize,
                fincode,
                firstName);

            return new PageResultDto<DebtorDto>
            {
                Items = _mapper.Map<List<DebtorDto>>(result.items),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }
    }
}
