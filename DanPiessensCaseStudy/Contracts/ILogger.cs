// Author : Shuo Zhang
// 
// Creation :2018-03-15 20:53

namespace DanPiessensCaseStudy.Contracts
{
  public interface ILogger
  {
    void Debug(string format, params object[] args);
  }
}