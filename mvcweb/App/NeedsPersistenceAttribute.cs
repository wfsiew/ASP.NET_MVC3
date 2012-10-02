using System;
using System.Web;
using System.Web.Mvc;
using NHibernate;

namespace mvcweb.App
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true)]
    public class NeedsPersistenceAttribute : NHibernateSessionAttribute
    {
        protected ISession Session
        {
            get
            {
                return SessionFactory.GetCurrentSession();
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Session.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ITransaction tx = Session.Transaction;
            if (tx != null && tx.IsActive)
                tx.Commit();

            base.OnActionExecuted(filterContext);
        }
    }
}