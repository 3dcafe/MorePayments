using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Payment.Yookassa
{
    public class Metadata
    {
    }

    public class PaymentMethod
    {
        public string type { get; set; }
        public string id { get; set; }
        public bool saved { get; set; }
    }

    public class Recipient
    {
        public string account_id { get; set; }
        public string gateway_id { get; set; }
    }

    public class YookassaResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public YookassaAmount amount { get; set; }
        public string description { get; set; }
        public Recipient recipient { get; set; }
        public PaymentMethod payment_method { get; set; }
        public DateTime created_at { get; set; }
        public YookassaConfirmation confirmation { get; set; }
        public bool test { get; set; }
        public bool paid { get; set; }
        public bool refundable { get; set; }
        public Metadata metadata { get; set; }
    }
}
