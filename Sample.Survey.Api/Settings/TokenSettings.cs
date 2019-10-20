using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Survey.Api.Settings
{
    public class TokenSettings
    {
        public string Key { get; set; }
        public double Lifetime { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public bool Enable { get; set; }
    }
}
