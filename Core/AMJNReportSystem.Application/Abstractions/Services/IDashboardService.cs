using AMJNReportSystem.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Abstractions.Services
{
    public interface IDashboardService
    {
        Task<DashboardCountDto> DashBoardCount();
    }
}
