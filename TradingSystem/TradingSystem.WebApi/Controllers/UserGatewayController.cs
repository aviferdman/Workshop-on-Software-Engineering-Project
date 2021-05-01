using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingSystem.Business.UserManagement;
using TradingSystem.Service;

namespace TradingSystem.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserGatewayController : ControllerBase
    {
        [HttpPost]
        public String AddGuest()
        {
            return MarketUserService.Instance.AddGuest();
        }

        [HttpPost]
        public ActionResult<String> Login([FromBody] LoginInfo info)
        {
            string res = MarketUserService.Instance.login(info.username, info.password, info.guestusername);
            if (!res.Equals("success"))
                return BadRequest(res);
            
            return Ok(res);
        }

        [HttpPost]
        public ActionResult<String> SignUp([FromBody] SignUpInfo info)
        {
            string res = UserService.Instance.Signup(info.guestusername, info.username, info.password, info._state, info._city, info._street, info._apartmentNum, info.phone);
            if (!res.Equals("success"))
                return BadRequest(res);
            return Ok(res);
        }
    }
}
