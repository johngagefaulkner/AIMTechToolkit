using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace AIMTechToolkit.Services
{
	internal static class MicrosoftGraphAPIService
	{
		public static async Task<MicrosoftAccountProfileInfo> GetProfileAsync()
		{
			try
			{
				if (!MicrosoftLoginService.IsUserLoggedIn)
				{
					return null;
				}

				// Call Microsoft Graph using the access token acquired above.
				using var graphRequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
				graphRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", MicrosoftLoginService.GetAccessToken());
				var graphResponseMessage = await HttpService.InstanceHttpClient.SendAsync(graphRequest);
				graphResponseMessage.EnsureSuccessStatusCode();

				// Present the results to the user (formatting the json for readability)
				using var graphResponseJson = JsonDocument.Parse(await graphResponseMessage.Content.ReadAsStreamAsync());
				var accountInfoJson = JsonSerializer.Serialize(graphResponseJson, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

				Log.Info($"Microsoft Graph Response: Successfully retrieve user's profile information.");
				return MicrosoftAccountProfileInfo.FromJson(accountInfoJson);
			}

			catch (Exception ex)
			{
				Log.Error($"Microsoft Graph API Error: {ex.Message}");
				return null;
			}
		}

		public static async Task<IRandomAccessStream> GetUserPhotoAsStreamAsync(string userId)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users/{userId}/photo/$value");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", MicrosoftLoginService.GetAccessToken());

			using var response = await new HttpClient().SendAsync(request);
			response.EnsureSuccessStatusCode();

			var stream = new InMemoryRandomAccessStream();
			var bytes = await response.Content.ReadAsByteArrayAsync();
			var writer = new DataWriter(stream.GetOutputStreamAt(0));
			writer.WriteBytes(bytes);
			writer.StoreAsync().GetResults();
			stream.Seek(0);

			return stream;
		}


		public static async Task<string> GetUserPhotoAsBase64StringAsync(string userId)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users/{userId}/photo/$value");
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", MicrosoftLoginService.GetAccessToken());

			using var response = await new HttpClient().SendAsync(request);
			response.EnsureSuccessStatusCode();

			var bytes = await response.Content.ReadAsByteArrayAsync();
			var base64 = Convert.ToBase64String(bytes);
			return base64;
		}

		public static string ConvertToBase64String(byte[] bytes) => Convert.ToBase64String(bytes);
	}
}
