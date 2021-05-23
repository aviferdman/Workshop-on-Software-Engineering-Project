using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TradingSystem.Service;
using TradingSystem.WebApi.DTO;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserGatewayController : ControllerBase
    {
        [HttpPost]
        public String AddGuest()
        {
            return MarketUserService.Instance.AddGuest();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login([FromBody] LoginInfo info)
        {
            string res = await MarketUserService.Instance.loginAsync(info.username, info.password, info.guestusername);
            if (res != "success" && res != "admin")
                return BadRequest(res);

            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult<String>> SignUp([FromBody] SignUpInfo info)
        {
            string res = await UserService.Instance.SignupAsync(info.guestusername, info.username, info.password, info._state, info._city, info._street, info._apartmentNum, info.zip, info.phone);
            if (!res.Equals("success"))
                return BadRequest(res);
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> Logout([FromBody] UsernameDTO usernameDTO)
        {
            if (string.IsNullOrWhiteSpace(usernameDTO.Username))
            {
                return BadRequest("Invalid username");
            }

            string? result = await MarketUserService.Instance.logoutAsync(usernameDTO.Username);
            if (string.IsNullOrWhiteSpace(result))
            {
                return InternalServerError();
            }

            return Ok(result);
        }
    }
}
