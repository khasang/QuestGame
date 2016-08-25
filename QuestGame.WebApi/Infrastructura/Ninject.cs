using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using QuestGame.Domain.Interfaces;
using QuestGame.Domain.Implementations;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace QuestGame.WebApi.Infrastructura
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        public NinjectDependencyScope(IResolutionRoot resolver)
        {
            this.resolver = resolver;
        }

        public void Dispose()
        {
            var disposable = this.resolver as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            this.resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (this.resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return this.resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this.resolver == null)
            {
                throw new ObjectDisposedException("this", "This scope has already been disposed");
            }

            return this.resolver.GetAll(serviceType);
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
                : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel.BeginBlock());
        }
    }

    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataManager>().To<EFDataManager>().InSingletonScope();
        }
    }
}