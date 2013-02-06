using System.Web.Configuration;
namespace LendingTreeWeb
{
	public class ConnectionStrings
	{
		public static string LendingTreeWebConnectionString= WebConfigurationManager.ConnectionStrings["LendingTreeWebConnectionString"].ConnectionString;
	}
}