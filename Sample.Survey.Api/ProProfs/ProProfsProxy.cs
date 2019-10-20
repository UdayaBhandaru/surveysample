using Sample.Survey.Api.ProProfs.ProxyModels;
using Sample.Survey.Api.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Survey.Api.ProProfs
{
    public class ProProfsProxy
    {
        private IOptions<ProProfsSettings> profopsOptions;
        private HttpHandler _httpclienthandler;


        public ProProfsProxy(IOptions<ProProfsSettings> options, Api.HttpHandler httpClientHandler)
        {
            this.profopsOptions = options;
            this._httpclienthandler = httpClientHandler;
        }


        public virtual async Task<HttpResponseMessage> GetProgress(ProProfsProgressModel model)
        {
            model.token = this.profopsOptions.Value.ApplicationKey;
            model.username = this.profopsOptions.Value.UserName;
            string json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json"); ;
            return await this._httpclienthandler.PostAsync(this.profopsOptions.Value.ProProfsReportUrl, httpContent);
        }

        public virtual async Task<HttpResponseMessage> Register(ProProfsRegistrationModel model)
        {
            model.token = this.profopsOptions.Value.ApplicationKey;
            model.username = this.profopsOptions.Value.UserName;
            string json = JsonConvert.SerializeObject(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await this._httpclienthandler.PostAsync(this.profopsOptions.Value.ProProfsRegisterUrl, httpContent);
        }
    }
}
