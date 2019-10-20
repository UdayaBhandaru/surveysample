using Sample.Survey.Library.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Survey.Library.MongoDB
{
    public abstract class MongoContext
    {
        public IMongoDatabase Database { get; set; }


        public MongoContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            if (client != null)
            {
                this.Database = client.GetDatabase(options.Value.DBName);
            }
        }
    }
}
