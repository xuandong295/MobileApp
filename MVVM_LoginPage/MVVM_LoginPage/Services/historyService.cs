using MVVM_LoginPage.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM_LoginPage.Services
{
    class historyService : IHistoryService
    {
        HttpClient client;
        JsonSerializerOptions serializerOptions;
        public List<HistoryModel> historyList { get; private set; }
        public historyService()
        {
            client = new HttpClient();
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        
        public async Task<bool> CreateHistory(HistoryModel history)
        {
            string base_url = "http://ltnc-api.somee.com/api/tbhistory/post";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://ltnc-api.somee.com/api/tbhistory/post";
            }
            Uri uri = new Uri(string.Format(base_url, string.Empty));
            try
            {

                string json = JsonSerializer.Serialize<HistoryModel>(history, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);


                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteHistory(int id)
        {
            string base_url = "";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "";
            }
            Uri uri = new Uri(string.Format(base_url, id));

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<List<HistoryModel>> GetHistoryData()
        {
            string base_url = "http://ltnc-api.somee.com/api/tbled/getall";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://ltnc-api.somee.com/api/tbled/getall";
            }

            Uri uri = new Uri(string.Format(base_url, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    historyList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HistoryModel>>(content);
                    return historyList;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<bool> RefreshHistory(int id, HistoryModel history)
        {
            string base_url = "";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "";
            }
            Uri uri = new Uri(string.Format(base_url, id));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<HistoryModel>(history, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                return await Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return await Task.FromResult(false);
            }
        }
    }
}
