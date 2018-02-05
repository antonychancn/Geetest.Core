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
                http.Timeout = _geetestConfiguration.Timeout;

                var url = _geetestConfiguration.Protocol + _geetestConfiguration.ApiServerUrl +
                          _geetestConfiguration.ApiRegisterUrl + $"?{_geetestConfiguration.ToQueryString()}";

                var result = new GeetestRegisterResult
                {
                    Success = false,
                    Gt = _geetestConfiguration.Id,
                    Challenge = Guid.NewGuid().ToString("N"),
                    NewCaptcha = true
                };
                try
                {
                    var responseMessage = await http.GetAsync(url);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        result.Challenge = JsonConvert.DeserializeObject<GeetestRegisterResult>(
                            await responseMessage.Content.ReadAsStringAsync()).Challenge;

                        result.Success = true;
                        result.Challenge = (result.Challenge + _geetestConfiguration.Key).Md5();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

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
                http.Timeout = _geetestConfiguration.Timeout;

                var url = _geetestConfiguration.Protocol + _geetestConfiguration.ApiServerUrl +
                          _geetestConfiguration.ApiValidateUrl;

                try
                {
                    var responseMessage = await http.PostAsync(url, new FormUrlEncodedContent(
                        new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("gt", _geetestConfiguration.Id),
                            new KeyValuePair<string, string>("seccode", geetestValidate.Seccode),
                            new KeyValuePair<string, string>("json_format",
                                _geetestConfiguration.JsonFormat ? "1" : "0")
                        }));

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    var result = JsonConvert.DeserializeObject<GeetestValidateResult>(
                        await responseMessage.Content.ReadAsStringAsync());
                    if (result.Seccode.Length < 16)
                    {
                        return false;
                    }
                    return result.Seccode == geetestValidate.Seccode.Md5();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}