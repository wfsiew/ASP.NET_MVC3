﻿using System;
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

        protected NHibernateFixtureBase(string assemblyWithMappings)
        {
            CreateSessionFactory(assemblyWithMappings, SQLiteConnectionString());
        }

        private void CreateSessionFactory(string assemblyName, string connStr)
        {
            Configuration cfg = new Configuration();
            cfg.CurrentSessionContext<ThreadStaticSessionContext>();
            cfg.DataBaseIntegration(o =>
                {
                    o.ConnectionProvider<DriverConnectionProvider>();
                    o.Dialect<SQLiteDialect>();
                    o.Driver<SQLite20Driver>();
                    o.ConnectionString = connStr;
                    o.BatchSize = 100;
                });
            cfg.AddAssembly(assemblyName);

            SessionFactory = cfg.BuildSessionFactory();

            SchemaExport = new SchemaExport(cfg);
            SchemaExport.Create(false, true);
        }

        private static string SQLiteConnectionString()
        {
            return "Data Source=test.db;Version=3";
        }

        public virtual void Dispose()
        {
            SchemaExport.Drop(false, true);
        }
    }
}