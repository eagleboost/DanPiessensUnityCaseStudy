// Author : Shuo Zhang
// 
// Creation :2018-03-15 20:54

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using System.Reflection;
  using Microsoft.Practices.ObjectBuilder2;

  public class LoggerNameConstuctorParameterPolicy : IParameterResolverPolicy
  {
    public IDependencyResolverPolicy CreateResolver(
      Type currentType, ParameterInfo param)
    {
      return new LoggerNameResolverPolicy(param.ParameterType, null,currentType);
    }
  }
}
