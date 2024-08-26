using AMJNReportSystem.Application.Models;
using AMJNReportSystem.Application.Wrapper;

namespace AMJNReportSystem.Application.Identity.Users
{
    public class UserListFilter : PaginationFilter
    {
        public bool? IsActive { get; set; }
    }
}