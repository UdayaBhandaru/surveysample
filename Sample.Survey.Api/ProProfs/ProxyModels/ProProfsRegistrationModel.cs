namespace Sample.Survey.Api.ProProfs.ProxyModels
{
    public class ProProfsRegistrationModel
    {
        public string token { get; set; }
        public string username { get; set; }
        public string email { get; set; }

        public string id { get; set; }

        public string fname { get; set; }

        public string lname { get; set; }

        public string password { get; set; }

        public string[] quiz_assignment { get; set; }

        public string course_assignment { get; set; }

        public string[] group { get; set; }

        public string sub_group { get; set; }
    }
}
