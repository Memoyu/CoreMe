using Autofac;
using System.Reflection;

namespace CoreMe.Modules
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly assemblysRepository = Assembly.Load("CoreMe.Infrastructure");
            builder.RegisterAssemblyTypes(assemblysRepository)
                    .Where(a => a.Name.EndsWith("Repo"))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
        }
    }
}
