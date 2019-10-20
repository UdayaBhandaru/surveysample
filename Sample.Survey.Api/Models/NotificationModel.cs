using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Models
{
    public class NotificationModel
    {
        public int result_id { get; set; }

        public string status { get; set; }

        public string quiz_id { get; set; }

        public string quiz_title { get; set; }

        public string quiz_name { get; set; }

        public DateTimeOffset attempt_date { get; set; }

        public int total_marks { get; set; }

        public int user_obtained_marks { get; set; }

        public int user_percent_marks { get; set; }

        public int user_totalcorrect_answers { get; set; }

        public string user_Id { get; set; }

        public string user_Email { get; set; }

        public string user_Address { get; set; }

        public string user_City { get; set; }

        public string user_State { get; set; }

        public string user_Zipcode { get; set; }

        public string user_Phone { get; set; }

        public string user_name { get; set; }

        public string min_pass_marks { get; set; }
    }
}
