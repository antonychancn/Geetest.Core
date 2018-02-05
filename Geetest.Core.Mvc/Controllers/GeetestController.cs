using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Geetest.Core.Mvc.Controllers
{
    [Produces("application/json")]
    [Route("api/Geetest")]
    public class GeetestController : Controller
    {
        private readonly IGeetestManager _geetestManager;

        public GeetestController(IGeetestManager geetestManager)
        {
            _geetestManager = geetestManager;
        }

        // GET: api/Geetest
        [HttpGet]
        public async Task<GeetestRegisterResult> Register()
        {
            return await _geetestManager.RegisterAsync();
        }
    }
}