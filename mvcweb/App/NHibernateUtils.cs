using NHibernate;
using NHibernate.Cfg;
using System.Reflection;

namespace mvcweb.App
{
    public class NHibernateUtils
    {
        private static ISessionFactory sessionFactory;
        private static bool startUpComplete = false;
        private static readonly object locker = new object();

        public static ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory;
            }

            set
            {
                sessionFactory = value;
            }
        }

        public static ISession CurrentSession
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void EnsureStartup()
        {
            if (!startUpComplete)
            {
                lock (locker)
                {
                    if (!startUpComplete)
                    {
                        PerformStartup();
                        startUpComplete = true;
                    }
                }
            }
        }

        public static Configuration BuildConfiguration()
        {
            Configuration cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(Assembly.GetExecutingAssembly());

            return cfg;
        }

        private static void PerformStartup()
        {
            InitSessionFactory();
        }

        private static void InitSessionFactory()
        {
            Configuration cfg = BuildConfiguration();
            sessionFactory = cfg.BuildSessionFactory();
        }
    }
}