using EcsRx.Infrastructure.Ninject;
using EcsRx.Infrastructure.Extensions;
using System;
using EcsRx.Infrastructure;
using Ninject;
using Ninject.Injection;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Extensions;

namespace EcsRxStructTest
{
    class Program
    {
        static readonly IKernel _kernel;
        static IDependencyContainer _container;

        static Program()
        {
            var settings = new NinjectSettings();
            _kernel = new StandardKernel(settings);
            _kernel.Bind<IDependencyContainer>().To<NinjectDependencyContainer>().InScope(ctx => new NinjectDependencyContainer(_kernel));
        }

        static void Main(string[] args)
        {
            _container = _kernel.Get<IDependencyContainer>();

            var app = _container.Resolve<Application>();
            app.StartApplication();

            while (!System.Console.KeyAvailable)
            {
            }

            app.StopApplication();
        }
    }
}
