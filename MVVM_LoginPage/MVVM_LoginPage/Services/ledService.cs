using MVVM_LoginPage.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM_LoginPage.Services
{
    public class ledService : ILedService
    {
        HttpClient client;
        JsonSerializerOptions serializerOptions;

        public List<LedModel> ledList { get; private set; }
        public ledService()
        {
            client = new HttpClient();
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<LedModel>> GetLedData()
        {
            string base_url = "http://ltnc-api.somee.com/api/tbled/getall";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://ltnc-api.somee.com/api/tbled/getall";
            }
            ledList = new List<LedModel>();

            Uri uri = new Uri(string.Format(base_url, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ledList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LedModel>>(content);
                    return ledList;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public async Task<bool> CreateLed(LedModel led)
        {
            string base_url = "http://10.0.2.2/ledapi/api/tblleds";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://localhost/ledapi/api/tblleds";
            }
            Uri uri = new Uri(string.Format(base_url, string.Empty));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<LedModel>(led, serializerOptions);
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
        public async Task<bool> RefreshLed(int id, LedModel led)
        {
            string base_url = "http://ltnc-api.somee.com/api/tbled/put";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://ltnc-api.somee.com/api/tbled/put";
            }
            Uri uri = new Uri(string.Format(base_url, id));
            try
            {

                string json = System.Text.Json.JsonSerializer.Serialize<LedModel>(led, serializerOptions);
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
        public async Task<bool> DeleteLed(int id)
        {
            string base_url = "http://10.0.2.2/ledapi/api/tblleds/{0}";
            if (Device.RuntimePlatform != Device.Android)
            {
                base_url = "http://localhost/ledapi/api/tblleds/{0}";
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
    }
}
