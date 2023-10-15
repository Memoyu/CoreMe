using Autofac;
using System.Reflection;

namespace CoreMe.Modules;

public class RepositoryModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        Assembly assemblyRepository = Assembly.Load("CoreMe.Infrastructure");
        builder.RegisterAssemblyTypes(assemblyRepository)
                .Where(a => a.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
    }
}
