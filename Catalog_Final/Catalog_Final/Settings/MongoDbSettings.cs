using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog_Final.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        //Below is without username and password
        //public string ConnectionString { 
        //    get 
        //    {
        //        return $"mongodb://{Host}:{Port}";
        //    } 
        //}

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}
