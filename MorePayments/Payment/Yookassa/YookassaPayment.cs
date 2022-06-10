using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Payment.Yookassa
{
    public class YookassaAmount
    {
        public string value { get; set; }
        public string currency { get; set; }
    }

    public class YookassaConfirmation
    {
        public string type { get; set; }
        public string return_url { get; set; }
    }

    public class YookassaPaymentMethodData
    {
        public string type { get; set; }
    }

    public class YookassaPayment
    {
        public YookassaAmount amount { get; set; }
        public YookassaPaymentMethodData payment_method_data { get; set; }
        public YookassaConfirmation confirmation { get; set; }
        public string description { get; set; }
    }
}
