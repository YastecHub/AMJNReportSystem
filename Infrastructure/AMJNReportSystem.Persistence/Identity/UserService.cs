using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Persistence.Auth;
using AMJNReportSystem.Persistence.Context;
using AMJNReportSystem.Application.Authorization;
using AMJNReportSystem.Application.Exceptions;
using AMJNReportSystem.Application.FileStorage;
using AMJNReportSystem.Application.Interfaces;
using AMJNReportSystem.Application.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using AMJNReportSystem.Application;
using AMJNReportSystem.Domain.Entities;

namespace AMJNReportSystem.Persistence.Identity
{
    public partial class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationContext _db;
        private readonly IStringLocalizer _t;
        private readonly ICurrentUser _currentUser;
        private readonly IGatewayHandler _gatewayHandler;
        private readonly SecuritySettings _securitySettings;
        private readonly IFileStorageService _fileStorage;

        public UserService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationContext db,
            IStringLocalizer<UserService> localizer,
            ICurrentUser currentUser,
            IOptions<SecuritySettings> securitySettings, IGatewayHandler gatewayHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _t = localizer;
            _securitySettings = securitySettings.Value;
            _currentUser = currentUser;
            _gatewayHandler = gatewayHandler;
        }

        public async Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken)
        {
            //var spec = new EntitiesByPaginationFilterSpec<ApplicationUser>(filter);

            var users = await _userManager.Users
                // .WithSpecification(spec)
                .ProjectToType<UserDetailsDto>()
                .ToListAsync(cancellationToken);
            int count = await _userManager.Users
                .CountAsync(cancellationToken);

            return new PaginationResponse<UserDetailsDto>(users, count, filter.PageNumber, filter.PageSize);
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            EnsureValidTenant();
            return await _userManager.FindByNameAsync(name) is not null;
        }

        public async Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.FindByEmailAsync(email.Normalize()) is ApplicationUser user && user.Id != exceptId;
        }

        public async Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
        {
            EnsureValidTenant();
            return await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber) is ApplicationUser user && user.Id != exceptId;
        }

        private void EnsureValidTenant()
        {
            /*if (string.IsNullOrWhiteSpace(_currentTenant?.Id))
            {
                throw new UnauthorizedException(_t["Invalid Tenant."]);
            }*/
        }

        public async Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken) =>
            (await _userManager.Users
                    .AsNoTracking()
                    .ToListAsync(cancellationToken))
                .Adapt<List<UserDetailsDto>>();

        public Task<int> GetCountAsync(CancellationToken cancellationToken) =>
            _userManager.Users.AsNoTracking().CountAsync(cancellationToken);

        public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            return user.Adapt<UserDetailsDto>();
        }

        public async Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

            _ = user ?? throw new NotFoundException(_t["User Not Found."]);

            bool isAdmin = await _userManager.IsInRoleAsync(user, GXRoles.SuperAdmin);
            if (isAdmin)
            {
                throw new ConflictException(_t["Administrators Profile's Status cannot be toggled"]);
            }

            user.IsActive = request.ActivateUser;

            await _userManager.UpdateAsync(user);
        }

        public Task<string[]> GetMemberRoleAsync(int chandaNo)
        {
           return _gatewayHandler.GetMemberRoleAsync(chandaNo);
        }

        public Task<UserApi> GetMemberByChandaNoAsync(int chandaNo)
        {
            return _gatewayHandler.GetMemberByChandaNoAsync(chandaNo);
        }
        public Task<TokenResponse> GenerateToken() 
        {
            return _gatewayHandler.GenerateToken();
        }

    }
}