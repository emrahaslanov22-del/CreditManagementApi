using CreditManagementApi.Context;
using CreditManagementApi.Entities;
using CreditManagementApi.Models;
using CreditManagementApi.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CreditManagementApi.Repository.Konkret
{
    public class DebtorRepository : IDebtorRepository
    {
        private readonly DebtorContext _context;

        public DebtorRepository(DebtorContext context)
        {
            _context = context;
        }

        public async Task<Debtor> AddAsync(Debtor debtor)
        {
            var result = await _context.debtors.AddAsync(debtor);
            return result.Entity;
        }

        public async Task<bool> DeleteDebtorAsync(Guid id)
        {
            var debtor = await _context.debtors.FindAsync(id);
            _context.debtors.Remove(debtor);
            return true;
        }

        public async Task<PageResult<Debtor>> GetAll(int page,int pageSize,string? fincode,string? firstName)
        {
            var query = _context.debtors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(fincode))
            {
                query = query.Where(d => d.Fincode == fincode);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(d =>d.FirstName.Contains(firstName));
            }

            var totalCount = await query.CountAsync();

            var debtors = await query
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResult<Debtor>
            {
                items = debtors,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<Debtor?> GetByFincodeAsync(string fincode)
        {
            return await _context.debtors.FirstOrDefaultAsync(d => d.Fincode == fincode);
        }

        public async Task<Debtor?> GetDebtorAsync(Guid id)
        {
            return await _context.debtors.FindAsync(id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<Debtor> UpdateAsync(Debtor debtor)
        {
            _context.debtors.Update(debtor);
            return Task.FromResult(debtor);
        }
    }
}
