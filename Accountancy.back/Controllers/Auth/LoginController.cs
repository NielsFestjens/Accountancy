using System.Linq;
using System.Threading.Tasks;
using Accountancy.Domain.Security;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Exceptions;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Auth
{
    public class LoginCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IRepository _repository;
        private readonly ISecurityService _securityService;

        public LoginController(IRepository repository, ISecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        [HttpPost]
        public async Task Post([FromBody]LoginCommand command)
        {
            var user = _repository.Query<User>().SingleOrDefault(x => x.Username == command.Username);
            var salt = user?.PasswordSalt ?? _securityService.GetSalt();
            var hashedPassword = _securityService.CalculateHash(command.Password, salt);
            if (user == null || user.Password != hashedPassword)
                throw new BadUsernamePasswordCombinationException();

            await _securityService.SignIn(HttpContext, user);
        }
    }

    public class BadUsernamePasswordCombinationException : KnownException
    {
        public BadUsernamePasswordCombinationException() : base("Bad username/password combination")
        {
        }
    }
}