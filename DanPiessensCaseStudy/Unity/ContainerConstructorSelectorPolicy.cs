// Author : Shuo Zhang
// 
// Creation :2018-03-15 20:54

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Reflection;
  using Microsoft.Practices.ObjectBuilder2;
  using Microsoft.Practices.Unity;

  public class ContainerConstructorSelectorPolicy : IConstructorSelectorPolicy
  {
    public SelectedConstructor SelectConstructor(IBuilderContext context, IPolicyList resolverPolicyDestination)
    {
      IBuilderContext builderContext = context;
      if (builderContext == null)
        throw new ArgumentNullException(nameof(context));
      Type type = builderContext.BuildKey.Type;
      ConstructorInfo constructorInfo = FindInjectionConstructor(type);
      if ((object)constructorInfo == null)
        constructorInfo = FindLongestConstructor(type);
      ConstructorInfo ctor = constructorInfo;
      if (ctor != (ConstructorInfo)null)
        return this.CreateSelectedConstructor(context, ctor);
      return (SelectedConstructor)null;
    }

    private SelectedConstructor CreateSelectedConstructor(IBuilderContext context, ConstructorInfo ctor)
    {
      SelectedConstructor selectedConstructor = new SelectedConstructor(ctor);
      foreach (ParameterInfo parameter in ctor.GetParameters())
        selectedConstructor.AddParameterResolver(this.CreateResolver(context, ctor, parameter));
      return selectedConstructor;
    }

    protected IDependencyResolverPolicy CreateResolver(IBuilderContext context, ConstructorInfo ctor, ParameterInfo parameterInfo)
    {
      var list = parameterInfo.GetCustomAttributes(false).OfType<DependencyResolutionAttribute>().ToList<DependencyResolutionAttribute>();
      if (list.Count > 0)
        return list[0].CreateResolver(parameterInfo.ParameterType);

      ////Get the resolver
      var policy = context.Policies.Get<IParameterResolverPolicy>(
          new NamedTypeBuildKey(parameterInfo.ParameterType));
      if (policy != null)
      {
        return policy.CreateResolver(context.BuildKey.Type,
          parameterInfo);
      }

      return new FixedTypeResolverPolicy(parameterInfo.ParameterType);
    }

    private static ConstructorInfo FindInjectionConstructor(Type typeToConstruct)
    {
      ConstructorInfo[] array = typeToConstruct.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>)(c =>
      {
        if (!c.IsStatic && c.IsPublic)
          return c.IsDefined(typeof(InjectionConstructorAttribute), true);
        return false;
      })).ToArray<ConstructorInfo>();
      switch (array.Length)
      {
        case 0:
          return (ConstructorInfo)null;
        case 1:
          return array[0];
        default:
          throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The type {0} has multiple constructors marked with the InjectionConstructor attribute. Unable to disambiguate.", new object[1]
          {
            (object) typeToConstruct.GetTypeInfo().Name
          }));
      }
    }

    private static ConstructorInfo FindLongestConstructor(Type typeToConstruct)
    {
      ConstructorInfo[] array = typeToConstruct.GetTypeInfo().DeclaredConstructors.Where<ConstructorInfo>((Func<ConstructorInfo, bool>)(c =>
      {
        if (!c.IsStatic)
          return c.IsPublic;
        return false;
      })).ToArray<ConstructorInfo>();
      Array.Sort<ConstructorInfo>(array, (IComparer<ConstructorInfo>)new ConstructorLengthComparer());
      switch (array.Length)
      {
        case 0:
          return (ConstructorInfo)null;
        case 1:
          return array[0];
        default:
          int length = array[0].GetParameters().Length;
          if (array[1].GetParameters().Length == length)
            throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The type {0} has multiple constructors of length {1}. Unable to disambiguate.", new object[2]
            {
              (object) typeToConstruct.GetTypeInfo().Name,
              (object) length
            }));
          return array[0];
      }
    }

    private class ConstructorLengthComparer : IComparer<ConstructorInfo>
    {
      public int Compare(ConstructorInfo x, ConstructorInfo y)
      {
        ConstructorInfo constructorInfo1 = y;
        if ((object)constructorInfo1 == null)
          throw new ArgumentNullException(nameof(y));
        int length1 = constructorInfo1.GetParameters().Length;
        ConstructorInfo constructorInfo2 = x;
        if ((object)constructorInfo2 == null)
          throw new ArgumentNullException(nameof(x));
        int length2 = constructorInfo2.GetParameters().Length;
        return length1 - length2;
      }
    }
  }
}
