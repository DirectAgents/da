
using System.Xml.Linq;
namespace LendingTreeLib.Common
{
    public interface ILogger
    {
        void Log(XElement xElement, EEventType eEventType);
    }
}
