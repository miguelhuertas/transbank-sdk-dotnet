using System.Net.Http;
using Newtonsoft.Json;
using Transbank.Common;

namespace Transbank.WebpayRest.Oneclick.Requests
{
    public class MallCaptureRequest : BaseRequest
    {
        [JsonProperty("commerce_code")]
        internal int CommerceCode { get; set; }

        [JsonProperty("buy_order")]
        internal string BuyOrder { get; set; }

        [JsonProperty("capture_amount")]
        internal decimal Amount { get; set; }

        [JsonProperty("authorization_code")]
        internal string AuthorizationCode { get; set; }

        public MallCaptureRequest(int comerceCode, string buyOrder, decimal amount, string authorizationCode)
            : base($"/rswebpaytransaction/api/oneclick/v1.0/transactions/capture", HttpMethod.Put)
        {
            CommerceCode = comerceCode;
            BuyOrder = buyOrder;
            Amount = amount;
            AuthorizationCode = authorizationCode;
        }
    }
}