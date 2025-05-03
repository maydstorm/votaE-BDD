using RestSharp;

namespace VotaE_BDD.Utils
{
    public static class RestClientFactory
    {
        public static RestClient Create()
        {
            var options = new RestClientOptions("https://vota-e-stg-abdra7guena2cub6.brazilsouth-01.azurewebsites.net/")
            {
                ThrowOnAnyError = false,
            };
            return new RestClient(options);
        }
    }
}