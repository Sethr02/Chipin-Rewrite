using Chipin_Rewrite.Models.Entities;
using Newtonsoft.Json;

namespace Chipin_Rewrite.Utility.WooCommerce
{
    public class WooCommerceApiService
    {
        private readonly HttpClient _httpClient;

        public WooCommerceApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<WooCommerceCartInfo>> GetCartInfoAsync()
        {
            var response = await _httpClient.GetAsync("https://bestwayshop.co.za/wp-json/chipin/v1/cart");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<WooCommerceCartInfo>>(json);
            }

            return null;
        }
    }
}
