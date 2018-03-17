using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DanPiessensCaseStudyTest
{
  using DanPiessensCaseStudy.Contracts;
  using DanPiessensCaseStudy.Data;
  using DanPiessensCaseStudy.Unity;
  using Microsoft.Practices.Unity;

  [TestClass]
  public class LoggerNameResolverTest
  {
    [TestMethod]
    public void ResolveTest()
    {
      var c = new UnityContainer();
      c.AddNewExtension<LoggerNameExtension>();
      c.RegisterType<ILogger, Logger>();

      var myclass = c.Resolve<MyClass>();
      Assert.AreEqual(((Logger)myclass.Logger).CtorLogSourceType, typeof(MyClass));
      Assert.AreEqual(((Logger)myclass.Logger).LogSourceType, typeof(MyClass));

      var myclass2 = c.Resolve<MyClass2>();
      Assert.AreEqual(((Logger)myclass2.Logger).CtorLogSourceType, typeof(MyClass2));
      Assert.AreEqual(((Logger)myclass2.Logger).LogSourceType, typeof(MyClass2));
    }
  }
}
