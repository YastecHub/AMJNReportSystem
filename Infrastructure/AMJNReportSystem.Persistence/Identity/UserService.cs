using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Persistence.Auth;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.FileStorage;
using AMJNReportSystem.Application.Interfaces;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using AMJNReportSystem.Application;
using AMJNReportSystem.Domain.Entities;
using AMJNReportSystem.Application.Identity.Tokens;

namespace AMJNReportSystem.Persistence.Identity
{
    public partial class UserService : IUserService
    {
        
        private readonly ApplicationContext _db;
        private readonly IStringLocalizer _t;
        private readonly ICurrentUser _currentUser;
        private readonly IGatewayHandler _gatewayHandler;
        private readonly SecuritySettings _securitySettings;
        private readonly IFileStorageService _fileStorage;

        public UserService(
            ApplicationContext db,
            IStringLocalizer<UserService> localizer,
            ICurrentUser currentUser,
            IOptions<SecuritySettings> securitySettings, IGatewayHandler gatewayHandler)
        {
            _db = db;
            _t = localizer;
            _securitySettings = securitySettings.Value;
            _currentUser = currentUser;
            _gatewayHandler = gatewayHandler;
        }

       
        public Task<string[]> GetMemberRoleAsync(int chandaNo)
        {
           return _gatewayHandler.GetMemberRoleAsync(chandaNo);
        }

        public Task<User> GetMemberByChandaNoAsync(int chandaNo)
        {
            return _gatewayHandler.GetMemberByChandaNoAsync(chandaNo);
        }
        public Task<MemberApiLoginResponse> GenerateToken(TokenRequest request) 
        {
            return _gatewayHandler.GenerateToken(request);
        }

    }
}