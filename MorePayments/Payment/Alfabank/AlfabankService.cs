using MorePayments.Diagnostics;
using MorePayments.Extensions;
using MorePayments.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;

namespace MorePayments.Payment.Alfabank
{
    /// <summary>
    /// Сервис(Репозиторий) для работы с Альфа банком
    /// Документация - https://pay.alfabank.ru/ecommerce/instructions/merchantManual/pages/prod_environment.html
    /// </summary>
    public class AlfabankService: IPayment
    {
        /// <summary>
        /// Если в заказе передать дополнительный параметр с именем merchantOrderId, то именно его значение будет передано в
        /// процессинг в качестве номера заказа(вместо значения поля orderNumber).
        /// </summary>
        public const string ReturnUrlParamNameMerchantOrder = "merchantorderid";
        /// <summary>
        /// Номер заказа в платежной системе. Уникален в пределах системы. Отсутствует если регистрация заказа на
        ///  удалась по причине ошибки, детализированной в errorCode
        /// </summary>
        public const string ReturnUrlParamNameAlfaOrder = "orderId";
        /// <summary>
        /// URL тестового контура
        /// </summary>
        private const string TestUrl = "https://web.rbsuat.com/ab/rest/";
        /// <summary>
        /// Основной URL с боевыми платежами
        /// </summary>
        private const string GeneralUrl = "https://pay.alfabank.ru/payment/rest/";
        /// <summary>
        /// Имя пользователя
        /// </summary>
        private readonly string _userName;
        /// <summary>
        /// Пароль
        /// </summary>
        private readonly string _password;
        /// <summary>
        /// Логин
        /// </summary>
        private readonly string _merchantLogin;
        /// <summary>
        /// В тестовом режиме ?
        /// </summary>
        private readonly bool _useTestMode;
        /// <summary>
        /// Базовый конструктор
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="merchantLogin"></param>
        /// <param name="useTestMode"></param>
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
        public AlfabankRegisterResponse Register(Order order, string description, string returnUrl, string failUrl)
        {
            var sum = (int)(Math.Round(order.Sum, 2) * 100);

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
#warning рефакторить
                var data = new Dictionary<string, string>()
                {
                    {"userName", _userName},
                    {"password", _password},
                    {"orderNumber", orderStrId},
                    {"amount", sum.ToString(CultureInfo.InvariantCulture)},    // Сумма платежа в копейках (или центах)
                    //{"currency", ""}, // ISO 4217
                    {"returnUrl", string.Format("{0}{1}", returnUrl, 
                                  string.Format("{0}{1}={2}", (returnUrl.Contains("?") ? "&" : "?"), 
                                                ReturnUrlParamNameMerchantOrder, HttpUtility.UrlEncode(order.Number.ToString())))},
                    {"failUrl", failUrl},
                    //{"pageView", "DESKTOP"}, // "MOBILE"
                    {"clientId", order.userId},
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
                                                    response.ErrorCode, response.ErrorMessage, JsonSerializer.Serialize(response)));
                }
                retriesNum++;
            } while (response.ErrorCode == 1 && retriesNum < 20);

            return success ? response : null;
        }
        /// <summary>
        /// Запрос состояния заказа
        /// </summary>
        public AlfabankOrderStatusResponse GetOrderStatus(string alfaOrderId, string merchantOrderid)
        {
            var data = new Dictionary<string, string>()
            {
                {"userName", _userName},
                {"password", _password},
            };

            if (!string.IsNullOrEmpty(alfaOrderId))
                data.Add("orderId", alfaOrderId);

            if (!string.IsNullOrEmpty(merchantOrderid))
                data.Add("merchantOrderNumber", merchantOrderid);

            var response = MakeRequest<AlfabankOrderStatusResponse>("getOrderStatusExtended.do", data);

            if (response == null)
                return null;

            var success = response.ErrorCode == 0;

            if (!success)
            {
                Debug.Log.Info(string.Format("AlfabankService GetOrderStatus. code: {0} error: {1}, obj: {2}",
                                                response.ErrorCode, response.ErrorMessage, JsonSerializer.Serialize(response)));
            }

            return response;
        }
        #region Private methods

        private T MakeRequest<T>(string url, Dictionary<string, string> data = null) where T : class
        {
            try
            {
                var request = WebRequest.Create((_useTestMode ? TestUrl : GeneralUrl) + url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (data != null)
                {
                    string dataPost = "";
                    foreach (var key in data.Keys)
                    {
                        var value = data[key];

                        if (string.IsNullOrEmpty(value))
                            continue;

                        if (dataPost != "")
                            dataPost += "&";

                        dataPost += key + "=" + HttpUtility.UrlEncode(value);
                    }

                    byte[] bytes = Encoding.UTF8.GetBytes(dataPost);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }

                var dataAnswer = JsonSerializer.Deserialize<T>(responseContent);

                return dataAnswer;
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    Debug.Log.Error(error, ex);
                                }
                            else
                                Debug.Log.Error(ex);
                    }
                    else
                        Debug.Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        #endregion
    }
}
