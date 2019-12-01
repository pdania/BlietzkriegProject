using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
                httpClient.DefaultRequestHeaders.Add("token", user.Token);
                return user;
            }
            return null;
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

        internal static async Task<HttpStatusCode> PutMoney(MoneyTo put)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/put", put);
            return response.StatusCode;
        }
        internal static async Task<HttpStatusCode> WithdrawMoney(MoneyFrom withdraw)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/withdraw", withdraw);
            return response.StatusCode;
        }
        internal static async Task<HttpStatusCode> TransferMoney(TranferInput transfer)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(
                "api/transfer", transfer);
            return response.StatusCode;
        }
    }
}