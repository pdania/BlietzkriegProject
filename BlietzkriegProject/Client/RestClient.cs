using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Templates;
using Newtonsoft.Json;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

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

        internal static async Task<ObservableCollection<Account>> GetAccounts()
        {
            ObservableCollection<Account> accounts = null;
            HttpResponseMessage response = await httpClient.GetAsync("api/balance");

            if (response.IsSuccessStatusCode)
            {
                accounts = await response.Content.ReadAsAsync<ObservableCollection<Account>>();
            }

            return accounts;
        }

        internal static async Task<HttpStatusCode> PutMoney(Money put)
        {
            var json = JsonConvert.SerializeObject(put);
            StringContent sc = new StringContent(JsonConvert.SerializeObject(put));
            sc.Headers.Remove("Content-Type"); // "{text/plain; charset=utf-8}"
            sc.Headers.Add("token", StationManager.CurrentUser.Token);
            sc.Headers.Add("Content-Type", "application/json");
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(
                "api /put", sc);
            return response.StatusCode;
        }
        internal static async Task<HttpStatusCode> WithdrawMoney(Money withdraw)
        {
            var json = JsonConvert.SerializeObject(withdraw);
            StringContent sc = new StringContent(JsonConvert.SerializeObject(withdraw));
            sc.Headers.Remove("Content-Type"); // "{text/plain; charset=utf-8}"
            sc.Headers.Add("token", StationManager.CurrentUser.Token);
            sc.Headers.Add("Content-Type", "application/json");
            HttpResponseMessage response = await httpClient.PutAsJsonAsync(
                "api /withdraw", sc);
            return response.StatusCode;
        }
    }
}