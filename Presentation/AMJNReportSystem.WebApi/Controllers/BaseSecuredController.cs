using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMJNReportSystem.WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseSecuredController : ControllerBase
    {
        //private readonly ICurrentUser _currentUser;
        ICurrentUser authenticationContext = null!;

        private ICurrentUser AuthenticationContext
        {
            get
            {
                if (authenticationContext == null)
                {
                    authenticationContext = HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
                }
                return authenticationContext;
            }
        }
        /// <summary>
        /// User context of currently logged in user 
        /// </summary>
        protected string UserContext => AuthenticationContext.GetUserEmail();
    }
}
