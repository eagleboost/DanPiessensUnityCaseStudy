// Author : Shuo Zhang
// 
// Creation :2018-03-15 21:00

namespace DanPiessensCaseStudy.Data
{
  using System;
  using DanPiessensCaseStudy.Contracts;
  using Microsoft.Practices.Unity;

  public class Logger : ILogger
  {
    private readonly string _namespaceFormat;

    ////LoggerNameResolverPolicy would register a temp item with a NamedTypeBuildKey with Name=LogSourceType
    ////this [Dependency("LogSourceType")] would ask that item from the Unity Container
    public Logger([Dependency("LogSourceType")]Type logSourceType)
    {
      CtorLogSourceType = logSourceType;
      _namespaceFormat = logSourceType.FullName + " : {0}";
    }

    /// <summary>
    /// For demo purpose, the LogSourceType would also be injected to this property
    /// </summary>
    [Dependency("LogSourceType")]
    public Type LogSourceType { get; set; }
    
    public Type CtorLogSourceType { get; private set; }

    public void Debug(string format, params object[] args)
    {
      var log = string.Format(format, args);
      Console.WriteLine(_namespaceFormat, log);
    }
  }
}