using Sample.Survey.Api.Models;
using Sample.Survey.Api.ProProfs;
using Sample.Survey.Api.ProProfs.ProxyModels;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

namespace Sample.Survey.Api.Adapter
{
    public class ProProfsAdapter
    {
        private readonly ProProfsProxy profProfsProxy;
        public ProProfsAdapter(ProProfsProxy profProfsProxy)
        {
            this.profProfsProxy = profProfsProxy;
        }

        public virtual async Task<bool> AddUserToGroup(ProProfsUser userModel)
        {
            var reegistermodel = new ProProfsRegistrationModel();
            reegistermodel.email = userModel.Email;
            reegistermodel.group = userModel.Group.ToArray();
            var result = await this.profProfsProxy.Register(reegistermodel);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            var response = result.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(response.Result);
            var status = jsonResponse["status"];
            if (status.Value<string>() == ProProfsStatus.ERROR.ToString())
            {
                return false;
            }

            return true;
        }

        public virtual async Task<bool> AddUserToQuiz(ProProfsUser userModel)
        {
            var reegistermodel = new ProProfsRegistrationModel();
            reegistermodel.email = userModel.Email;
            reegistermodel.quiz_assignment = userModel.Quizzes.Select(x => x.EncodedQuizLink).ToArray();
            var result = await this.profProfsProxy.Register(reegistermodel);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            var response = result.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(response.Result);
            var status = jsonResponse["status"];
            if (status.Value<string>() == ProProfsStatus.ERROR.ToString())
            {
                return false;
            }

            return true;
        }

        public virtual async Task<ProProfsUser> QuizStatus(string userName)
        {
            var model = new ProProfsProgressModel();
            model.email_or_id = userName;
            var result = await this.profProfsProxy.GetProgress(model);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }


            var response = result.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(response.Result);
            var status = jsonResponse["status"];
            if (status.Value<string>() == ProProfsStatus.ERROR.ToString())
            {
                return null;
            }

            var data = (JArray)jsonResponse["result"];

            if (data.Count > 0)
            {
                var user = new ProProfsUser();
                user.Email = data[0]["Email"].Value<string>();
                user.UserName = data[0]["Name"].Value<string>();
                user.Group = ((JArray)data[0]["Group"]).Select(x => x.Value<string>()).ToList();
                user.Quizzes = ((JArray)data[0]["assignment"]).Select(x => new ProProfsUserQuiz()
                {
                    QuizId = x["ID"]?.Value<string>(),
                    QuizTitle = x["title"]?.Value<string>(),
                    QuizUniqueName = x["quiz_unique_title"]?.Value<string>(),
                    QuizStatus = x["Status"]?.Value<string>(),
                    AttemptDate = x["assignedOnDate"]?.Value<DateTime>(),
                    PercentComplete = x["percentCompleted"]?.Value<double>(),
                }).ToList();

                return user;
            }

            return null;
        }
    }
}
