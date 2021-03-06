using System;
using System.Collections.Generic;
using System.Text;

namespace MorePayments.Payment.Yookassa
{
#warning выпилить методату отсюда
    public class Metadata
    {
    }


    public class YookassaResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public YookassaAmount amount { get; set; }
        public string description { get; set; }
        public YookassaRecipient recipient { get; set; }
        public YookassaPaymentMethod payment_method { get; set; }
        public DateTime created_at { get; set; }
        public YookassaConfirmation confirmation { get; set; }
        public bool test { get; set; }
        public bool paid { get; set; }
        public bool refundable { get; set; }
        public Metadata metadata { get; set; }
    }
}
