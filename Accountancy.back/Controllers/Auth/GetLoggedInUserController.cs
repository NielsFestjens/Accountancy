using Accountancy.Domain.Auth;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Auth
{
    [Route("api/Auth/[controller]")]
    [AllowAnonymous]
    public class GetLoggedInUserController : Controller
    {
        private readonly IRepository _repository;
        private readonly ISecurityService _securityService;

        public GetLoggedInUserController(IRepository repository, ISecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        [HttpGet]
        public GetLoggedInUserResponse GetLoggedInUser()
        {
            if (!User.Identity.IsAuthenticated)
                return new GetLoggedInUserResponse();

            var userId = _securityService.GetUserId(User);
            var user = _repository.Get<User>(userId);

            return new GetLoggedInUserResponse
            {
                User = User.Identity.IsAuthenticated ? new UserDto { Username = user.Username } : null
            };
        }
    }

    public class GetLoggedInUserResponse
    {
        public UserDto User { get; set; }
    }

    public class UserDto
    {
        public string Username { get; set; }
    }
}