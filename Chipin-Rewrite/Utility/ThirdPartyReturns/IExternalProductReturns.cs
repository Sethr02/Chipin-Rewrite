using Chipin_Rewrite.Models.Entities;

namespace Chipin_Rewrite.Utility.ThirdPartyReturns
{
    public interface IExternalProductReturns
    {
        public Task<bool> SendReturnModel(ProductListWallet productListWallet);
    }
}
