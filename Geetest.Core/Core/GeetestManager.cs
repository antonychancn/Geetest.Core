using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public async Task<GeetestRegisterResult> RegisterAsync()
        {
            using (var http = new HttpClient())
            {
                var url = _geetestConfiguration.Protocol + _geetestConfiguration.ApiServerUrl +
                          _geetestConfiguration.ApiRegisterUrl + $"?{_geetestConfiguration.ToQueryString()}";

                var responseMessage = await http.GetAsync(url);

                var result = JsonConvert.DeserializeObject<GeetestRegisterResult>(
                    await responseMessage.Content.ReadAsStringAsync());

                if (!responseMessage.IsSuccessStatusCode || result.Challenge.IsNullOrWhiteSpace())
                {
                    //Dang 🐔
                    result.Success = false;

                    var rnd = new Random();
                    var a = rnd.Next(0, 90).ToString().Md5();
                    var b = rnd.Next(0, 90).ToString().Md5();

                    result.Challenge = a + b.Substring(0, 2);
                }
                else
                {
                    result.Success = true;
                    result.Challenge = (result.Challenge + _geetestConfiguration.Key).Md5();
                }

                result.Gt = _geetestConfiguration.Id;
                result.NewCaptcha = true;
                return result;
            }
        }

        public async Task<bool> ValidateAsync(GeetestValidate geetestValidate)
        {
            if (geetestValidate.Offline)
            {
                return geetestValidate.Challenge.Md5() == geetestValidate.Validate.ToLower();
            }

            var hash = $"{_geetestConfiguration.Key}geetest{geetestValidate.Challenge}";
            if (hash.Md5() != geetestValidate.Validate)
            {
                return false;
            }

            using (var http = new HttpClient())
            {
                var url = _geetestConfiguration.Protocol + _geetestConfiguration.ApiServerUrl +
                          _geetestConfiguration.ApiValidateUrl;

                var responseMessage = await http.PostAsync(url, new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("gt", _geetestConfiguration.Id),
                        new KeyValuePair<string, string>("seccode", geetestValidate.Seccode),
                        new KeyValuePair<string, string>("json_format", "1")
                    }));

                var result = JsonConvert.DeserializeObject<GeetestValidateResult>(
                    await responseMessage.Content.ReadAsStringAsync());
                if (result.Seccode == "false")
                {
                    return false;
                }
                return result.Seccode == geetestValidate.Seccode.Md5();
            }
        }
    }
}