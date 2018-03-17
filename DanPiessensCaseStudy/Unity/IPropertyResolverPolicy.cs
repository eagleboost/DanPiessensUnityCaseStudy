// Author : Shuo Zhang
// 
// Creation :2018-03-16 09:50

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using System.Reflection;
  using Microsoft.Practices.ObjectBuilder2;

  public interface IPropertyResolverPolicy : IBuilderPolicy
  {
    IDependencyResolverPolicy CreateResolver(Type currentType, PropertyInfo property);
  }
}