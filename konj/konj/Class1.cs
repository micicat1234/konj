using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Configuration;


namespace konj
{
        
        public class BazaConn
        {
            public static string connect()
            {
                var uriString = ConfigurationManager.AppSettings["ELEPHANTSQL_URL"] ?? "postgres://aqjettlx:RH7eUDXFOqHHu2u0CwpO0FSOQWSnZNDt@kandula.db.elephantsql.com:5432/aqjettlx";
                var uri = new Uri(uriString);
                var db = uri.AbsolutePath.Trim('/');
                var user = uri.UserInfo.Split(':')[0];
                var passwd = uri.UserInfo.Split(':')[1];
                var port = uri.Port > 0 ? uri.Port : 5432;
                var connStr = string.Format("Server={0};Database={1};User Id={2};Password={3};Port={4}",
                uri.Host, db, user, passwd, port);
                return connStr;
            }

        }
        
}

        

