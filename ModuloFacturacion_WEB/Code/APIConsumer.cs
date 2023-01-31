using ModuloFacturacion_WEB.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;

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

        public static Models.FactClient[] Clients_SearchFor(string apiUrl, string? searchFor)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("content-type", "application/json");
            api.Headers.Add("Accept", "application/json");
            var json = api.DownloadString(apiUrl + "?searchFor=" + searchFor);
            var datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactClient[]>(json);
            return datos;
        }

        public static Models.FactClient ClienteElegido(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactClient>(json);
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
        public static void SaveFactInvoiceHead(string apiUrl, int id, Models.FactInvoiceHead invoice)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(invoice);
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
        public static Models.FactInvoiceHead InvoiceHead(string apiUrl, int id)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl + "/" + id);
            return JsonConvert.DeserializeObject<List<FactInvoiceHead>>(json)[0];
            //return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactInvoiceHead>(json);
        }

        public static Models.Product[] Productos(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Product[]>(json);
        }

        public static Models.Product ProductoElegido(string apiUrl)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = api.DownloadString(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Product>(json);
        }

        public static int InsertFactInvoiceHead(string apiUrl, FactInvoiceHead data)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            json = api.UploadString(apiUrl, "POST", json);
            var text = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactInvoiceHead>(json);
            return textFactId(text);    
        }

        public static FactInvoiceDetail InsertFactInvoiceDetail(string apiUrl, FactInvoiceDetail data)
        {
            var api = new System.Net.WebClient();
            api.Headers.Add("Content-Type", "application/json");
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            json = api.UploadString(apiUrl, "POST", json);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Models.FactInvoiceDetail>(json);
        }

        private static int textFactId(FactInvoiceHead resp)
        {
            return resp.InvoiceHeadId;
        }

    }
}
