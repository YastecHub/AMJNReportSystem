using AMJNReportSystem.Application.Identity.Tokens;
using AMJNReportSystem.Application.Identity.Users;
using AMJNReportSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace AMJNReportSystem.WebApi.Controllers.Identity
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) => _userService = userService;

        [HttpGet("get-member-roles-by-chanda")]
        [AllowAnonymous]
        [OpenApiOperation("get member roles.", "")]
        public Task<string[]> GetMemberRoles([FromQuery] int chandaNo)
        {
            return _userService.GetMemberRoleAsync(chandaNo);
        }
        
        [HttpGet("get-member-by-chanda")]
        [AllowAnonymous]
        [OpenApiOperation("get member by chanda.", "")]
        public Task<User> GetMemberByChandaNoAsync([FromQuery] int chandaNo)
        {
            return _userService.GetMemberByChandaNoAsync(chandaNo);
        }

        [HttpGet("generate-token")]
        [AllowAnonymous]
        [OpenApiOperation("generate token", "")]
        public Task<MemberApiLoginResponse> GenerateToken(TokenRequest request) 
        {
            return _userService.GenerateToken(request);
        }


        private string GetOriginFromRequest() => $"{Request.Scheme}://{Request.Host}{Request.PathBase.Value}";
    }
}
