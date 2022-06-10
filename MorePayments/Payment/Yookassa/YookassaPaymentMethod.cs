using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorePayments.Payment.Yookassa
{

    public class Card
    {
        public string first6 { get; set; }
        public string last4 { get; set; }
        public string expiry_year { get; set; }
        public string expiry_month { get; set; }
        public string card_type { get; set; }
        public string issuer_country { get; set; }
    }


    public class YookassaPaymentMethod
    {
        public string type { get; set; }
        public string id { get; set; }
        public bool saved { get; set; }
        public string title { get; set; }
        public Card card { get; set; }
    }
}
