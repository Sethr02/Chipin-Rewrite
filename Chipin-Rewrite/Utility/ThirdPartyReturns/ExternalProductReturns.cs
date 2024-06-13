using Chipin_Rewrite.Models.Entities;
using Chipin_Rewrite.Models.SignedModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Chipin_Rewrite.Utility.ThirdPartyReturns
{
    public class ExternalProductReturns : IExternalProductReturns
    {

        private readonly ChipinDbContext _context;
        private readonly ILogger<ExternalProductReturns> _logger;
        public ExternalProductReturns(ChipinDbContext context, ILogger<ExternalProductReturns> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Given a ProductListWallet object, this method will send a ReturnModel object via a post to the url.
        public async Task<bool> SendReturnModel(ProductListWallet productListWallet)
        {
            string chipinId = productListWallet.ChipinId;
            int productListWalletId = productListWallet.ProductListWalletId;

            BillingAddress billingAddress = await GetBillingAddress(chipinId);
            Address shippingAddress = await GetShippingAddress(chipinId);
            List<ExternalProduct> externalProducts = await GetExternalProducts(productListWalletId);

            //print all external products
            foreach (ExternalProduct externalProduct in externalProducts)
            {
                Console.WriteLine(externalProduct.ProductName);
                Console.WriteLine(externalProduct.ReturnUrl);
                Console.WriteLine(externalProduct.Price);
                Console.WriteLine(externalProduct.ProductId);
            }

            //group external products into a list where each list contains External Products with the same url


            if (billingAddress == null || shippingAddress == null || externalProducts == null)
            {
                _logger.LogInformation($"Billing : {billingAddress} \n Shipping : {shippingAddress} \n Products : {externalProducts}");
                return false;
            }

            List<List<ExternalProduct>> groupedExternalProducts = GroupExternalProducts(externalProducts);

            List<ReturnModel> returnModels = new List<ReturnModel>();

            foreach (List<ExternalProduct> group in groupedExternalProducts)
            {
                ReturnModel returnModel = new ReturnModel
                {
                    BillingAddress = billingAddress,
                    ShippingAddress = shippingAddress,
                    Products = group
                };
                returnModels.Add(returnModel);
            }

            int expectedReturns = returnModels.Count;
            int returnsSent = 0;

            foreach (ReturnModel returnModel in returnModels)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(returnModel, settings);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(returnModel.Products[0].ReturnUrl, data);
                if (response.IsSuccessStatusCode)
                {
                    returnsSent++;
                }


            }

            if (returnsSent == expectedReturns)
            {
                return true;
            }
            else
            {
                return false;
            }

            //post the returnModel to the url

        }

        //use a chipin_id to get the associated billing address and return it
        private async Task<BillingAddress> GetBillingAddress(string chipinId)
        {
            //find billing adress containing the ChipinId that maches the chipin_id paramater
            try
            {
                BillingAddress billingAddress = await _context.BillingAddresses.FirstAsync(x => x.ChipinId == chipinId);
                billingAddress.Chipin = null;
                return billingAddress;
            }
            catch
            {
                //if no billing address is found, return null
                return null;
            }


        }

        //use a ProductListWalletId to get the associated shipping address and return it
        private async Task<Address> GetShippingAddress(string chipinId)
        {
            //find shipping adress containing the ProductListWalletId that maches the productListWalletId paramater
            try
            {
                Address shippingAddress = await _context.Addresses.FirstAsync(x => x.ChipinId == chipinId);
                shippingAddress.Chipin = null;
                return shippingAddress;

            }
            catch
            {
                //if no shipping address is found, return null
                return null;
            }
        }

        //Group lists by ReturnUrl
        public List<List<ExternalProduct>> GroupExternalProducts(List<ExternalProduct> externalProducts)
        {
            List<List<ExternalProduct>> groupedExternalProducts = new List<List<ExternalProduct>>();
            foreach (ExternalProduct externalProduct in externalProducts)
            {
                bool found = false;
                foreach (List<ExternalProduct> groupedExternalProduct in groupedExternalProducts)
                {
                    if (groupedExternalProduct[0].ReturnUrl == externalProduct.ReturnUrl)
                    {
                        groupedExternalProduct.Add(externalProduct);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    List<ExternalProduct> newGroup = new List<ExternalProduct>();
                    newGroup.Add(externalProduct);
                    groupedExternalProducts.Add(newGroup);
                }
            }
            return groupedExternalProducts;
        }

        //use a ProductListWalletId to get list of associated ProductListItems which have an ExternalProductId and then return a list of ExternalProducts
        private async Task<List<ExternalProduct>> GetExternalProducts(int productListWalletId)
        {
            //find all ProductListItems containing the ProductListWalletId that maches the productListWalletId paramater
            try
            {
                List<ProductListItem> productListItemList = await _context.ProductListItems.Where(x => x.ProductListWalletId == productListWalletId).ToListAsync();
                List<ExternalProduct> externalProductList = new List<ExternalProduct>();
                foreach (ProductListItem productListItem in productListItemList)
                {
                    //find all ExternalProducts containing the ExternalProductId that maches the ExternalProductId of the ProductListItem
                    ExternalProduct externalProduct = await _context.ExternalProducts.FirstAsync(x => x.ExternalProductId == productListItem.ExternalProductId);
                    // externalProduct.ProductListItems = null;
                    externalProductList.Add(externalProduct);
                }
                return externalProductList;
            }
            catch
            {
                //if no ExternalProducts are found, return null
                return null;
            }
        }


    }
}
