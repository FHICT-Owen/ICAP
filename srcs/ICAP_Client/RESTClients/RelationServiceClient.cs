using System.Net.Http.Json;

namespace ICAP_Client.RESTClients
{
    public class RelationServiceClient(HttpClient httpClient, IConfiguration config)
    {
        private readonly string _serviceUrl = config["ServerUrl"] + "/relations" ?? throw new InvalidOperationException();

        
    }
}
