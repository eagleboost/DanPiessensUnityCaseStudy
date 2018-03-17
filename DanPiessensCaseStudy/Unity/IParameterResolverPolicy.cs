// Author : Shuo Zhang
// 
// Creation :2018-03-15 22:48

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using System.Reflection;
  using Microsoft.Practices.ObjectBuilder2;

  public interface IParameterResolverPolicy : IBuilderPolicy
  {
    IDependencyResolverPolicy CreateResolver(Type currentType, ParameterInfo param);
  }
}