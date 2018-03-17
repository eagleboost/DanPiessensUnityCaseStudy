// Author : Shuo Zhang
// 
// Creation :2018-03-15 20:58

namespace DanPiessensCaseStudy.Data
{
  using DanPiessensCaseStudy.Contracts;
  using Microsoft.Practices.Unity;

  public class MyClass
  {
    private readonly ILogger _logger;

    public MyClass(ILogger logger)
    {
      _logger = logger;
    }

    public ILogger Logger
    {
      get { return _logger; }
    }

    public void SayHello(string name)
    {
      // Logger name here will be 'DanPiessensCaseStudy.Data.MyClass'
      _logger.Debug("Saying Hello to {0}", name);
    }
  }

  public class MyClass2
  {
    [Dependency]
    public ILogger Logger { get; set; }

    public void SayHello(string name)
    {
      // Logger name here will be 'DanPiessensCaseStudy.Data.MyClass2'
      Logger.Debug("Saying Hello to {0}", name);
    }
  }
}