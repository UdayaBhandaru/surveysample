namespace Sample.Survey.Api.ProProfs.ProxyModels
{
    public class ProProfsProgressModel
    {
        public string token { get; set; }
        public string username { get; set; }
        public int start { get; set; }
        public int num { get; set; }

        public string email_or_id { get; set; }

        public string group_name { get; set; }
    }
}
