using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Payment.Alfabank
{
    public class AlfabankService
    {
        public const string ReturnUrlParamNameMerchantOrder = "merchantorderid";
        public const string ReturnUrlParamNameAlfaOrder = "orderId";

        private const string TestUrl = "https://web.rbsuat.com/ab/rest/";

        private const string GeneralUrl = "https://pay.alfabank.ru/payment/rest/";

        private readonly string _userName;
        private readonly string _password;
        private readonly string _merchantLogin;
        private readonly bool _useTestMode;

        public AlfabankService(string userName, string password, string merchantLogin, string useTestMode)
        {
            _userName = userName;
            _password = password;
            _merchantLogin = merchantLogin;
            _useTestMode = useTestMode.TryParseBool(true) ?? true;
        }

        /// <summary>
        /// Регистрация заказа
        /// </summary>
        public AlfabankRegisterResponse Register(Order order, string description, Func<OrderCurrency, float> getCurrencyRate, string returnUrl, string failUrl)
        {
            var sum = (int)(Math.Round(order.Sum * getCurrencyRate(order.OrderCurrency), 2) * 100);

            int retriesNum = 0;
            string orderStrId;
            bool success = false;
            AlfabankRegisterResponse response;

            do
            {
                // если заказ уже есть в сбербанке, но был изменен на стороне магазина, подменяем id на id_номерпопытки
                orderStrId = retriesNum > 0
                    ? string.Format("{0}_{1}", order.OrderID, retriesNum)
                    : order.OrderID.ToString();

                var data = new Dictionary<string, string>()
                {
                    {"userName", _userName},
                    {"password", _password},
                    {"orderNumber", orderStrId},
                    {"amount", sum.ToString(CultureInfo.InvariantCulture)},    // Сумма платежа в копейках (или центах)
                    //{"currency", ""}, // ISO 4217
                    {"returnUrl", string.Format("{0}{1}", returnUrl, string.Format("{0}{1}={2}", (returnUrl.Contains("?") ? "&" : "?"), ReturnUrlParamNameMerchantOrder, HttpUtility.UrlEncode(order.Number)))},
                    {"failUrl", failUrl},
                    //{"pageView", "DESKTOP"}, // "MOBILE"
                    {"clientId", order.OrderCustomer.CustomerID.ToString()},
                    //{"bindingId", "" }                // Идентификатор связки, созданной ранее. Может использоваться, только если у магазина есть разрешение на работу со связками. Если этот параметр передаётся в данном запросе, то это означает: 1. Данный заказ может быть оплачен только с помощью связки; 2. Плательщик будет перенаправлен на платёжную страницу, где требуется только ввод CVC.
                };

                if (!string.IsNullOrEmpty(_merchantLogin))
                    data.Add("merchantLogin", _merchantLogin);  // Чтобы зарегистрировать заказ от имени дочернего мерчанта, укажите его логин в этом параметре.


                response = MakeRequest<AlfabankRegisterResponse>("register.do", data);

                if (response == null)
                    return null;

                success = response.ErrorCode == 0;

                if (!success)
                {
                    Debug.Log.Info(string.Format("AlfabankService Register. code: {0} error: {1}, obj: {2}",
                                                    response.ErrorCode, response.ErrorMessage, JsonConvert.SerializeObject(response)));
                }
                retriesNum++;
            } while (response.ErrorCode == 1 && retriesNum < 20);

            return success ? response : null;
        }
    }
}
