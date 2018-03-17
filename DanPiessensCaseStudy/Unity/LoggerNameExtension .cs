// Author : Shuo Zhang
// 
// Creation :2018-03-15 20:53

namespace DanPiessensCaseStudy.Unity
{
  using DanPiessensCaseStudy.Contracts;
  using Microsoft.Practices.ObjectBuilder2;
  using Microsoft.Practices.Unity;
  using IConstructorSelectorPolicy = Microsoft.Practices.ObjectBuilder2.IConstructorSelectorPolicy;
  using NamedTypeBuildKey = Microsoft.Practices.ObjectBuilder2.NamedTypeBuildKey;

  public class LoggerNameExtension : UnityContainerExtension
  {
    protected override void Initialize()
    {
      // Override base Unity policies.
      Context.Policies.ClearDefault<IConstructorSelectorPolicy>();
      Context.Policies.SetDefault<IConstructorSelectorPolicy>(
        new ContainerConstructorSelectorPolicy());
      Context.Policies.ClearDefault<IPropertySelectorPolicy>();
      Context.Policies.SetDefault<IPropertySelectorPolicy>(
        new ContainerPropertySelectorPolicy());

      // Set logging specific policies
      var buildKey = new NamedTypeBuildKey(typeof(ILogger));
      Context.Policies.Set<IParameterResolverPolicy>(
        new LoggerNameConstuctorParameterPolicy(), buildKey);
      Context.Policies.Set<IPropertyResolverPolicy>(
        new LoggerNamePropertyPolicy(), buildKey);
    }
  }
}