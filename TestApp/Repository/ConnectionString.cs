using TestApp.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp
{
    public sealed class ConnectionString : IConnectionString
    {
        public string Value { get; }
 
        public ConnectionString()
        { 
             Value = "data source=database-1.cfycuqjk6ac8.us-east-1.rds.amazonaws.com,1433;initial catalog=info;User ID = Admin; Password = Adminadmin123?";
        }
    }
}
