using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanPiessensCaseStudy
{
  using DanPiessensCaseStudy.Contracts;
  using DanPiessensCaseStudy.Data;
  using Microsoft.Practices.Unity;
  using Unity;

  class Program
  {
    static void Main(string[] args)
    {
      var c = new UnityContainer();
      c.AddNewExtension<LoggerNameExtension>();
      c.RegisterType<ILogger, Logger>();

      var myclass = c.Resolve<MyClass>();
      myclass.SayHello("Unity");

      var myclass2 = c.Resolve<MyClass2>();
      myclass2.SayHello("Unity");

      Console.ReadKey();
    }
  }
}
