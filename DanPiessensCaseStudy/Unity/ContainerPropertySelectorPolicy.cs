// Author : Shuo Zhang
// 
// Creation :2018-03-16 09:52

namespace DanPiessensCaseStudy.Unity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Microsoft.Practices.ObjectBuilder2;
  using Microsoft.Practices.Unity;
  using Microsoft.Practices.Unity.Utility;

  public class ContainerPropertySelectorPolicy : IPropertySelectorPolicy
  {
    /// <summary>
    /// Returns sequence of properties on the given type that
    /// should be set as part of building that object.
    /// </summary>
    /// <param name="context">Current build context.</param>
    /// <param name="resolverPolicyDestination">The <see cref="T:Microsoft.Practices.ObjectBuilder2.IPolicyList" /> to add any
    /// generated resolver objects into.</param>
    /// <returns>Sequence of <see cref="T:System.Reflection.PropertyInfo" /> objects
    /// that contain the properties to set.</returns>
    public virtual IEnumerable<SelectedProperty> SelectProperties(IBuilderContext context, IPolicyList resolverPolicyDestination)
    {
      Type t = context.BuildKey.Type;
      foreach (PropertyInfo propertyInfo in t.GetPropertiesHierarchical().Where<PropertyInfo>((Func<PropertyInfo, bool>)(p => p.CanWrite)))
      {
        MethodInfo propertyMethod = propertyInfo.SetMethod ?? propertyInfo.GetMethod;
        if (!propertyMethod.IsStatic && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.IsDefined(typeof(DependencyAttribute), false))
          yield return this.CreateSelectedProperty(context, propertyInfo);
      }
    }

    private SelectedProperty CreateSelectedProperty(IBuilderContext context, PropertyInfo property)
    {
      IDependencyResolverPolicy resolver = this.CreateResolver(context, property);
      return new SelectedProperty(property, resolver);
    }

    /// <summary>
    /// Create a <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy" /> for the given
    /// property.
    /// </summary>
    /// <param name="property">Property to create resolver for.</param>
    /// <returns>The resolver object.</returns>
    protected IDependencyResolverPolicy CreateResolver(IBuilderContext context, PropertyInfo property)
    {
      var policy = context.Policies.Get<IPropertyResolverPolicy>(
          new NamedTypeBuildKey(property.PropertyType));

      if (policy != null)
      {
        return policy.CreateResolver(context.BuildKey.Type,property);
      }

      return property.GetCustomAttributes(typeof(DependencyResolutionAttribute), false).OfType<DependencyResolutionAttribute>().ToList<DependencyResolutionAttribute>()[0].CreateResolver(property.PropertyType);
    }
  }
}
