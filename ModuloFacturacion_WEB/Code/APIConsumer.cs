namespace ModuloFacturacion_WEB.Code
{
    public class APIConsumer
    {
        public static Models.FactClient[] Clients(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactClient[]>(json);
        }

        public static Models.FactClient Client(string apiUrl, string id)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl + "/" + id);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactClient>(json);
        }

        public static void SaveClient(string apiUrl, string id, Models.FactClient client)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(client);
            api.UploadString(apiUrl + "/" + id, "PUT", json);
        }

        public static Models.FactClient CreateClient(string apiUrl, Models.FactClient client)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(client);
            json = api.UploadString(apiUrl, "POST", json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactClient>(json);
        }

        public static void DeleteClient(string apiUrl, string id)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            api.UploadString(apiUrl + "/" + id, "DELETE", "");
        }

        public static Models.FactPayType[] PayTypes(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactPayType[]>(json);
        }

        public static Models.FactInvoiceHead[] InvoiceHead(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactInvoiceHead[]>(json);
        }
    }
}
