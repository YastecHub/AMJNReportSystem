using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Persistence.Extensions;
using application.Models.ResponseModels;
using Application.Abstractions.Repositories;
using Application.Models;
using Application.Wrapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly ApplicationContext _context;

        public OfficeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Office> AddOffice(Office office)
        {
            await _context.AddAsync(office);
            await _context.SaveChangesAsync();
            return office;

        }

        public async Task<bool> ExistByName(string name)
        {
            return await _context.Office.AnyAsync(c => c.Name == name);
        }

        public async Task<IList<Office>> GetAllOfficesAsync()
        {
            return await _context.Office.ToListAsync();
        }

        public async Task<PaginatedResult<OfficeResponseModel>> GetAllOfficesAsync(PaginationFilter filter)
        {
            var offices = _context.Office;
            if (string.IsNullOrWhiteSpace(filter.Keyword))
            {
                return await offices
                .ToMappedPaginatedResultAsync<Office, OfficeResponseModel>
                (filter.PageNumber, filter.PageSize);

            }
            return await offices.SearchByKeyword(filter.Keyword)
            .ToMappedPaginatedResultAsync<Office, OfficeResponseModel>
            (filter.PageNumber, filter.PageSize);

        }

        public async Task<Office> GetByName(string name)
        {
            return await _context.Office.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Office> GetOfficeById(Guid id)
        {
            return await _context.Office.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Office> UpdateOffice(Office office)
        {
            _context.Update(office);
            await _context.SaveChangesAsync();
            return office;
        }
    }
}
