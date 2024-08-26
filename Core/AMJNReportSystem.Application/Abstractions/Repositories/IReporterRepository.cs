using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Repositories
{
    public interface IReporterRepository
    {
        Task<Reporter> AddReporter(Reporter reporter);
        Task<Reporter> GetReporterById(Guid reporterId);
    }
}
