using CreditManagementApi.Dtos;
using CreditManagementApi.Entities;
using CreditManagementApi.Models;

namespace CreditManagementApi.Services.Abstract
{
    public interface IDebtorService
    {
        Task<PageResultDto<DebtorDto>> GetAll(int page, int pageSize, string? fincode, string? firstName);
        Task<Debtor> GetDebtorAsync(Guid id);
        Task<bool> DeleteDebtorAsync(Guid id);
        Task<Debtor> UpdateAsync(Debtor debtor);
        Task<Debtor> AddAsync(Debtor debtor);
        Task<Debtor> GetByFincodeAsync(string fincode);

    }
}
