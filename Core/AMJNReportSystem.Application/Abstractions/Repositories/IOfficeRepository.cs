using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;
using application.Models.ResponseModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IOfficeRepository
    {
        Task<Office> AddOffice(Office office);
        Task<Office> UpdateOffice(Office office);
        Task<IList<Office>> GetAllOfficesAsync();
        Task<PaginatedResult<OfficeResponseModel>> GetAllOfficesAsync(PaginationFilter filter);
        Task<Office> GetOfficeById(Guid id);
        Task<bool> ExistByName(string name);
        Task<Office> GetByName(string name);
    }
}
