using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlietzkriegProject.Templates;
using Client_ServerConnector.Templates;

namespace Client_ServerConnector.Client
{
    public class RestClient
    {
        //Create an HTTP client object
        static HttpClient httpClient = new HttpClient();

        static RestClient()
        {
            httpClient.BaseAddress = new Uri("https://moopblizkrieg.azurewebsites.net/");
        }

        static async Task<User> AuthPost(string cardNumber, int pin)
        {
            User user = null;
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/auth", "{ \"UserCardNumber\": \""+cardNumber+ "\" }, \"CardPIN\": \""+pin+ "\" }");

            // return URI of the created resource.
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }

        static async Task<ObservableCollection<Transaction>> GetTransfers()
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