using System;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Context;

namespace mvcweb.App
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class NHibernateSessionAttribute : ActionFilterAttribute
    {
        public NHibernateSessionAttribute()
        {
            Order = 100;
        }

        protected ISessionFactory SessionFactory
        {
            get
            {
                return NHibernateUtils.SessionFactory;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ISession s = NHibernateUtils.OpenSession();
            CurrentSessionContext.Bind(s);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ISession s = CurrentSessionContext.Unbind(SessionFactory);
            s.Close();
        }
    }
}