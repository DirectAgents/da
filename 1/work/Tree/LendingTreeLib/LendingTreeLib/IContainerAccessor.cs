using Microsoft.Practices.Unity;

namespace LendingTreeLib
{
    /// <summary>
    /// This interface supports using the Unity container for DI with ASP.NET
    /// See: http://blogs.microsoft.co.il/blogs/gilf/archive/2008/07/01/how-to-use-unity-container-in-asp-net.aspx
    /// </summary>
    public interface IContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}
