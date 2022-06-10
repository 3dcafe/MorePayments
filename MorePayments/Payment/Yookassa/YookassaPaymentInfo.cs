using System;

namespace MorePayments.Payment.Yookassa
{
    public class AuthorizationDetails
    {
        public string rrn { get; set; }
        public string auth_code { get; set; }
    }

    public class YookassaPaymentInfo
    {
        public string id { get; set; }
        public string status { get; set; }
        public YookassaAmount amount { get; set; }
        public string description { get; set; }
        public YookassaRecipient recipient { get; set; }
        public YookassaPaymentMethod payment_method { get; set; }
        public DateTime created_at { get; set; }
        public DateTime expires_at { get; set; }
        public bool test { get; set; }
        public bool paid { get; set; }
        public bool refundable { get; set; }
        public Metadata metadata { get; set; }
        public AuthorizationDetails authorization_details { get; set; }
    }
}
