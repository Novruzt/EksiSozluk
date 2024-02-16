using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public Guid? UserId
        {
            get
            {
                var value = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return value is null ? null : new Guid(value);
            }
        }
    }
}
