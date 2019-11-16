using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Templates;

namespace UI.Client
{
    public class RestClient
    {
        //Create an HTTP client object
        static HttpClient httpClient = new HttpClient();

        static RestClient()
        {
            httpClient.BaseAddress = new Uri("https://moopblizkrieg.azurewebsites.net/");
        }

        internal static async Task<User> AuthPost(Card card)
        {
            User user = null;
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/auth", card);

            // return URI of the created resource.
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
                return user;
            }
            return user;
        }

        internal static async Task<ObservableCollection<Transaction>> GetTransfers()
        {
            ObservableCollection<Transaction> transfers = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/history");
            
            if (response.IsSuccessStatusCode)
            {
                transfers = await response.Content.ReadAsAsync<ObservableCollection<Transaction>>();
            }
            return transfers;
        }

    }
}