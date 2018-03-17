// Author : Shuo Zhang
// 
// Creation :2018-03-15 22:40

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using Microsoft.Practices.ObjectBuilder2;
  using Microsoft.Practices.Unity;

  public sealed class LoggerNameResolverPolicy : IDependencyResolverPolicy
  {
    private readonly Type _type;
    private readonly string _name;
    private readonly Type _logSourceType;

    public LoggerNameResolverPolicy(Type type, string name,
      Type logSourceType)
    {
      _type = type;
      _name = name;
      _logSourceType = logSourceType;
    }

    public object Resolve(IBuilderContext context)
    {
      var lifetimeManager = new ContainerControlledLifetimeManager();
      lifetimeManager.SetValue(_logSourceType);
      var loggerNameKey = new NamedTypeBuildKey(typeof(Type), "LogSourceType");

      //Create the build context for the logger
      var newKey = new NamedTypeBuildKey(_type, _name);

      //Register the item as a transient policy
      context.Policies.Set<IBuildKeyMappingPolicy>(new BuildKeyMappingPolicy(loggerNameKey), loggerNameKey);
      context.Policies.Set<ILifetimePolicy>(lifetimeManager, loggerNameKey);
      context.Lifetime.Add(lifetimeManager);

      try
      {
        return context.NewBuildUp(newKey);
      }
      finally
      {
        context.Lifetime.Remove(lifetimeManager);
        context.Policies.Clear<IBuildKeyMappingPolicy>(loggerNameKey);
        context.Policies.Clear<ILifetimePolicy>(loggerNameKey);
      }
    }
  }
}