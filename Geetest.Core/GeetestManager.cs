using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Geetest.Core.Configuration;
using Newtonsoft.Json;

namespace Geetest.Core
{
    public class GeetestManager : IGeetestManager
    {
        private readonly IGeetestConfiguration _geetestConfiguration;

        public GeetestManager(IGeetestConfiguration geetestConfiguration)
        {
            _geetestConfiguration = geetestConfiguration;
        }

        public async Task<RegisterResult> Register()
        {
            using (var http = new HttpClient())
            {
                var url = _geetestConfiguration.Protocol + _geetestConfiguration.ApiServerUrl +
                          _geetestConfiguration.ApiRegisterUrl + $"?{_geetestConfiguration.ToQueryString()}";

                var responseMessage = await http.GetAsync(url);

                var result = JsonConvert.DeserializeObject<RegisterResult>(
                    await responseMessage.Content.ReadAsStringAsync());
                result.Success = true;
                result.Gt = _geetestConfiguration.Id;
                result.NewCaptcha = true;
                return result;
            }
        }

        public async Task Validate()
        {
            throw new NotImplementedException();
        }
    }
}
