using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Auth
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LogoutController : Controller
    {
        [HttpPost]
        public async void Post([FromBody]LoginCommand command)
        {
            await HttpContext.SignOutAsync("DatScheme");
        }
    }
}