using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace OfficeSolutions.SharePoint
{
	[Activity (Label = "OfficeSolutions.SharePoint", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : ListActivity
	{
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
		}

		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			var authResult = await AuthenticationHelper.GetAccessToken (AuthenticationHelper.SharePointURL, 
				new PlatformParameters (this));
			await CreateList (authResult.AccessToken);
			await CreateItems (authResult.AccessToken);
			await FetchListItems (authResult.AccessToken);
		}


		protected async Task<bool> CreateList(string token)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var mediaType = new MediaTypeWithQualityHeaderValue ("application/A");
			mediaType.Parameters.Add (new NameValueHeaderValue ("odata", "verbose"));
			client.DefaultRequestHeaders.Accept.Add (mediaType);
			var body = "{\"__metadata\":{\"type\":\"SP.List\"},\"AllowContentTypes\":true,\"BaseTemplate\":107,\"ContentTypesEnabled\":true,\"Description\":\"Tasks by Xamarin.Android\",\"Title\":\"TasksByAndroid\"}";
			var contents = new StringContent (body);
			contents.Headers.ContentType = MediaTypeHeaderValue.Parse( "application/json;odata=verbose");
			try {
				var postResult = await client.PostAsync ("https://classsolutions.sharepoint.com/sites/Vinicius/_api/web/lists/", contents);
				var result = postResult.EnsureSuccessStatusCode();
				Toast.MakeText (this, "List created successfully! Seeding tasks.", ToastLength.Long).Show();
				return true;
			} catch (Exception ex) {
				Toast.MakeText (this, "List already exists! Fetching tasks.", ToastLength.Long).Show();
				return false;

			}
		}

		protected async Task<bool> CreateItems(string token)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			var mediaType = new MediaTypeWithQualityHeaderValue ("application/json");
			mediaType.Parameters.Add (new NameValueHeaderValue ("odata", "verbose"));
			client.DefaultRequestHeaders.Accept.Add (mediaType);

			var itemToCreateTitle = "Item created on: " + DateTime.Now.ToString("dd/MM HH:mm");
			var body = "{\"__metadata\":{\"type\":\"SP.Data.TasksByAndroidListItem\"},\"Title\":\"" + itemToCreateTitle + "\",\"Status\": \"Not Started\"}";
			var contents = new StringContent (body);
			contents.Headers.ContentType = MediaTypeHeaderValue.Parse( "application/json;odata=verbose");
			try {

				var postResult = await client.PostAsync ("https://classsolutions.sharepoint.com/sites/Vinicius/_api/web/lists/GetByTitle('TasksByAndroid')/items", contents);
				var result = postResult.EnsureSuccessStatusCode();
				if(result.IsSuccessStatusCode)
					Toast.MakeText (this, "List item created successfully!", ToastLength.Long).Show();
				return true;
			} catch (Exception ex) {
				var msg = "Unable to create list item. " + ex.Message;
				Toast.MakeText (this, msg, ToastLength.Long).Show();
				return false;
			}
		}

		protected async Task<bool> FetchListItems(string token)
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var mediaType = new MediaTypeWithQualityHeaderValue ("application/json");
			mediaType.Parameters.Add (new NameValueHeaderValue ("odata", "verbose"));
			client.DefaultRequestHeaders.Accept.Add (mediaType);
			try {
				var result = await client.GetStringAsync("https://classsolutions.sharepoint.com/sites/Vinicius/_api/web/lists/GetByTitle('TasksByAndroid')/items");
				var data = JsonConvert.DeserializeObject<OfficeSolutions.SharePoint.Model.ListItemModels>(result);
				ListAdapter = new ListItemAdapter(this, data.D.Results);
				}

		 	catch (Exception ex) {
				var msg = "Unable to fetch list items. " + ex.Message;
				Toast.MakeText (this, msg, ToastLength.Long).Show();
			}
			return true;
		}

	}
}


