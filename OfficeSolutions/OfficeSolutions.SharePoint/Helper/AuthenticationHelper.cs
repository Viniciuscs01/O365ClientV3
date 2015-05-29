using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;

namespace OfficeSolutions.SharePoint
{
	public class AuthenticationHelper
	{
		public const string Authority = "https://login.windows.net/common";
		public static Uri returnUri = new Uri("http://<your redirect uri>/");
		public static string clientId = "<your client id>";
		public static AuthenticationContext authContext = null;
		public static string SharePointURL = "https://<your sharepoint tenant>.sharepoint<optional>.com/";


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

