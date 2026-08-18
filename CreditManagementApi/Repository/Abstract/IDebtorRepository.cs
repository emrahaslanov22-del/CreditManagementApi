using CreditManagementApi.Entities;
using CreditManagementApi.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CreditManagementApi.Repository.Abstract
{
    public interface IDebtorRepository
    {
        Task<Debtor> GetDebtorAsync(Guid id);
        Task<bool> DeleteDebtorAsync(Guid id);
        Task<Debtor> UpdateAsync(Debtor debtor);
        Task<Debtor> AddAsync(Debtor debtor);
        Task<bool> SaveChangesAsync();
        Task<Debtor> GetByFincodeAsync(string fincode);
        Task<PageResult<Debtor>> GetAll(int page,int pageSize,string? fincode,string? firstName);
    }
}
