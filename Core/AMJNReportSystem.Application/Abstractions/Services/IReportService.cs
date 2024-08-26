using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Application.Models.RequestModels.Reports;
using AMJNReportSystem.Application.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Method for submitting report 
        /// </summary>
        Task<Result<bool>> SaveReport(ReportRequest request);
    }
}
