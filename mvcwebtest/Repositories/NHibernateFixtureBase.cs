using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace mvcwebtest.Facts
{
    public abstract class NHibernateFixtureBase
    {
        protected ISessionFactory SessionFactory { get; set; }
        private SchemaExport SchemaExport { get; set; }

        protected NHibernateFixtureBase(string assemblyWithMappings, string server, int port, string db,
            string username, string pwd)
        {
            CreateSessionFactory(assemblyWithMappings, BuildConnectionString(server, port, db, username, pwd));
        }

        private void CreateSessionFactory(string assemblyName, string connStr)
        {
            Configuration cfg = new Configuration();
            cfg.CurrentSessionContext<ThreadStaticSessionContext>();
            cfg.DataBaseIntegration(o =>
                {
                    o.ConnectionProvider<DriverConnectionProvider>();
                    o.Dialect<MySQLDialect>();
                    o.Driver<MySqlDataDriver>();
                    o.ConnectionString = connStr;
                    o.BatchSize = 100;
                });
            cfg.AddAssembly(assemblyName);

            SessionFactory = cfg.BuildSessionFactory();

            SchemaExport = new SchemaExport(cfg);
            SchemaExport.Create(false, true);
        }

        private static string BuildConnectionString(string server, int port, string db, string username, string pwd)
        {
            return string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};",
                server, port, db, username, pwd);
        }

        public virtual void Dispose()
        {
            SchemaExport.Drop(false, true);
        }
    }
}