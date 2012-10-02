using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using mvcweb.Repositories;

namespace mvcweb.Factory
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, System.Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<ICarRepository>().To<CarRepository>();
        }
    }
}