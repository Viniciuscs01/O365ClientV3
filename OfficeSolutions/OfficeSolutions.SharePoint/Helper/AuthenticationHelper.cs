using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;

namespace OfficeSolutions.SharePoint
{
	public class AuthenticationHelper
	{
		public const string Authority = "https://login.windows.net/common";
		public static Uri returnUri = new Uri("http://xam-demo-redirect");
		public static string clientId = "9e9ab416-f20b-4e26-9b24-d6bd3c1882ce";
		public static AuthenticationContext authContext = null;
		public static string SharePointURL = "https://classsolutions.sharepoint.com/";


		public static async Task<AuthenticationResult> GetAccessToken(string serviceResourceId, PlatformParameters param)
		{
			authContext = new AuthenticationContext(Authority);
			if (authContext.TokenCache.ReadItems ().Any ())
				authContext = new AuthenticationContext (authContext.TokenCache.ReadItems ().First ().Authority);
			var authResult = await authContext.AcquireTokenAsync(serviceResourceId, clientId, returnUri, param);
			return authResult;
		}
	}
}

