using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MorePayments.Payment.Yookassa
{
    public class YookassaService
    {
        private readonly int? _shopId;
        private readonly string? _secretKey;
        public YookassaService(int shopId, string secretKey)
        {
            this._shopId = shopId;
            this._secretKey = secretKey;
        }
        /// <summary>
        /// Create payment and get link for payment
        /// </summary>
        /// <param name="data"></param>
        /// <param name="IdempotenceKey"></param>
        /// <returns></returns>
        public async Task<YookassaResponse> CreatePaymentAsync(YookassaPayment data,string IdempotenceKey)
        {
            HttpSimpleClientRepository client = new(_shopId.ToString(), _secretKey);
            client.Headers.Add("Idempotence-Key", IdempotenceKey);
            string url = $"https://api.yookassa.ru/v3/payments";
            var obj = await client.PostAsync<YookassaResponse>(url, data);
            if (obj == null) return null;
            if (obj.Response == null) return null;
            if (obj.Response == null) return null;
            return obj.Response;
        }
    }
}
